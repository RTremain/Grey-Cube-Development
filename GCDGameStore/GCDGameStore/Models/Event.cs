using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GCDGameStore.Models
{
    public class Event
    {
        public int EventId { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Title { get; set; }

        [Required]
        public DateTime EventDate { get; set; }

        [Required]
        [StringLength(255, MinimumLength = 5)]
        public string Description { get; set; }

        public virtual ICollection<Attendance> Attendees { get; set; }
    }
}
