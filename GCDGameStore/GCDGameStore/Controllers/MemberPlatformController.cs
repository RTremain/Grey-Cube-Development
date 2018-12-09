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
    public class MemberPlatformController : Controller
    {
        private readonly GcdGameStoreContext _context;
        private readonly LoginStatus _loginStatus;
        private readonly ILogger _logger;

        public MemberPlatformController(GcdGameStoreContext context, ILogger<MemberPlatformController> logger, IHttpContextAccessor accessor)
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

            var platformList = await _context.Platform.ToListAsync();
            var memberId = _loginStatus.GetMemberId();

            var favouriteList = await _context.MemberPlatform
                .Where(a => a.MemberId == memberId)
                .Include(a => a.Platform)
                .ToListAsync();

            var viewModelList = new List<MemberPlatformViewModel>();

            foreach (Platform p in platformList)
            {
                var newFavourite = new MemberPlatformViewModel(p);

                var result = favouriteList.Find(a => a.PlatformId == p.PlatformId);
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
        public async Task<IActionResult> Create(string platformId)
        {
            if (_loginStatus.IsNotLoggedIn())
            {
                _logger.LogInformation("Redirect: {Message}", "Redirecting to login");
                return RedirectToAction("Login", "Member");
            }


            var platformFavourite = new MemberPlatform { MemberId = _loginStatus.GetMemberId(), PlatformId = Convert.ToInt32(platformId) };

            _context.Add(platformFavourite);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        // POST: MemberEvent/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string platformId)
        {
            if (_loginStatus.IsNotLoggedIn())
            {
                _logger.LogInformation("Redirect: {Message}", "Redirecting to login");
                return RedirectToAction("Login", "Member");
            }


            var platformFavourite = await _context.MemberPlatform
                                        .Where(a => a.PlatformId == Convert.ToInt32(platformId))
                                        .Where(a => a.MemberId == _loginStatus.GetMemberId())
                                        .SingleAsync();

            _context.MemberPlatform.Remove(platformFavourite);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}