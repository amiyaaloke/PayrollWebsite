using PayrollWebsite.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PayrollWebsite.Data
{
    public static class DbInitializer
    {
        public static void Initialize(PayrollContext context)
        {
            context.Database.EnsureCreated();

            if(context.Payroll.Any())
            {
                return;
            }

            if (!context.Job.Any())
            {
                var jobs = new Job[]
                {
                new Job{JobCode="A", HourlyRate=20},
                new Job{JobCode="B", HourlyRate=30}
                };

                context.Job.AddRange(jobs);
                context.SaveChanges();
            }
        }
    }
}
