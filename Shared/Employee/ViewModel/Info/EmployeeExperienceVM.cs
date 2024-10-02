using Shared.Models;
using System;
using System.ComponentModel.DataAnnotations;


namespace Shared.Employee.ViewModel.Info
{
    public class EmployeeExperienceVM : BaseViewModel1
    {
        public long EmployeeExperienceId { get; set; }
        [Range(1, long.MaxValue)]
        public long EmployeeId { get; set; }
        [StringLength(100)]
        public string EmployeeCode { get; set; }
        [Required, StringLength(200)]
        public string ExCompanyname { get; set; }
        [Required, StringLength(200)]
        public string ExCompanyBusinees { get; set; }
        [Required, StringLength(300)]
        public string ExCompanyLocation { get; set; }
        [Required, StringLength(200)]
        public string ExCompanyDepartment { get; set; }
        [Required, StringLength(200)]
        public string ExCompanyDesignation { get; set; }
        [StringLength(200)]
        public string ExCompanyExperience { get; set; }
        [Required]
        public DateTime? EmploymentFrom { get; set; }
        [Required]
        public DateTime? EmploymentTo { get; set; }
    }
}
