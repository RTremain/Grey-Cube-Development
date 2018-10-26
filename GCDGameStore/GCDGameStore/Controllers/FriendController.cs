using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GCDGameStore.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace GCDGameStore.Controllers
{
    public class FriendController : Controller
    {
        private readonly GcdGameStoreContext _context;
        private readonly ILogger _logger;

        public FriendController(GcdGameStoreContext context, ILogger<FriendController> logger)
        {
            _context = context;
            _logger = logger;
        }

        private bool IsNotLoggedIn()
        {
            var memberId = HttpContext.Session.GetString("MemberId");
            if (memberId == null || memberId == "")
            {
                return true;
            }

            return false;
        }

        // GET: Friend
        public async Task<IActionResult> Index()
        {
            if (IsNotLoggedIn())
            {
                _logger.LogInformation("Redirect: {Message}", "Redirecting to login");
                return RedirectToAction("Login", "Member");
            }

            var memberId = Convert.ToInt32(HttpContext.Session.GetString("MemberId"));
            var gcdGameStoreContext = 
                _context.Friend
                    .Include(f => f.FriendMember)
                    .Include(f => f.Member)
                    .Where(f => f.MemberId == memberId);
            return View(await gcdGameStoreContext.ToListAsync());
        }

        
        // GET: Friend/Create
        public IActionResult Create()
        {
            if (IsNotLoggedIn())
            {
                _logger.LogInformation("Redirect: {Message}", "Redirecting to login");
                return RedirectToAction("Login", "Member");
            }
            HttpContext.Session.SetString("Error", "");
            return View();
        }

        // POST: Friend/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(String username)
        {
            if (IsNotLoggedIn())
            {
                _logger.LogInformation("Redirect: {Message}", "Redirecting to login");
                return RedirectToAction("Login", "Member");
            }

            _logger.LogInformation("Received username: {Message}", username);
            var friend = await _context.Member.Where(m => m.Username == username).SingleOrDefaultAsync();
            var memberId = HttpContext.Session.GetString("MemberId");

            HttpContext.Session.SetString("Error", "");

            if (friend == null)
            {
                HttpContext.Session.SetString("Error", "User not found.");
                return View();
            }

            var friendship = await _context.Friend.Where(f => f.MemberId == Convert.ToInt32(memberId)).Where(f => f.FriendMemberId == friend.MemberId).SingleOrDefaultAsync();

            if (friendship != null)
            {
                HttpContext.Session.SetString("Error", "Friendship already exists.");
                return View();
            }

            var newFriendship1 = new Friend { MemberId = Convert.ToInt32(memberId), FriendMemberId = friend.MemberId };
            var newFriendship2 = new Friend { MemberId = friend.MemberId, FriendMemberId = Convert.ToInt32(memberId) };

            try
            {
                _context.Add(newFriendship1);
                _context.Add(newFriendship2);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                HttpContext.Session.SetString("Error", String.Format("Error on add: {0}", ex.Message));
            }
            
            return View();
        }

       // POST: Friend/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string friendId)
        {
            if (IsNotLoggedIn())
            {
                _logger.LogInformation("Redirect: {Message}", "Redirecting to login");
                return RedirectToAction("Login", "Member");
            }

            var memberId = Convert.ToInt32(HttpContext.Session.GetString("MemberId"));
            var friendIdInt = Convert.ToInt32(friendId);

            var friendship1 = await _context.Friend.Where(f => f.MemberId == memberId).Where(f => f.FriendMemberId == friendIdInt).SingleOrDefaultAsync();
            var friendship2 = await _context.Friend.Where(f => f.MemberId == friendIdInt).Where(f => f.FriendMemberId == memberId).SingleOrDefaultAsync();

            if (friendship1 == null || friendship2 == null)
            {
                _logger.LogError("One or both friendship entires null");
                return RedirectToAction(nameof(Index));
            }

            _context.Friend.Remove(friendship1);
            _context.Friend.Remove(friendship2);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FriendExists(int id)
        {
            return _context.Friend.Any(e => e.FriendId == id);
        }
    }
}
