using Shared.BaseModels.For_ViewModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Payroll.ViewModel.Payment
{
    public class EmployeeProjectedAllowanceProcessInfoViewModel : BaseViewModel3
    {
        public long ProjectedAllowanceProcessInfoId { get; set; }
        [StringLength(100)]
        public string ProcessCode { get; set; } // PROCD-0000001
        public int HeadCount { get; set; }
        public long? FiscalYearId { get; set; }
        public short PaymentMonth { get; set; }
        public short PaymentYear { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? TotalAmount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? TotalPayableAmount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? TotalDisbursedAmount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? TotalTaxAmount { get; set; }
        [StringLength(100)]
        public string StateStatus { get; set; }
        public bool IsApproved { get; set; }
        public bool? IsDisbursed { get; set; }
        public bool? ShowInPayslip { get; set; }
        public bool? ShowInSalarySheet { get; set; }

        public string PaymentMode { get; set; }
        public bool? WithCOC { get; set; }
        public string PaymentMonthName { get; set; }
        public string FiscalYearRange { get; set; }
    }
}
