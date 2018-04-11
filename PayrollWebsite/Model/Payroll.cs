using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PayrollWebsite.Model
{
    public class Payroll
    {
        [Key]
        public int PayrollId { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime StopDate { get; set; }

        [ForeignKey("Employee")]
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
    }
}
