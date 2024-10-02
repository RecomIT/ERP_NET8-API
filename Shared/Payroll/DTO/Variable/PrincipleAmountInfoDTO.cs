using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Payroll.DTO.Variable
{
    public class PrincipleAmountInfoDTO
    {
        public long? EmployeeId { get; set; }
        public long? DesignationId { get; set; }
        public long? GradeId { get; set; }
        public long? DepartmentId { get; set; }
        [StringLength(50)]
        public string JobType { get; set; }
        public short Month { get; set; } = 0;
        public short Year { get; set; } = 0;
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; } = 0;
        public long VariableId { get; set; }
        [StringLength(150)]
        public string VariableType { get; set; }
    }
}
