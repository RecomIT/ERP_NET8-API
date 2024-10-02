using Shared.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Payroll.ViewModel.Variable
{
    public class PeriodicallyVariableAllowanceDetailViewModel : BaseViewModel2
    {
        public long PeriodicallyVariableAllowanceDetailId { get; set; }
        public long AllownanceNameId { get; set; }
        [StringLength(50)]
        public string SalaryVariableFor { get; set; } // Employee / Grade / Designation / All
        public long? EmployeeId { get; set; }
        public long? DesignationId { get; set; }
        public long? GradeId { get; set; }
        //// Duration
        [StringLength(50)]
        public string DurationType { get; set; } // IncomeYear / DateRange
        public long? FiscalYearId { get; set; }
        public DateTime? EffectiveFrom { get; set; }
        [Column(TypeName = "date")]
        public DateTime? EffectiveTo { get; set; }
        //// Amount
        [StringLength(50)]
        public string AmountBaseOn { get; set; } // Gross / Basic / Flat / Principal
        [Column(TypeName = "decimal(18,2)")]
        public decimal? PrincipalAmount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? Amount { get; set; }
        [StringLength(50)]
        public string StateStatus { get; set; }
        public bool IsApproved { get; set; }
        public long PeriodicallyVariableAllowanceInfoId { get; set; }

        //// Custom Properties
        public string AllowanceName { get; set; }
        public string EmployeeName { get; set; }
        public string DesignationName { get; set; }
        public string GradeName { get; set; }

    }
}
