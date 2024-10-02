using Microsoft.AspNetCore.Http;
using Shared.Helpers.ValidationFilters;
using System.ComponentModel.DataAnnotations;

namespace Shared.Payroll.DTO.Tax
{
    public class UploadTaxDocumentDTO
    {
        [Required(ErrorMessage = "Income Year is missing"),Range(1,long.MaxValue,ErrorMessage ="Income Year is missing")]
        public long FiscalYearId { get; set; }
        [StringLength(20), Required(ErrorMessage ="Missng AIT/Refund")]
        public string CertificateType { get; set; }
        [Required(ErrorMessage ="File is required"),AllowedExtensions(new string[] { ".xls", ".xlsx" },ErrorMessage ="File is invalid")]
        public IFormFile File { get; set; }
    }
}
