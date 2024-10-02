using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Shared.Payroll.Process.Tax
{
    public class TaxProcessSlab
    {
        public long TaxProcessSlabId { get; set; }
        public long EmployeeId { get; set; }
        public long? FiscalYearId { get; set; }
        public short Month { get; set; }
        public short Year { get; set; }
        public long? IncomeTaxSlabId { get; set; }
        [StringLength(20)]
        public string ImpliedCondition { get; set; }
        [Column(TypeName = "decimal(4,2)")]
        public decimal SlabPercentage { get; set; }
        [StringLength(100)]
        public string ParameterName { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? TaxableIncome { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? TaxLiability { get; set; }
        public DateTime? SalaryDate { get; set; }
        public long TaxProcessId { get; set; }
    }
}
