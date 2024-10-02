using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Payroll.DTO.CashSalary
{
    public class UploadCashSalaryDTO
    {
        public long? UploadCashSalaryId { get; set; }
        public long? EmployeeId { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string CashSalaryHeadName { get; set; }
        public short SalaryMonth { get; set; }
        public string SalaryMonthName { get; set; }
        public short SalaryYear { get; set; }
        public decimal Amount { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsApproved { get; set; }
        public string StateStatus { get; set; }
        public long? CashSalaryProcessId { get; set; }
        public long? CashSalaryHeadId { get; set; }
        [Column(TypeName = "date")]
        public DateTime? CreatedDate { get; set; }
        [Column(TypeName = "date")]
        public DateTime? UpdatedDate { get; set; }
    }
}
