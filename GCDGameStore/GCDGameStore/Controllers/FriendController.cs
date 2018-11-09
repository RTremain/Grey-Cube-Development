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
using GCDGameStore.Classes;

namespace GCDGameStore.Controllers
{
    public class FriendController : Controller
    {
        private readonly GcdGameStoreContext _context;
        private readonly ILogger _logger;
        private readonly LoginStatus _loginStatus;

        public FriendController(GcdGameStoreContext context, ILogger<FriendController> logger, IHttpContextAccessor accessor)
        {
            _context = context;
            _logger = logger;
            _loginStatus = new LoginStatus(accessor);
        }

        

        // GET: Friend
        public async Task<IActionResult> Index()
        {
            if (_loginStatus.IsNotLoggedIn())
            {
                _logger.LogInformation("Redirect: {Message}", "Redirecting to login");
                return RedirectToAction("Login", "Member");
            }

            var memberId = _loginStatus.GetMemberId();
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
            if (_loginStatus.IsNotLoggedIn())
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
            if (_loginStatus.IsNotLoggedIn())
            {
                _logger.LogInformation("Redirect: {Message}", "Redirecting to login");
                return RedirectToAction("Login", "Member");
            }

            _logger.LogInformation("Received username: {Message}", username);
            var friend = await _context.Member.Where(m => m.Username == username).SingleOrDefaultAsync();
            var memberId = _loginStatus.GetMemberId();

            HttpContext.Session.SetString("Error", "");

            if (memberId == friend.MemberId)
            {
                HttpContext.Session.SetString("Error", "You can't add yourself.");
                return View();
            }

            if (friend == null)
            {
                HttpContext.Session.SetString("Error", "User not found.");
                return View();
            }

            var friendship = await _context.Friend.Where(f => f.MemberId == memberId).Where(f => f.FriendMemberId == friend.MemberId).SingleOrDefaultAsync();

            if (friendship != null)
            {
                HttpContext.Session.SetString("Error", "Friendship already exists.");
                return View();
            }

            var newFriendship1 = new Friend { MemberId = memberId, FriendMemberId = friend.MemberId };
            var newFriendship2 = new Friend { MemberId = friend.MemberId, FriendMemberId = memberId };

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
            if (_loginStatus.IsNotLoggedIn())
            {
                _logger.LogInformation("Redirect: {Message}", "Redirecting to login");
                return RedirectToAction("Login", "Member");
            }

            var memberId = _loginStatus.GetMemberId();
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
