using Microsoft.AspNetCore.Http;
using Shared.Helpers.ValidationFilters;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Payroll.DTO.Tax
{
    public class TaxRefundSubmissionDTO
    {
        public long SubmissionId { get; set; }
        [Required(ErrorMessage ="Employee id is missing"),Range(1,long.MaxValue, ErrorMessage = "Employee id is missing")]
        public long EmployeeId { get; set; } = 0;
        [Range(1, long.MaxValue,ErrorMessage = "Fiscal year is missing")]
        public long FiscalYearId { get; set; }
        [Required(ErrorMessage = "Amount is required"), Column(TypeName = "decimal(18,2)")]
        public decimal? Amount { get; set; }
        public short? NumberOfCar { get; set; }
        [StringLength(300)]
        public string FilePath { get; set; }
        public bool? IsAuction { get; set; }
        [RequiredIfValue("SubmissionId", new string[] { "0" }), AllowedExtensions(new string[] { ".pdf", ".png", ".jpg", ".jpeg" })]
        public IFormFile File { get; set; }
    }
}
