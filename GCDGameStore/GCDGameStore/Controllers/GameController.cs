using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GCDGameStore.Models;
using Microsoft.AspNetCore.Http;
using GCDGameStore.Classes;

namespace GCDGameStore.Controllers
{
    public class GameController : Controller
    {
        private readonly GcdGameStoreContext _context;
        private readonly LoginStatus _loginStatus;

        public GameController(GcdGameStoreContext context, IHttpContextAccessor accessor)
        {
            _context = context;
            _loginStatus = new LoginStatus(accessor);
        }

        // GET: Game
        public async Task<IActionResult> Index()
        {
            if (!_loginStatus.IsEmployee())
            {
                return RedirectToAction("Login", "Employee");
            }

            return View(await _context.Game.ToListAsync());
        }

        // GET: Game/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (!_loginStatus.IsEmployee())
            {
                return RedirectToAction("Login", "Employee");
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

        // GET: Game/Create
        public IActionResult Create()
        {
            if (!_loginStatus.IsEmployee())
            {
                return RedirectToAction("Login", "Employee");
            }

            return View();
        }

        // POST: Game/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("GameID,Title,ReleaseDate")] Game game)
        {
            if (!_loginStatus.IsEmployee())
            {
                return RedirectToAction("Login", "Employee");
            }

            if (ModelState.IsValid)
            {
                _context.Add(game);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(game);
        }

        // GET: Game/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (!_loginStatus.IsEmployee())
            {
                return RedirectToAction("Login", "Employee");
            }

            if (id == null)
            {
                return NotFound();
            }

            var game = await _context.Game.FindAsync(id);
            if (game == null)
            {
                return NotFound();
            }
            return View(game);
        }

        // POST: Game/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("GameID,Title,ReleaseDate")] Game game)
        {
            if (!_loginStatus.IsEmployee())
            {
                return RedirectToAction("Login", "Employee");
            }

            if (id != game.GameId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(game);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GameExists(game.GameId))
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
            return View(game);
        }

        // GET: Game/Delete/5
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

            var game = await _context.Game
                .FirstOrDefaultAsync(m => m.GameId == id);
            if (game == null)
            {
                return NotFound();
            }

            return View(game);
        }

        // POST: Game/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!_loginStatus.IsEmployee())
            {
                return RedirectToAction("Login", "Employee");
            }

            var game = await _context.Game.FindAsync(id);
            _context.Game.Remove(game);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GameExists(int id)
        {
            return _context.Game.Any(e => e.GameId == id);
        }
    }
}
