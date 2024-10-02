using Shared.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Payroll.ViewModel.Salary
{
    public class SalaryProcessExecution : BaseViewModel1
    {
        [Required]
        public string ProcessBy { get; set; }
        public string ExecutionOn { get; set; }
        [Range(1, short.MaxValue)]
        public short Month { get; set; }
        [Range(1, short.MaxValue)]
        public short Year { get; set; }
        [Required, Column(TypeName = "date")]
        public DateTime? SalaryDate { get; set; }
        public DateTime? PaymentDate { get; set; }
        public DateTime? ProcessDate { get; set; }
        public string SelectedEmployees { get; set; }
        public long? ProcessBranchId { get; set; }
        public long? ProcessDepartmentId { get; set; }
        public long? BranchId { get; set; }
        public bool? WithTaxProcess { get; set; }
        public bool? IsMargeProcess { get; set; }
    }
}
