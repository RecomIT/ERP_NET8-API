using System;
using System.ComponentModel.DataAnnotations;

namespace Shared.Payroll.DTO.Bonus
{
    public class DisbursedUndoBonusDTO
    {
        [Range(1, long.MaxValue)]
        public long BonusId { get; set; }
        public long BonusConfigId { get; set; }
        [Range(1, long.MaxValue)]
        public long BonusProcessId { get; set; }
    }
}
