using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GCDGameStore.ViewModels
{
    public class SaleAggregate
    {
        public int SaleAggregateId { get; set; }
        public String Title { get; set; }
        public int Quantity { get; set; }
        public float Revenue { get; set; }
    }
}
