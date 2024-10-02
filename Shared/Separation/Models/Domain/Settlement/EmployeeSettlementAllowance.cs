using Shared.BaseModels.For_DomainModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Separation.Models.Domain.Settlement
{
    [Table("Payroll_EmployeeSettlementAllowance")]
    public class EmployeeSettlementAllowance : BaseModel
    {
        [Key]
        public int SettlementAllowanceId { get; set; }
        public long? SettlementId { get; set; }
        public int? AllowanceId { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? Amount { get; set; }
        public int? DueAllowanceId { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? DueAmount { get; set; }
        public long? ResignationRequestId { get; set; }
        public long? EmployeeId { get; set; }
    }
}
