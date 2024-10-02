using System.ComponentModel.DataAnnotations;

namespace Shared.Payroll.ViewModel.Allowance
{
    public class EmployeeTaxableAllowance
    {
        public long AllowanceNameId { get; set; }
        [StringLength(100)]
        public string AllowanceName { get; set; }
        public decimal Amount { get; set; }
        public bool? IsOnceOffTax { get; set; }
        public bool? ProjectRestYear { get; set; }
        public bool? IsTaxDistributed { get; set; }
    }
}
