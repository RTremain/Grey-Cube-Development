using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GCDGameStore.Models
{
    public class CreditCard
    {
        public int CreditCardId { get; set; }

        [Required]
        [CreditCard]
        public string CcNum { get; set; }

        [Range(1, 12)]
        public int ExpMonth { get; set; }

        // TODO: PROPER VALIDATION
        [Range(2018, 2028)]
        public int ExpYear { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string StreetAddress { get; set; }
        [Required]
        public string City { get; set; }

        [Required]
        public string Province { get; set; }

        [Required]
        public string PostalCode { get; set; }

        [Required]
        public int MemberId { get; set; }
        public Member Member { get; set; }
    }
}
