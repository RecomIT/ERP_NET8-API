using Microsoft.AspNetCore.Http;
using Shared.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Shared.Employee.ViewModel.Info
{
    public class EmployeeInformationViewModel : BaseViewModel3
    {
        public long EmployeeId { get; set; }
        [StringLength(50)]
        public string EmployeeCode { get; set; }
        // Basic Info
        [Required, StringLength(100)]
        public string FullName { get; set; }
        //[Range(1,int.MaxValue)]
        public int? GradeId { get; set; }
        [Range(1, int.MaxValue)]
        public int? DesignationId { get; set; }
        public long? InternalDesignationId { get; set; }
        [Range(1, int.MaxValue)]
        //[Range(1, int.MaxValue)]
        public long DivisionId { get; set; }
        //[Range(1, int.MaxValue)]
        public int? DepartmentId { get; set; }
        public int? SectionId { get; set; }
        public int? SubSectionId { get; set; }
        public long? DivisionalHead { get; set; }
        public long? HeadOfDepartment { get; set; }
        public long? LineManager { get; set; }
        public long? Supervisor { get; set; }
        public string SupervisorName { get; set; }
        public long? LeadManagement { get; set; }
        public long? HRAuthority { get; set; }
        public decimal? Gross { get; set; }
        //[Range(0.00, double.MaxValue)]
        public decimal? Basic { get; set; }
        //[Range(0.00, double.MaxValue)]
        public decimal? HouseRent { get; set; }
        //[Range(0.00, double.MaxValue)]
        public decimal? Medical { get; set; }
        //[Range(0.00, double.MaxValue)]
        public decimal? Conveyance { get; set; }
        // Other Information
        [Range(0, long.MaxValue)]
        public long WorkShiftId { get; set; }
        public DateTime? DateOfJoining { get; set; }
        public DateTime? DateOfConfirmation { get; set; }
        //[Range(0, long.MaxValue)]
        public long? JobStatusId { get; set; }
        [StringLength(100)]
        public string Taxzone { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? MinimumTaxAmount { get; set; }
        [StringLength(15)]
        public string OfficeMobile { get; set; }
        [EmailAddress, StringLength(200)]
        public string OfficeEmail { get; set; }
        [StringLength(100)]
        public string ReferenceNo { get; set; }
        [StringLength(100)]
        public string FingerID { get; set; }
        [StringLength(100)]
        public string TINNo { get; set; }
        public IFormFile TINFile { get; set; }
        [StringLength(300)]
        public string TINFilePath { get; set; }
        [StringLength(30)]
        public string NIDNo { get; set; }
        public IFormFile NIDFile { get; set; }
        [StringLength(300)]
        public string NIDFilePath { get; set; }
        [StringLength(30)]
        public string PassportNo { get; set; }
        public IFormFile PassportFile { get; set; }
        [StringLength(300)]
        public string PassportFilePath { get; set; }
        [Range(0.00, double.MaxValue)]
        public double? MobileAllowance { get; set; }
        [StringLength(100)]
        public string StateStatus { get; set; }
        public int? JobLocationId { get; set; }
        public int? BankId { get; set; }
        public int? BankBranchId { get; set; }
        [StringLength(20)]
        public string AccountNo { get; set; }
        public int? BloodGroupId { get; set; }
        public int? ReligionId { get; set; }
        public int? NationalityId { get; set; }
        public long CurrentWorkShiftId { get; set; }
        public int? JobTypeId { get; set; }
        [StringLength(30)]
        public string JobType { get; set; }
        //
        public string GradeName { get; set; }
        public string DesignationName { get; set; }
        public string DivisionName { get; set; }
        public string DepartmentName { get; set; }
        public string SectionName { get; set; }
        public string SubSectionName { get; set; }
        public string DivisionalHeadName { get; set; }
        public string HeadOfDepartmentName { get; set; }
        public string LineManagerName { get; set; }
        public string LeadManagementName { get; set; }
        public string HRAuthorityName { get; set; }
        public string WorkShiftName { get; set; }
        public string JobStatusName { get; set; }
        public string JobLocationName { get; set; }
        public string BankName { get; set; }
        public string BankBranchName { get; set; }
        [StringLength(30)]
        public string PersonalMobileNo { get; set; }
        [EmailAddress, StringLength(200)]
        public string PersonalEmailAddress { get; set; }
        public DateTime? DateOfBirth { get; set; }
        [StringLength(5)]
        public string Feet { get; set; }
        [StringLength(5)]
        public string Inch { get; set; }
        [Required, StringLength(10)]
        public string Gender { get; set; }
        public string BloodGroupName { get; set; }
        public string ReligionName { get; set; }
        public string NationalityName { get; set; }
        [StringLength(10)]
        public string MaritalStatus { get; set; }
        [StringLength(200)]
        public string PresentAddress { get; set; }
        [StringLength(200)]
        public string PermanentAddress { get; set; }
        [StringLength(100)]
        public string FatherName { get; set; }
        [StringLength(100)]
        public string MotherName { get; set; }
        [StringLength(100)]
        public string SpouseName { get; set; }
        [StringLength(100)]
        public string EmergencyContactPerson { get; set; }
        [StringLength(50)]
        public string RelationWithEmergencyContactPerson { get; set; }
        [StringLength(30)]
        public string EmergencyContactNo { get; set; }
        [StringLength(200)]
        public string EmergencyContactAddress { get; set; }
        [StringLength(200)]
        public string Remarks { get; set; }
        [StringLength(50)]
        public string JobTypeName { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string ServiceTenure { get; set; }
        public long? PreviousReviewId { get; set; }
        public bool IsActive { get; set; }
    }
}
