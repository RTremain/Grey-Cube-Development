using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GCDGameStore.Models
{
    public class Game
    {
        public int GameID { get; set; }

        [Required]
        public string Title { get; set; }

        public DateTime ReleaseDate { get; set; }
    }
}
