using System.ComponentModel.DataAnnotations;

namespace Shared.Payroll.DTO.Payment
{
    public class DeleteProjectedAllowanceDTO
    {
        [Range(1,long.MaxValue)]
        public long Id { get; set; }
        public long EmployeeId { get; set; } = 0;
        public long AllowanceNameId { get; set; } = 0;
    }
}
