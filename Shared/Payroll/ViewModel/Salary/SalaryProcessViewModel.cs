using Shared.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Payroll.ViewModel.Salary
{
    public class SalaryProcessViewModel : BaseViewModel6
    {
        public long SalaryProcessId { get; set; }
        [StringLength(36)]
        public string SalaryProcessUniqId { get; set; }
        [StringLength(30)]
        public string BatchNo { get; set; }
        [StringLength(150)]
        public string DocFileNumber { get; set; }
        [Column(TypeName = "date")]
        public DateTime? SalaryDate { get; set; }
        public short SalaryMonth { get; set; }
        public short SalaryYear { get; set; }
        [Required, Column(TypeName = "date")]
        public DateTime? ProcessDate { get; set; }
        [Required, Column(TypeName = "date")]
        public DateTime? PaymentDate { get; set; }
        public long? DivisionId { get; set; }
        public long? DepartmentId { get; set; }
        [StringLength(10)]
        public string Gender { get; set; }
        public bool IsDisbursed { get; set; }
        public int TotalEmployees { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAllowance { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalArrearAllowance { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalPFAmount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalPFArrear { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalArrearDeduction { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal OtherDeduction { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalDeduction { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalMonthlyTax { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalBonus { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalGrossPay { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalNetPay { get; set; }
        public short? TotalHoldDays { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? TotalHoldAmount { get; set; }
        public short? TotalUnholdDays { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? TotalUnholdAmount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? TotalTaxDeductedAmount { get; set; }
        // Custom Properties
        public string SalaryMonthYear { get; set; }
        public string DivisionName { get; set; }
        public string DepartmentName { get; set; }
        public string EmployeeIds { get; set; }
        public List<SalaryProcessDetailViewModel> SalaryProcessDetails { get; set; }
    }
}
