using Shared.BaseModels.For_DomainModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Payroll.Domain.Incentive.MonthlyIncentive
{
    [Table("Payroll_MonthlyIncentiveProcess")]
    public class MonthlyIncentiveProcess : BaseModel2
    {
        [Key]
        public long MonthlyIncentiveProcessId { get; set; }
        [StringLength(100)]
        public string BatchNo { get; set; }
        public short IncentiveMonth { get; set; }
        public short IncentiveYear { get; set; }
        public long? MonthlyIncentiveNoId { get; set; }
        [StringLength(100)]
        public string MonthlyIncentiveName { get; set; }
        public bool? IsActive { get; set; }
        [Column(TypeName = "date")]
        public Nullable<DateTime> ProcessDate { get; set; }
        [Column(TypeName = "date")]
        public Nullable<DateTime> PaymentDate { get; set; }
        public bool? IsDisbursed { get; set; }
        public bool? IsApproved { get; set; }
        [StringLength(20)]
        public string StateStatus { get; set; }
        [StringLength(100)]
        public string Remarks { get; set; }
        public short? PayableMonth { get; set; }
        public short? PayableYear { get; set; }
        [Column(TypeName = "date")]
        public Nullable<DateTime> PayableDate { get; set; }
        public ICollection<MonthlyIncentiveProcessDetail> MonthlyIncentiveProcessDetail { get; set; }
    }
}
