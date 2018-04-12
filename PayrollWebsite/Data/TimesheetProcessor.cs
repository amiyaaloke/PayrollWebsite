using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PayrollWebsite.Model;
using PayrollWebsite.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace PayrollWebsite.Data
{
    public class TimesheetFileProcessor: IFileProcessor
    {
        private const string COL_EMPID = "employee id";
        private const string COL_DATE = "date";
        private const string COL_HOURSWORKED = "hours worked";
        private const string COL_JOBCODE = "job group";

        public void ProcessFile(IFormFile timesheet, DbContext context, out int reportId, out string errorString)
        {
            DataTable dt = FileHelpers.Csv2Table(timesheet, ",");

            PayrollContext payrollContext = context as PayrollContext;

            // Validate report with report id
            errorString = ExtractAndValidateReportId(dt, payrollContext, out reportId);

            if(!string.IsNullOrEmpty(errorString))
            {
                return;
            }

            AddNewEmployees(dt, payrollContext);

            SaveTimesheet(dt, payrollContext, reportId);

            return;
        }

        private string ExtractAndValidateReportId(DataTable dt, PayrollContext context, out int reportId)
        {
            string errorString = string.Empty;

            // Extract report id
            reportId = int.Parse(dt.Rows[dt.Rows.Count - 1][COL_HOURSWORKED].ToString());
            dt.Rows.RemoveAt(dt.Rows.Count - 1);
            dt.AcceptChanges();

            int id = reportId;
            var report = context.Report.FirstOrDefault(r => r.ReportId == id);
            if (report == null)
            {
                context.Report.Add(new Report { ReportId = id });
                context.SaveChanges();
            }
            else
            {
                errorString = string.Format("Report with ID: {0} already exists in database.", reportId);
            }

            return errorString;
        }

        private void AddNewEmployees(DataTable dt, PayrollContext context)
        {
            DataView dv = new DataView(dt);
            dv.Sort = COL_EMPID;
            DataTable distinctEmployees = dv.ToTable(true, COL_EMPID);

            context.Database.EnsureCreated();
            var employeedToAdd = new List<Employee>();

            foreach (DataRow dr in distinctEmployees.Rows)
            {
                var employeeId = int.Parse(dr[COL_EMPID].ToString());

                if (context.Employee.FirstOrDefault(e => e.EmployeeId == employeeId) == null)
                {
                    employeedToAdd.Add(new Employee { EmployeeId = employeeId });
                }
            }

            if (employeedToAdd.Count > 0)
            {
                context.Employee.AddRange(employeedToAdd.ToArray());
                context.SaveChanges();
            }
        }

        private void SaveTimesheet(DataTable dt, PayrollContext context, int reportId)
        {
            foreach (DataRow dr in dt.Rows)
            {
                var date = DateTime.ParseExact(dr[COL_DATE].ToString(), "d/m/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                var employeeId = int.Parse(dr[COL_EMPID].ToString());
                var hoursWorked = decimal.Parse(dr[COL_HOURSWORKED].ToString());
                var jobCode = dr[COL_JOBCODE].ToString();
                var jobId = context.Job.FirstOrDefault(j => j.JobCode == jobCode).JobId;
                context.Timesheet.Add(new Timesheet { Date = date, EmployeeId = employeeId, HoursWorked = hoursWorked, ReportId = reportId, JobId = jobId });
            }

            context.SaveChanges();
        }
    }
}
