using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PayrollWebsite.Model
{
    public class FileUpload
    {
        [Display(Name = "File Name")]
        public string FileName { get; set; }

        [Required]
        [Display(Name = "Timesheet File")]
        public IFormFile Timesheet { get; set; }
    }
}
