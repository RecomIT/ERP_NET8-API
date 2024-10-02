using Shared.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Employee.Domain.Termination
{
    [Table("HR_DiscontinuedEmployee")]
    public class DiscontinuedEmployee : BaseModel3
    {
        [Key]
        public long DiscontinuedId { get; set; }
        [Range(1, long.MaxValue)]
        public long EmployeeId { get; set; }
        [Column(TypeName = "date")]
        public DateTime? LastWorkingDate { get; set; }
        public bool? CalculateFestivalBonusTaxProratedBasis { get; set; }
        public bool? CalculateProjectionTaxProratedBasis { get; set; }
        [StringLength(100)]
        public string StateStatus { get; set; }
        [StringLength(50)]
        public string Releasetype { get; set; } //Voluntary //Nonvoluntary
        public bool? IsFullMonthSalaryHold { get; set; }

    }
}
