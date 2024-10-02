using Shared.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Payroll.ViewModel.Salary
{
    public class SalaryHoldViewModel : BaseViewModel6
    {
        public long SalaryHoldId { get; set; }
        public long EmployeeId { get; set; }
        public short Month { get; set; }
        public short Year { get; set; }
        public bool? IsHolded { get; set; }
        [StringLength(200)]
        public string HoldReason { get; set; }
        [Column(TypeName = "date")]
        public DateTime? HoldFrom { get; set; }
        [Column(TypeName = "date")]
        public DateTime? HoldTo { get; set; }
        public bool? WithSalary { get; set; }
        public bool? WithoutSalary { get; set; }
        public bool? PFContinue { get; set; }
        public bool? GFContinue { get; set; }
        [Column(TypeName = "date")]
        public DateTime? UnholdDate { get; set; }
        public string UnholdReason { get; set; }
        public long? EmployeeResignationId { get; set; }
        [StringLength(50)]
        public string StateStatus { get; set; }
        public long? IsApproved { get; set; }

        //
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string MonthName { get; set; }
    }
}
