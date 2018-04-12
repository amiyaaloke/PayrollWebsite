using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PayrollWebsite.Model
{
    public class PayrollContext: DbContext
    {
        public PayrollContext(DbContextOptions<PayrollContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Job>()
                .HasIndex(j => j.JobCode)
                .IsUnique(true);

            var t = modelBuilder.Entity<Employee>();
            t.HasKey(n => n.EmployeeId);
            t.Property(o => o.EmployeeId).ValueGeneratedNever();

        }
        public DbSet<Payroll> Payroll { get; set; }
        public DbSet<Job> Job { get; set; }
        public DbSet<Employee> Employee { get; set; }
        public DbSet<Timesheet> Timesheet { get; set; }
        public DbSet<Report> Report { get; set; }

        public IList<PayrollReport> GetPayrolls(DateTime startDate, DateTime endDate)
        {

            var timesheets = Timesheet
                .Where(t => t.Date >= startDate && t.Date <= endDate)
                .OrderBy(x => x.Date);

            // Get start date
            // start date chosen is less than earliest date for which timesheet is submitted then start date to earlies start date

            if(timesheets.Count()>0)
            {
                var sd = timesheets.First().Date;
                startDate = new DateTime(sd.Year, sd.Month, 1);

                var ed = timesheets.Last().Date;
                endDate = new DateTime(ed.Year, ed.Month, DateTime.DaysInMonth(ed.Year, ed.Month));
            }

            IList<PayrollReport> payrolls = new List<PayrollReport>();

            while (startDate < endDate)
            {
                var sd = startDate;
                var ed = startDate.Day == 1 ? sd.AddDays(14) : sd.AddDays(DateTime.DaysInMonth(sd.Year, sd.Month) - sd.Day);

                var result = Timesheet.Where(t => t.Date >= sd && t.Date <= ed);

                var payroll = Timesheet.Where(t => t.Date >= sd && t.Date <= ed)
                    .GroupBy(g => new { g.EmployeeId, g.Job.JobCode })
                    .Select(x => new PayrollReport
                    {
                        EmployeeId = x.Select(e=>e.EmployeeId).FirstOrDefault(),
                        StartDate = sd,
                        EndDate = ed,
                        Amount = x.Sum(y => y.HoursWorked * y.Job.HourlyRate)
                    });

                startDate = ed.AddDays(1);

                foreach (var item in payroll)
                {
                    payrolls.Add(item);
                }
            }


            var hours = from t in timesheets
                        group t by t.EmployeeId into employeeGroup
                        select new PayrollReport
                        {
                            EmployeeId = employeeGroup.Key,
                            Amount = employeeGroup.Sum(x => x.HoursWorked),
                        };

            return payrolls.OrderBy(x=>x.EmployeeId).ToList();
        }
    }
}
