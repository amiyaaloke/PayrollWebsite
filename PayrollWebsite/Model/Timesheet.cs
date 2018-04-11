using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PayrollWebsite.Model
{
    public class Timesheet
    {
        [Key]
        public int TimesheetId { get; set; }

        [ForeignKey("Report")]
        public int ReportId { get; set; }
        public Report Report { get; set; }

        public DateTime Date { get; set; }
        public Decimal HoursWorked { get; set; }

        [ForeignKey("Employee")]
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }

        [ForeignKey("Job")]
        public int JobId { get; set; }
        public Job Job { get; set; }

    }
}
