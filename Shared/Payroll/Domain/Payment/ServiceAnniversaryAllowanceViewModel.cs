using Shared.BaseModels.For_ViewModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Payroll.Domain.Payment
{
    public class ServiceAnniversaryAllowanceViewModel : BaseViewModel2
    {
        public long Id { get; set; }
        [StringLength(100)]
        public string Code { get; set; } // CDPS-1
        public long? AllowanceHeadId { get; set; }
        public long AllowanceNameId { get; set; }
        public bool? ConsiderPaymentMonthLastDate { get; set; }
        public int? CutOffDay { get; set; }
        [StringLength(50)]
        public string JobType { get; set; } // N/A / Permanent / Contrutual / Trainee / Probotion
        [StringLength(50)]
        public string Religion { get; set; } // N/A / Islam /Christian
        [StringLength(50)]
        public string MaritalStatus { get; set; } // N/A / Married /Single
        [StringLength(50)]
        public string Citizen { get; set; } // N/A / YES /NO
        [StringLength(50)]
        public string Gender { get; set; } // N/A /Male/Female
        [StringLength(50)]
        public string PhysicalCondition { get; set; } // N/A/Disabled/Undisabled
        public bool? IsVisibleInPayslip { get; set; }
        public bool? IsVisibleInSalarySheet { get; set; }
        [StringLength(50)]
        public string BaseOfPayment { get; set; } // Flat(5000/=) / Basic (50%) / Gross / Gross Without Conveyance
        [Column(TypeName = "decimal(18,2)")]
        public decimal? Percentage { get; set; } // Basic/Gross = 100%
        [Column(TypeName = "decimal(18,2)")]
        public decimal? Amount { get; set; } // 5000/=
        [StringLength(100)]
        public string StateStatus { get; set; } // Pending / Approved / Processed / Disbursed
        public bool IsApproved { get; set; }
        [Column(TypeName = "date")]
        public Nullable<DateTime> ActivationFrom { get; set; }
        [Column(TypeName = "date")]
        public Nullable<DateTime> ActivationTo { get; set; }
        [StringLength(100)]
        public string AllowanceName { get; set; }
    }
}
