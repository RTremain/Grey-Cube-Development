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
using System.Net;

namespace GCDGameStore.Controllers
{
    public class MemberController : Controller
    {
        private readonly GcdGameStoreContext _context;
        private readonly ILogger _logger;
        private readonly LoginStatus _loginStatus;
        private readonly Cart _cart;
        private readonly IEmailSender _emailSender;

        public MemberController(GcdGameStoreContext context, ILogger<MemberController> logger, IHttpContextAccessor accessor, IEmailSender emailSender)
        {
            _context = context;
            _logger = logger;
            _loginStatus = new LoginStatus(accessor);
            _cart = new Cart(accessor, logger);
            _emailSender = emailSender;
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


        public IActionResult ResetPassword()
        {
            HttpContext.Session.SetString("Error", "");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPasswordConfirm(string email)
        {
            if (!_loginStatus.IsNotLoggedIn())
            {
                return RedirectToAction(nameof(Details));
            }

            var member = await _context.Member.Where(m => m.Email == email).SingleOrDefaultAsync();

            if (member == null)
            {
                HttpContext.Session.SetString("Error", "Email not found.");
                return View("ResetPassword", null);
            }

            // hash will be made from both a generated salt to be the password, and another salt to be the salt
            var verification = new ResetPasswordVerify
            {
                MemberId = member.MemberId,
                VerificationHash = WebUtility.UrlEncode(AccountHashing.GenHash(Convert.ToBase64String(AccountHashing.GenSalt()), AccountHashing.GenSalt()))
            };

            string body = "Click the following link: https://localhost:5001/Member/ResetPasswordEmail?request=" 
                + verification.VerificationHash;

            _context.Add(verification);
            await _context.SaveChangesAsync();
            await _emailSender.SendEmailAsync(email, "Reset Password", body);

            return View();
        }

        [HttpGet]
        public async Task<IActionResult>ResetPasswordEmail()
        {
            string verificationHash = Uri.EscapeDataString(HttpContext.Request.Query["request"].ToString());

            if (verificationHash == null || verificationHash == "")
            {
                _logger.LogError("Error: No hash given.");
                return RedirectToAction("Index", "Home");
            }

            var requestVerify = await _context.ResetPasswordVerify
                .Where(r => r.VerificationHash == verificationHash)
                .SingleOrDefaultAsync();

            if (requestVerify == null)
            {
                _logger.LogError("Error: Request hash not found.");
                return RedirectToAction("Index", "Home");
            }

            if (verificationHash == requestVerify.VerificationHash)
            {
                _loginStatus.MemberReset(requestVerify.MemberId.ToString());
                HttpContext.Session.SetString("Error", "");
                return View();
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPasswordEmail(string newPassword, string newPasswordConfirm)
        {
            if (_loginStatus.IsNotLoggedIn())
            {
                _logger.LogInformation("Redirect: {Message}", "Redirecting to login");
                return RedirectToAction(nameof(Login));
            }

            var memberId = _loginStatus.GetMemberId();
            var member = await _context.Member.Where(m => m.MemberId == memberId).SingleOrDefaultAsync();
            var verifyReset = await _context.ResetPasswordVerify.Where(m => m.MemberId == memberId).SingleOrDefaultAsync();

            if (!_loginStatus.MemberIsResetting())
            {
                _logger.LogError("Error: {message}", "Invalid member attempting reset.");
                return RedirectToAction("Index", "Home");
            }

            if (verifyReset == null)
            {
                _logger.LogError("Error: {message}", "No password reset entry found.");
                return RedirectToAction("Index", "Home");
            }

                // compare new password entries
            if (newPassword == newPasswordConfirm)
            {
                // if they match, generate new salt, store new salt and store hashed new password

                var newSalt = AccountHashing.GenSalt();
                var newHash = AccountHashing.GenHash(newPassword, newSalt);

                member.PwSalt = Convert.ToBase64String(newSalt);
                member.PwHash = newHash;

                try
                {
                    _context.Update(member);
                    _context.ResetPasswordVerify.Remove(verifyReset);
                    await _context.SaveChangesAsync();

                    _loginStatus.MemberLogin(member.MemberId.ToString());
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
                return RedirectToAction(nameof(Details));
            }
            else
            {
                HttpContext.Session.SetString("Error", "New passwords do not match");

            }
            
            return View();
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

            // Get all reviews from user and place ids in dictionary
            Dictionary<int, int> reviewLookup = new Dictionary<int, int>();
            var memberReviews = await _context.Review.Where(r => r.MemberId == memberId).ToListAsync();

            foreach (Review r in memberReviews)
            {
                reviewLookup.Add(r.GameId, r.ReviewId);
            }

            // Get all ratings from user and place ids in second dictionary
            Dictionary<int, Rating> ratingLookup = new Dictionary<int, Rating>();
            var memberRatings = await _context.Rating.Where(r => r.MemberId == memberId).ToListAsync();

            foreach (Rating r in memberRatings)
            {
                ratingLookup.Add(r.GameId, r);
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

                if (ratingLookup.ContainsKey(l.GameId))
                {
                    memberLibraryItem.RatingId = ratingLookup[l.GameId].RatingId;
                    memberLibraryItem.HasRating = true;
                    memberLibraryItem.Rating = ratingLookup[l.GameId];
                }
                else
                {
                    memberLibraryItem.HasRating = false;
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

        // GET: Member/Details
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
            if (_loginStatus.IsNotLoggedIn())
            {
                _logger.LogInformation("Redirect: {Message}", "Redirecting to login");
                return RedirectToAction(nameof(Login));
            }

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
                    _cart.ClearCart();
                    return RedirectToAction(nameof(Details));
                }
                HttpContext.Session.SetString("Error", "Invalid username or password.");

            }
            user.Password = "";
            return View(user);
        }

        public IActionResult Logout()
        {
            _loginStatus.MemberLogout();
            _cart.ClearCart();
            return RedirectToAction("Index", "Home");
        }


        public IActionResult ChangePassword()
        {
            if (_loginStatus.IsNotLoggedIn())
            {
                _logger.LogInformation("Redirect: {Message}", "Redirecting to login");
                return RedirectToAction(nameof(Login));
            }

            HttpContext.Session.SetString("Error", "");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(string oldPassword, string newPassword, string newPasswordConfirm)
        {
            if (_loginStatus.IsNotLoggedIn())
            {
                _logger.LogInformation("Redirect: {Message}", "Redirecting to login");
                return RedirectToAction(nameof(Login));
            }

            var memberId = _loginStatus.GetMemberId();
            var member = await _context.Member.Where(m => m.MemberId == memberId).SingleOrDefaultAsync();

            // hash old password, compare to stored
            byte[] salt = Convert.FromBase64String(member.PwSalt);
            var testHash = AccountHashing.GenHash(oldPassword, salt);

            
            if (testHash == member.PwHash)
            {
                // if valid, compare new password entries
                if (newPassword == newPasswordConfirm)
                {
                    // if they match, generate new salt, store new salt and store hashed new password

                    var newSalt = AccountHashing.GenSalt();
                    var newHash = AccountHashing.GenHash(newPassword, newSalt);

                    member.PwSalt = Convert.ToBase64String(newSalt);
                    member.PwHash = newHash;

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
                    return RedirectToAction(nameof(Details));
                }
                else
                {
                    HttpContext.Session.SetString("Error", "New passwords do not match");

                }
            }
            else
            {
                HttpContext.Session.SetString("Error", "Invalid old password");
            }

            return View();
        }

        public IActionResult Register()
        {
            HttpContext.Session.SetString("Error", "");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("Username,Email,Password,PasswordConfirm")] MemberRegister user)
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

                var memberCheck = await _context.Member.AsNoTracking()
                    .FirstOrDefaultAsync(m => m.Username == user.Username);

                if (memberCheck != null)
                {
                    HttpContext.Session.SetString("Error", "Username already exists.");
                    user.Username = "";
                    return View(user);
                }

                memberCheck = await _context.Member.AsNoTracking()
                    .FirstOrDefaultAsync(m => m.Email == user.Email);

                if (memberCheck != null)
                {
                    HttpContext.Session.SetString("Error", "Email already in use.");
                    user.Email = "";
                    return View(user);
                }

                if (user.Password == user.PasswordConfirm)
                {
                    byte[] salt = AccountHashing.GenSalt();
                    var hash = AccountHashing.GenHash(user.Password, salt);

                    Member newMember = new Member
                    {
                        Username = user.Username,
                        PwSalt = Convert.ToBase64String(salt),
                        PwHash = hash,
                        Email = user.Email
                    };

                    try
                    {
                        _context.Add(newMember);
                        await _context.SaveChangesAsync();

                        // retrieve member from DB for new member ID
                        var storedNewMember = await _context.Member.AsNoTracking()
                            .FirstOrDefaultAsync(m => m.Username == user.Username);

                        _loginStatus.MemberLogin(storedNewMember.MemberId.ToString());
                        return RedirectToAction(nameof(Details));
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", $"Error: {ex.GetBaseException().Message}");
                    }
                    
                }
                else
                {
                    HttpContext.Session.SetString("Error", "Passwords do not match.");
                    user.Password = "";
                    user.PasswordConfirm = "";
                    return View(user);
                }

            } // end if (ModelState.IsValid)

            return View(user);
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

            // These should not be passed to the view
            member.PwHash = "Redacted";
            member.PwSalt = "Redacted";

            return View(member);
        }

        // POST: Member/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MemberId,Username,PwHash,PwSalt,Email,Phone,MailingStreetAddress,MailingPostalCode,MailingCity,MailingProvince,ShippingStreetAddress,ShippingPostalCode,ShippingCity,ShippingProvince")] Member member)
        {
            if (_loginStatus.IsNotLoggedIn())
            {
                _logger.LogInformation("Redirect: {Message}", "Redirecting to login");
                return RedirectToAction(nameof(Login));
            }

            if (id != member.MemberId)
            {
                return NotFound();
            }

            if (member.MemberId != _loginStatus.GetMemberId())
            {
                _logger.LogError("Error: {Message}", "Session does not match model");
                _logger.LogInformation("Redirect: {Message}", "Redirecting to profile");
                return RedirectToAction(nameof(Details));
            }

            if (ModelState.IsValid)
            {
                // These var is readonly, so we do not want tracking for when we save changes
                var storedMember = await _context.Member.AsNoTracking().Where(m => m.MemberId == _loginStatus.GetMemberId()).SingleOrDefaultAsync();

                // Restore Username, hash, and salt
                member.Username = storedMember.Username;
                member.PwHash = storedMember.PwHash;
                member.PwSalt = storedMember.PwSalt;

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

            // Action is disabled for now
            return RedirectToAction(nameof(Index));
            //return View(member);
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
