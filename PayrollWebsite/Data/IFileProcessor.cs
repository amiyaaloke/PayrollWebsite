using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PayrollWebsite.Data
{
    interface IFileProcessor
    {
        void ProcessFile(IFormFile file, DbContext context, out int reportId, out string errorString);
    }
}
