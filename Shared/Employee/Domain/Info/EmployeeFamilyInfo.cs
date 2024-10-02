using Microsoft.EntityFrameworkCore;
using Shared.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Shared.Employee.Domain.Info
{
    [Table("HR_EmployeeFamilyInfo"), Index("Relatation", "Name", "Age", "CompanyId", "OrganizationId", IsUnique = false, Name = "IX_HR_EmployeeFamilyInfo_NonClusteredIndex")]
    public class EmployeeFamilyInfo : BaseModel1
    {
        [Key]
        public long EmployeeFamilyInfoId { get; set; }
        [ForeignKey("EmployeeInformation")]
        public long EmployeeId { get; set; }
        public EmployeeInformation EmployeeInformation { get; set; }
        [StringLength(100)]
        public string Relatation { get; set; }
        [StringLength(100)]
        public string Name { get; set; }
        [StringLength(100)]
        public string Age { get; set; }
        [StringLength(100)]
        public string DateOfBirth { get; set; }
        [StringLength(100)]
        public string LifeStatus { get; set; }
        [StringLength(500)]
        public string DocumentFilePath { get; set; }
    }
}
