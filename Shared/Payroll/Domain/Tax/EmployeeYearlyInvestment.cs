using Shared.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Payroll.Domain.Tax
{
    [Table("Payroll_EmployeeYearlyInvestment"), Index("EmployeeId", "FiscalYearId", "CompanyId", "OrganizationId", IsUnique = false, Name = "IX_Payroll_EmployeeYearlyInvestment_NonClusteredIndex")]
    public class EmployeeYearlyInvestment : BaseModel1
    {
        [Key]
        public long Id { get; set; }
        public long FiscalYearId { get; set; }
        public long EmployeeId { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal InvestmentAmount { get; set; }
    }
}
