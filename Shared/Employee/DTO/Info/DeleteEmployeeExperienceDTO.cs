using System.ComponentModel.DataAnnotations;

namespace Shared.Employee.DTO.Info
{
    public class DeleteEmployeeExperienceDTO
    {
        [Range(1, long.MaxValue)]
        public long EmployeeExperienceId { get; set; }
        [Range(1, long.MaxValue)]
        public long EmployeeId { get; set; }
    }
}
