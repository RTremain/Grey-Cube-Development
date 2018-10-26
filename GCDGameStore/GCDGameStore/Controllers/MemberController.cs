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
using Microsoft.Extensions.Logging;

namespace GCDGameStore.Controllers
{
    public class MemberController : Controller
    {
        private readonly GcdGameStoreContext _context;
        private readonly ILogger _logger;

        public MemberController(GcdGameStoreContext context, ILogger<MemberController> logger)
        {
            _context = context;
            _logger = logger;
        }

        private bool IsNotLoggedIn()
        {
            if (HttpContext.Session.GetString("MemberId") == null
                || HttpContext.Session.GetString("MemberId") == "")
            {
                return true;
            }

            return false;
        }

        private bool IsEmployee()
        {
            if (HttpContext.Session.GetString("EmployeeLogin") == "true")
            {
                return true;
            }

            return false;
        }

        // GET: Member
        public async Task<IActionResult> Index()
        {
            if (IsNotLoggedIn())
            {
                _logger.LogInformation("Redirect: {Message}", "Redirecting to login");
                return RedirectToAction(nameof(Login));
            }

            if (IsEmployee())
            {
                return View(await _context.Member.ToListAsync());
            }

            return RedirectToAction(nameof(Details));

        }

        public async Task<IActionResult> Library()
        {
            if (IsNotLoggedIn())
            {
                _logger.LogInformation("Redirect: {Message}", "Redirecting to login");
                return RedirectToAction(nameof(Login));
            }

            int id = Convert.ToInt32(HttpContext.Session.GetString("MemberId"));
            
            var library = await _context.Library.Include(a => a.Game)
                .Where(m => m.MemberId == id).ToListAsync();

            if (library == null)
            {
                return NotFound();
            }

            return View(library);
        }

        public async Task<IActionResult> Wishlist()
        {
            if (IsNotLoggedIn())
            {
                _logger.LogInformation("Redirect: {Message}", "Redirecting to login");
                return RedirectToAction(nameof(Login));
            }

            int id = Convert.ToInt32(HttpContext.Session.GetString("MemberId"));

            var wishlist = await _context.Wishlist.Include(a => a.Game)
                .Where(m => m.MemberId == id).ToListAsync();

            if (wishlist == null)
            {
                return NotFound();
            }

            return View(wishlist);
        }

        public async Task<IActionResult> CreditCardView()
        {
            if (IsNotLoggedIn())
            {
                _logger.LogInformation("Redirect: {Message}", "Redirecting to login");
                return RedirectToAction(nameof(Login));
            }

            int id = Convert.ToInt32(HttpContext.Session.GetString("MemberId"));

            var creditCards = await _context.CreditCard
                .Where(c => c.MemberId == id).ToListAsync();


            return View(creditCards);
        }

        // GET: Member/Details/5
        public async Task<IActionResult> GameDetails(int? id)
        {
            // TODO: If member is not logged in, redirect to visitor store page for game

            if (id == null)
            {
                return NotFound();
            }

            var game = await _context.Game
                .FirstOrDefaultAsync(m => m.GameId == id);
            if (game == null)
            {
                return NotFound();
            }

            return View(game);
        }

        // GET: Member/Details/5
        public async Task<IActionResult> Details()
        {
            if (IsNotLoggedIn())
            {
                _logger.LogInformation("Redirect: {Message}", "Redirecting to login");
                return RedirectToAction(nameof(Login));
            }

            int id = Convert.ToInt32(HttpContext.Session.GetString("MemberId"));

            var member = await _context.Member
                .FirstOrDefaultAsync(m => m.MemberId == id);
            if (member == null)
            {
                return NotFound();
            }

            return View(member);
        }

        public IActionResult CreateCreditCard()
        {
            var newCreditCard = new CreditCard();
            newCreditCard.MemberId = Convert.ToInt32(HttpContext.Session.GetString("MemberId"));
            return View(newCreditCard);
        }

        // POST: Member/CreateCreditCard
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCreditCard([Bind("CcNum,ExpMonth,ExpYear,Name,StreetAddress,City,Province,PostalCode")] CreditCard creditCard)
        {
            if (IsNotLoggedIn())
            {
                _logger.LogInformation("Redirect: {Message}", "Redirecting to login");
                return RedirectToAction(nameof(Login));
            }

            int id = Convert.ToInt32(HttpContext.Session.GetString("MemberId"));

            if (ModelState.IsValid)
            {
                creditCard.MemberId = id;
                _context.CreditCard.Add(creditCard);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(CreditCardView));
            }
            return View(creditCard);
        }


