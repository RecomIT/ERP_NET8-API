using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Payroll.DTO.Tax
{
    public class TaxChallanDTO
    {
        public long TaxChallanId { get; set; }
        [Range(1, long.MaxValue, ErrorMessage = "Employee id is missing")]
        public long EmployeeId { get; set; }
        [StringLength(50)]
        public string EmployeeCode { get; set; }
        [Required(ErrorMessage = "Challan number is missing"),StringLength(100,ErrorMessage = "Challan number character length upto 100 is not allowed")]
        public string ChallanNumber { get; set; }
        [Column(TypeName = "date"),Required(ErrorMessage = "Challan date is required")]
        public Nullable<DateTime> ChallanDate { get; set; }
        [Required(ErrorMessage= "Deposite bank is missing"),StringLength(100,ErrorMessage = "Deposite bank character length upto 100 is not allowed")]
        public string DepositeBank { get; set; }
        [Required(ErrorMessage = "Deposite bank branch is missing"), StringLength(100, ErrorMessage = "Bank bank character length upto 100 is not allowed")]
        public string DepositeBranch { get; set; }
        [Range(1,long.MaxValue,ErrorMessage = "Income year is missing")]
        public long FiscalYearId { get; set; }
        [Range(1, short.MaxValue, ErrorMessage = "Tax month is missing")]
        public short TaxMonth { get; set; }
        [Range(1, short.MaxValue, ErrorMessage = "Tax year is missing")]
        public short TaxYear { get; set; }
        [Column(TypeName = "decimal(18,2)"),Required(ErrorMessage = "Deposite amount is missing")]
        public decimal Amount { get; set; }
        public short Month { get; set; } = 0;
    }
}
