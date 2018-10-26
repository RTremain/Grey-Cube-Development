using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GCDGameStore.Models;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Http;

namespace GCDGameStore.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly GcdGameStoreContext _context;

        public EmployeeController(GcdGameStoreContext context)
        {
            _context = context;
        }

        private bool IsEmployee()
        {
            if (HttpContext.Session.GetString("EmployeeLogin") == "true")
            {
                return true;
            }

            return false;
        }

        // GET: Employee
        public async Task<IActionResult> Index()
        {
            if (!IsEmployee())
            {
                return RedirectToAction(nameof(Login));
            }

            return View(await _context.Employee.ToListAsync());
        }

        // GET: Employee/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (!IsEmployee())
            {
                return RedirectToAction(nameof(Login));
            }

            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employee
                .FirstOrDefaultAsync(m => m.EmployeeId == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // GET: Employee/Create
        public IActionResult Create()
        {
            if (!IsEmployee())
            {
                return RedirectToAction(nameof(Login));
            }

            return View();
        }

        // GET: Employee/Login
        public IActionResult Login()
        {
            if (IsEmployee())
            {
                return RedirectToAction(nameof(Index));
            }

            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.SetString("EmployeeLogin", "false");

            return RedirectToAction("Index", "Home");
        }

        public IActionResult LoginSuccess()
        {
            if (!IsEmployee())
            {
                return RedirectToAction(nameof(Login));
            }
            return View();
        }

        // POST: Employee/Login
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([Bind("Name,PwHash")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                var employeeCheck = await _context.Employee
                .FirstOrDefaultAsync(m => m.Name == employee.Name);
                if (employee == null)
                {
                    return NotFound();
                }

                employee.PwSalt = employeeCheck.PwSalt;

                byte[] salt = Convert.FromBase64String(employee.PwSalt);

                string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password: employee.PwHash,
                    salt: salt,
                    prf: KeyDerivationPrf.HMACSHA1,
                    iterationCount: 10000,
                    numBytesRequested: 256 / 8));
                employee.PwHash = hashed;

                if (employee.PwHash == employeeCheck.PwHash)
                {
                    HttpContext.Session.SetString("EmployeeLogin", "true");
                    return RedirectToAction(nameof(LoginSuccess));
                }

            }
            return View(employee);
        }

        // POST: Employee/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,PwHash")] Employee employee)
        {
            if (!IsEmployee())
            {
                return RedirectToAction(nameof(Login));
            }

            if (ModelState.IsValid)
            {
                string password = employee.PwHash;
                byte[] salt = new byte[128 / 8];
                using (var rng = RandomNumberGenerator.Create())
                {
                    rng.GetBytes(salt);
                }

                employee.PwSalt = Convert.ToBase64String(salt);

                string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password: password,
                    salt: salt,
                    prf: KeyDerivationPrf.HMACSHA1,
                    iterationCount: 10000,
                    numBytesRequested: 256 / 8));
                employee.PwHash = hashed;

                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Add(employee);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                    catch (Exception ex)
                    {

                        ModelState.AddModelError("", $"Error: {ex.GetBaseException().Message}");
                    }
                }

            }
            return View(employee);
        }

        // GET: Employee/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (!IsEmployee())
            {
                return RedirectToAction(nameof(Login));
            }

            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employee.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }

        // POST: Employee/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EmployeeId,Name,PwHash")] Employee employee)
        {
            if (!IsEmployee())
            {
                return RedirectToAction(nameof(Login));
            }

            if (id != employee.EmployeeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(employee);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(employee.EmployeeId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }

        // GET: Employee/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (!IsEmployee())
            {
                return RedirectToAction(nameof(Login));
            }

            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employee
                .FirstOrDefaultAsync(m => m.EmployeeId == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // POST: Employee/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!IsEmployee())
            {
                return RedirectToAction(nameof(Login));
            }

            var employee = await _context.Employee.FindAsync(id);
            _context.Employee.Remove(employee);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employee.Any(e => e.EmployeeId == id);
        }
    }
}
