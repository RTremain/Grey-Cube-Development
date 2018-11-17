using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GCDGameStore.Classes
{

    public class Cart
    {
        private List<CartItem> cartList;

        private readonly IHttpContextAccessor _accessor;
        private readonly ILogger _logger;


        private int count;

        public int Count
        {
            get { return count; }
        }

        public Cart(IHttpContextAccessor accessor, ILogger logger)
        {
            _accessor = accessor;
            _logger = logger;

            // populate our cartList from the session
            GetCartSession();

            // update count in the session
            SetSessionCount();
        }

        private void SetSessionCount()
        {
            if (cartList != null)
            {
                count = cartList.Count();
            }
            else
            {
                count = 0;
            }

            _accessor.HttpContext.Session.SetString("cartCount", Count.ToString());
        }


        /// <summary>
        ///     Updates session with current cart and with current count of items in cart
        ///     to session keys "cart" and "cartCount"
        /// </summary>
        private void SetCartSession()
        {
            var httpContext = _accessor.HttpContext;
            httpContext.Session.SetString("cart", JsonConvert.SerializeObject(cartList));
            SetSessionCount();
        }

        private void GetCartSession()
        {
            var httpContext = _accessor.HttpContext;
            var sessionValue = httpContext.Session.GetString("cart");

            if (sessionValue == "" || sessionValue == null)
            {
                cartList = new List<CartItem>();
                count = 0;
            }
            else
            {
                cartList = JsonConvert.DeserializeObject<List<CartItem>>(sessionValue);
                count = cartList.Count();
            }
        }

        public List<CartItem> GetCart()
        {
            // return copy of list
            return cartList.ToList();
        }

        public bool OnCart(int id)
        {
            return cartList.Any(c => c.Id == id);
        }

        public void AddItem(int id, int quantity)
        {
            var cartItem = new CartItem(id, quantity);

            cartList.Add(cartItem);

            // update session
            SetCartSession();
        }

        public void UpdateCart(List<CartItem> newCart)
        {
            // assign copy of supplied list
            cartList = newCart.ToList();
            SetCartSession();
        }

        public void RemoveItem(int id)
        {
            var cartItem = cartList.Find(c => c.Id == id);

            if (cartList.Any(c => c.Id == id))
            {
                cartList.Remove(cartItem);
            }
            else
            {
                _logger.LogError("Error: {Message}", "Cart Item requested for deletion not found.");
            }

            SetCartSession();
        }

    }

    public struct CartItem
    {
        public CartItem(int id, int quantity)
        {
            Id = id;
            Quantity = quantity;
        }

        public int Id { get; set; }

        public int Quantity { get; set; }
    }
}
