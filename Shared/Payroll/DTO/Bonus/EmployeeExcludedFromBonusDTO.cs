using System.ComponentModel.DataAnnotations;

namespace Shared.Payroll.DTO.Bonus
{
    public class EmployeeExcludedFromBonusDTO
    {
        public long ExcludeId { get; set; }
        [Required]
        public long EmployeeId { get; set; }
        [Required]
        public long BonusId { get; set; }
        [Required]
        public long BonusConfigId { get; set; }
    }
}
