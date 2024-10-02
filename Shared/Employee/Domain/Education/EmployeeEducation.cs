using Shared.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Shared.Employee.Domain.Info;

namespace Shared.Employee.Domain.Education
{
    [Table("HR_EmployeeEducation"), Index("LevelOfEducationId", "DegreeId", "Major", "InstitutionName", "CompanyId", "OrganizationId", IsUnique = false, Name = "IX_HR_EmployeeEducation_NonClusteredIndex")]
    public class EmployeeEducation : BaseModel1
    {
        [Key]
        public long EmployeeEducationId { get; set; }
        [ForeignKey("EmployeeInformation")]
        public long EmployeeId { get; set; }
        public EmployeeInformation EmployeeInformation { get; set; }
        [StringLength(100)]
        public string EmployeeCode { get; set; }
        public int LevelOfEducationId { get; set; }
        public int DegreeId { get; set; }
        [StringLength(50)]
        public string Major { get; set; }
        [StringLength(200)]
        public string InstitutionName { get; set; }
        [StringLength(50)]
        public string Result { get; set; }
        [StringLength(50)]
        public string ScaleDivisionClass { get; set; }
        [StringLength(4)]
        public string YearOfPassing { get; set; }
        [StringLength(100)]
        public string Duration { get; set; }
        public bool IsDelete { get; set; }
    }
}
