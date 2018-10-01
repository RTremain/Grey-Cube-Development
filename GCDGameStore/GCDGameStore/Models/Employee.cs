using System;
using System.Collections.Generic;

namespace GCDGameStore.Models
{
    public partial class Employee
    {
        public int EmployeeId { get; set; }
        public string Name { get; set; }
        public string PwHash { get; set; }
    }
}
