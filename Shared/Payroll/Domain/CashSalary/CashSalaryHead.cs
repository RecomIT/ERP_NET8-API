using Shared.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Shared.Payroll.Domain.CashSalary
{

    [Table("Payroll_CashSalaryHead")]
    public class CashSalaryHead : BaseModel1
    {
        [Key]
        public long CashSalaryHeadId { get; set; }
        [Required, StringLength(200)]
        public string CashSalaryHeadName { get; set; }
        [StringLength(20)]
        public string CashSalaryHeadCode { get; set; }
        [StringLength(200)]
        public string CashSalaryHeadNameInBengali { get; set; }
        public bool? IsActive { get; set; }
    }

}
