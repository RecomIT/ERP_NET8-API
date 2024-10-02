using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Payroll.Helpers.SalaryProcess
{
    public class EligibleEmployeeForSalary
    {
        public int SerialNo { get; set; }
        public long EmployeeId { get; set; }
        public string EmployeeCode { get; set; }
        public string FullName { get; set; }
        public long? GradeId { get; set; }
        public string GradeName { get; set; }
        public long? DesignationId { get; set; }
        public long? DesignationName { get; set; }
        public long? InternalDesignationId { get; set; }
        public long? InternalDesignationName { get; set; }
        public long? DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public long? SectionId { get; set; }
        public string SectionName { get; set; }
        public long SubSectionId { get; set; }
        public string SubSectionName { get; set; }
        public long CostCenterId { get; set; }
        public string CostCenterName { get; set; }
        public string JobCategory { get; set; }
        public long BranchId { get; set; }
        public long BranchName { get; set; }
        public string EmployementType { get; set; }
        [Column(TypeName = "date")]
        public Nullable<DateTime> DateOfJoining { get; set; }
        [Column(TypeName = "date")]
        public Nullable<DateTime> DateOfConfirmation { get; set; }
        [Column(TypeName = "date")]
        public Nullable<DateTime> ContractStartDate { get; set; }
        [Column(TypeName = "date")]
        public Nullable<DateTime> ContractEndDate { get; set; }
        public long? BankId { get; set; }
        public long? BankBranchId { get; set; }
        public string BankAccount { get; set; }
        public string WalletAgent { get; set; }
        public string WalletNumber { get; set; }
        public Nullable<DateTime> TerminationDate { get; set; }
        public string TerminationStatus { get; set; }
        public string NationalityId { get; set; }
        public string Nationality { get; set; }
        public long ReligionId { get; set; }
        public string ReligionName { get; set; }
        public bool IsDiscontinued { get; set; }
        public short DaysWorked { get; set; }
        public Nullable<DateTime> PFActiovationDate { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsApproved { get; set; }
        public bool? IsConfirmed { get; set; }
        public bool? IsPFMember { get; set; }
        public bool? IsResidential { get; set; }
        public bool? IsMobility { get; set; }

    }
}
