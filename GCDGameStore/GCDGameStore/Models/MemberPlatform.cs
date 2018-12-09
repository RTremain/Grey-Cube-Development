using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GCDGameStore.Models
{
    public class MemberPlatform
    {
        public int MemberPlatformId { get; set; }

        public int PlatformId { get; set; }
        public Platform Platform { get; set; }

        public int MemberId { get; set; }
        public Member Member { get; set; }
    }
}
