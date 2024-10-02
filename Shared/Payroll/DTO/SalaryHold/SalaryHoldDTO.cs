using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Payroll.DTO.SalaryHold
{
    public class SalaryHoldDTO
    {
        public long SalaryHoldId { get; set; }
        [Range(1, long.MaxValue)]
        public long EmployeeId { get; set; }
        [Range(1, 12)]
        public short Month { get; set; }
        [Range(2015, 2060)]
        public short Year { get; set; }
        [Required, StringLength(200)]
        public string HoldReason { get; set; }
        [Required, Column(TypeName = "date")]
        public DateTime? HoldFrom { get; set; }
        [Required, Column(TypeName = "date")]
        public DateTime? HoldTo { get; set; }
        public bool? WithSalary { get; set; } // Arrear
        public bool? WithoutSalary { get; set; } // 
        public bool? PFContinue { get; set; }
        public bool? GFContinue { get; set; }
        public string EmployeeCode { get; set; }

    }
}
