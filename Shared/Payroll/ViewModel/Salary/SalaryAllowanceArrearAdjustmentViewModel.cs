using Shared.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Payroll.ViewModel.Salary
{
    public class SalaryAllowanceArrearAdjustmentViewModel : BaseViewModel1
    {
        public long Id { get; set; }
        [StringLength(50)]
        public string EmployeeCode { get; set; }
        [StringLength(150)]
        public string EmployeeName { get; set; }
        public long EmployeeId { get; set; }
        public long AllowanceNameId { get; set; }
        [StringLength(100)]
        public string AllowanceName { get; set; }
        public short? SalaryMonth { get; set; }
        [StringLength(50)]
        public string SalaryMonthName { get; set; }
        public short? SalaryYear { get; set; }
        [Column(TypeName = "date")]
        public DateTime? SalaryDate { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? CalculationForDays { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }
        [StringLength(50)]
        public string Flag { get; set; }
        [StringLength(50)]
        public string StateStatus { get; set; }
        public short? ArrearAdjustmentMonth { get; set; }
        public short? ArrearAdjustmentYear { get; set; }
    }
}
