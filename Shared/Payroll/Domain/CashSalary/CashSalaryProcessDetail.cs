using Shared.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Shared.Payroll.Domain.CashSalary
{
    [Table("Payroll_CashSalaryProcessDetail")]
    public class CashSalaryProcessDetail : BaseModel4
    {
        [Key]
        public long CashSalaryDetailId { get; set; }
        [StringLength(100)]
        public string BatchNo { get; set; }
        public long? EmployeeId { get; set; }
        public long? GradeId { get; set; }
        public long? DesignationId { get; set; }
        public long? DepartmentId { get; set; }
        [Column(TypeName = "date")]
        public DateTime? SalaryDate { get; set; }
        public short SalaryMonth { get; set; }
        public short SalaryYear { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? GrossPay { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? TotalCashAllowance { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? CashPayWithoutCOC { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? PayrollCardTransfer { get; set; }

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
        [StringLength(150)]
        public string Remarks { get; set; }

        [ForeignKey("CashSalaryProcess")]
        public long CashSalaryProcessId { get; set; }
        public CashSalaryProcess CashSalaryProcess { get; set; }
    }
}
