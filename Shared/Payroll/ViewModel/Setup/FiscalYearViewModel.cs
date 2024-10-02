using System;
using Shared.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Payroll.ViewModel.Setup
{
    public class FiscalYearViewModel : BaseViewModel3
    {
        public long FiscalYearId { get; set; }
        [Required, StringLength(100)]
        public string FiscalYearRange { get; set; }
        [StringLength(100)]
        public string AssesmentYear { get; set; }
        [Column(TypeName = "date")]
        public DateTime? FiscalYearFrom { get; set; }
        [Column(TypeName = "date")]
        public DateTime? FiscalYearTo { get; set; }
        public long? AuthorizedBy { get; set; }
        public long? AuthorizerDesignationId { get; set; }
        [MaxLength]
        public byte[] AuthorizedSignForTax { get; set; }
        [StringLength(50)]
        public string StateStatus { get; set; }
        public bool IsApproved { get; set; }
    }
}
