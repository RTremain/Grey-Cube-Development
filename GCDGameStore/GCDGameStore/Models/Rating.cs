using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GCDGameStore.Models
{
    public class Rating
    {
        public int RatingId { get; set; }

        [Required]
        [Range(1, 5)]
        public int RatingScore { get; set; }

        [Required]
        public int MemberId { get; set; }
        public Member Member { get; set; }

        [Required]
        public int GameId { get; set; }
        public Game Game { get; set; }
    }
}
