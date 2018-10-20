using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GCDGameStore.Models
{
    public class Member
    {
        public int MemberId { get; set; }

        [Required]
        [MinLength(5)]
        [StringLength(15)]
        public string Username { get; set; }

        [Required]
        public string PwHash { get; set; }

        public string PwSalt { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [Phone]
        public string Phone { get; set; }

        public string MailingStreetAddress { get; set; }

        [DataType(DataType.PostalCode)]
        public string MailingPostalCode { get; set; }
        public string MailingCity { get; set; }
        public string MailingProvince { get; set; }

        public string ShippingStreetAddress { get; set; }

        [DataType(DataType.PostalCode)]
        public string ShippingPostalCode { get; set; }
        public string ShippingCity { get; set; }
        public string ShippingProvince { get; set; }

    }
    
}
