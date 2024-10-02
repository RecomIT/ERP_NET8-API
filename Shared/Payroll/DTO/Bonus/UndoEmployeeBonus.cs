using System.ComponentModel.DataAnnotations;

namespace Shared.Payroll.DTO.Bonus
{
    public class UndoEmployeeBonus
    {
        [Range(1, long.MaxValue)]
        public long BonusId { get; set; }
        public long BonusConfigId { get; set; }
        [Range(1, long.MaxValue)]
        public long EmployeeId { get; set; }
        [Range(1, long.MaxValue)]
        public long BonusProcessId { get; set; }
    }
}
