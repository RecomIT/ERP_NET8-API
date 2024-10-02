using Shared.Helpers.ValidationFilters;
using System;
using System.ComponentModel.DataAnnotations;

namespace Shared.Employee.ViewModel.Stage
{
    public class EmploymentProbationaryStatusDTO
    {
        [Range(1, long.MaxValue)]
        public long ProbationaryExtensionId { get; set; }
        [Range(1, long.MaxValue)]
        public long EmployeeId { get; set; }
        [StringLength(200)]
        public string Remarks { get; set; }
        [StringLength(50), RequiredValue(new string[] { "Approved", "Recheck", "Cancelled" })]
        public string StateStatus { get; set; }
    }
}
