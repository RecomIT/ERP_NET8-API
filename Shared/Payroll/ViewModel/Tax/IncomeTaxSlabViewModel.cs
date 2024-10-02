using Shared.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Payroll.ViewModel.Tax
{
    public class IncomeTaxSlabViewModel : BaseViewModel3
    {
        public long IncomeTaxSlabId { get; set; }
        public long FiscalYearId { get; set; }
        public string FiscalYearRange { get; set; }
        [StringLength(20)]
        public string AssesmentYear { get; set; }
        [StringLength(20)]
        public string ImpliedCondition { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal SlabMininumAmount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal SlabMaximumAmount { get; set; }
        [Column(TypeName = "decimal(4,2)")]
        public decimal SlabPercentage { get; set; }
        [StringLength(50)]
        public string StateStatus { get; set; }
        public long EmployeeId { get; set; } = 0;
    }
}
