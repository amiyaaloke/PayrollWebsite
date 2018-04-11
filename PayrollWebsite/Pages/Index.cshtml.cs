using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PayrollWebsite.Model;
using PayrollWebsite.Utilities;

namespace PayrollWebsite.Pages
{
    public class IndexModel : PageModel
    {
        private readonly PayrollContext _context;

        public IndexModel(PayrollContext context)
        {
            _context = context;
        }

        [BindProperty]
        public FileUpload FileUpload { get; set; }

        [BindProperty]
        public IList<PayrollReport> PayrollReport { get; private set; }

        [BindProperty]
        public string ErrorMessage { get; set; }

        private const string VIEWDATA_SHOW_REPORT = "Show Report";
        private const string VIEWDATA_ERROR_MESSAGE = "Error Message";

        public void OnGet()
        {
            PayrollReport = new List<PayrollReport>().ToArray();
            ErrorMessage = string.Empty;
            ViewData[VIEWDATA_SHOW_REPORT] = "hidden";
            ViewData[VIEWDATA_ERROR_MESSAGE] = ErrorMessage;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var data = await FileHelpers.ProcessFormFile(FileUpload.Timesheet, ModelState);

            var fileName = WebUtility.HtmlEncode(Path.GetFileName(FileUpload.Timesheet.FileName));

            DataTable dt = FileHelpers.CsvDb(FileUpload.Timesheet, ",");

            const string COL_EMPID = "employee id";
            const string COL_DATE = "date";
            const string COL_HOURSWORKED = "hours worked";
            const string COL_JOBCODE = "job group";

            var reportId = int.Parse(dt.Rows[dt.Rows.Count - 1][COL_HOURSWORKED].ToString());
            dt.Rows.RemoveAt(dt.Rows.Count - 1);
            dt.AcceptChanges();

            var report = _context.Report.FirstOrDefault(r => r.ReportId == reportId);
            if (report == null)
            {
                _context.Report.Add(new Report { ReportId = reportId });
                _context.SaveChanges();
            }
            else
            {
                ErrorMessage = string.Format("Report with ID: {0} already exists in database.", reportId);
                ViewData["Error Message"] = ErrorMessage;
            }

            DataView dv = new DataView(dt);
            dv.Sort = COL_EMPID;
            DataTable distinctEmployees = dv.ToTable(true, COL_EMPID);

            _context.Database.EnsureCreated();
            var employeedToAdd = new List<Employee>();

            foreach (DataRow dr in distinctEmployees.Rows)
            {
                var employeeId = int.Parse(dr[COL_EMPID].ToString());

                if (_context.Employee.FirstOrDefault(e => e.EmployeeId == employeeId) == null)
                {
                    employeedToAdd.Add(new Employee { EmployeeId = employeeId });
                }
            }

            if (employeedToAdd.Count > 0)
            {
                _context.Employee.AddRange(employeedToAdd.ToArray());
                _context.SaveChanges();
            }

            foreach (DataRow dr in dt.Rows)
            {
                var date = DateTime.ParseExact(dr[COL_DATE].ToString(), "d/m/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                var employeeId = int.Parse(dr[COL_EMPID].ToString());
                var hoursWorked = decimal.Parse(dr[COL_HOURSWORKED].ToString());
                var jobCode = dr[COL_JOBCODE].ToString();
                var jobId = _context.Job.FirstOrDefault(j => j.JobCode == jobCode).JobId;
                _context.Timesheet.Add(new Timesheet { Date = date, EmployeeId = employeeId, HoursWorked = hoursWorked, ReportId = reportId, JobId = jobId });
            }

            _context.SaveChanges();


            PayrollReport = new PayrollReport[]
            {
                new PayrollReport{EmployeeId=1, StartDate=DateTime.Parse("1/1/2011"), EndDate=DateTime.Parse("1/15,2011"), Amount=100},
                new PayrollReport{EmployeeId=1, StartDate=DateTime.Parse("1/16/2011"), EndDate=DateTime.Parse("1/31,2011"), Amount=100}
            };
            
            if(PayrollReport.Count > 0)
            {
                ViewData[VIEWDATA_SHOW_REPORT] = "";
            }

            //return new OkObjectResult(data);
            return RedirectToPage("./Index"); ;
        }
    }
}