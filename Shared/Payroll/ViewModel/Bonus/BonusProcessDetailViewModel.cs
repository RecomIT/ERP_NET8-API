using System;
using Shared.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Payroll.ViewModel.Bonus
{
    public class BonusProcessDetailViewModel : BaseViewModel1
    {
        public long BonusProcessDetailId { get; set; }
        [StringLength(50)]
        public string BatchNo { get; set; }
        public string BonusName { get; set; }
        public string BonusConfigCode { get; set; }
        public long EmployeeId { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? OnceOffTax { get; set; }
        public short BonusMonth { get; set; }
        public short BonusYear { get; set; }
        [Column(TypeName = "date")]
        public DateTime? ProcessDate { get; set; }
        public long BonusProcessId { get; set; }
        public long BonusId { get; set; }
        public bool IsDisbursed { get; set; }
        // prop
        public string EmployeeName { get; set; }
        public string EmployeeCode { get; set; }
        public string DesignationName { get; set; }
        public string DepartmentName { get; set; }
        public string FiscalYearRange { get; set; }
        public string BonusMonthName { get; set; }
    }
}
