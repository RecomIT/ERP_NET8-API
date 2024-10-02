using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace Shared.Overtime.DTO
{
    public class GetOvertimePolicyDTO
    {
        public long OvertimeId { get; set; }
        public string OvertimeName { get; set; }
        public string OvertimeNameInBengali { get; set; } = string.Empty;
        public string Unit { get; set; }
        public string AmountType { get; set; }
        public bool IsFlatAmountType { get; set; } = false;
        public bool IsPercentageAmountType { get; set; } = false;
        public decimal Amount { get; set; }
        public decimal AmountRate { get; set; } = 1;
        public bool LimitationOfUnit { get; set; } = false;
        public decimal MaxUnit { get; set; } = 0;
        public decimal MinUnit { get; set; } = 0;
        public bool LimitationOfAmount { get; set; } = false;
        public decimal MaxAmount { get; set; } = 0;
        public decimal MinAmount { get; set; } = 0;
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

    }
}