using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PayrollWebsite.Model
{
    public class Job
    {
        [Key]
        public int JobId { get; set; }

        public string JobCode { get; set; }
        public Decimal HourlyRate { get; set; }

        //public ICollection<Employee> Employees { get; set; }
    }
}
