using System.ComponentModel.DataAnnotations;

namespace Shared.Payroll.DTO.Payment
{
    public class DisbursedOrUndoPaymentDTO
    {
        [Range(1,long.MaxValue)]
        public long Id { get; set; }
        public Nullable<DateTime> PaymentDate { get; set; }
        [Required,StringLength(10)]
        public string Status { get; set; } // Disbursed / Undo
    }
}
