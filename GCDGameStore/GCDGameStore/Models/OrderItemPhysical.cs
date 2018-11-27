using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GCDGameStore.Models
{
    public class OrderItemPhysical
    {
        public int OrderItemPhysicalId { get; set; }

        [Required]
        public int GameId { get; set; }
        public Game Game { get; set; }

        [Required]
        public float Price { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public int OrderId { get; set; }
        public Order Order { get; set; }
    }
}
