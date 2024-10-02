using System;
using Shared.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Shared.Employee.Domain.Info
{
    [Table("HR_EmployeeDetail"), Index("EmployeeId", "Gender", "Religion", "MaritalStatus", "IsResidential", "IsPhysicallyDisabled", "IsFreedomFighter", "IsMobility", "CompanyId", "OrganizationId", IsUnique = false, Name = "IX_HR_EmployeeDetail_NonClusteredIndex")]
    public class EmployeeDetail : BaseModel1
    {
        [Key]
        public long EmployeeDetailId { get; set; }
        public long EmployeeId { get; set; }
        [StringLength(200)]
        public string LegalName { get; set; }
        [StringLength(100)]
        public string FatherName { get; set; }
        [StringLength(100)]
        public string MotherName { get; set; }
        [StringLength(100)]
        public string SpouseName { get; set; }
        [StringLength(200)]
        public string PersonalMobileNo { get; set; }
        [EmailAddress, StringLength(200)]
        public string PersonalEmailAddress { get; set; }
        [EmailAddress, StringLength(200)]
        public string AlternativeEmailAddress { get; set; }
        public DateTime? DateOfBirth { get; set; }
        [StringLength(5)]
        public string Feet { get; set; }
        [StringLength(5)]
        public string Inch { get; set; }
        [StringLength(10)]
        public string Gender { get; set; }
        [StringLength(100)]
        public string BloodGroup { get; set; }
        [StringLength(100)]
        public string Religion { get; set; }
        public int? NationalityId { get; set; }
        [StringLength(10)]
        public string MaritalStatus { get; set; }
        [StringLength(200)]
        public string PresentAddress { get; set; }
        [StringLength(100)]
        public string PresentAddressCity { get; set; }
        [StringLength(100)]
        public string PresentAddressZipCode { get; set; }
        [StringLength(100)]
        public string PresentAddressContactNo { get; set; }
        [StringLength(200)]
        public string PermanentAddress { get; set; }
        [StringLength(200)]
        public string PermanentAddressDistrict { get; set; }
        [StringLength(200)]
        public string PermanentAddressUpazila { get; set; }
        [StringLength(200)]
        public string PermanentAddressZipCode { get; set; }
        [StringLength(200)]
        public string PermanentAddressContactNumber { get; set; }
        [StringLength(100)]
        public string EmergencyContactPerson { get; set; }
        [StringLength(50)]
        public string RelationWithEmergencyContactPerson { get; set; }
        [StringLength(50)]
        public string EmergencyContactNo { get; set; }
        [StringLength(200)]
        public string EmergencyContactAddress { get; set; }
        [StringLength(30)]
        public string EmergencyContactFax { get; set; }
        [EmailAddress, StringLength(200)]
        public string EmergencyContactEmailAddress { get; set; }
        [StringLength(100)]
        public string EmergencyContactPerson2 { get; set; }
        [StringLength(50)]
        public string RelationWithEmergencyContactPerson2 { get; set; }
        [StringLength(50)]
        public string EmergencyContactNo2 { get; set; }
        [StringLength(200)]
        public string EmergencyContactAddress2 { get; set; }
        [StringLength(30)]
        public string EmergencyContactFax2 { get; set; }
        [EmailAddress, StringLength(200)]
        public string EmergencyContactEmailAddress2 { get; set; }
        public bool? IsResidential { get; set; }
        public bool? IsPhysicallyDisabled { get; set; }
        public bool? IsFreedomFighter { get; set; }
        public bool? IsMobility { get; set; }
        [StringLength(100)]
        public string Photo { get; set; }
        [StringLength(200)]
        public string PhotoPath { get; set; }
        [StringLength(50)]
        public string PhotoSize { get; set; }
        [StringLength(100)]
        public string PhotoFormat { get; set; }
        [StringLength(200)]
        public string ActualPhotoName { get; set; }
        [StringLength(100)]
        public string Taxzone { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? MinimumTaxAmount { get; set; }
        [StringLength(10)]
        public string NumberOfChild { get; set; }
    }
}
