using Shared.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Payroll.ViewModel.Payment
{
    public class SupplementaryPaymentAmountViewModel : BaseViewModel1
    {
        public long PaymentAmountId { get; set; }
        public long AllowanceNameId { get; set; }
        [StringLength(100)]
        public string AllowanceName { get; set; }
        public long EmployeeId { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string Designation { get; set; }
        public short PaymentMonth { get; set; }
        public short PaymentYear { get; set; }
        public string BaseOfPayment { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Percentage { get; set; }
        public string StateStatus { get; set; }
        public string PaymentMonthName { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? OnceOffAmount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? TaxAmount { get; set; }
        public decimal DisbursedAmount { get; set; } = 0;
        public long PaymentProcessInfoId { get; set; } = 0;
    }
}
