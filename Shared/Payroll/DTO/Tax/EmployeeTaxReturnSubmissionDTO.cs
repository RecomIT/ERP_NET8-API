using Microsoft.AspNetCore.Http;
using Shared.Helpers.ValidationFilters;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Payroll.DTO.Tax
{
    public class EmployeeTaxReturnSubmissionDTO
    {
        public long TaxSubmissionId { get; set; }

        [Required]
        public long EmployeeId { get; set; }

        [Required]
        public long FiscalYearId { get; set; }
        [Required, StringLength(100)]
        public string RegistrationNo { get; set; }
        [Required, StringLength(100)]
        public string TaxZone { get; set; }
        [Required, StringLength(100)]
        public string TaxCircle { get; set; }
        [Required, Column(TypeName = "decimal(18,2)")]
        public decimal PaidAmount { get; set; }
        [Required, Column(TypeName = "decimal(18,2)")]
        public decimal TaxPayable { get; set; }
        public string SubmissionDate { get; set; }

        [RequiredIfValue("TaxSubmissionId", new string[] { "0" }), AllowedExtensions(new string[] { ".pdf", ".jpg", ".jpeg", ".png" })]
        public IFormFile File { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
    }
}
