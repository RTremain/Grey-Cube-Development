using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GCDGameStore.Models;
using Microsoft.Extensions.Logging;
using GCDGameStore.Classes;
using Microsoft.AspNetCore.Http;
using GCDGameStore.ViewModels;

namespace GCDGameStore.Controllers
{
    public class MemberGameController : Controller
    {
        private readonly GcdGameStoreContext _context;
        private readonly ILogger _logger;
        private readonly LoginStatus _loginStatus;
        private readonly Cart _cart;

        public MemberGameController(GcdGameStoreContext context, ILogger<MemberGameController> logger, IHttpContextAccessor accessor)
        {
            _context = context;
            _logger = logger;
            _loginStatus = new LoginStatus(accessor);
            _cart = new Cart(accessor, logger);
        }

        // GET: MemberGame
        public async Task<IActionResult> Index()
        {
            if (_loginStatus.IsNotLoggedIn())
            {
                _logger.LogInformation("Redirect: {Message}", "Redirecting to login");
                return RedirectToAction("Login", "Member");
            }

            return View(await _context.Game.ToListAsync());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SearchIndex(string searchText)
        {
            var results = await _context.Game.Where(g => g.Title.Contains(searchText)).ToListAsync();

            return View("Index", results);
        }

        public IActionResult Download()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateLibraryItem(string GameId)
        {
            if (_loginStatus.IsNotLoggedIn())
            {
                _logger.LogInformation("Redirect: {Message}", "Redirecting to login");
                return RedirectToAction("Login", "Member");
            }

            var gameId = Convert.ToInt32(GameId);

            if (await _context.Game.FindAsync(gameId) == null)
            {
                return NotFound();
            }

            var LibraryItem = new Library { MemberId = _loginStatus.GetMemberId(), GameId = gameId };

            Wishlist wishlistCheck = null;

            try
            {
                wishlistCheck = await _context.Wishlist.Where(w => w.GameId == gameId)
                                                       .Where(w => w.MemberId == _loginStatus.GetMemberId())
                                                       .SingleOrDefaultAsync();
            }
            catch (Exception e)
            {
                _logger.LogError("Error retrieving single wishlist: {Message}", e.Message);
            }

            if (wishlistCheck != null)
            {
                _context.Wishlist.Remove(wishlistCheck);
            }

            _context.Add(LibraryItem);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id = gameId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateWishlistItem(string GameId)
        {
            if (_loginStatus.IsNotLoggedIn())
            {
                _logger.LogInformation("Redirect: {Message}", "Redirecting to login");
                return RedirectToAction("Login", "Member");
            }

            var gameId = Convert.ToInt32(GameId);

            if (await _context.Game.FindAsync(gameId) == null)
            {
                return NotFound();
            }

            var wishlistItem = new Wishlist { MemberId = _loginStatus.GetMemberId(), GameId = gameId };

            _context.Add(wishlistItem);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id = gameId });
        }

        // POST: MemberEvent/Delete/5
        [HttpPost, ActionName("DeleteWishlistItem")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteWishlistItem(string GameId)
        {
            if (_loginStatus.IsNotLoggedIn())
            {
                _logger.LogInformation("Redirect: {Message}", "Redirecting to login");
                return RedirectToAction("Login", "Member");
            }

            var gameId = Convert.ToInt32(GameId);

            var wishlistItem = await _context.Wishlist
                                        .Where(a => a.GameId == gameId)
                                        .Where(a => a.MemberId == _loginStatus.GetMemberId())
                                        .SingleAsync();

            if (wishlistItem == null)
            {
                return NotFound();
            }

            _context.Wishlist.Remove(wishlistItem);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Details), new { id = gameId });
        }

        // GET: MemberGame/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (_loginStatus.IsNotLoggedIn())
            {
                _logger.LogInformation("Redirect: {Message}", "Redirecting to login");
                return RedirectToAction("Login", "Member");
            }

            if (id == null)
            {
                return NotFound();
            }

            int gameId = (int)id;

            var game = await _context.Game
                .FirstOrDefaultAsync(m => m.GameId == gameId);

            if (game == null)
            {
                return NotFound();
            }

            var MemberGameDetail = new MemberGameDetail(game);
            var memberId = _loginStatus.GetMemberId();

            var libraryEntry = await _context.Library.Where(l => l.GameId == gameId)
                                                     .Where(l => l.MemberId == memberId)
                                                     .SingleOrDefaultAsync();

            var wishlistEntry = _context.Wishlist.Where(l => l.GameId == gameId)
                                                 .Where(l => l.MemberId == memberId)
                                                 .SingleOrDefaultAsync();

            if (libraryEntry != null)
            {
                MemberGameDetail.InLibrary = true;
            }
            else
            {
                MemberGameDetail.OnCart = _cart.OnCart(gameId);

                if (await wishlistEntry != null)
                {
                    MemberGameDetail.OnWishlist = true;
                }
            }

            var reviews = await _context.Review
                                        .Where(r => r.GameId == gameId)
                                        .Where(r => r.Approved == true)
                                        .Include(r => r.Member)
                                        .ToListAsync();

            if (reviews != null)
            {
                MemberGameDetail.HasReview = true;
                MemberGameDetail.Reviews = reviews;
            }


            return View(MemberGameDetail);
        }

    } // End class MemberGameController

} // End Namespace GCDGameStore.Controllers
