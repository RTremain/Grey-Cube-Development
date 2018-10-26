using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace GCDGameStore.Models
{
    public class Wishlist
    {
        public int WishlistId { get; set; }

        [Required]
        [ForeignKey("Member")]
        public int MemberId { get; set; }

        [Required]
        public int GameId { get; set; }
        public Game Game { get; set; }
    }
}
