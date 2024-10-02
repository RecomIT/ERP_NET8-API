using Microsoft.AspNetCore.Http;
using Shared.Helpers.ValidationFilters;
using System.ComponentModel.DataAnnotations;

namespace Shared.Payroll.DTO.Tax
{
    public class TaxChallanUploadDTO
    {
        [Range(1, 12)]
        public short TaxMonth { get; set; }
        [Range(2020, 2080)]
        public short TaxYear { get; set; }
        public long FiscalYearId { get; set; }
        [AllowedExtensions(new string[] { ".xls", ".xlsx" })]
        public IFormFile File { get; set; }
    }
}
