using System.ComponentModel.DataAnnotations;

namespace Shared.Overtime.DTO
{
    public class CreateOvertimePolicyDTO
    {

        [Required, StringLength(200)]
        public string OvertimeName { get; set; }

        [StringLength(200)]
        public string OvertimeNameInBengali { get; set; } = string.Empty;

        [Required, StringLength(100)]
        public string Unit { get; set; }

        [Required, StringLength(100)]
        public string AmountType { get; set; }
        public bool IsFlatAmountType { get; set; } = false;
        public bool IsPercentageAmountType { get; set; } = false;

        [Required]

        public decimal Amount { get; set; } = 0;

        [Required]

        public decimal AmountRate { get; set; } = 1;

        public bool LimitationOfUnit { get; set; } = false;


        public decimal MaxUnit { get; set; } = 0;


        public decimal MinUnit { get; set; } = 0;

        public bool LimitationOfAmount { get; set; } = false;


        public decimal MaxAmount { get; set; } = 0;


        public decimal MinAmount { get; set; } = 0;

    }
}