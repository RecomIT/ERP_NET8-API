using Shared.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Payroll.Domain.Salary
{
    [Table("Payroll_SalaryHold"), Index("EmployeeId", "IsHolded", "HoldFrom", "HoldTo", "StateStatus", "CompanyId", "OrganizationId", IsUnique = false, Name = "IX_Payroll_SalaryHold_NonClusteredIndex")]
    public class SalaryHold : BaseModel5
    {
        [Key]
        public long SalaryHoldId { get; set; }
        public long EmployeeId { get; set; }
        public short Month { get; set; }
        public short Year { get; set; }
        public bool? IsHolded { get; set; }
        [StringLength(200)]
        public string HoldReason { get; set; }
        [Column(TypeName = "date")]
        public Nullable<DateTime> HoldFrom { get; set; }
        [Column(TypeName = "date")]
        public Nullable<DateTime> HoldTo { get; set; }
        public bool? WithSalary { get; set; }
        public bool? WithoutSalary { get; set; }
        public bool? PFContinue { get; set; }
        public bool? GFContinue { get; set; }
        [Column(TypeName = "date")]
        public Nullable<DateTime> UnholdDate { get; set; }
        public string UnholdReason { get; set; }
        public long? EmployeeResignationId { get; set; } // Separation Module
        [StringLength(50)]
        public string StateStatus { get; set; }
        public long? IsApproved { get; set; }
        public long? DiscontinuedId { get; set; } // HR_DiscontinuedEmployee
    }
}
