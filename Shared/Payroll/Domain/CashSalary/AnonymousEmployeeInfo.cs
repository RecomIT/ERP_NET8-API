using Shared.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace Shared.Payroll.Domain.CashSalary
{
    [Table("HR_AnonymousEmployeeInfo")]
    public class AnonymousEmployeeInfo : BaseModel2
    {
        [Key]
        public long EmployeeId { get; set; }
        [StringLength(50)]
        public string EmployeeCode { get; set; }
        [StringLength(50)]
        public string FirstName { get; set; }
        [StringLength(50)]
        public string LastName { get; set; }
        [Required, StringLength(100)]
        public string FullName { get; set; }
        [StringLength(10)]
        public string Gender { get; set; }
        public long? GradeId { get; set; }
        public long? DesignationId { get; set; }
        public long? DepartmentId { get; set; }
        public long? SectionId { get; set; }
        public long? SubSectionId { get; set; }
        public long? UnitId { get; set; }
        public long? ZoneId { get; set; }
        public long? CostCenterId { get; set; }
        public long WorkShiftId { get; set; }
        [Column(TypeName = "date")]
        public DateTime? DateOfJoining { get; set; }

        [StringLength(50)]
        public string ContractEndStatus { get; set; }
        [Column(TypeName = "date")]
        public DateTime? ContractEndDate { get; set; }
        [StringLength(100)]
        public string PayrollCardNumber { get; set; }
        [StringLength(100)]
        public string WalletNumber { get; set; }
        [StringLength(50)]
        public string WalletAgent { get; set; }
        [StringLength(100)]
        public string ReferenceNo { get; set; }

        [StringLength(30)]
        public string NIDNo { get; set; }
        [StringLength(300)]
        public string NIDFilePath { get; set; }

        [StringLength(100)]
        public string StateStatus { get; set; }
        public long? DivisionId { get; set; }
        [StringLength(200)]
        public string Remarks { get; set; }
        [StringLength(100)]
        public string JobType { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsApproved { get; set; }
        public bool? IsConfirmed { get; set; }
        [StringLength(50)]
        public string TerminationStatus { get; set; }
        [Column(TypeName = "date")]
        public DateTime? TerminationDate { get; set; }

        [StringLength(100)]
        public string FatherName { get; set; }
        [StringLength(100)]
        public string MotherName { get; set; }
        [StringLength(100)]
        public string SpouseName { get; set; }
        [StringLength(30)]
        public string PersonalMobileNo { get; set; }
        [EmailAddress, StringLength(200)]
        public string PersonalEmailAddress { get; set; }
        public DateTime? DateOfBirth { get; set; }
        [StringLength(5)]
        public string Feet { get; set; }
        [StringLength(5)]
        public string Inch { get; set; }

        [StringLength(100)]
        public string BloodGroup { get; set; }
        [StringLength(100)]
        public string Religion { get; set; }
        [StringLength(10)]
        public string MaritalStatus { get; set; }
        [StringLength(200)]
        public string PresentAddress { get; set; }
        [StringLength(200)]
        public string PermanentAddress { get; set; }
        [StringLength(100)]
        public string EmergencyContactPerson { get; set; }
        [StringLength(50)]
        public string RelationWithEmergencyContactPerson { get; set; }
        [StringLength(30)]
        public string EmergencyContactNo { get; set; }
        [StringLength(200)]
        public string EmergencyContactAddress { get; set; }
        public bool? IsResidential { get; set; }
        public bool? IsPhysicallyDisabled { get; set; }
        public bool? IsFreedomFighter { get; set; }
    }
}
