using Microsoft.EntityFrameworkCore;
using Shared.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Shared.Employee.Domain.Info
{
    [Table("HR_EmployeeDocument"), Index("EmployeeId", "DocumentName", "DocumentNumber", "CompanyId", "OrganizationId", IsUnique = false, Name = "IX_HR_EmployeeDocument_NonClusteredIndex")]
    public class EmployeeDocument : BaseModel1
    {
        [Key]
        public long DocumentId { get; set; }
        public long EmployeeId { get; set; }
        [StringLength(200)]
        public string DocumentName { get; set; } // Birth Certificate // NID // Passport // TIN // Police Verification
        [StringLength(100)]
        public string DocumentNumber { get; set; }
        [StringLength(200)]
        public string ActualFileName { get; set; }
        [StringLength(200)]
        public string FileName { get; set; }
        [StringLength(50)]
        public string FileSize { get; set; }
        [StringLength(300)]
        public string FilePath { get; set; }
        [StringLength(100)]
        public string FileFormat { get; set; }
        public string File { get; set; }
    }
}
