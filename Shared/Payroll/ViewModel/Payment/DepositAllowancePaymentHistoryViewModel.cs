using Shared.BaseModels.For_ViewModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Payroll.ViewModel.Payment
{
    public class DepositAllowancePaymentHistoryViewModel : BaseViewModel3
    {
        public long Id { get; set; }
        [StringLength(50)]
        public string Code { get; set; }
        public long EmployeeId { get; set; }
        public long? AllowanceNameId { get; set; }
        public int PaymentMonth { get; set; }
        public int PaymentYear { get; set; }
        public bool? IsVisibleInPayslip { get; set; }
        public bool? IsVisibleInSalarySheet { get; set; }
        [StringLength(50)]
        public string PaymentApproach { get; set; } // Proposal amount (Less or Equal) // Exact Proposal amount // Remaining deposit amount // Remaining deposit amount with current month.
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
        public long? EmployeeDepositAllowanceConfigId { get; set; }
        public long? ConditionalDepositAllowanceConfigId { get; set; }
        public long? SalaryProcessId { get; set; }
        public long? SalaryProcessDetailId { get; set; }
        public string AllowanceName { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeCode { get; set; }
    }
}
