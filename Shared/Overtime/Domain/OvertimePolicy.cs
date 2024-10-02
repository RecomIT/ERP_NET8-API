using Shared.BaseModels.For_DomainModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Overtime.Domain
{

    [Table("Payroll_OvertimePolicy")]
    public class OvertimePolicy : BaseModel1
    {
        [Key]
        public long OvertimeId { get; set; }

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
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal AmountRate { get; set; } = 1;




        public bool LimitationOfUnit { get; set; } = false;

        [Column(TypeName = "decimal(18,2)")]
        public decimal MaxUnit { get; set; } = 0;

        [Column(TypeName = "decimal(18,2)")]
        public decimal MinUnit { get; set; } = 0;

        public bool LimitationOfAmount { get; set; } = false;

        [Column(TypeName = "decimal(18,2)")]
        public decimal MaxAmount { get; set; } = 0;

        [Column(TypeName = "decimal(18,2)")]
        public decimal MinAmount { get; set; } = 0;

    }

}
