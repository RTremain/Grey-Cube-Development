using GCDGameStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GCDGameStore.ViewModels
{
    public class MemberLibrary
    {
        public MemberLibrary() { }

        public MemberLibrary(Library library)
        {
            MemberId = library.MemberId;
            GameId = library.GameId;
            Game = library.Game;
            HasReview = false;
        }

        public int MemberLibraryId { get; set; }
        public int MemberId { get; set; }
        public int GameId { get; set; }
        public Game Game { get; set; }

        public bool HasReview { get; set; }
        public int ReviewId { get; set; }
    }
}
