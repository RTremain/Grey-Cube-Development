using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GCDGameStore.Models;
using Microsoft.AspNetCore.Http;
using GCDGameStore.Classes;

namespace GCDGameStore.Controllers
{
    public class ReportController : Controller
    {
        private readonly GcdGameStoreContext _context;
        private readonly LoginStatus _loginStatus;

        public ReportController(GcdGameStoreContext context, IHttpContextAccessor accessor)
        {
            _context = context;
            _loginStatus = new LoginStatus(accessor);
        }

        

        public IActionResult Index()
        {
            if (!_loginStatus.IsEmployee())
            {
                return RedirectToAction("Login", "Employee");
            }

            return View();
        }

        public IActionResult GameList()
        {
            if (!_loginStatus.IsEmployee())
            {
                return RedirectToAction("Login", "Employee");
            }

            return View();
        }

        public IActionResult GameDetail()
        {
            if (!_loginStatus.IsEmployee())
            {
                return RedirectToAction("Login", "Employee");
            }

            return View();
        }

        public IActionResult MemberList()
        {
            if (!_loginStatus.IsEmployee())
            {
                return RedirectToAction("Login", "Employee");
            }

            return View();
        }

        public IActionResult MemberDetail()
        {
            if (!_loginStatus.IsEmployee())
            {
                return RedirectToAction("Login", "Employee");
            }

            return View();
        }

        public IActionResult WishList()
        {
            if (!_loginStatus.IsEmployee())
            {
                return RedirectToAction("Login", "Employee");
            }

            return View();
        }

        public IActionResult Sales()
        {
            if (!_loginStatus.IsEmployee())
            {
                return RedirectToAction("Login", "Employee");
            }

            return View();
        }
    }
}