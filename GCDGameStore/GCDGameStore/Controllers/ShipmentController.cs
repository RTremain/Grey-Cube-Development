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
    public class ShipmentController : Controller
    {
        private readonly GcdGameStoreContext _context;
        private readonly ILogger _logger;
        private readonly LoginStatus _loginStatus;

        public ShipmentController(GcdGameStoreContext context, ILogger<MemberController> logger, IHttpContextAccessor accessor)
        {
            _context = context;
            _logger = logger;
            _loginStatus = new LoginStatus(accessor);
        }

        // GET: Shipment
        public async Task<IActionResult> Index()
        {
            if (!_loginStatus.IsEmployee())
            {
                return RedirectToAction("Login", "Employee");
            }

            var gcdGameStoreContext = _context.Shipment
                .Include(s => s.ShipItems)
                    .ThenInclude(shipItem => shipItem.Game)
                .Include(s => s.Employee)
                .Include(s => s.Member)
                .Where(s => s.IsShipped == false);

            return View(await gcdGameStoreContext.ToListAsync());
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BeginProcessing(string shipmentId)
        {
            if (!_loginStatus.IsEmployee())
            {
                return RedirectToAction("Login", "Employee");
            }

            if (shipmentId == null || shipmentId == "")
            {
                _logger.LogError("Error: {Message}", "null ID supplied to shipment BeginProcessing");
                return RedirectToAction(nameof(Index));
            }

            var id = Convert.ToInt32(shipmentId);

            var shipment = await _context.Shipment.Where(s => s.ShipmentId == id).SingleOrDefaultAsync();

            if (shipment != null)
            {
                if (!shipment.IsProcessing && !shipment.IsShipped)
                {
                    shipment.IsProcessing = true;
                    shipment.EmployeeId = _loginStatus.GetEmployeeId();
                    _context.Update(shipment);
                    await _context.SaveChangesAsync();
                }
                else if (shipment.IsShipped)
                {
                    _logger.LogError("Error: {Message}", "Shipment is already shipped.");
                }
                else
                {
                    _logger.LogError("Error: {Message}", "Shipment is already being processed.");
                }
            }
            else
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelProcessing(string shipmentId)
        {
            if (!_loginStatus.IsEmployee())
            {
                return RedirectToAction("Login", "Employee");
            }

            if (shipmentId == null || shipmentId == "")
            {
                _logger.LogError("Error: {Message}", "null ID supplied to shipment CancelProcessing");
                return RedirectToAction(nameof(Index));
            }

            var id = Convert.ToInt32(shipmentId);

            var shipment = await _context.Shipment.Where(s => s.ShipmentId == id).SingleOrDefaultAsync();
            var employeeId = _loginStatus.GetEmployeeId();

            if (shipment != null)
            {
                if (shipment.IsProcessing && !shipment.IsShipped && employeeId == shipment.EmployeeId)
                {
                    shipment.IsProcessing = false;
                    shipment.EmployeeId = null;
                    _context.Update(shipment);
                    await _context.SaveChangesAsync();
                }
                else if (shipment.IsShipped)
                {
                    _logger.LogError("Error: {Message}", "Shipment is already shipped.");
                }
                else if (employeeId != shipment.EmployeeId)
                {
                    _logger.LogError("Error: {Message}", "Current employee does not match owner.");
                }
                else
                {
                    _logger.LogError("Error: {Message}", "Shipment is not being processed already.");
                }
            }
            else
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ShipmentSent(string shipmentId)
        {
            if (!_loginStatus.IsEmployee())
            {
                return RedirectToAction("Login", "Employee");
            }

            if (shipmentId == null || shipmentId == "")
            {
                _logger.LogError("Error: {Message}", "null ID supplied to shipment ShipmentSent");
                return RedirectToAction(nameof(Index));
            }

            var id = Convert.ToInt32(shipmentId);

            var shipment = await _context.Shipment.Where(s => s.ShipmentId == id).SingleOrDefaultAsync();
            var employeeId = _loginStatus.GetEmployeeId();

            if (shipment != null)
            {
                if (shipment.IsProcessing && !shipment.IsShipped && employeeId == shipment.EmployeeId)
                {
                    shipment.IsShipped = true;
                    shipment.ShippedDate = DateTime.Now;
                    _context.Update(shipment);
                    await _context.SaveChangesAsync();
                }
                else if (shipment.IsShipped)
                {
                    _logger.LogError("Error: {Message}", "Shipment is already shipped.");
                }
                else if (employeeId != shipment.EmployeeId)
                {
                    _logger.LogError("Error: {Message}", "Current employee does not match owner.");
                }
                else
                {
                    _logger.LogError("Error: {Message}", "Shipment is not being processed.");
                }
            }
            else
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }

    }
}
