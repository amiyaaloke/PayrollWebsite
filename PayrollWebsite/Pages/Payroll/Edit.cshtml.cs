using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PayrollWebsite.Model;

namespace PayrollWebsite.Pages.Payroll
{
    public class EditModel : PageModel
    {
        private readonly PayrollWebsite.Model.PayrollContext _context;

        public EditModel(PayrollWebsite.Model.PayrollContext context)
        {
            _context = context;
        }

        [BindProperty]
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
           ViewData["EmployeeId"] = new SelectList(_context.Set<Employee>(), "EmployeeId", "EmployeeId");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Payroll).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PayrollExists(Payroll.PayrollId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool PayrollExists(int id)
        {
            return _context.Payroll.Any(e => e.PayrollId == id);
        }
    }
}
