using System;
using System.ComponentModel.DataAnnotations;

namespace Shared.Control_Panel.ViewModels
{
    public class loginViewModel
    {
        [Required, MinLength(3)]
        public string Username { get; set; }
        [Required, MinLength(3)]
        public string Password { get; set; }
    }
    public class AppUserLoggedInfo
    {
        public string UserId { get; set; }
        public string Username { get; set; }
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public long DivisionId { get; set; }
        public string DivisionName { get; set; }
        public long BranchId { get; set; }
        public string BranchName { get; set; }
        public long EmployeeId { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public long GradeId { get; set; }
        public string GradeName { get; set; }
        public long DesignationId { get; set; }
        public string DesignationName { get; set; }
        public long DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public long SectionId { get; set; }
        public string SectionName { get; set; }
        public long SubSectionId { get; set; }
        public string SubSectionName { get; set; }
        public long CompanyId { get; set; }
        public long OrganizationId { get; set; }
        public long WorkShiftId { get; set; }
        public string WorkShiftName { get; set; }
        public bool? IsDefaultPassword { get; set; }
        public string DefaultCode { get; set; }
        public string PasswordExpiredDate { get; set; }
        public short RemainExpireDays { get; set; }
        public bool IsActive { get; set; }
        public string SiteThumbnailPath { get; set; }
        public string SiteShortName { get; set; }
        public string PhotoPath { get; set; }
        public string CompanyName { get; set; }
        public string OrganizationName { get; set; }
        public DateTime? TerminationDate { get; set; }
    }
}
