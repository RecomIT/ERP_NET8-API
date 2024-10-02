using System;
using Shared.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Shared.Payroll.ViewModel.Salary
{
    public class SalaryDeductionViewModel : BaseViewModel1
    {
        public long SalaryDeductionId { get; set; }
        public int SalaryProcessId { get; set; }
        public long SalaryProcessDetailId { get; set; }
        public short SalaryMonth { get; set; }
        public short SalaryYear { get; set; }
        public DateTime? SalaryDate { get; set; }
        [Column(TypeName = "decimal(4,2)")]
        public decimal CalculationForDays { get; set; }
        public long EmployeeId { get; set; }
        public long DeductionNameId { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal ArrearAmount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal AdjustmentAmount { get; set; }
        [StringLength(200)]
        public string Remarks { get; set; }
        public long? PeriodicallyDeductionId { get; set; }
        public long? MonthlyDeductionId { get; set; }

        // Custom Properties
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string DeductionName { get; set; }
    }
}
