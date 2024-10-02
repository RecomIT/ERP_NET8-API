using System;
using Microsoft.AspNetCore.Http;
using Shared.Helpers.ValidationFilters;
using System.ComponentModel.DataAnnotations;

namespace Shared.Payroll.ViewModel.Salary
{
    public class SalaryDeductionUpload
    {
        [Range(1, long.MaxValue), Required]
        public long DeductionNameId { get; set; }
        [AllowedExtensions(new string[] { ".xls", ".xlsx" })]
        public IFormFile UploadedFile { get; set; }
        [Range(1, 12), Required]
        public short SalaryMonth { get; set; }
        [Range(2020, 2050), Required]
        public short SalaryYear { get; set; }
    }
}
