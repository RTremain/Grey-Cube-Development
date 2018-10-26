using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GCDGameStore.Models;
using Microsoft.AspNetCore.Http;

namespace GCDGameStore.Controllers
{
    public class ReportController : Controller
    {
        private readonly GcdGameStoreContext _context;

        public ReportController(GcdGameStoreContext context)
        {
            _context = context;
        }

        private bool IsEmployee()
        {
            if (HttpContext.Session.GetString("EmployeeLogin") == "true")
            {
                return true;
            }

            return false;
        }

        public IActionResult Index()
        {
            if (!IsEmployee())
            {
                return RedirectToAction("Login", "Employee");
            }

            return View();
        }

        public IActionResult GameList()
        {
            if (!IsEmployee())
            {
                return RedirectToAction("Login", "Employee");
            }

            return View();
        }

        public IActionResult GameDetail()
        {
            if (!IsEmployee())
            {
                return RedirectToAction("Login", "Employee");
            }

            return View();
        }

        public IActionResult MemberList()
        {
            if (!IsEmployee())
            {
                return RedirectToAction("Login", "Employee");
            }

            return View();
        }

        public IActionResult MemberDetail()
        {
            if (!IsEmployee())
            {
                return RedirectToAction("Login", "Employee");
            }

            return View();
        }

        public IActionResult WishList()
        {
            if (!IsEmployee())
            {
                return RedirectToAction("Login", "Employee");
            }

            return View();
        }

        public IActionResult Sales()
        {
            if (!IsEmployee())
            {
                return RedirectToAction("Login", "Employee");
            }

            return View();
        }
    }
}