
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Payroll.DTO.Payment
{
    public class SupplementaryProcessExcelReadDTO
    {
        [Required]
        public long AllowanceNameId { get; set; }
        [Required]
        public short PaymentMonth { get; set; }
        [Required]
        public short PaymentYear { get; set; }
        [Required]
        public string EmployeeCode { get; set; }
        public long EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string Designation { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Required]
        public decimal? Amount { get; set; }
        [Required]
        public string PaymentMode { get; set; }
        public bool? WithCOC { get; set; }

        public string PaymentAmountIds { get; set; }
        public string ProcessType { get; set; }   
        public bool? ShowInPayslip { get; set; }
        public bool? ShowInSalarySheet { get; set; }
    }
}
