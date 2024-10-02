using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Shared.Payroll.DTO.WalletPayment
{
    public class WalletPaymentConfigurationDTO
    {
        public long WalletConfigId { get; set; }
        [StringLength(100)]
        public string BaseOfPayment { get; set; }
        public long InternalDesignationId { get; set; }
        [StringLength(100)]
        public string InternalDesignationName { get; set; }
        public bool? IsActive { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? WalletFlatAmount { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal? WalletTransferPercentage { get; set; }
        [Column(TypeName = "date")]
        public DateTime? ActivationDate { get; set; }
        [Column(TypeName = "date")]
        public DateTime? DeactivationDate { get; set; }
        public bool? IsApproved { get; set; }
        [StringLength(50)]
        public string StateStatus { get; set; }
        [StringLength(150)]
        public string Remarks { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal? COCInWalletTransferPercentage { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal? COCInWalletTransfer { get; set; }
    }
}
