using Microsoft.AspNetCore.Http;
using Shared.Helpers.ValidationFilters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Payroll.ViewModel.Salary
{
    public class UploadSalaryReviewViewModel
    {
        [AllowedExtensions(new string[] { ".xls", ".xlsx" })]
        public IFormFile ExcelFile { get; set; }
    }
}
