using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Payroll.ViewModel.Payment
{
    public class MonthlyAllowanceConfigViewModel
    {
        public long Id { get; set; }
        public long EmployeeId { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public long AllowanceNameId { get; set; }
        public string AllowanceName { get; set; }
        public string BaseOfPayment { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? Percentage { get; set; } // Basic/Gross = 100%
        [Column(TypeName = "decimal(18,2)")]
        public decimal? Amount { get; set; } // 5000/=
        [Column(TypeName = "decimal(18,2)")]
        public decimal? TotalAmount { get; set; } = 0;
        [StringLength(100)]
        public string StateStatus { get; set; }
        public bool IsActive { get; set; }
        public bool IsProrated { get; set; }
        public bool IsVisibleInSalarySheet { get; set; }
        public string Remarks { get; set; }
        [Column(TypeName = "date")]
        public Nullable<DateTime> ActivationFrom { get; set; }
        [Column(TypeName = "date")]
        public Nullable<DateTime> ActivationTo { get; set; }

    }
}
