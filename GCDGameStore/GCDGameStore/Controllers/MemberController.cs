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
using GCDGameStore.Classes;
using GCDGameStore.ViewModels;
using System.Collections;

namespace GCDGameStore.Controllers
{
    public class MemberController : Controller
    {
        private readonly GcdGameStoreContext _context;
        private readonly ILogger _logger;
        private readonly LoginStatus _loginStatus;

        public MemberController(GcdGameStoreContext context, ILogger<MemberController> logger, IHttpContextAccessor accessor)
        {
            _context = context;
            _logger = logger;
            _loginStatus = new LoginStatus(accessor);
        }

        // GET: Member
        public async Task<IActionResult> Index()
        {
            if (_loginStatus.IsEmployee())
            {
                return View(await _context.Member.ToListAsync());
            }

            if (!_loginStatus.IsNotLoggedIn())
            {
                return RedirectToAction(nameof(Details));
            }

            return RedirectToAction("Login", "Employee");
        }

        public async Task<IActionResult> Library()
        {
            if (_loginStatus.IsNotLoggedIn())
            {
                _logger.LogInformation("Redirect: {Message}", "Redirecting to login");
                return RedirectToAction(nameof(Login));
            }

            int memberId = _loginStatus.GetMemberId();
            
            var libraryList = await _context.Library.Include(a => a.Game)
                .Where(m => m.MemberId == memberId).ToListAsync();

            if (libraryList == null)
            {
                return NotFound();
            }

            var memberLibrary = new List<MemberLibrary>();

            // get all reviews from user and place ids in dictionary
            Dictionary<int, int> reviewLookup = new Dictionary<int, int>();
            var memberReviews = await _context.Review.Where(r => r.MemberId == memberId).ToListAsync();

            foreach (Review r in memberReviews)
            {
                reviewLookup.Add(r.GameId, r.ReviewId);
            }

            foreach (Library l in libraryList)
            {
                var memberLibraryItem = new MemberLibrary(l);

                if (reviewLookup.ContainsKey(l.GameId))
                {
                    memberLibraryItem.ReviewId = reviewLookup[l.GameId];
                    memberLibraryItem.HasReview = true;
                }
                else
                {
                    memberLibraryItem.HasReview = false;
                }

                memberLibrary.Add(memberLibraryItem);
            }

            return View(memberLibrary);
        }

        public async Task<IActionResult> Wishlist()
        {
            if (_loginStatus.IsNotLoggedIn())
            {
                _logger.LogInformation("Redirect: {Message}", "Redirecting to login");
                return RedirectToAction(nameof(Login));
            }

            int id = _loginStatus.GetMemberId();

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
            if (_loginStatus.IsNotLoggedIn())
            {
                _logger.LogInformation("Redirect: {Message}", "Redirecting to login");
                return RedirectToAction(nameof(Login));
            }

            int id = _loginStatus.GetMemberId();

            var creditCards = await _context.CreditCard
                .Where(c => c.MemberId == id).ToListAsync();


            return View(creditCards);
        }

        // GET: Member/Details/5
        public async Task<IActionResult> GameDetails(int? id)
        {
            if (_loginStatus.IsNotLoggedIn())
            {
                _logger.LogInformation("Redirect: {Message}", "Redirecting to login");
                return RedirectToAction(nameof(Login));
            }

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
            if (_loginStatus.IsNotLoggedIn())
            {
                _logger.LogInformation("Redirect: {Message}", "Redirecting to login");
                return RedirectToAction(nameof(Login));
            }

            int id = _loginStatus.GetMemberId();

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
            var newCreditCard = new CreditCard { MemberId = _loginStatus.GetMemberId() };

            return View(newCreditCard);
        }

        // POST: Member/CreateCreditCard
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCreditCard([Bind("CcNum,ExpMonth,ExpYear,Name,StreetAddress,City,Province,PostalCode")] CreditCard creditCard)
        {
            if (_loginStatus.IsNotLoggedIn())
            {
                _logger.LogInformation("Redirect: {Message}", "Redirecting to login");
                return RedirectToAction(nameof(Login));
            }

            int id = _loginStatus.GetMemberId();

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
            if (!_loginStatus.IsEmployee())
            {
                return RedirectToAction("Login", "Employee");
            }
            return View();
        }

        // POST: Member/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Username,PwHash,Email,Phone,MailingStreetAddress,MailingPostalCode,MailingCity,MailingProvince,ShippingStreetAddress,ShippingPostalCode,ShippingCity,ShippingProvince")] Member member)
        {
            if (!_loginStatus.IsEmployee())
            {
                return RedirectToAction("Login", "Employee");
            }

            if (ModelState.IsValid)
            {
                string password = member.PwHash;
                byte[] salt = AccountHashing.GenSalt();

                member.PwSalt = Convert.ToBase64String(salt);
                member.PwHash = AccountHashing.GenHash(password, salt);

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
            if (_loginStatus.IsEmployee())
            {
                return RedirectToAction(nameof(Index));
            }
            else if (_loginStatus.IsNotLoggedIn())
            {
                HttpContext.Session.SetString("Error", "");
                return View();
            }

            return RedirectToAction(nameof(Details));
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

                var memberCheck = await _context.Member
                    .FirstOrDefaultAsync(m => m.Username == user.Username);

                if (memberCheck == null)
                {
                    HttpContext.Session.SetString("Error", "Invalid username or password.");
                    user.Password = "";
                    return View(user);
                }

                byte[] salt = Convert.FromBase64String(memberCheck.PwSalt);

                // convert plaintext password into hash for comparison
                var testHash = AccountHashing.GenHash(user.Password, salt);

                if (testHash == memberCheck.PwHash)
                {
                    _loginStatus.MemberLogin(memberCheck.MemberId.ToString());                    
                    return RedirectToAction(nameof(Details), new { id = memberCheck.MemberId });
                }
                HttpContext.Session.SetString("Error", "Invalid username or password.");

            }
            user.Password = "";
            return View(user);
        }

        public IActionResult Logout()
        {
            _loginStatus.MemberLogout();
            return RedirectToAction("Index", "Home");
        }


        // GET: Member/Edit/5
        public async Task<IActionResult> Edit()
        {
            if (_loginStatus.IsNotLoggedIn())
            {
                _logger.LogInformation("Redirect: {Message}", "Redirecting to login");
                return RedirectToAction(nameof(Login));
            }

            int id = _loginStatus.GetMemberId();

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
            if (!_loginStatus.IsEmployee())
            {
                return RedirectToAction("Login", "Employee");
            }

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

        public async Task<IActionResult> DeleteCreditCard(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (_loginStatus.IsNotLoggedIn())
            {
                _logger.LogInformation("Redirect: {Message}", "Redirecting to login");
                return RedirectToAction(nameof(Login));
            }

            int memberId = _loginStatus.GetMemberId();

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
            if (!_loginStatus.IsEmployee())
            {
                return RedirectToAction("Login", "Employee");
            }

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
