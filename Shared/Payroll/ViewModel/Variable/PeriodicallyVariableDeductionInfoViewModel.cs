using Shared.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Payroll.ViewModel.Variable
{
    public class PeriodicallyVariableDeductionInfoViewModel : BaseViewModel4
    {
        public long PeriodicallyVariableDeductionInfoId { get; set; }
        [StringLength(50)]
        public string SalaryVariableFor { get; set; } // Employee / Grade / Designation / All
        [StringLength(50)]
        public string AmountBaseOn { get; set; } // Gross / Basic / Flat / Principal
        [Column(TypeName = "decimal(18,2)")]
        public decimal? PrincipalAmount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? Amount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? Percentage { get; set; }
        [StringLength(50)]
        public string DurationType { get; set; } // IncomeYear / DateRange
        public long? FiscalYearId { get; set; }
        [Column(TypeName = "date")]
        public DateTime? EffectiveFrom { get; set; }
        [Column(TypeName = "date")]
        public DateTime? EffectiveTo { get; set; }
        public bool IsActive { get; set; }
        [StringLength(50)]
        public string StateStatus { get; set; }
        [StringLength(200)]
        public string Remarks { get; set; }
        public bool IsApproved { get; set; }
        public long DeductionNameId { get; set; }
        //// Custom Propeties
        [StringLength(200)]
        public string DeductionName { get; set; }
        public string EmployeeName { get; set; }
        public string DesignationName { get; set; }
        public string GradeName { get; set; }
        public string HeadCount { get; set; }
        public string JsonDetails { get; set; }
        public List<PeriodicalDetails> PeriodicalDetails { get; set; }
    }
}
