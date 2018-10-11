using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GCDGameStore.Models;

namespace GCDGameStore.Controllers
{
    public class ReportController : Controller
    {
        private readonly GcdGameStoreContext _context;

        public ReportController(GcdGameStoreContext context)
        {
            _context = context;
        }
        
        public async Task<IActionResult> Index()
        {
            return View(await _context.Employee.ToListAsync());
        }
    }
}