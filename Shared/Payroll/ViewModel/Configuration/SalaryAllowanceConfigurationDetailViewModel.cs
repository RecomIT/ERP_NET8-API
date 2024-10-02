using System;
using Shared.Models;
using Shared.Helpers.ValidationFilters;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Payroll.ViewModel.Configuration
{
    public class SalaryAllowanceConfigurationDetailViewModel : BaseModel2
    {
        public long SalaryAllowanceConfigDetailId { get; set; }
        public string AllowanceBase { get; set; }
        [StringLength(100)]
        public long AllowanceNameId { get; set; }
        [StringLength(100)]
        public string AllowanceName { get; set; }
        public long? EmployeeId { get; set; }
        public long? GradeId { get; set; }
        public long? DesignationId { get; set; }
        public string JobType { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? Percentage { get; set; }
        public bool? IsFixed { get; set; }
        public bool IsActive { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? Amount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? MaxAmount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? MinAmount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? AdditionalAmount { get; set; }
        public bool? IsApproved { get; set; }
        public bool IsPeriodically { get; set; }
        [RequiredIfTrue(nameof(IsPeriodically))]
        public DateTime? EffectiveFrom { get; set; }
        [RequiredIfTrue(nameof(IsPeriodically))]
        public DateTime? EffectiveTo { get; set; }
        public long SalaryAllowanceConfigId { get; set; }
    }
}
