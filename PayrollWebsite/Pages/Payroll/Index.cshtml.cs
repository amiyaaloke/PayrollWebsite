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
    public class IndexModel : PageModel
    {
        private readonly PayrollWebsite.Model.PayrollContext _context;

        public IndexModel(PayrollWebsite.Model.PayrollContext context)
        {
            _context = context;
        }

        public IList<PayrollWebsite.Model.Payroll> Payroll { get;set; }

        public async Task OnGetAsync()
        {
            Payroll = await _context.Payroll
                .Include(p => p.Employee).ToListAsync();
        }
    }
}
