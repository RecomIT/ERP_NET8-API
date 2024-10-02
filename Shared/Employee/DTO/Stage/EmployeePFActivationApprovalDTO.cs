using System.ComponentModel.DataAnnotations;

namespace Shared.Employee.DTO.Stage
{
    public class EmployeePFActivationApprovalDTO
    {
        [Range(1, long.MaxValue)]
        public long PFActivationId { get; set; }
        [Range(1, long.MaxValue)]
        public long EmployeeId { get; set; }
        [StringLength(50)]
        public string StateStatus { get; set; }
        [StringLength(200)]
        public string Remarks { get; set; }
    }
}
