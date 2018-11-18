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
            HasReview = false;
            OnCart = false;

            if (game.AverageRating != default(float))
            {
                HasRating = true;
                AverageRating = (float)(Math.Round((double)game.AverageRating, 2));
            }
        }

        public int MemberGameDetailId { get; set; }

        public string Title { get; set; }

        public DateTime ReleaseDate { get; set; }

        public bool InLibrary { get; set; }
        public bool OnWishlist { get; set; }
        public bool HasRating { get; set; }
        public float AverageRating { get; set; }

        public bool HasReview { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }

        public bool OnCart { get; set; }
    }
}
