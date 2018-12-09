using GCDGameStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GCDGameStore.ViewModels
{
    public class MemberPlatformViewModel
    {
        public MemberPlatformViewModel (Platform p)
        {
            MemberPlatformViewModelId = p.PlatformId;
            Name = p.Name;
            Added = false;
        }

        /// <summary>
        /// Parameterless constructor is needed for certain things to play nice.
        /// </summary>
        public MemberPlatformViewModel() { }

        public int MemberPlatformViewModelId { get; set; }
        public string Name { get; set; }
        public bool Added { get; set; }

    }
}
