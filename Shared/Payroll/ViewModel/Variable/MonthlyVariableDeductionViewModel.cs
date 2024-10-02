using Shared.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Payroll.ViewModel.Variable
{
    public class MonthlyVariableDeductionViewModel : BaseViewModel3
    {
        public long MonthlyVariableDeductionId { get; set; }
        [Required, Range(1, long.MaxValue)]
        public long EmployeeId { get; set; }
        public long? DesignationId { get; set; }
        [Column(TypeName = "date")]
        public DateTime? SalaryMonthYear { get; set; }
        public short? SalaryMonth { get; set; }
        public short? SalaryYear { get; set; }
        [StringLength(50)]
        public string AmountBaseOn { get; set; } // Gross / Basic / Flat / Principal
        [Required, Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? Percentage { get; set; }
        [Required, Range(1, long.MaxValue)]
        public long DeductionNameId { get; set; }
        [Column(TypeName = "date")]
        public DateTime? EffectiveFrom { get; set; }
        [Column(TypeName = "date")]
        public DateTime? EffectiveTo { get; set; }
        [StringLength(50)]
        public string StateStatus { get; set; }
        public bool IsApproved { get; set; }
        [Column(TypeName = "date")]
        // Custom Properties
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string DesignationName { get; set; }
        public string DeductionName { get; set; }
        [Required, StringLength(50)]
        public string DeductionForYearOfMonth { get; set; }
    }
}
