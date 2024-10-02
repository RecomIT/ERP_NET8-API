using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Payroll.DTO.Payment
{
    public class SupplementaryPaymentProcessInfoDTO
    {
        public long PaymentId { get; set; }
        [StringLength(50)]
        public string BatchNo { get; set; }
        public int TotalEmployees { get; set; }
        public short PaymentMonth { get; set; }
        public short PaymentYear { get; set; }
        [Column(TypeName = "date")]
        public DateTime? CutOffDate { get; set; }
        public bool EffectOnPayslip { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalOnceOffAmount { get; set; }
        public bool? IsDisbursed { get; set; }
        [Column(TypeName = "date")]
        public DateTime? PaymentDate { get; set; }
        [StringLength(50)]
        public string StateStatus { get; set; }
        public bool IsApproved { get; set; }
    }
}
