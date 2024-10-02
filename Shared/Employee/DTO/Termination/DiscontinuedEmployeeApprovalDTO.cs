using Shared.Helpers.ValidationFilters;
using System.ComponentModel.DataAnnotations;

namespace Shared.Employee.DTO.Termination
{
    public class DiscontinuedEmployeeApprovalDTO
    {
        [Range(1, long.MaxValue)]
        public long DiscontinuedId { get; set; }
        [Range(1, long.MaxValue)]
        public long EmployeeId { get; set; }
        [StringLength(50), RequiredValue(new string[] { "Approved", "Cancelled" })]
        public string StateStatus { get; set; }
    }
}
