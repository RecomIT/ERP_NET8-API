using Microsoft.EntityFrameworkCore;
using Shared.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Shared.Employee.Domain.Info
{
    [Table("HR_EmployeeSkill"), Index("Organization", "TopicCovers", "CompanyId", "OrganizationId", IsUnique = false, Name = "IX_HR_EmployeeSkill_NonClusteredIndex")]
    public class EmployeeSkill : BaseModel1
    {
        [Key]
        public long EmployeeSkillId { get; set; }
        [ForeignKey("EmployeeInformation")]
        public long EmployeeId { get; set; }
        [StringLength(100)]
        public string EmployeeCode { get; set; }
        public EmployeeInformation EmployeeInformation { get; set; }
        [StringLength(200)]
        public string TrainingName { get; set; }
        [StringLength(100)]
        public string Organization { get; set; }
        [StringLength(200)]
        public string Location { get; set; }
        [StringLength(300)]
        public string TopicCovers { get; set; }
        [Column(TypeName = "date")]
        public DateTime? FromDate { get; set; }
        [Column(TypeName = "date")]
        public DateTime? ToDate { get; set; }
        [StringLength(200)]
        public string Duration { get; set; }
        public string SkillCertificateFilePath { get; set; }
        public bool IsDelete { get; set; }
    }
}
