using Shared.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Employee.ViewModel.Termination
{
    public class DiscontinuedEmployeeViewModel : BaseViewModel4
    {
        public long DiscontinuedId { get; set; }
        public long EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeCode { get; set; }
        [Column(TypeName = "Date")]
        public DateTime? LastWorkingDate { get; set; }
        [StringLength(100)]
        public string StateStatus { get; set; }
        public bool? CalculateFestivalBonusTaxProratedBasis { get; set; }
        public bool? CalculateProjectionTaxProratedBasis { get; set; }
        [StringLength(50)]
        public string Releasetype { get; set; }
        public bool? IsFullMonthSalaryHold { get; set; }
    }
}
