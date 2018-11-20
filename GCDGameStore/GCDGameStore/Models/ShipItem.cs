using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GCDGameStore.Models
{
    public class ShipItem
    {
        public int ShipItemId { get; set; }

        [Required]
        public int GameId { get; set; }
        public Game Game { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public int ShipmentId { get; set; }
        public Shipment Shipment { get; set; }
    }
}
