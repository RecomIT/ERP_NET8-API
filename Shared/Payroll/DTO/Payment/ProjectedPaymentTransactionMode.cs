using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace Shared.Payroll.DTO.Payment
{
    public class ProjectedPaymentTransactionMode
    {
        public long ProjectedAllowanceId { get; set; }
        public long EmployeeId { get; set; }
        public long? EmployeeAccountId { get; set; }
        public long? EmployeeWalletId { get; set; }
        [StringLength(50)]
        public string PaymentMode { get; set; }
        [StringLength(150)]
        public string BankName { get; set; }
        [StringLength(150)]
        public string BankBranchName { get; set; }
        [StringLength(100)]
        public string BankAccountNumber { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? BankTransferAmount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? WalletTransferAmount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? COCInWalletTransfer { get; set; }
        [StringLength(50)]
        public string WalletNumber { get; set; }
        [StringLength(50)]
        public string WalletAgent { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? CashAmount { get; set; }
        [StringLength(100)]
        public string PayrollCardNumber { get; set; }
        [Column(TypeName = "decimal(18,6)")]
        public decimal? COCPercentage { get; set; }

    }

}
