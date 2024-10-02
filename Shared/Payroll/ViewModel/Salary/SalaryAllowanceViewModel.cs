using Shared.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Shared.Payroll.ViewModel.Salary
{
    public class SalaryAllowanceViewModel : BaseModel
    {
        public long SalaryAllowanceId { get; set; }
        public long SalaryProcessId { get; set; }
        public long SalaryProcessDetailId { get; set; }
        public short SalaryMonth { get; set; }
        public short SalaryYear { get; set; }
        public DateTime? SalaryDate { get; set; }
        [Column(TypeName = "decimal(4,2)")]
        public decimal CalculationForDays { get; set; }
        public long EmployeeId { get; set; }
        public long AllowanceNameId { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal ArrearAmount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal AdjustmentAmount { get; set; }
        [StringLength(200)]
        public string Remarks { get; set; }
        public long? PeriodicallyAllowanceId { get; set; }
        public long? MonthlyAllowanceId { get; set; }

        // Custom Properties
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string AllowanceName { get; set; }
    }
}
