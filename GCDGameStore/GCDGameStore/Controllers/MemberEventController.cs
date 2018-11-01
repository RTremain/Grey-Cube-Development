using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GCDGameStore.Models;
using GCDGameStore.Classes;
using GCDGameStore.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace GCDGameStore.Controllers
{
    public class MemberEventController : Controller
    {
        private readonly GcdGameStoreContext _context;
        private readonly LoginStatus _loginStatus;
        private readonly ILogger _logger;

        public MemberEventController(GcdGameStoreContext context, ILogger<MemberController> logger, IHttpContextAccessor accessor)
        {
            _context = context;
            _logger = logger;
            _loginStatus = new LoginStatus(accessor);
        }

        // GET: MemberEvent
        public async Task<IActionResult> Index()
        {
            if (_loginStatus.IsNotLoggedIn())
            {
                _logger.LogInformation("Redirect: {Message}", "Redirecting to login");
                return RedirectToAction("Login", "Member");
            }

            var eventList = await _context.Event.ToListAsync();
            var memberId = Convert.ToInt32(HttpContext.Session.GetString("MemberId"));

            var attendanceList = await _context.Attendance
                .Where(a => a.MemberId == memberId)
                .Include(a => a.Event)
                .ToListAsync();

            var viewModelList = new List<AttendingEvent>();

            foreach (Event e in eventList)
            {
                var newEvent = new AttendingEvent
                {
                    AttendingEventId = e.EventId,
                    Title = e.Title,
                    EventDate = e.EventDate,
                    Description = e.Description,
                    Registered = false
                };

                var result = attendanceList.Find(a => a.EventId == e.EventId);
                if (result != null)
                {
                    newEvent.Registered = true;
                }

                viewModelList.Add(newEvent);
            }

            return View(viewModelList);
        }

        // GET: MemberEvent/Details/5
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

            var @event = await _context.Event
                .FirstOrDefaultAsync(m => m.EventId == id);
            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }

        // POST: MemberEvent/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EventId,Title,EventDate,Description")] Event @event)
        {
            if (_loginStatus.IsNotLoggedIn())
            {
                _logger.LogInformation("Redirect: {Message}", "Redirecting to login");
                return RedirectToAction("Login", "Member");
            }

            if (ModelState.IsValid)
            {
                _context.Add(@event);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(@event);
        }

       
        // POST: MemberEvent/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_loginStatus.IsNotLoggedIn())
            {
                _logger.LogInformation("Redirect: {Message}", "Redirecting to login");
                return RedirectToAction("Login", "Member");
            }

            var @event = await _context.Event.FindAsync(id);
            _context.Event.Remove(@event);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
