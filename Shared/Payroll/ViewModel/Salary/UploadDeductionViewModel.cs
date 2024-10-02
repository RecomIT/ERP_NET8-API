using Shared.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Payroll.ViewModel.Salary
{
    public class UploadDeductionViewModel : BaseViewModel2
    {
        public long Id { get; set; }
        public short Month { get; set; }
        public short Year { get; set; }
        public long EmployeeId { get; set; }
        public long DeductionNameId { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }
        public string Remarks { get; set; }
        // Custom Properties
        public string DeductionName { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string MonthYear { get; set; }
    }
}
