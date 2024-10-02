using System;
using Shared.Models;
using Shared.Helpers.ValidationFilters;
using System.ComponentModel.DataAnnotations;

namespace Shared.Payroll.ViewModel.Tax
{
    public class TaxProcessViewModel : BaseViewModel2
    {
        public long TaxProcessId { get; set; }
        public long? EmployeeId { get; set; }
        [StringLength(100), Required]
        public string ProcessType { get; set; }
        [StringLength(100), Required]
        public string ExecutionOn { get; set; } // All, Branch, Department, Selected Employees
        [RequiredBasedOnProperty("ExecutionOn", "Selected Employees", false, ErrorMessage = "Selected is required")]
        public string SelectedEmployees { get; set; } // When ExecutionOn is selected employees then required
        [Range(1, short.MaxValue)]
        public short SalaryMonth { get; set; }
        [Range(1, short.MaxValue)]
        public short SalaryYear { get; set; }
        [RequiredBasedOnProperty("ExecutionOn", "Branch", true, ErrorMessage = "Process Branch is required")]
        public long? ProcessBranchId { get; set; } // When ExecutionOn is branch then required
        [RequiredBasedOnProperty("ExecutionOn", "Department", true, ErrorMessage = "Process Department is required")]
        public long? ProcessDepartmentId { get; set; } // When ExecutionOn is department then required
        public bool EffectOnSalary { get; set; }
    }
}
