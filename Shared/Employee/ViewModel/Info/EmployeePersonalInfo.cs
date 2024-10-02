using System.ComponentModel.DataAnnotations;

namespace Shared.Employee.ViewModel.Info
{
    public class EmployeePersonalInfo
    {
        public long EmployeeId { get; set; }
        public long EmployeeDetailId { get; set; }
        [StringLength(200)]
        public string LegalName { get; set; }
        [StringLength(100)]
        public string FatherName { get; set; }
        [StringLength(100)]
        public string MotherName { get; set; }
        [StringLength(10)]
        public string MaritalStatus { get; set; }
        [StringLength(100)]
        public string SpouseName { get; set; }
        [Required, StringLength(100)]
        public string Gender { get; set; }
        //[Required]
        public string DateOfBirth { get; set; }
        [StringLength(100)]
        public string Religion { get; set; }
        [StringLength(5)]
        public string Feet { get; set; }
        [StringLength(5)]
        public string Inch { get; set; }
        [StringLength(100)]
        public string BloodGroup { get; set; }
        [StringLength(15)]
        public string PersonalMobileNo { get; set; }
        [StringLength(200)]
        public string PersonalEmailAddress { get; set; }
        [StringLength(200)]
        public string PresentAddress { get; set; }
        [StringLength(200)]
        public string PermanentAddress { get; set; }
        public bool IsResidential { get; set; }
        [StringLength(100)]
        public string EmergencyContactPerson { get; set; }
        [StringLength(50)]
        public string RelationWithEmergencyContactPerson { get; set; }
        [StringLength(50)]
        public string EmergencyContactNo { get; set; }
        [StringLength(200)]
        public string EmergencyContactAddress { get; set; }
        [StringLength(200)]
        public string EmergencyContactEmailAddress { get; set; }
        [StringLength(50)]
        public string EmergencyContactPerson2 { get; set; }
        [StringLength(50)]
        public string RelationWithEmergencyContactPerson2 { get; set; }
        [StringLength(30)]
        public string EmergencyContactNo2 { get; set; }
        [StringLength(200)]
        public string EmergencyContactAddress2 { get; set; }
        [StringLength(200)]
        public string EmergencyContactEmailAddress2 { get; set; }
        [StringLength(20)]
        public string NumberOfChild { get; set; }
    }
}
