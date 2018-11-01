using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GCDGameStore.Models;

namespace GCDGameStore.ViewModels
{
    public class AttendingEvent
    {
        public int EventId { get; set; }

        public string Title { get; set; }

        public DateTime EventDate { get; set; }

        public string Description { get; set; }

        public virtual ICollection<Attendance> Attendees { get; set; }
    }
}
