using Shared.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Payroll.Domain.Incentive.QuarterlyIncentive
{
    [Table("Payroll_QuarterlyIncentiveProcess")]
    public class QuarterlyIncentiveProcess : BaseModel5
    {
        [Key]
        public long QuarterlyIncentiveProcessId { get; set; }
        [StringLength(100)]
        public string BatchNo { get; set; }
        public long? IncentiveQuarterNoId { get; set; }
        [StringLength(100)]
        public string IncentiveQuarterNumber { get; set; }
        public short IncentiveYear { get; set; }
        public bool? IsDisbursed { get; set; }
        [Column(TypeName = "date")]
        public DateTime? ProcessDate { get; set; }
        [Column(TypeName = "date")]
        public DateTime? PaymentDate { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsApproved { get; set; }
        [StringLength(50)]
        public string StateStatus { get; set; }
        [StringLength(150)]
        public string Remarks { get; set; }
        public short? PayableMonth { get; set; }
        public short? PayableYear { get; set; }
        [Column(TypeName = "date")]
        public Nullable<DateTime> PayableDate { get; set; }
        public ICollection<QuarterlyIncentiveProcessDetail> QuarterlyIncentiveProcessDetail { get; set; }
    }

}


