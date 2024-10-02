using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Payroll.ViewModel.Tax
{
    public class TaxSlabAmount
    {
        public long IncomeTaxSlabId { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal SlabMininumAmount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal SlabMaximumAmount { get; set; }
        [Column(TypeName = "decimal(4,2)")]
        public decimal SlabPercentage { get; set; }
    }
}
