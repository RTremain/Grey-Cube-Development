using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GCDGameStore.Models
{
    public class Friend
    {
        public int FriendId { get; set; }

        [Required]
        public int MemberId { get; set; }
        public Member Member { get; set; }

        [Required]
        public int FriendMemberId { get; set; }
        public Member FriendMember { get; set; }
    }

}
