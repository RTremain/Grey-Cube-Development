using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GCDGameStore.Models
{
    public class MemberGenre
    {
        public int MemberGenreId { get; set; }

        public int GenreId { get; set; }
        public Genre Genre { get; set; }

        public int MemberId { get; set; }
        public Member Member { get; set; }
    }
}
