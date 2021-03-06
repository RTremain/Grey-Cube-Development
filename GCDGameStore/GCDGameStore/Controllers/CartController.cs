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
            if (_loginStatus.IsNotLoggedIn())
            {
                _logger.LogInformation("Redirect: {Message}", "Redirecting to login");
                return RedirectToAction("Login", "Member");
            }

            var cartList = _cart.GetCart();
            var cartItemList = new List<CartViewModel>();

            foreach (CartItem item in cartList)
            {
                var game = await _context.Game.Where(g => g.GameId == item.Id).FirstOrDefaultAsync();
                var newCartViewItem = new CartViewModel(item, game);

                cartItemList.Add(newCartViewItem);
            }

            return View(cartItemList);
        }

        public async Task<IActionResult> Checkout()
        {
            if (_loginStatus.IsNotLoggedIn())
            {
                _logger.LogInformation("Redirect: {Message}", "Redirecting to login");
                return RedirectToAction("Login", "Member");
            }

            if (_cart.GetCart().Count() == 0)
            {
                _logger.LogError("Error: Cannot checkout empty cart");
                _logger.LogInformation("Redirect: {Message}", "Redirecting to cart Index");
                return RedirectToAction(nameof(Index));
            }

            var cartList = _cart.GetCart();
            var cartItemList = new List<CartViewModel>();

            foreach (CartItem item in cartList)
            {
                var game = await _context.Game.Where(g => g.GameId == item.Id).FirstOrDefaultAsync();
                var newCartViewItem = new CartViewModel(item, game);

                cartItemList.Add(newCartViewItem);
            }

            return View(cartItemList);
        }


        [HttpPost, ActionName("AddItem")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddItem(string GameId)
        {
            if (_loginStatus.IsNotLoggedIn())
            {
                _logger.LogInformation("Redirect: {Message}", "Redirecting to login");
                return RedirectToAction("Login", "Member");
            }

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
                    _cart.AddItem(id, 1, false);
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

        [HttpPost, ActionName("AddPhysicalItem")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPhysicalItem(string GameId)
        {
            if (_loginStatus.IsNotLoggedIn())
            {
                _logger.LogInformation("Redirect: {Message}", "Redirecting to login");
                return RedirectToAction("Login", "Member");
            }

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
                if (gameCheck.PhysicalAvailable)
                {
                    if (!_cart.OnCart(id))
                    {
                        _cart.AddItem(id, 1, true);
                    }
                    else
                    {
                        _logger.LogError("Error: {Message}", "Item already on cart");
                    }
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

        [HttpPost, ActionName("DeleteItemFromGameDetails")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteItemFromGameDetails(string GameId)
        {
            if (_loginStatus.IsNotLoggedIn())
            {
                _logger.LogInformation("Redirect: {Message}", "Redirecting to login");
                return RedirectToAction("Login", "Member");
            }

            if (GameId == null || GameId == "")
            {
                _logger.LogError("Error: {Message}", "null ID supplied to cart DeleteItem");
                return RedirectToAction(nameof(Index));
            }

            var id = Convert.ToInt32(GameId);

            // make sure id exists as a game
            var gameCheck = await _context.Game.Where(g => g.GameId == id).SingleOrDefaultAsync();

            if (gameCheck != null)
            {
                if (_cart.OnCart(id))
                {
                    _cart.RemoveItem(id);
                }
                else
                {
                    _logger.LogError("Error: {Message}", "Game not on cart");
                }
            }
            else
            {
                return NotFound();
            }

            return RedirectToAction("Details", "MemberGame", new { id } );
        }

        [HttpPost, ActionName("DeleteItem")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteItem(string GameId)
        {
            if (_loginStatus.IsNotLoggedIn())
            {
                _logger.LogInformation("Redirect: {Message}", "Redirecting to login");
                return RedirectToAction("Login", "Member");
            }

            if (GameId == null || GameId == "")
            {
                _logger.LogError("Error: {Message}", "null ID supplied to cart DeleteItem");
                return RedirectToAction(nameof(Index));
            }

            var id = Convert.ToInt32(GameId);

            // make sure id exists as a game
            var gameCheck = await _context.Game.Where(g => g.GameId == id).SingleOrDefaultAsync();

            if (gameCheck != null)
            {
                if (_cart.OnCart(id))
                {
                    _cart.RemoveItem(id);
                }
                else
                {
                    _logger.LogError("Error: {Message}", "Game not on cart");
                }
            }
            else
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }

        
        [HttpPost, ActionName("UpdateCart")]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateCart(List<CartViewModel> cartViewModels)
        {
            if (_loginStatus.IsNotLoggedIn())
            {
                _logger.LogInformation("Redirect: {Message}", "Redirecting to login");
                return RedirectToAction("Login", "Member");
            }

            var updatedList = new List<CartItem>();
            foreach (CartViewModel item in cartViewModels)
            {
                var newCartItem = new CartItem(item.GameId, item.Quantity, item.IsPhysical);
                updatedList.Add(newCartItem);
            }

            _cart.UpdateCart(updatedList);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost, ActionName("SubmitOrder")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitOrder(List<CartViewModel> cartViewModels)
        {
            if (_loginStatus.IsNotLoggedIn())
            {
                _logger.LogInformation("Redirect: {Message}", "Redirecting to login");
                return RedirectToAction("Login", "Member");
            }

            if (cartViewModels == null)
            {
                _logger.LogError("Error: {Message}", "null cart given to SubmitOrder");
                return NotFound();
            }

            var newShipment = new Shipment()
            {
                OrderDate = DateTime.Now,
                MemberId = _loginStatus.GetMemberId(),
                IsShipped = false,
                IsProcessing = false,
                ShipItems = new List<ShipItem>()
            };

            var newOrder = new Order()
            {
                OrderDate = DateTime.Now,
                MemberId = _loginStatus.GetMemberId(),
                DigitalItems = new List<OrderItemDigital>(),
                PhysicalItems = new List<OrderItemPhysical>()
            };

            foreach (CartViewModel item in cartViewModels)
            {
                var newCartItem = new CartItem(item.GameId, item.Quantity, item.IsPhysical);

                var gameId = item.GameId;

                if (await _context.Game.FindAsync(gameId) == null)
                {
                    return NotFound();
                }

                if (item.IsPhysical)
                {
                    var newShipItem = new ShipItem()
                    {
                        GameId = item.GameId,
                        Quantity = item.Quantity,
                        Shipment = newShipment
                    };
                    newShipment.ShipItems.Add(newShipItem);

                    var newOrderItemPhysical = new OrderItemPhysical()
                    {
                        GameId = item.GameId,
                        Price = item.Price,
                        Quantity = item.Quantity,
                        Order = newOrder
                    };

                    newOrder.PhysicalItems.Add(newOrderItemPhysical);
                }
                else
                {
                    var newOrderItemDigital = new OrderItemDigital()
                    {
                        GameId = item.GameId,
                        Price = item.Price,
                        Order = newOrder
                    };

                    newOrder.DigitalItems.Add(newOrderItemDigital);
                }
                


                var LibraryItem = new Library { MemberId = _loginStatus.GetMemberId(), GameId = gameId };

                Wishlist wishlistCheck = null;

                try
                {
                    wishlistCheck = await _context.Wishlist.Where(w => w.GameId == gameId)
                                                           .Where(w => w.MemberId == _loginStatus.GetMemberId())
                                                           .SingleOrDefaultAsync();
                }
                catch (Exception e)
                {
                    _logger.LogError("Error retrieving single wishlist: {Message}", e.Message);
                }

                if (wishlistCheck != null)
                {
                    _context.Wishlist.Remove(wishlistCheck);
                }

                _context.Add(LibraryItem);
            }

            _cart.ClearCart();
            if (newShipment.ShipItems.Count > 0)
            {
                _context.Shipment.Add(newShipment);
            }
            _context.Order.Add(newOrder);
            await _context.SaveChangesAsync();
            return RedirectToAction("Library", "Member");
        }

    }
}
