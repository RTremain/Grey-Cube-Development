using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GCDGameStore.Models
{
    public class Order
    {
        public int OrderId { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }

        [Required]
        public int MemberId { get; set; }
        public Member Member { get; set; }

        public virtual List<OrderItemDigital> DigitalItems { get; set; }
        public virtual List<OrderItemPhysical> PhysicalItems { get; set; }
    }
}
