﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GCDGameStore.Models
{
    public class Attendance
    {
        public int AttendanceId { get; set; }

        public int MemberId { get; set; }
        public Member Member { get; set; }

        public int EventId { get; set; }
        public Event Event { get; set; }
    }
}
