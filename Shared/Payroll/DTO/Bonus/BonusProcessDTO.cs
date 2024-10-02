using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Payroll.DTO.Bonus
{
    public class BonusProcessDTO
    {
        public long BonusProcessId { get; set; }
        public long BonusId { get; set; }
        public long BonusConfigId { get; set; }
        public long FiscalYearId { get; set; }
        public short BonusMonth { get; set; }
        public short BonusYear { get; set; }
        public bool IsDisbursed { get; set; }
        public int TotalEmployees { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? TotalTax { get; set; }
        [StringLength(50)]
        public string BatchNo { get; set; }
        [Column(TypeName = "date")]
        public Nullable<DateTime> ProcessDate { get; set; } // Cutoff Date
        [Column(TypeName = "date")]
        public Nullable<DateTime> PaymentDate { get; set; }
        public long? DepartmentId { get; set; }
        public long? BranchId { get; set; }
    }
}
