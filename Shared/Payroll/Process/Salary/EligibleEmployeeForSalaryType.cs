using System;

namespace Shared.Payroll.Process.Salary
{
    public class EligibleEmployeeForSalaryType
    {
        public int SL { get; set; }
        public long EmployeeId { get; set; }
        public string EmployeeCode { get; set; }
        public string FullName { get; set; }
        public long? BranchId { get; set; }
        public string BranchName { get; set; }
        public long? DivisionId { get; set; }
        public string DivisionName { get; set; }
        public long GradeId { get; set; }
        public string GradeName { get; set; }
        public long DesignationId { get; set; }
        public string DesignationName { get; set; }
        public long InternalDesignationId { get; set; }
        public string InternalDesignationName { get; set; }
        public long DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public long SectionId { get; set; }
        public string SectionName { get; set; }
        public long SubSectionId { get; set; }
        public string SubSectionName { get; set; }
        public long CostCenterId { get; set; }
        public string CostCenterName { get; set; }
        public string CostCenterCode { get; set; }
        public string JobType { get; set; }
        public string Gender { get; set; }
        public string MaritalStatus { get; set; }
        public string PhysicalCondition { get; set; }
        public long? JobCategoryId { get; set; }
        public string JobCategory { get; set; }
        public long? EmployeeTypeId { get; set; }
        public string EmployeeType { get; set; }
        public DateTime? DateOfJoining { get; set; }
        public DateTime? DateOfConfirmation { get; set; }
        public long AccountInfoId { get; set; } = 0;
        public long BankId { get; set; }
        public string BankName { get; set; }
        public long BankBranchId { get; set; }
        public long BankBranchName { get; set; }
        public string RoutingNumber { get; set; }
        public string BankAccount { get; set; }
        public string WalletAgent { get; set; }
        public string WalletNumber { get; set; }
        public DateTime? TerminationDate { get; set; }
        public string TerminationStatus { get; set; }
        public bool IsDiscontinued { get; set; }
        public long NationalityId { get; set; }
        public string Nationality { get; set; }
        public bool IsResidential { get; set; }
        public string NonResidentialType { get; set; } //
        public string Religion { get; set; }
        public int DaysWorked { get; set; }
        public DateTime? PFActiovationDate { get; set; }
        public bool IsActive { get; set; }
        public bool IsApproved { get; set; }
        public bool IsConfirmed { get; set; }
        public DateTime? ConfirmationDate { get; set; }
        public bool IsPFMember { get; set; }
        public DateTime? PFEffectiveDate { get; set; }
        public bool IsMobility { get; set; }

    }
}
