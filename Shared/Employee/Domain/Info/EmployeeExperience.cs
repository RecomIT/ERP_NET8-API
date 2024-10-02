using System;
using Shared.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Employee.Domain.Info
{
    [Table("HR_EmployeeExperience"), Index("EmployeeCode", "ExCompanyname", "CompanyId", "OrganizationId", IsUnique = false, Name = "IX_HR_EmployeeExperience_NonClusteredIndex")]
    public class EmployeeExperience : BaseModel1
    {
        [Key]
        public long EmployeeExperienceId { get; set; }
        [ForeignKey("EmployeeInformation")]
        public long EmployeeId { get; set; }
        [StringLength(100)]
        public string EmployeeCode { get; set; }
        public EmployeeInformation EmployeeInformation { get; set; }
        [StringLength(200)]
        public string ExCompanyname { get; set; }
        [StringLength(200)]
        public string ExCompanyBusinees { get; set; }
        [StringLength(300)]
        public string ExCompanyLocation { get; set; }
        [StringLength(200)]
        public string ExCompanyDepartment { get; set; }
        [StringLength(200)]
        public string ExCompanyDesignation { get; set; }
        [StringLength(200)]
        public string ExCompanyExperience { get; set; }
        public bool IsDelete { get; set; }
        public DateTime? EmploymentFrom { get; set; }
        public DateTime? EmploymentTo { get; set; }
    }
}
