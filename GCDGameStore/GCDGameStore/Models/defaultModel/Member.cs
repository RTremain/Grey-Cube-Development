﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GCDGameStore.Models
{
    public partial class Member
    {
        public int MemberId { get; set; }

        public string Username { get; set; }

        public string PwHash { get; set; }

        public string PwSalt { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string MailingStreetAddress { get; set; }

        public string MailingPostalCode { get; set; }
        public string MailingCity { get; set; }
        public string MailingProvince { get; set; }

        public string ShippingStreetAddress { get; set; }

        public string ShippingPostalCode { get; set; }
        public string ShippingCity { get; set; }
        public string ShippingProvince { get; set; }

        public virtual ICollection<Friend> MyFriends { get; set; }
        public virtual ICollection<Friend> FriendsOf { get; set; }

    }
}
