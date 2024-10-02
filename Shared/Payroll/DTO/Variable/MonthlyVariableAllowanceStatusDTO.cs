using System.ComponentModel.DataAnnotations;

namespace Shared.Payroll.DTO.Variable
{
    public class MonthlyVariableAllowanceStatusDTO
    {
        [Range(1, long.MaxValue)]
        public long MonthlyVariableAllowanceId { get; set; }
        [Range(1, long.MaxValue)]
        public long EmployeeId { get; set; }
        [StringLength(50)]
        public string StateStatus { get; set; }
        public string Remarks { get; set; }
    }
}
