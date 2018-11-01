using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GCDGameStore.Models;

namespace GCDGameStore.ViewModels
{
    public class AttendingEvent
    {
        public AttendingEvent (Event e)
        {
            AttendingEventId = e.EventId;
            Title = e.Title;
            EventDate = e.EventDate;
            Description = e.Description;
            Registered = false;
        }

        /// <summary>
        ///     We need this parameterless constructor for certain things to play nice
        /// </summary>
        public AttendingEvent() { }

        public int AttendingEventId { get; set; }

        public string Title { get; set; }

        public DateTime EventDate { get; set; }

        public string Description { get; set; }

        public bool Registered { get; set; }
    }
}
