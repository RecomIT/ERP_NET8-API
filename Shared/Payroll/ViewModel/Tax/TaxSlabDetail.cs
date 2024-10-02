using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Payroll.ViewModel.Tax
{
    public class TaxSlabDetail
    {
        [Range(1, long.MaxValue)]
        public long FiscalYearId { get; set; }
        [Required, StringLength(20)]
        public string ImpliedCondition { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal SlabMininumAmount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal SlabMaximumAmount { get; set; }
        [Column(TypeName = "decimal(4,2)")]
        public decimal SlabPercentage { get; set; }
    }
}
