
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Payroll.DTO.Tax
{
    public class TaxDocumentSubmissionDTO
    {
        public long SubmissionId { get; set; }
        [Range(1, long.MaxValue, ErrorMessage = "Fiscal Year is missing")]
        public long EmployeeId { get; set; }
        [Range(1, long.MaxValue,ErrorMessage ="Fiscal Year is missing")]
        public long FiscalYearId { get; set; }
        public string FiscalYearRange { get; set; }
        [Required(ErrorMessage = "Certificate type is required"), StringLength(20, ErrorMessage = "Maximum length of certificate type is 20")]
        public string CertificateType { get; set; }
        [Required(ErrorMessage = "Amount is missing"), Column(TypeName = "decimal(18,2)")]
        public decimal? Amount { get; set; }
        public bool IsAuction { get; set; }
        [StringLength(200)]
        public string FileName { get; set; }
        [StringLength(200)]
        public string ActualFileName { get; set; }
        [StringLength(50)]
        public string FileSize { get; set; }
        [StringLength(300)]
        public string FilePath { get; set; }
        [StringLength(100)]
        public string FileFormat { get; set; }
        public IFormFile File { get; set; }
        public string EmployeeCode { get; set; }
    }
}
