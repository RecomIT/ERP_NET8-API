using Microsoft.AspNetCore.Http;
using Shared.Helpers.ValidationFilters;
using System.ComponentModel.DataAnnotations;


namespace Shared.Payroll.DTO.Tax
{
    public class UploadActualTaxDeductionDTO
    {
        [Range(1,12)]
        public short SalaryMonth { get; set; }
        [Range(2020, 2080)]
        public short SalaryYear { get; set; }
        [AllowedExtensions(new string[] { ".xls", ".xlsx" })]
        public IFormFile File { get; set; }
        
    }
}
