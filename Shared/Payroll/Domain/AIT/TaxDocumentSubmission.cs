using Shared.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Payroll.Domain.AIT
{
    [Table("Payroll_TaxDocumentSubmission"), Index("EmployeeId", "FiscalYearId", "CompanyId", "OrganizationId", IsUnique = false, Name = "IX_Payroll_TaxDocumentSubmission_NonClusteredIndex")]
    public class TaxDocumentSubmission : BaseModel5
    {
        [Key]
        public long SubmissionId { get; set; }
        public long EmployeeId { get; set; }
        public long FiscalYearId { get; set; }
        [StringLength(100)]
        public string CertificateType { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? Amount { get; set; }
        public bool IsAuction { get; set; }
        [StringLength(200)]
        public string ActualFileName { get; set; }
        [StringLength(200)]
        public string FileName { get; set; }
        [StringLength(50)]
        public string FileSize { get; set; }
        [StringLength(300)]
        public string FilePath { get; set; }
        [StringLength(100)]
        public string FileFormat { get; set; }

        [StringLength(100)]
        public string RegSlNo { get; set; }
        public short? NumberOfCar { get; set; }
        [StringLength(100)]
        public string TaxZone { get; set; }
        [StringLength(100)]
        public string TaxCircle { get; set; }
        [StringLength(100)]
        public string TaxUnit { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? TaxPayable { get; set; }
        public Nullable<DateTime> SubmissionDate { get; set; }
        [StringLength(100)]
        public string StateStatus { get; set; }
        public bool IsApproved { get; set; }
    }
}
