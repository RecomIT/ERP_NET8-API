using System;
using Shared.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Payroll.ViewModel.Payment
{
    public class SupplementaryPaymentProcessInfoViewModel : BaseViewModel3
    {
        public long PaymentProcessInfoId { get; set; }
        [StringLength(50)]
        public string BatchNo { get; set; }
        public int TotalEmployees { get; set; }
        public short PaymentMonth { get; set; }
        public short PaymentYear { get; set; }
        [StringLength(50)]
        public string PaymentMonthName { get; set; }
        [Column(TypeName = "date")]
        public DateTime? CutOffDate { get; set; }
        public bool EffectOnPayslip { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalOnceOffAmount { get; set; } = 0;
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalTax { get; set; } = 0;
        public bool? IsDisbursed { get; set; }
        [Column(TypeName = "date")]
        public DateTime? PaymentDate { get; set; }
        [StringLength(50)]
        public string StateStatus { get; set; }
        public bool IsApproved { get; set; }
        [StringLength(100)]
        public string PaymentMode { get; set; }
        [StringLength(100)]
        public string ProcessType { get; set; }
        public bool? ShowInPayslip { get; set; }
        public bool? ShowInSalarySheet { get; set; }
        public long FiscalYearId { get; set; }
        [StringLength(100)]
        public string FiscalYearRange { get; set; }
        public long AllowanceNameId { get; set; }
        public string AllowanceName { get; set; }
    }
}
