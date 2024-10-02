using System;
using Shared.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Shared.Payroll.ViewModel.Tax
{
    //Income Tax Zone
    public class EmployeeTaxZoneViewModel : BaseViewModel4
    {
        public long EmployeeTaxZoneId { get; set; }
        [Range(1, long.MaxValue)]
        public long EmployeeId { get; set; }
        [Required, StringLength(100)]
        public string TaxZone { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        [Required, Column(TypeName = "decimal(18,2)")]
        public decimal? MinimumTaxAmount { get; set; }
        [Column(TypeName = "date")]
        public DateTime? EffectiveDate { get; set; }
        [Column(TypeName = "date")]
        public DateTime? ActiveDate { get; set; }
        [Column(TypeName = "date")]
        public DateTime? InActiveDate { get; set; }
        [StringLength(50)]
        public string StateStatus { get; set; }
        public bool IsApproved { get; set; }
    }
}
