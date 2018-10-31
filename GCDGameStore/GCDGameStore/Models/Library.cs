using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace GCDGameStore.Models
{
    public class Library
    {
        public int LibraryId { get; set; }


        public int MemberId { get; set; }
        public int GameId { get; set; }
        public Game Game { get; set; }
    }
}
