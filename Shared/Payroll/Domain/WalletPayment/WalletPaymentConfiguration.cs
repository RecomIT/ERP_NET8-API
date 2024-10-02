using Shared.Models;
using System.ComponentModel.DataAnnotations;
using Shared.Employee.Domain.InternalDesignation;
using System.ComponentModel.DataAnnotations.Schema;
namespace Shared.Payroll.Domain.WalletPayment
{
    [Table("Payroll_WalletPaymentConfiguration")]
    public class WalletPaymentConfiguration : BaseModel4
    {
        [Key]
        public long WalletConfigId { get; set; }
        [ForeignKey("InternalDesignation")]
        public long InternalDesignationId { get; set; }
        public InternalDesignation InternalDesignation { get; set; }
        [StringLength(100)]
        public string BaseOfPayment { get; set; }
        public bool? IsActive { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? WalletFlatAmount { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal? WalletTransferPercentage { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal? COCInWalletTransferPercentage { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal? COCInWalletTransfer { get; set; }
        [Column(TypeName = "date")]
        public DateTime? ActivationDate { get; set; }
        [Column(TypeName = "date")]
        public DateTime? DeactivationDate { get; set; }
        public bool? IsApproved { get; set; }
        [StringLength(50)]
        public string StateStatus { get; set; }
        [StringLength(150)]
        public string Remarks { get; set; }

    }
}
