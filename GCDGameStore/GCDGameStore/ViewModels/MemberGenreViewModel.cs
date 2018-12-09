using GCDGameStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GCDGameStore.ViewModels
{
    public class MemberGenreViewModel
    {
        public MemberGenreViewModel(Genre g)
        {
            MemberGenreViewModelId = g.GenreId;
            Name = g.Name;
            Added = false;
        }

        /// <summary>
        /// Parameterless constructor is needed for certain things to play nice.
        /// </summary>
        public MemberGenreViewModel() { }

        public int MemberGenreViewModelId { get; set; }
        public string Name { get; set; }
        public bool Added { get; set; }
    }
}
