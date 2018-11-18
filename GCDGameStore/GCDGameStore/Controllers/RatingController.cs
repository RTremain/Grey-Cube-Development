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
    public class RatingController : Controller
    {
        private readonly GcdGameStoreContext _context;
        private readonly ILogger _logger;
        private readonly LoginStatus _loginStatus;

        public RatingController(GcdGameStoreContext context, ILogger<RatingController> logger, IHttpContextAccessor accessor)
        {
            _context = context;
            _logger = logger;
            _loginStatus = new LoginStatus(accessor);
        }

        /// <summary>
        ///     Get: Rating/Create
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

            // make sure user does not already have a rating for this game
            var ratingCheck = await _context.Rating.Where(r => r.GameId == gameId)
                                             .Where(r => r.MemberId == _loginStatus.GetMemberId())
                                             .SingleOrDefaultAsync();

            if (ratingCheck != null)
            {
                _logger.LogError("Error: {Message}", "Member already has a rating for this game.");
                return RedirectToAction("Library", "Member");
            }

            var rating = new Rating
            {
                GameId = gameId,
                MemberId = _loginStatus.GetMemberId(),
                RatingScore = 1
            };

            return View(rating);
        }

        // POST: Rating/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RatingScore,MemberId,GameId")] Rating rating)
        {
            if (_loginStatus.IsNotLoggedIn())
            {
                _logger.LogInformation("Redirect: {Message}", "Redirecting to login");
                return RedirectToAction("Login", "Member");
            }

            // does the memberId on the rating line up with our session
            if (rating.MemberId != _loginStatus.GetMemberId())
            {
                _logger.LogError("Error: {Message}", "MemberId on rating does not match session.");
                return RedirectToAction("Library", "Member");
            }

            if (ModelState.IsValid)
            {
                
                // check if member has game in their library
                var libraryCheck = await _context.Library.Where(l => l.GameId == rating.GameId)
                                                   .Where(l => l.MemberId == _loginStatus.GetMemberId())
                                                   .SingleOrDefaultAsync();

                if (libraryCheck == null)
                {
                    _logger.LogError("Error: {Message}", "Library entry not found.");
                    return RedirectToAction("Library", "Member");
                }

                // make sure user does not already have a rating for this game
                var ratingCheck = await _context.Rating.Where(r => r.GameId == rating.GameId)
                                                 .Where(r => r.MemberId == _loginStatus.GetMemberId())
                                                 .SingleOrDefaultAsync();

                if (ratingCheck != null)
                {
                    _logger.LogError("Error: {Message}", "Member already has a rating for this game.");
                    return RedirectToAction("Library", "Member");
                }

                var ratingList = await _context.Rating.Where(r => r.GameId == rating.GameId).ToListAsync();
                var gameRated = await _context.Game.Where(g => g.GameId == rating.GameId).SingleOrDefaultAsync();

                if (ratingList == null)
                {
                    gameRated.AverageRating = (float)rating.RatingScore;
                }
                else
                {
                    var total = ratingList.Count();
                    var curRating = gameRated.AverageRating;
                    var newRating = (float)rating.RatingScore;
                    var newAverage = ((curRating * total) + newRating) / (total + 1);

                    gameRated.AverageRating = newAverage;
                }

                _context.Add(rating);
                _context.Game.Update(gameRated);
                await _context.SaveChangesAsync();
                return RedirectToAction("Library", "Member");
            }
            

            return View(rating);
        }
    } // End RatingController
}
