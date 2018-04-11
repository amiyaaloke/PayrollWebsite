using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace PayrollWebsite.Model
{
    public class PayrollReport
    {
        [DisplayName("Employee ID")]
        public int EmployeeId { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        [DisplayName("Pay Period")]
        public string PayPeriod
        {
            get { return StartDate.Date.ToString("d/m/yyyy") + " - " + EndDate.Date.ToString("d/m/yyyy"); }

            private set { }
        }

        [DisplayName("Amount")]
        public decimal Amount { get; set; }
    }
}
