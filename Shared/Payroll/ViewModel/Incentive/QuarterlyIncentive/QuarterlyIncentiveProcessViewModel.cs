using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Shared.BaseModels.For_ViewModel;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Payroll.ViewModel.Incentive.QuarterlyIncentive
{
    public class QuarterlyIncentiveProcessViewModel
    {
        public long QuarterlyIncentiveProcessId { get; set; }
        [StringLength(150)]
        public string BatchNo { get; set; }
        public long IncentiveQuarterNoId { get; set; }
        [StringLength(100)]
        public string IncentiveQuarterNumber { get; set; }
        public short IncentiveYear { get; set; }
        [Column(TypeName = "date")]
        public DateTime? PaymentDate { get; set; }
        [Column(TypeName = "date")]
        public DateTime? ProcessDate { get; set; }
        public bool? IsDisbursed { get; set; }
        public bool? IsApproved { get; set; }
        public int HeadCount { get; set; }
    }
}
