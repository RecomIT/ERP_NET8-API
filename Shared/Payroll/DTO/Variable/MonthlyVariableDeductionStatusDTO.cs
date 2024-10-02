using System.ComponentModel.DataAnnotations;

namespace Shared.Payroll.DTO.Variable
{
    public class MonthlyVariableDeductionStatusDTO
    {
        [Range(1, long.MaxValue)]
        public long MonthlyVariableDeductionId { get; set; }
        [Range(1, long.MaxValue)]
        public long EmployeeId { get; set; }
        [StringLength(50)]
        public string StateStatus { get; set; }
        [StringLength(200)]
        public string Remarks { get; set; }
    }
}
