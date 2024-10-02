using Shared.BaseModels.For_ViewModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Payroll.ViewModel.Payment
{
    public class EmployeeProjectedPaymentViewModel
    {
        public long ProjectedAllowanceId { get; set; }
        [StringLength(100)]
        public string ProjectedAllowanceCode { get; set; } // PA-0000001
        [Range(1, long.MaxValue)]
        public long EmployeeId { get; set; }
        public long? FiscalYearId { get; set; }
        public short? PaymentMonth { get; set; }
        public short? PaymentYear { get; set; }
        public short? PayableYear { get; set; }
        public long? AllowanceHeadId { get; set; }
        [Range(1, long.MaxValue)]
        public long AllowanceNameId { get; set; }
        [StringLength(50)]
        public string BaseOfPayment { get; set; } // Flat(5000/=) / Basic (50%) / Gross
        [Column(TypeName = "decimal(18,2)")]
        public decimal? Percentage { get; set; } // Basic/Gross = 100%
        [Column(TypeName = "decimal(18,2)")]
        public decimal? Amount { get; set; } // 5000/=
        [Column(TypeName = "decimal(18,2)")]
        public decimal? PayableAmount { get; set; } // Basic 50% 10000/=
        [Column(TypeName = "decimal(18,2)")]
        public decimal? DisbursedAmount { get; set; } // 8500/=
        [Column(TypeName = "decimal(18,2)")]
        public decimal? TaxAmount { get; set; } // 8500/=
        [StringLength(100)]
        public string StateStatus { get; set; } // Pending / Approved / Processed / Disbursed
        public long? ProjectedAllowanceProcessInfoId { get; set; }
        //
        public string EmployeeName { get; set; }
        public string EmployeeCode { get; set; }
        public string PaymentMonthName { get; set; }
        public string AllowanceName { get; set; }
        public string PaymentMode { get; set; }
        public string FiscalYearRange { get; set; }
        public string AllowanceReason { get; set; }
    }
}
