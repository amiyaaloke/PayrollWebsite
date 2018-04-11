using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PayrollWebsite.Model
{
    public class Employee
    {
        [Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int EmployeeId { get; set; }

        public string LastName { get; set; }
        public string FirstName { get; set; }

        public ICollection<Timesheet> Timesheets { get; set; }
        public ICollection<Payroll> Payrolls { get; set; }
    }
}
