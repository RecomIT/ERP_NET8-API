using Microsoft.AspNetCore.Http;
using Shared.Helpers.ValidationFilters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Payroll.ViewModel.CashSalary
{
    public class UploadCashSalaryAmountViewModel
    {
        [Range(1, long.MaxValue), Required]
        public long CashSalaryHeadId { get; set; }
        [AllowedExtensions(new string[] { ".xls", ".xlsx" })]
        public IFormFile ExcelFile { get; set; }

        [Range(1, 12), Required]
        public short SalaryMonth { get; set; }
        [Range(2020, 2050), Required]
        public short SalaryYear { get; set; }
    }
}
