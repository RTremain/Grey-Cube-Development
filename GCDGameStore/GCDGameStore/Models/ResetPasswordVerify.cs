using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GCDGameStore.Models
{
    public class ResetPasswordVerify
    {
        public int ResetPasswordVerifyId { get; set; }

        public int MemberId { get; set; }
        public Member Member { get; set; }
        public string VerificationHash { get; set; }
    }
}