        // GET: Member/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Member/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Username,PwHash,Email,Phone,MailingStreetAddress,MailingPostalCode,MailingCity,MailingProvince,ShippingStreetAddress,ShippingPostalCode,ShippingCity,ShippingProvince")] Member member)
        {
            if (ModelState.IsValid)
            {
                string password = member.PwHash;
                byte[] salt = new byte[128 / 8];
                using (var rng = RandomNumberGenerator.Create())
                {
                    rng.GetBytes(salt);
                }

                member.PwSalt = Convert.ToBase64String(salt);

                string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password: password,
                    salt: salt,
                    prf: KeyDerivationPrf.HMACSHA1,
                    iterationCount: 10000,
                    numBytesRequested: 256 / 8));
                member.PwHash = hashed;

                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Add(member);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                    catch (Exception ex)
                    {

                        ModelState.AddModelError("", $"Error: {ex.GetBaseException().Message}");
                    }
                }
            }
            return View(member);
        }

        public IActionResult Login()
        {
            return View();
        }

        // POST: Employee/Login
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([Bind("Username,PwHash")] Member member)
        {
            if (ModelState.IsValid)
            {
                var memberCheck = await _context.Member
                .FirstOrDefaultAsync(m => m.Username == member.Username);
                if (member == null)
                {
                    return NotFound();
                }

                member.PwSalt = memberCheck.PwSalt;

                byte[] salt = Convert.FromBase64String(member.PwSalt);

                string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password: member.PwHash,
                    salt: salt,
                    prf: KeyDerivationPrf.HMACSHA1,
                    iterationCount: 10000,
                    numBytesRequested: 256 / 8));
                member.PwHash = hashed;

                if (member.PwHash == memberCheck.PwHash)
                {
                    HttpContext.Session.SetString("Login", "True");
                    HttpContext.Session.SetString("MemberId", memberCheck.MemberId.ToString());
                    return RedirectToAction(nameof(Details), new { id = memberCheck.MemberId });
                }

            }
            member.PwHash = "";
            return View(member);
        }


        // GET: Member/Edit/5
        public async Task<IActionResult> Edit()
        {
            if (IsNotLoggedIn())
            {
                _logger.LogInformation("Redirect: {Message}", "Redirecting to login");
                return RedirectToAction(nameof(Login));
            }

            int id = Convert.ToInt32(HttpContext.Session.GetString("MemberId"));

            var member = await _context.Member.FindAsync(id);
            if (member == null)
            {
                return NotFound();
            }
            return View(member);
        }

        // POST: Member/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MemberId,Username,PwHash,PwSalt,Email,Phone,MailingStreetAddress,MailingPostalCode,MailingCity,MailingProvince,ShippingStreetAddress,ShippingPostalCode,ShippingCity,ShippingProvince")] Member member)
        {
            if (id != member.MemberId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(member);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MemberExists(member.MemberId))
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
            return View(member);
        }

        // GET: Member/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await _context.Member
                .FirstOrDefaultAsync(m => m.MemberId == id);
            if (member == null)
            {
                return NotFound();
            }

            return View(member);
        }

        public async Task<IActionResult> DeleteWishlistItem(int? id)
        {
            if (IsNotLoggedIn())
            {
                _logger.LogInformation("Redirect: {Message}", "Redirecting to login");
                return RedirectToAction(nameof(Login));
            }

            int memberId = Convert.ToInt32(HttpContext.Session.GetString("MemberId"));

            if (id == null)
            {
                return NotFound();
            }

            var wishlist = await _context.Wishlist
                .Include(w => w.Game)
                .FirstOrDefaultAsync(w => w.WishlistId == id);

            if (wishlist == null)
            {
                return NotFound();
            }
            else if (wishlist.MemberId != memberId)
            {
                return BadRequest();
            }
            

            return View(wishlist);
        }

        public async Task<IActionResult> DeleteCreditCard(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (IsNotLoggedIn())
            {
                _logger.LogInformation("Redirect: {Message}", "Redirecting to login");
                return RedirectToAction(nameof(Login));
            }

            int memberId = Convert.ToInt32(HttpContext.Session.GetString("MemberId"));

            var creditCard = await _context.CreditCard
                .FirstOrDefaultAsync(w => w.CreditCardId == id);

            if (creditCard == null)
            {
                return NotFound();
            }
            else if (creditCard.MemberId != memberId)
            {
                return BadRequest();
            }

            

            return View(creditCard);
        }

        // POST: Member/DeleteWishlistItem/5
        [HttpPost, ActionName("DeleteCreditCard")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCreditCardConfirmed(int id)
        {
            var creditCard = await _context.CreditCard.FindAsync(id);
            _context.CreditCard.Remove(creditCard);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(CreditCardView));
        }

        // POST: Member/DeleteWishlistItem/5
        [HttpPost, ActionName("DeleteWishlistItem")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteWishlistItemConfirmed(int id)
        {
            var wishlist = await _context.Wishlist.FindAsync(id);
            _context.Wishlist.Remove(wishlist);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Wishlist));
        }

        // POST: Member/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var member = await _context.Member.FindAsync(id);
            _context.Member.Remove(member);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MemberExists(int id)
        {
            return _context.Member.Any(e => e.MemberId == id);
        }
    }
}
