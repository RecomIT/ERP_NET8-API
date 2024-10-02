using System;
using System.ComponentModel.DataAnnotations;

namespace Shared.Employee.DTO.Info
{
    public class EmployeeExperienceDTO
    {
        public long EmployeeExperienceId { get; set; }
        public long EmployeeId { get; set; }
        [StringLength(100)]
        public string EmployeeCode { get; set; }
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
        public DateTime? EmploymentFrom { get; set; }
        public DateTime? EmploymentTo { get; set; }
    }
}
