using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Payroll.ViewModel.Tax
{
    public class EmployeeTaxProcessDetailViewModel
    {
        public long TaxProcessDetailId { get; set; }
        public long TaxProcessId { get; set; }
        public long EmployeeId { get; set; }
        public long AllowanceHeadId { get; set; }
        [StringLength(200)]
        public string AllowanceHeadName { get; set; }
        public long AllowanceNameId { get; set; }
        [StringLength(200)]
        public string AllowanceName { get; set; }
        public long BonusId { get; set; }
        [StringLength(100)]
        public string BonusName { get; set; }
        [StringLength(100)]
        public string TaxItem { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal TillDateIncome { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal CurrentMonthIncome { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal ProjectedIncome { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal GrossAnnualIncome { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal LessExempted { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalTaxableIncome { get; set; }
        public long SalaryProcessId { get; set; }
    }
}
