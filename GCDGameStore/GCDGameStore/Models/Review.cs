using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GCDGameStore.Models
{
    public class Review
    {
        public int ReviewId { get; set; }

        [Required]
        public bool Recommended { get; set; }

        [Required]
        public string ReviewText { get; set; }

        [Required]
        public bool Approved { get; set; }

        [Required]
        public int MemberId { get; set; }
        public Member Member { get; set; }

        [Required]
        public int GameId { get; set; }
        public Game Game { get; set; }
    }
}
