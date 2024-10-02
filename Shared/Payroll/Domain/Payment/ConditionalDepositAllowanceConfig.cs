using Microsoft.EntityFrameworkCore;
using Shared.BaseModels.For_DomainModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Payroll.Domain.Payment
{
    [Table("Payroll_ConditionalDepositAllowanceConfig"), Index("AllowanceNameId", "CompanyId", "OrganizationId", IsUnique = false, Name = "IX_Payroll_ConditionalDepositAllowanceConfig_NonClusteredIndex")]
    public class ConditionalDepositAllowanceConfig : BaseModel2
    {
        [Key]
        public long Id { get; set; }
        [StringLength(100)]
        public string Code { get; set; } // CDPS-1
        public int? ServiceLength { get; set; }
        [StringLength(50)]
        public string ServiceLengthUnit { get; set; }
        public long? AllowanceHeadId { get; set; }
        public long AllowanceNameId { get; set; }
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
        public string PhysicalCondition { get; set; } // None/Disabled/Undisabled
        public bool? IsVisibleInPayslip { get; set; }
        public bool? IsVisibleInSalarySheet { get; set; }
        [StringLength(50)]
        public string DepositType { get; set; } // Monthly / Yearly
        public bool IsTreatAsSalaryBreakdownComponent { get; set; } // When DepositType is Monthly this prop will be visible.
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
    }
}
