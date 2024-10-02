using Microsoft.EntityFrameworkCore;
using Shared.BaseModels.For_DomainModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Payroll.Domain.Payment
{
    [Table("Payroll_DepositAllowanceHistory"), Index("EmployeeId", "AllowanceNameId", "CompanyId", "OrganizationId", IsUnique = false, Name = "IX_Payroll_DepositAllowanceHistory_NonClusteredIndex")]
    public class DepositAllowanceHistory : BaseModel2
    {
        public long Id { get; set; }
        public long EmployeeId { get; set; }
        public long? AllowanceNameId { get; set; }
        public long? FiscalYearId { get; set; }
        public int? DepositMonth { get; set; }
        public int? DepositYear { get; set; }
        public short? PayableDays { get; set; }
        [Column(TypeName = "date")]
        public Nullable<DateTime> DepositDate { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal BaseAmount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Arrear { get; set; }
        [StringLength(50)]
        public string IncomingFlag { get; set; } // Conditional / Individual
        public long? SalaryProcessId { get; set; }
        public long? SalaryProcessDetailId { get; set; }
        public long? EmployeeDepositAllowanceConfigId { get; set; }
        public long? ConditionalDepositAllowanceConfigId { get; set; }
    }
}
