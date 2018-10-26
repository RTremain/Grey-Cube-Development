using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GCDGameStore.Models;
using Microsoft.AspNetCore.Http;

namespace GCDGameStore.Controllers
{
    public class FriendController : Controller
    {
        private readonly GcdGameStoreContext _context;

        public FriendController(GcdGameStoreContext context)
        {
            _context = context;
        }

        // GET: Friend
        public async Task<IActionResult> Index()
        {
            var memberId = Convert.ToInt32(HttpContext.Session.GetString("MemberId"));
            var gcdGameStoreContext = 
                _context.Friend
                    .Include(f => f.FriendMember)
                    .Include(f => f.Member)
                    .Where(f => f.MemberId == memberId);
            return View(await gcdGameStoreContext.ToListAsync());
        }

        // GET: Friend/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var friend = await _context.Friend
                .Include(f => f.FriendMember)
                .Include(f => f.Member)
                .FirstOrDefaultAsync(m => m.FriendId == id);
            if (friend == null)
            {
                return NotFound();
            }

            return View(friend);
        }

        // GET: Friend/Create
        public IActionResult Create()
        {
            ViewData["FriendId"] = new SelectList(_context.Member, "MemberId", "PwHash");
            ViewData["MemberId"] = new SelectList(_context.Member, "MemberId", "PwHash");
            return View();
        }

        // POST: Friend/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FriendId,MemberId,FriendMemberId")] Friend friend)
        {
            if (ModelState.IsValid)
            {
                _context.Add(friend);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FriendId"] = new SelectList(_context.Member, "MemberId", "PwHash", friend.FriendId);
            ViewData["MemberId"] = new SelectList(_context.Member, "MemberId", "PwHash", friend.MemberId);
            return View(friend);
        }

        // GET: Friend/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var friend = await _context.Friend.FindAsync(id);
            if (friend == null)
            {
                return NotFound();
            }
            ViewData["FriendId"] = new SelectList(_context.Member, "MemberId", "PwHash", friend.FriendId);
            ViewData["MemberId"] = new SelectList(_context.Member, "MemberId", "PwHash", friend.MemberId);
            return View(friend);
        }

        // POST: Friend/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FriendId,MemberId,FriendMemberId")] Friend friend)
        {
            if (id != friend.FriendId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(friend);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FriendExists(friend.FriendId))
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
            ViewData["FriendId"] = new SelectList(_context.Member, "MemberId", "PwHash", friend.FriendId);
            ViewData["MemberId"] = new SelectList(_context.Member, "MemberId", "PwHash", friend.MemberId);
            return View(friend);
        }

        // GET: Friend/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var friend = await _context.Friend
                .Include(f => f.FriendMember)
                .Include(f => f.Member)
                .FirstOrDefaultAsync(m => m.FriendId == id);
            if (friend == null)
            {
                return NotFound();
            }

            return View(friend);
        }

        // POST: Friend/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var friend = await _context.Friend.FindAsync(id);
            _context.Friend.Remove(friend);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FriendExists(int id)
        {
            return _context.Friend.Any(e => e.FriendId == id);
        }
    }
}
