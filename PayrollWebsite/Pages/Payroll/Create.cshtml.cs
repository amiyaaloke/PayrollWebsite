using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using PayrollWebsite.Model;

namespace PayrollWebsite.Pages.Payroll
{
    public class CreateModel : PageModel
    {
        private readonly PayrollWebsite.Model.PayrollContext _context;

        public CreateModel(PayrollWebsite.Model.PayrollContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["EmployeeId"] = new SelectList(_context.Set<Employee>(), "EmployeeId", "EmployeeId");
            return Page();
        }

        [BindProperty]
        public PayrollWebsite.Model.Payroll Payroll { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Payroll.Add(Payroll);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}