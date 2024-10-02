using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Shared.BaseModels.For_DomainModel;

namespace Shared.Payroll.Domain.Salary
{
    [Table("Payroll_SalaryComponentHistory")]
    public class SalaryComponentHistory : BaseModel
    {
        [Key]
        public long Id { get; set; }
        public long EmployeeId { get; set; }
        public long SalaryProcessId { get; set; }
        public long SalaryProcessDetailId { get; set; }
        public short SalaryMonth { get; set; }
        public short SalaryYear { get; set; }
        /// <summary>
        /// Variable allowance
        /// Variable deduction
        /// Periodical allowance
        /// Periodical deduction
        /// Monthly allowance
        /// Monthly deduction
        /// </summary>
        public string Flag { get; set; }
        public string ComponentId { get; set; }
        public string Amount { get; set; }
    }
}
