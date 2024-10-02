using Shared.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Shared.Payroll.Domain.CashSalary
{

    [Table("Payroll_CashSalaryProcess")]
    public class CashSalaryProcess : BaseModel5
    {
        [Key]
        public long CashSalaryProcessId { get; set; }
        [StringLength(100)]
        public string BatchNo { get; set; } // BH-202308001 BH-202308002 BH-202308003     
        [Column(TypeName = "date")]
        public DateTime? SalaryDate { get; set; }
        public short SalaryMonth { get; set; }
        public short SalaryYear { get; set; }
        [Column(TypeName = "date")]
        public DateTime? ProcessDate { get; set; }
        [Column(TypeName = "date")]
        public DateTime? PaymentDate { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal CashAmount { get; set; }
        public bool? IsDisbursed { get; set; }
    }
}
