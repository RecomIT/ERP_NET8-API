using Microsoft.EntityFrameworkCore;
using Shared.BaseModels.For_DomainModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Payroll.Domain.Payment
{
    [Table("Payroll_MonthlyAllowanceConfig"), Index("EmployeeId", "AllowanceNameId", "CompanyId", "OrganizationId", IsUnique = false, Name = "IX_Payroll_MonthlyAllowanceConfig_NonClusteredIndex")]
    public class MonthlyAllowanceConfig : BaseModel2
    {
        [Key]
        public long Id { get; set; }
        public long EmployeeId { get; set; }
        public long AllowanceNameId { get; set; }
        [StringLength(50)]
        public string BaseOfPayment { get; set; } // Flat(5000/=) / Basic (50%) / Gross / Gross Without Conveyance
        [Column(TypeName = "decimal(18,2)")]
        public decimal? Percentage { get; set; } // Basic/Gross = 100%
        [Column(TypeName = "decimal(18,2)")]
        public decimal? Amount { get; set; } // 5000/=
        [Column(TypeName = "decimal(18,2)")]
        public decimal? TotalAmount { get; set; } = 0;
        [StringLength(100)]
        public string StateStatus { get; set; } // Pending / Approved / Processed / Disbursed
        public bool IsApproved { get; set; }
        public bool IsActive { get; set; }
        public bool IsProrated { get; set; } = false;
        public bool IsVisibleInSalarySheet { get; set; } = true;
        [StringLength(300)]
        public string Remarks { get; set; }
        [Column(TypeName = "date")]
        public Nullable<DateTime> ActivationFrom { get; set; }
        [Column(TypeName = "date")]
        public Nullable<DateTime> ActivationTo { get; set; }
        public ICollection<MonthlyAllowanceHistory> MonthlyAllowanceHistories { get; set; }
    }
}
