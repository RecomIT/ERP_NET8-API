using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Shared.Models;

namespace Shared.Payroll.DTO.CashSalary
{
    public class CashSalaryProcessExecutionDTO : BaseViewModel2
    {
        public long? CashSalaryProcessId { get; set; }
        public long? CashSalaryDetailId { get; set; }
        public string BatchNo { get; set; } // BH-202308001 BH-202308002 BH-202308003
        public string ExecutionOn { get; set; }
        [Column(TypeName = "date")]
        public DateTime? SalaryDate { get; set; }
        [Range(1, short.MaxValue)]
        public short Month { get; set; }
        [Range(1, short.MaxValue)]
        public short Year { get; set; }
        [Column(TypeName = "date")]
        public DateTime? ProcessDate { get; set; }
        [Column(TypeName = "date")]
        public DateTime? PaymentDate { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? CashAmount { get; set; }
        public bool? IsDisbursed { get; set; }
        public long? ProcessBranchId { get; set; }
        public long? ProcessDepartmentId { get; set; }
        public string EmployeeName { get; set; }
        public string DesignationName { get; set; }
        public string DepartmentName { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? Amount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? GrossPay { get; set; }
        [StringLength(100)]
        public string PayrollCardNumber { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? PayrollCardTransfer { get; set; }

    }
}
