using System.ComponentModel.DataAnnotations;

namespace Shared.Employee.DTO.Info
{
    public class DeleteEmployeeSkillDTO
    {
        [Range(1, long.MaxValue)]
        public long EmployeeSkillId { get; set; }
        [Range(1, long.MaxValue)]
        public long EmployeeId { get; set; }
    }
}
