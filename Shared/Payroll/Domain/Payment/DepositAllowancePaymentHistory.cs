using Microsoft.EntityFrameworkCore;
using Shared.BaseModels.For_DomainModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Payroll.Domain.Payment
{
    [Table("Payroll_DepositAllowancePaymentHistory"), Index("EmployeeId", "AllowanceNameId", "CompanyId", "OrganizationId", IsUnique = false, Name = "IX_Payroll_DepositAllowancePaymentHistory_NonClusteredIndex")]
    public class DepositAllowancePaymentHistory : BaseModel2
    {
        [Key]
        public long Id { get; set; }
        [StringLength(50)]
        public string Code { get; set; }
        public long EmployeeId { get; set; }
        public long? AllowanceNameId { get; set; }
        public int PaymentMonth { get; set; }
        public int PaymentYear { get; set; }
        [Column(TypeName = "date")]
        public Nullable<DateTime> PaymentDate { get; set; } // Last date of payment month
        public bool? IsVisibleInPayslip { get; set; }
        public bool? IsVisibleInSalarySheet { get; set; }
        [StringLength(50)]
        public string PaymentApproach { get; set; } // m
        [StringLength(50)]
        public string PaymentBeMade { get; set; } // With Salary // Out Of Salary
        public long? FiscalYearId { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal ProposalAmount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal PayableAmount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal DisbursedAmount { get; set; }
        [StringLength(50)]
        public string IncomingFlag { get; set; } // Conditional / Individual
        [StringLength(50)]
        public string StateStatus { get; set; }
        public bool IsApproved { get; set; }
        public bool IsDisbursed { get; set; }
        public long? EmployeeDepositAllowanceConfigId { get; set; }
        public long? ConditionalDepositAllowanceConfigId { get; set; }
        public long? SalaryProcessId { get; set; }
        public long? SalaryProcessDetailId { get; set; }

    }
}
