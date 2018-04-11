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
                //.HasKey(j => new { j.JobId, j.JobCode });
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

        public void AddEmployees(params Employee[] employees)
        {

        }
    }
}
