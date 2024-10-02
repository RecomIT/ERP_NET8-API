using Shared.Helpers.ValidationFilters;
using System.ComponentModel.DataAnnotations;

namespace Shared.Payroll.DTO.Allowance
{
    public class ApprovedPendingSalaryAllowanceConfigDTO
    {
        [Range(1, long.MaxValue, ErrorMessage = "Id is missing")]
        public long SalaryAllowanceConfigId { get; set; }
        [Required(ErrorMessage = "Config category is missing")]
        public string ConfigCategory { get; set; }
        [RequiredIfValue("ConfigCategory", new string[] { "Employee Wise", "Designation", "Grade" })]
        public long[] Id { get; set; }
        [RequiredIfValue("ConfigCategory", new string[] { "Job Type" }), StringLength(50)]
        public string JobType { get; set; }
    }
}
