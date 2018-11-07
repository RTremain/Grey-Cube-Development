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

        [Required]
        public int MemberId { get; set; }
        public Member Member { get; set; }

        [Required]
        public int GameId { get; set; }
        public Game Game { get; set; }
    }
}
