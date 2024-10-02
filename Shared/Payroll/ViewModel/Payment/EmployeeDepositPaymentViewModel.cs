using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Payroll.ViewModel.Payment
{
    public class EmployeeDepositPaymentViewModel
    {
        public long EmployeeId { get; set; }
        public string EmployeeCode { get; set; }
        public long AllowanceNameId { get; set; }
        public string EmployeeName { get; set; }
        public string DesignationName { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Total { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Paid { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal PayableAmount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal DisbursedAmount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal BaseAmount { get; set; } = 0;
        public decimal TillPaidAmount { get; set; } = 0;
        public decimal TillAccuredAmount { get; set; } = 0;
        public decimal ThisMonthPaidAmount { get; set; } = 0;
        public decimal ThisMonthAccuredAmount { get; set; } = 0;
        public decimal ThisMonthAccuredArrear { get; set; } = 0;
        public decimal RemainAmount { get; set; } = 0;
    }
}
