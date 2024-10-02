using Shared.Models;
using System;
using System.ComponentModel.DataAnnotations;


namespace Shared.Employee.ViewModel.Info
{
    public class EmployeePersonalInfoVM : BaseViewModel2
    {

        [StringLength(100)]
        public string DesignationName { get; set; }
        [StringLength(100)]
        public string DepartmentName { get; set; }
        [Range(1, int.MaxValue)]
        public int? BankId { get; set; }
        [Range(1, int.MaxValue)]
        public int? BankBranchId { get; set; }
        [Required, StringLength(20)]
        public string AccountNo { get; set; }
        public int? BloodGroupId { get; set; }
        [Range(1, int.MaxValue)]
        public int? ReligionId { get; set; }
        [Range(1, int.MaxValue)]
        public int? NationalityId { get; set; }
        public string BankName { get; set; }
        public string BankBranchName { get; set; }
        [StringLength(30)]
        public string PersonalMobileNo { get; set; }
        [EmailAddress, StringLength(200)]
        public string PersonalEmailAddress { get; set; }
        [Required]
        public DateTime? DateOfBirth { get; set; }
        [StringLength(5)]
        public string Feet { get; set; }
        [StringLength(5)]
        public string Inch { get; set; }
        [StringLength(10)]
        public string Gender { get; set; }
        public string BloodGroupName { get; set; }
        public string ReligionName { get; set; }
        public string NationalityName { get; set; }
        [Required, StringLength(10)]
        public string MaritalStatus { get; set; }
        [StringLength(200)]
        public string PresentAddress { get; set; }
        [Required, StringLength(200)]
        public string PermanentAddress { get; set; }
        [StringLength(100)]
        public string FatherName { get; set; }
        [StringLength(100)]
        public string MotherName { get; set; }
        [StringLength(100)]
        public string SpouseName { get; set; }
        [StringLength(100)]
        public string EmergencyContactPerson { get; set; }
        [StringLength(20)]
        public string RelationWithEmergencyContactPerson { get; set; }
        [StringLength(30)]
        public string EmergencyContactNo { get; set; }
        [StringLength(200)]
        public string EmergencyContactAddress { get; set; }
    }
}
