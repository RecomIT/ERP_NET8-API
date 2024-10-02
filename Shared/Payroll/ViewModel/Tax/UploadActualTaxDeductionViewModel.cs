using Shared.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Payroll.ViewModel.Tax
{
    public class ActualTaxDeductionViewModel : BaseViewModel1
    {
        public long ActualTaxDeductionId { get; set; }
        public long FiscalYearId { get; set; }
        public long EmployeeId { get; set; }
        public short SalaryMonth { get; set; }
        public short SalaryYear { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal ActualTaxAmount { get; set; }
        [StringLength(500)]
        public string ActualFileName { get; set; }
        [StringLength(500)]
        public string SystemFileName { get; set; }
        [StringLength(50)]
        public string FileFormat { get; set; }
        [StringLength(50)]
        public string StateStatus { get; set; }
        public bool? IsApproved { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string MonthName { get; set; }
        public string FiscalYearRange { get; set; }
    }
}
