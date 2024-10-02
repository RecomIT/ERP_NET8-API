using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Shared.Models;

namespace Shared.Payroll.Domain.CashSalary
{

    [Table("Payroll_UploadCashSalary")]
    public class UploadCashSalary : BaseModel2
    {
        [Key]
        public long UploadCashSalaryId { get; set; }
        public long? EmployeeId { get; set; }
        public long? GradeId { get; set; }
        public long? DesignationId { get; set; }
        public long? DepartmentId { get; set; }
        [Column(TypeName = "date")]
        public DateTime? SalaryDate { get; set; }
        public short SalaryMonth { get; set; }
        public short SalaryYear { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }
        [StringLength(100)]
        public string PayrollCardNumber { get; set; }
        [StringLength(100)]
        public string WalletNumber { get; set; }
        [StringLength(50)]
        public string WalletAgent { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsApproved { get; set; }
        [StringLength(50)]
        public string StateStatus { get; set; }
        [StringLength(100)]
        public string BatchNo { get; set; }
        public bool? IsProcessed { get; set; }
        public long? CashSalaryProcessId { get; set; }
        public long CashSalaryHeadId { get; set; }
        public CashSalaryHead CashSalaryHead { get; set; }
    }
}
