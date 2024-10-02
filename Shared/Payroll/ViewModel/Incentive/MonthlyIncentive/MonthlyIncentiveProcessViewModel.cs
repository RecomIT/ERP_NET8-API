using Shared.BaseModels.For_ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Payroll.ViewModel.Incentive.MonthlyIncentive
{
    public class MonthlyIncentiveProcessViewModel : BaseViewModel1
    {
        public long? MonthlyIncentiveProcessId { get; set; }
        [StringLength(100)]
        public string BatchNo { get; set; }
        public short IncentiveMonth { get; set; }
        public short IncentiveYear { get; set; }
        public long? MonthlyIncentiveNoId { get; set; }
        [StringLength(100)]
        public string MonthlyIncentiveName { get; set; }
        public bool? IsActive { get; set; }
        [Column(TypeName = "date")]
        public DateTime? ProcessDate { get; set; }
        [Column(TypeName = "date")]
        public DateTime? PaymentDate { get; set; }
        public bool? IsDisbursed { get; set; }
        public bool? IsApproved { get; set; }
        [StringLength(20)]
        public string StateStatus { get; set; }
        [StringLength(100)]
        public string Remarks { get; set; }
        public int HeadCount { get; set; }
        [StringLength(50)]
        public string IncentiveMonthName { get; set; }
    }
}
