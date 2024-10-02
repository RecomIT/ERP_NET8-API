using System;
using System.ComponentModel.DataAnnotations;

namespace Shared.Employee.DTO.Info
{
    public class EmployeeSkillDTO
    {
        public long EmployeeSkillId { get; set; }
        public long EmployeeId { get; set; }
        [StringLength(200)]
        public string TrainingName { get; set; }
        [StringLength(100)]
        public string Organization { get; set; }
        [StringLength(200)]
        public string Location { get; set; }
        [StringLength(300)]
        public string TopicCovers { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        [StringLength(200)]
        public string Duration { get; set; }
        public string SkillCertificateFilePath { get; set; }
    }
}
