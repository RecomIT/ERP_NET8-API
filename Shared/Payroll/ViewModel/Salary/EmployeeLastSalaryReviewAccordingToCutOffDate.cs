using System;
using System.ComponentModel.DataAnnotations;

namespace Shared.Payroll.ViewModel.Salary
{
    public class EmployeeLastSalaryReviewAccordingToCutOffDate
    {
        public long EmployeeId { get; set; }
        [StringLength(100)]
        public string EmployeeCode { get; set; }
        [StringLength(100)]
        public string FullName { get; set; }
        public long SalaryReviewInfoId { get; set; }
        public decimal? CurrentSalaryAmount { get; set; }
        public DateTime? ActivationDate { get; set; }
    }
}
