using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GCDGameStore.Models;
using Microsoft.AspNetCore.Http;
using GCDGameStore.Classes;
using GCDGameStore.ViewModels;

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

            var digitalQuery= _context.OrderItemDigital
                .Include(o => o.Game)
                .GroupBy(o => new { o.Game.Title })
                .Select(g => new
                {
                    g.Key.Title,
                    Quantity = g.Count(),
                    Revenue = g.Sum(o => o.Price)
                })
                .OrderByDescending(o => o.Revenue)
                .Take(5)
                .ToList();

            var digitalAggregate = new List<SaleAggregate>();

            foreach (var item in digitalQuery)
            {
                var digitalItem = new SaleAggregate();
                digitalItem.Title = item.Title;
                digitalItem.Quantity = item.Quantity;
                digitalItem.Revenue = item.Revenue;

                digitalAggregate.Add(digitalItem);
            }

            var physicalQuery= _context.OrderItemPhysical
                .Include(o => o.Game)
                .GroupBy(o => new { o.Game.Title })
                .Select(g => new
                {
                    g.Key.Title,
                    Quantity = g.Sum(o => o.Quantity),
                    Revenue = g.Sum(o => o.Price * o.Quantity)
                })
                .OrderByDescending(o => o.Revenue)
                .Take(5)
                .ToList();

            var physicalAggregate = new List<SaleAggregate>();

            foreach (var item in physicalQuery)
            {
                var physicalItem = new SaleAggregate();
                physicalItem.Title = item.Title;
                physicalItem.Quantity = item.Quantity;
                physicalItem.Revenue = item.Revenue;

                physicalAggregate.Add(physicalItem);
            }

            ViewData["DigitalSales"] = digitalAggregate;
            ViewData["PhysicalSales"] = physicalAggregate;


            return View();
        }
    }
}