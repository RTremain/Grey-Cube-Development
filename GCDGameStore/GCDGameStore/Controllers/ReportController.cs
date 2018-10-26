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
        
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GameList()
        {
            return View();
        }

        public IActionResult GameDetail()
        {
            return View();
        }

        public IActionResult MemberList()
        {
            return View();
        }

        public IActionResult MemberDetail()
        {
            return View();
        }

        public IActionResult WishList()
        {
            return View();
        }

        public IActionResult Sales()
        {
            return View();
        }
    }
}