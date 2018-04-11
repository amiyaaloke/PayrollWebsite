using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PayrollWebsite.Model;

namespace PayrollWebsite.Pages.Payroll
{
    public class DetailsModel : PageModel
    {
        private readonly PayrollWebsite.Model.PayrollContext _context;

        public DetailsModel(PayrollWebsite.Model.PayrollContext context)
        {
            _context = context;
        }

        public PayrollWebsite.Model.Payroll Payroll { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Payroll = await _context.Payroll
                .Include(p => p.Employee).SingleOrDefaultAsync(m => m.PayrollId == id);

            if (Payroll == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
