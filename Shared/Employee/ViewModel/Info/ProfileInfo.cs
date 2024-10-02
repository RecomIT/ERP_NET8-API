using System;
using System.ComponentModel.DataAnnotations;

namespace Shared.Employee.ViewModel.Info
{
    public class ProfileInfo
    {
        public long EmployeeId { get; set; }
        public long BranchId { get; set; }
        [StringLength(100)]
        public string EmployeeCode { get; set; }
        [StringLength(100)]
        public string EmployeeName { get; set; }
        [StringLength(100)]
        public string BranchName { get; set; }
        [StringLength(100)]
        public string GradeName { get; set; }
        [StringLength(100)]
        public string DesignationName { get; set; }
        //[Range(1, int.MaxValue)]
        [StringLength(100)]
        public string DepartmentName { get; set; }
        [StringLength(100)]
        public string SectionName { get; set; }
        [StringLength(100)]
        public string SubSectionName { get; set; }
        public DateTime? DateOfJoining { get; set; }
        public DateTime? DateOfBirth { get; set; }
        [StringLength(100)]
        public string Workshift { get; set; }
        [StringLength(15)]
        public string OfficeMobile { get; set; }
        [StringLength(200)]
        public string OfficeEmail { get; set; }
        [StringLength(100)]
        public string ReferenceNo { get; set; }
        [StringLength(100)]
        public string FingerId { get; set; }
        [StringLength(30)]
        public string JobType { get; set; }
        public bool IsActive { get; set; }
        public string StateStatus { get; set; }
        [StringLength(100)]
        public string Gender { get; set; }
        [StringLength(200)]
        public string PhotoPath { get; set; }
    }
}
