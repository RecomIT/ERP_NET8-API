using Microsoft.AspNetCore.Http;
using Shared.Helpers.ValidationFilters;
using System.ComponentModel.DataAnnotations;

namespace Shared.Payroll.DTO.Payment
{
    public class UploadProjectedPaymentDTO
    {
        [Range(1,long.MaxValue)]
        public long AllowanceNameId { get; set; }
        [Range(1, short.MaxValue)]
        public short PayableYear { get; set; }
        [Required,StringLength(150)]
        public string AllowanceReason { get; set; }
        [Required, AllowedExtensions(new string[] { ".xlsx" })]
        public IFormFile File { get; set; }
        public long? FiscalYearId { get; set; }

    }
}
