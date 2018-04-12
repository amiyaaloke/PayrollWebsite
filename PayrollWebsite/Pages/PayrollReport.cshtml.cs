using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PayrollWebsite.Model;

namespace PayrollWebsite.Pages
{
    public class PayrollReportModel : PageModel
    {
        private readonly PayrollContext _context;

        [BindProperty]
        public IList<PayrollReport> PayrollReport { get; private set; }

        [BindProperty]
        [DisplayName("Start Date")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [BindProperty]
        [DisplayName("End Date")]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        public PayrollReportModel(PayrollContext context)
        {
            _context = context;
            PayrollReport = new List<PayrollReport>();
        }

        public void OnGet()
        {
            EndDate = DateTime.Now;
            StartDate = DateTime.Now.AddDays(-30);
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            PayrollReport = _context.GetPayrolls(StartDate.Date, EndDate.Date);

            return Page();
        }
    }
}