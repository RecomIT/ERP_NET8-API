using Shared.BaseModels.For_DomainModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Payroll.Domain.Payment
{
    [Table("Payroll_MonthlyAllowanceHistory")]
    public class MonthlyAllowanceHistory: BaseModel
    {
        [Key]
        public long Id { get; set; }
        public long EmployeeId { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Days { get; set; } = 0;
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; } = 0;
        public short Month { get; set; }
        public short Year { get; set; }
        public long SalaryProcessId { get; set; } = 0;
        public long SalaryProcessDetailId { get; set; } = 0;
        public long FiscalYearId { get; set; }

        [ForeignKey("MonthlyAllowanceConfig")]
        public long MonthlyAllowanceConfigId { get; set; }
        public MonthlyAllowanceConfig MonthlyAllowanceConfig { get; set; }

    }
}
