using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using GCDGameStore.Classes;
using GCDGameStore.Models;

namespace GCDGameStore.ViewModels
{
    public class CartViewModel
    {
        public CartViewModel() { }

        public CartViewModel(CartItem cartItem, Game game)
        {
            Quantity = cartItem.Quantity;
            GameId = cartItem.Id;
            Game = game;
        }

        public int CartViewModelId { get; set; }

        [Required]
        [Range(1, 9999)]
        public int Quantity { get; set; }

        public int GameId { get; set; }
        public Game Game { get; set; }


    }
}
