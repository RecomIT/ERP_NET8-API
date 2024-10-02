using Shared.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Payroll.ViewModel.Tax
{
    public class EmployeeYearlyInvestmentViewModel : BaseViewModel1
    {
        public long Id { get; set; }
        public long FiscalYearId { get; set; }
        [Range(1, long.MaxValue)]
        public long EmployeeId { get; set; }
        [Required, Column(TypeName = "decimal(18,2)")]
        public decimal InvestmentAmount { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeCode { get; set; }
        public string FiscalYearRange { get; set; }
        public string AssesmentYear { get; set; }
    }
}
