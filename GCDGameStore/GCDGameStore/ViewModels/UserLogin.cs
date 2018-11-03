using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace GCDGameStore.ViewModels
{
    public class UserLogin
    {
        public int UserLoginId { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Password{ get; set; }
    }
}
