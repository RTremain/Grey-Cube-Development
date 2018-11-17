using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GCDGameStore.Models;

namespace GCDGameStore.ViewModels
{
    public class MemberGameDetail
    {
        public MemberGameDetail () { }

        public MemberGameDetail (Game game)
        {
            MemberGameDetailId = game.GameId;
            Title = game.Title;
            ReleaseDate = game.ReleaseDate;

            InLibrary = false;
            OnWishlist = false;
            HasRating = false;
            HasReview = false;
            OnCart = false;
        }

        public int MemberGameDetailId { get; set; }

        public string Title { get; set; }

        public DateTime ReleaseDate { get; set; }

        public bool InLibrary { get; set; }
        public bool OnWishlist { get; set; }
        public bool HasRating { get; set; }
        public bool HasReview { get; set; }
        public bool OnCart { get; set; }
    }
}
