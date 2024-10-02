using Shared.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Shared.Separation.Models.Domain.Settlement
{
    [Table("Payroll_EmployeeSettlementDeduction")]
    public class EmployeeSettlementDeduction : BaseModel
    {
        [Key]
        public int SettlementDeductionId { get; set; }
        public long? SettlementId { get; set; }
        public int? DeductionId { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? Amount { get; set; }
        public long? ResignationRequestId { get; set; }
        public long? EmployeeId { get; set; }
    }
}
