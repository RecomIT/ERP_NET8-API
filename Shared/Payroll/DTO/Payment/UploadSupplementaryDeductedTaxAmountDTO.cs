using Microsoft.AspNetCore.Http;
using Shared.Helpers.ValidationFilters;
using System.ComponentModel.DataAnnotations;

namespace Shared.Payroll.DTO.Payment
{
    public class UploadSupplementaryDeductedTaxAmountDTO
    {
        [Range(1,long.MaxValue)]
        public long ProcessId { get; set; }
        [Required, AllowedExtensions(new string[] { ".xlsx" })]
        public IFormFile File { get; set; }
    }
}
