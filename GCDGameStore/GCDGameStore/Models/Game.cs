using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GCDGameStore.Models
{
    public class Game
    {
        public int GameId { get; set; }

        [Required]
        public string Title { get; set; }

        public DateTime ReleaseDate { get; set; }

        public float AverageRating { get; set; }

        [Required]
        public float DigitalPrice { get; set; }

        public bool PhysicalAvailable { get; set; }
        public float? PhysicalPrice { get; set; }
    }
}
