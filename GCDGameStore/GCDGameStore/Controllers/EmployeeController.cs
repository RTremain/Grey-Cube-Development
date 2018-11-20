using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GCDGameStore.Models;
using Microsoft.AspNetCore.Http;
using GCDGameStore.Classes;
using GCDGameStore.ViewModels;
using Microsoft.Extensions.Logging;

namespace GCDGameStore.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly GcdGameStoreContext _context;
        private readonly LoginStatus _loginStatus;
        private readonly ILogger _logger;

        public EmployeeController(GcdGameStoreContext context, IHttpContextAccessor accessor, ILogger<EmployeeController> logger)
        {
            _context = context;
            _loginStatus = new LoginStatus(accessor);
            _logger = logger;
        }

        // GET: Employee
        public async Task<IActionResult> Index()
        {
            if (!_loginStatus.IsEmployee())
            {
                return RedirectToAction(nameof(Login));
            }

            return View(await _context.Employee.ToListAsync());
        }

        // GET: Employee/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (!_loginStatus.IsEmployee())
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
            if (!_loginStatus.IsEmployee())
            {
                return RedirectToAction(nameof(Login));
            }

            return View();
        }

        // GET: Employee/Login
        public IActionResult Login()
        {
            if (_loginStatus.IsEmployee())
            {
                return RedirectToAction(nameof(Index));
            }

            return View();
        }

        public IActionResult Logout()
        {
            _loginStatus.EmployeeLogout();

            return RedirectToAction("Index", "Home");
        }

        public IActionResult LoginSuccess()
        {
            if (!_loginStatus.IsEmployee())
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
        public async Task<IActionResult> Login([Bind("Username,Password")] UserLogin user)
        {
            if (_loginStatus.IsEmployee())
            {
                return RedirectToAction(nameof(Index));
            }

            HttpContext.Session.SetString("Error", "");

            if (ModelState.IsValid)
            {
                if (user == null)
                {
                    HttpContext.Session.SetString("Error", "Invalid username or password.");
                    return View();
                }

                var employeeCheck = await _context.Employee
                    .FirstOrDefaultAsync(m => m.Name == user.Username);

                if (employeeCheck == null)
                {
                    HttpContext.Session.SetString("Error", "Invalid username or password.");
                    user.Password = "";
                    return View(user);
                }


                byte[] salt = Convert.FromBase64String(employeeCheck.PwSalt);

                // convert plaintext password given to hash
                var testHash = AccountHashing.GenHash(user.Password, salt);

                if (testHash == employeeCheck.PwHash)
                {
                    _loginStatus.EmployeeLogin(employeeCheck.EmployeeId.ToString());
                    return RedirectToAction(nameof(LoginSuccess));
                }
                HttpContext.Session.SetString("Error", "Invalid username or password.");

            }
            user.Password = "";
            return View(user);
        }

        // POST: Employee/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,PwHash")] Employee employee)
        {
            if (!_loginStatus.IsEmployee())
            {
                return RedirectToAction(nameof(Login));
            }

            if (ModelState.IsValid)
            {
                string password = employee.PwHash;
                byte[] salt = AccountHashing.GenSalt();

                employee.PwSalt = Convert.ToBase64String(salt);                
                employee.PwHash = AccountHashing.GenHash(password, salt);

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
            if (!_loginStatus.IsEmployee())
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
            if (!_loginStatus.IsEmployee())
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
            if (!_loginStatus.IsEmployee())
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
            if (!_loginStatus.IsEmployee())
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

        public async Task<IActionResult> PendingReviewList()
        {
            if (!_loginStatus.IsEmployee())
            {
                return RedirectToAction(nameof(Login));
            }

            var pendingReviews = await _context.Review
                .Where(r => r.Approved == false)
                .Include(r => r.Game)
                .Include(r => r.Member)
                .ToListAsync();

            return View(pendingReviews);
        }

        [HttpPost, ActionName("ApproveReview")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApproveReview(string ReviewId)
        {
            if (!_loginStatus.IsEmployee())
            {
                return RedirectToAction(nameof(Login));
            }

            if (ReviewId == null || ReviewId == "")
            {
                _logger.LogError("Error: {Message}", "null ID supplied to employee ApproveReview");
                return RedirectToAction(nameof(PendingReviewList));
            }

            var id = Convert.ToInt32(ReviewId);

            // retrieve review to update, check if it exists
            var review = await _context.Review.Where(r => r.ReviewId == id).SingleOrDefaultAsync();

            if (review != null)
            {
                if (!review.Approved)
                {
                    review.Approved = true;
                    _context.Update(review);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    _logger.LogError("Error: {Message}", "Review is already approved.");
                }
            }
            else
            {
                return NotFound();
            }

            return RedirectToAction(nameof(PendingReviewList));
        }

        [HttpPost, ActionName("RejectReview")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RejectReview(string ReviewId)
        {
            if (!_loginStatus.IsEmployee())
            {
                return RedirectToAction(nameof(Login));
            }

            if (ReviewId == null || ReviewId == "")
            {
                _logger.LogError("Error: {Message}", "null ID supplied to employee ApproveReview");
                return RedirectToAction(nameof(PendingReviewList));
            }

            var id = Convert.ToInt32(ReviewId);

            // retrieve review to delete, check if it exists
            var review = await _context.Review.Where(r => r.ReviewId == id).SingleOrDefaultAsync();

            if (review != null)
            {
                review.Approved = true;
                _context.Review.Remove(review);
                await _context.SaveChangesAsync();
            }
            else
            {
                return NotFound();
            }

            return RedirectToAction(nameof(PendingReviewList));
        }
    }
}
