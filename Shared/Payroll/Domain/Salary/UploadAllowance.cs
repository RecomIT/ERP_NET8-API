using Shared.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Payroll.Domain.Salary
{
    [Table("Payroll_UploadAllowance"), Index("EmployeeId", "AllowanceNameId", "Month", "Year", "CompanyId", "OrganizationId", IsUnique = false, Name = "IX_Payroll_UploadAllowance_NonClusteredIndex")]
    public class UploadAllowance : BaseModel1
    {
        [Key]
        public long Id { get; set; }
        public short Month { get; set; }
        public short Year { get; set; }
        public long EmployeeId { get; set; }
        public long AllowanceNameId { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal ArrearAmount { get; set; }
        public string Remarks { get; set; }
    }
}
