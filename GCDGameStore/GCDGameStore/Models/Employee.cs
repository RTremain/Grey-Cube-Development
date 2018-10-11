using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GCDGameStore.Models
{
    public partial class Employee
    {
        public int EmployeeId { get; set; }

        [Required]
        [MinLength(3)]
        public string Name { get; set; }

        [Required]
        public string PwHash { get; set; }

        [Required]
        public string PwSalt { get; set; }
    }
}
