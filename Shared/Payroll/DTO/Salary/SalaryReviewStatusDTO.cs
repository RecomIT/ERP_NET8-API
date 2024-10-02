using System.ComponentModel.DataAnnotations;

namespace Shared.Payroll.DTO.Salary
{
    public class SalaryReviewStatusDTO
    {
        [Required, StringLength(200)]
        public string StateStatus { get; set; }
        [StringLength(200)]
        public string StatusRemarks { get; set; }
        [Range(1,long.MaxValue)]
        public long SalaryReviewInfoId { get; set; }
        [Range(1, long.MaxValue)]
        public long EmployeeId { get; set; }
    }
}
