using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Payroll.DTO.Payment
{
    public class EmployeeProjectedPaymentDTO
    {
        public long ProjectedAllowanceId { get; set; }
        [StringLength(100)]
        public string ProjectedAllowanceCode { get; set; } // PA-0000001
        [Range(1, long.MaxValue)]
        public long EmployeeId { get; set; }
        [StringLength(50)]
        public string EmployeeCode { get; set; }
        public long? FiscalYearId { get; set; }
        public short? PaymentMonth { get; set; }
        public short? PaymentYear { get; set; }
        [Range(1,short.MaxValue)]
        public short? PayableYear { get; set; }
        public long? AllowanceHeadId { get; set; }
        [Range(1, long.MaxValue)]
        public long AllowanceNameId { get; set; }
        [Required,StringLength(150)]
        public string AllowanceReason { get; set; }
        [StringLength(50)]
        public string BaseOfPayment { get; set; } // Flat(5000/=) / Basic (50%) / Gross
        [Column(TypeName = "decimal(18,2)")]
        public decimal? Percentage { get; set; } // Basic/Gross = 100%
        [Column(TypeName = "decimal(18,2)")]
        public decimal? Amount { get; set; } // 5000/=
        [Column(TypeName = "decimal(18,2)")]
        public decimal? PayableAmount { get; set; } // Basic 50% 10000/=
        [Column(TypeName = "decimal(18,2)")]
        public decimal? DisbursedAmount { get; set; } // 8500/=
        [Column(TypeName = "decimal(18,2)")]
        public decimal? TaxAmount { get; set; } // 8500/=
        [StringLength(100)]
        public string StateStatus { get; set; } // Pending / Approved / Processed / Disbursed
        public long? ProjectedAllowanceProcessInfoId { get; set; }

        //Added by Monzur 04-Jan-2024
        [StringLength(50)]
        public string PaymentMode { get; set; }
        public bool? WithCOC { get; set; }

    }
}
