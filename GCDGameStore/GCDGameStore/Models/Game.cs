using System;
using System.Collections.Generic;

namespace GCDGameStore.Models
{
    public class Game
    {
        public int GameID { get; set; }
        public string Title { get; set; }
        public DateTime ReleaseDate { get; set; }
    }
}
