using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GCDGameStore.Models
{
    public class Attendance
    {
        public int AttendanceId { get; set; }

        [Required]
        public int MemberId { get; set; }
        public Member Member { get; set; }

        [Required]
        public int EventId { get; set; }
        public Event Event { get; set; }
    }
}
