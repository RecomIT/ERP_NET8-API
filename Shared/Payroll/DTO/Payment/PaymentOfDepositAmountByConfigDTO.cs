using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Payroll.DTO.Payment
{
    public class PaymentOfDepositAmountByConfigDTO
    {
        public long Id { get; set; }
        [Range(1,long.MaxValue)]
        public long EmployeeId { get; set; }
        [Range(1, long.MaxValue)]
        public long AllowanceNameId { get; set; }
        [Range(1, short.MaxValue)]
        public int PaymentMonth { get; set; }
        [Range(1, short.MaxValue)]
        public int PaymentYear { get; set; }
        [Column(TypeName = "date")]
        public Nullable<DateTime> PaymentDate { get; set; }
        [StringLength(50)]
        public string PaymentApproach { get; set; }
        [StringLength(50)]
        public string PaymentBeMade { get; set; }
        public long? FiscalYearId { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal ProposalAmount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal PayableAmount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal DisbursedAmount { get; set; }
        [StringLength(50)]
        public string StateStatus { get; set; }
        public long? EmployeeDepositAllowanceConfigId { get; set; }
        public long? ConditionalDepositAllowanceConfigId { get; set; }
    }
}
