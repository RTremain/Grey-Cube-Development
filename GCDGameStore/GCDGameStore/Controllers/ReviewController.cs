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

namespace GCDGameStore.Controllers
{
    public class ReviewController : Controller
    {
        private readonly GcdGameStoreContext _context;
        private readonly ILogger _logger;
        private readonly LoginStatus _loginStatus;

        public ReviewController(GcdGameStoreContext context, ILogger<ReviewController> logger, IHttpContextAccessor accessor)
        {
            _context = context;
            _logger = logger;
            _loginStatus = new LoginStatus(accessor);
        }

/*
        // GET: Review
        public async Task<IActionResult> Index()
        {
            var gcdGameStoreContext = _context.Review.Include(r => r.Game).Include(r => r.Member);
            return View(await gcdGameStoreContext.ToListAsync());
        }
*/

        // GET: Review/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var review = await _context.Review
                .Include(r => r.Game)
                .Include(r => r.Member)
                .FirstOrDefaultAsync(m => m.ReviewId == id);
            if (review == null)
            {
                return NotFound();
            }

            return View(review);
        }

        /// <summary>
        ///     Review/Create
        /// </summary>
        /// <param name="id">GameId</param>
        /// <returns></returns>
        public async Task<IActionResult> Create(int? id)
        {
            if (_loginStatus.IsNotLoggedIn())
            {
                _logger.LogInformation("Redirect: {Message}", "Redirecting to login");
                return RedirectToAction("Login", "Member");
            }

            if (id == null)
            {
                return RedirectToAction("Library", "Member");
            }

            var gameId = (int)id;

            // check if member has game in their library
            var libraryCheck = await _context.Library.Where(l => l.GameId == gameId)
                                               .Where(l => l.MemberId == _loginStatus.GetMemberId())
                                               .SingleOrDefaultAsync();

            if (libraryCheck == null)
            {
                _logger.LogError("Error: {Message}", "Library entry not found.");
                return RedirectToAction("Library", "Member");
            }

            // make sure user does not already have a review for this game
            var reviewCheck = await _context.Review.Where(r => r.GameId == gameId)
                                             .Where(r => r.MemberId == _loginStatus.GetMemberId())
                                             .SingleOrDefaultAsync();

            if (reviewCheck != null)
            {
                _logger.LogError("Error: {Message}", "Member already has a review for this game.");
                return RedirectToAction("Library", "Member");
            }

            var review = new Review {
                GameId = gameId,
                MemberId = _loginStatus.GetMemberId(),
                Recommended = false,
                Approved = false
            };

            return View(review);
        }

        // POST: Review/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Recommended,ReviewText,Approved,MemberId,GameId")] Review review)
        {
            if (_loginStatus.IsNotLoggedIn())
            {
                _logger.LogInformation("Redirect: {Message}", "Redirecting to login");
                return RedirectToAction("Login", "Member");
            }

            // does the memberId on the review line up with our session
            if (review.MemberId != _loginStatus.GetMemberId())
            {
                _logger.LogError("Error: {Message}", "MemberId on review does not match session.");
                return RedirectToAction("Library", "Member");
            }

            if (ModelState.IsValid)
            {
                // ensure review has correct starting approval status
                review.Approved = false;

                // check if member has game in their library
                var libraryCheck = await _context.Library.Where(l => l.GameId == review.GameId)
                                                   .Where(l => l.MemberId == _loginStatus.GetMemberId())
                                                   .SingleOrDefaultAsync();
            
                if (libraryCheck == null)
                {
                    _logger.LogError("Error: {Message}", "Library entry not found.");
                    return RedirectToAction("Library", "Member");
                }

                // make sure user does not already have a review for this game
                var reviewCheck = await _context.Review.Where(r => r.GameId == review.GameId)
                                                 .Where(r => r.MemberId == _loginStatus.GetMemberId())
                                                 .SingleOrDefaultAsync();

                if (reviewCheck != null)
                {
                    _logger.LogError("Error: {Message}", "Member already has a review for this game.");
                    return RedirectToAction("Library", "Member");
                }

            
                _context.Add(review);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Details), new { id = review.ReviewId });
            }

            return View(review);
        }

    }
}
