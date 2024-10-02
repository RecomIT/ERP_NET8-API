using Shared.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Employee.ViewModel.Stage
{
    public class EmployeeSalaryBreakDownViewModel : BaseViewModel3
    {
        public long SalaryBreakDownId { get; set; }
        public long EmployeeId { get; set; }
        public long? GradeId { get; set; }
        public long? DesignationId { get; set; }
        public long? DepartmentId { get; set; }
        public long? AllowanceHeadId { get; set; }
        [StringLength(20)]
        public string AllowanceHead { get; set; }
        public long? AllowanceNameId { get; set; }
        [StringLength(20)]
        public string AllowanceName { get; set; }
        [StringLength(20)]
        public string PercentageOfTotalSalary { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }
        [StringLength(20)]
        public string AmountBN { get; set; }
        [Column(TypeName = "date")]
        public DateTime? ActivationDate { get; set; }
        [Column(TypeName = "date")]
        public DateTime? DeactivationDate { get; set; }
        public bool IsActive { get; set; }
        public bool? IsApproved { get; set; }
        [StringLength(50)]
        public string StateStatus { get; set; }
        [StringLength(50)]
        public string Flag { get; set; } // Joining / Increment
    }
}
