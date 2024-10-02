using Shared.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Payroll.ViewModel.Tax
{
    public class EmployeeTaxProcessViewModel : BaseViewModel5
    {
        public long TaxProcessId { get; set; }
        public long EmployeeId { get; set; }
        public long? SalaryProcessId { get; set; }
        public long? SalaryProcessDetailId { get; set; }
        public long FiscalYearId { get; set; }
        public short SalaryMonth { get; set; }
        public short SalaryYear { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? YearlyTaxableIncome { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? TotalTaxPayable { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? InvesmentRebate { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? AITAmount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? TaxReturnAmount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? ExcessTaxPaidRefundAmount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? YearlyTax { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? PaidTotalTax { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal MonthlyTax { get; set; }
    }
}
