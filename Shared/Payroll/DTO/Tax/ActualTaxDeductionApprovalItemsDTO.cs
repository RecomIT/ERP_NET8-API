using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Payroll.DTO.Tax
{
    public class ActualTaxDeductionApprovalItemsDTO
    {
        public long ActualTaxDeductionId { get; set; }
        public long EmployeeId { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal ActualTaxAmount { get; set; }
    }
}
