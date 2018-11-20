using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GCDGameStore.Classes;
using GCDGameStore.Models;
using GCDGameStore.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GCDGameStore.Controllers
{
    public class MemberGenreController : Controller
    {
        private readonly GcdGameStoreContext _context;
        private readonly LoginStatus _loginStatus;
        private readonly ILogger _logger;

        public MemberGenreController(GcdGameStoreContext context, ILogger<MemberGenreController> logger, IHttpContextAccessor accessor)
        {
            _context = context;
            _logger = logger;
            _loginStatus = new LoginStatus(accessor);
        }

        public async Task<IActionResult> Index()
        {
            if (_loginStatus.IsNotLoggedIn())
            {
                _logger.LogInformation("Redirect: {Message}", "Redirecting to login");
                return RedirectToAction("Login", "Member");
            }

            var genreList = await _context.Genre.ToListAsync();
            var memberId = _loginStatus.GetMemberId();

            var favouriteList = await _context.MemberGenre
                .Where(a => a.MemberId == memberId)
                .Include(a => a.Genre)
                .ToListAsync();

            var viewModelList = new List<MemberGenreViewModel>();

            foreach (Genre g in genreList)
            {
                var newFavourite = new MemberGenreViewModel(g);

                var result = favouriteList.Find(a => a.GenreId == g.GenreId);
                if (result != null)
                {
                    newFavourite.Added = true;
                }

                viewModelList.Add(newFavourite);
            }

            return View(viewModelList);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string genreId)
        {
            if (_loginStatus.IsNotLoggedIn())
            {
                _logger.LogInformation("Redirect: {Message}", "Redirecting to login");
                return RedirectToAction("Login", "Member");
            }


            var genreFavourite = new MemberGenre { MemberId = _loginStatus.GetMemberId(), GenreId = Convert.ToInt32(genreId) };

            _context.Add(genreFavourite);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        // POST: MemberEvent/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string genreId)
        {
            if (_loginStatus.IsNotLoggedIn())
            {
                _logger.LogInformation("Redirect: {Message}", "Redirecting to login");
                return RedirectToAction("Login", "Member");
            }


            var genreFavourite = await _context.MemberGenre
                                        .Where(a => a.GenreId == Convert.ToInt32(genreId))
                                        .Where(a => a.MemberId == _loginStatus.GetMemberId())
                                        .SingleAsync();

            _context.MemberGenre.Remove(genreFavourite);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}