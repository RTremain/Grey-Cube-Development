﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GCDGameStore.Models;
using GCDGameStore.ViewModels;
using Microsoft.Extensions.Logging;
using GCDGameStore.Classes;
using Microsoft.AspNetCore.Http;

namespace GCDGameStore.Controllers
{
    public class CartController : Controller
    {
        private readonly GcdGameStoreContext _context;
        private readonly ILogger _logger;
        private readonly LoginStatus _loginStatus;
        private readonly Cart _cart;

        public CartController(GcdGameStoreContext context, ILogger<MemberController> logger, IHttpContextAccessor accessor)
        {
            _context = context;
            _logger = logger;
            _loginStatus = new LoginStatus(accessor);
            _cart = new Cart(accessor, logger);
        }

        // GET: Cart
        public async Task<IActionResult> Index()
        {
            var cartList = _cart.GetCart();
            var cartViewList = new List<CartViewModel>();

            foreach (CartItem item in cartList)
            {
                var game = await _context.Game.Where(g => g.GameId == item.Id).FirstOrDefaultAsync();
                var newCartViewItem = new CartViewModel(item, game);

                cartViewList.Add(newCartViewItem);
            }

            return View(cartViewList);
        }


        [HttpPost, ActionName("AddItem")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddItem(string GameId)
        {
            if (GameId == null || GameId == "")
            {
                _logger.LogError("Error: {Message}", "null ID supplied to cart AddItem");
                return RedirectToAction("Index", "MemberGame");
            }

            var id = Convert.ToInt32(GameId);

            // make sure id exists as a game
            var gameCheck = await _context.Game.Where(g => g.GameId == id).SingleOrDefaultAsync();

            if (gameCheck != null)
            {
                if (!_cart.OnCart(id))
                {
                    _cart.AddItem(id, 1);
                }
                else
                {
                    _logger.LogError("Error: {Message}", "Item already on cart");
                }
            }
            else
            {
                return NotFound();
            }

            return RedirectToAction("Details", "MemberGame", new { id });
        }

        // POST: Cart/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            
            return RedirectToAction(nameof(Index));
        }

        
    }
}
