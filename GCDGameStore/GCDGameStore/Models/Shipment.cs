using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GCDGameStore.Models
{
    public class Shipment
    {
        public int ShipmentId { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }

        public DateTime? ShippedDate { get; set; }

        [Required]
        public int MemberId { get; set; }
        public Member Member { get; set; }

        [Required]
        public bool IsShipped { get; set; }
        [Required]
        public bool IsProcessing { get; set; }

        public int? EmployeeId { get; set; }
        public Employee Employee { get; set; }

        public virtual List<ShipItem> ShipItems { get; set; }
    }
}
