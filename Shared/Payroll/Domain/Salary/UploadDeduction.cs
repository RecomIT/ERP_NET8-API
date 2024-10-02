using Shared.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Shared.Payroll.Domain.Salary
{
    [Table("Payroll_UploadDeduction"), Index("EmployeeId", "Month", "Year", "CompanyId", "OrganizationId", IsUnique = false, Name = "IX_Payroll_UploadDeduction_NonClusteredIndex")]
    public class UploadDeduction : BaseModel1
    {
        [Key]
        public long Id { get; set; }
        public short Month { get; set; }
        public short Year { get; set; }
        public long EmployeeId { get; set; }
        public long DeductionNameId { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal AdjustmentAmount { get; set; }
        public string Remarks { get; set; }
    }
}
