using System.ComponentModel.DataAnnotations;

namespace Shared.Employee.DTO.Info
{
    public class EmployeeApprovalDTO
    {
        [Range(1, long.MaxValue)]
        public long EmployeeId { get; set; }
        [Required, StringLength(50)]
        public string StateStatus { get; set; }
        [StringLength(50)]
        public string Remarks { get; set; }
    }
}
