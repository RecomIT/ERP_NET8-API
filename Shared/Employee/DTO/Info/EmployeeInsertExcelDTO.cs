using Shared.Models;
using System;


namespace Shared.Employee.DTO.Info
{
    public class EmployeeInsertExcelDTO : BaseViewModel3
    {
        public string EmployeeCode { get; set; }
        public string FullName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public long? DesignationId { get; set; }
        public long? GradeId { get; set; }
        public long? DepartmentId { get; set; }
        public long? SectionId { get; set; }
        public long? SubSectionId { get; set; }
        public DateTime? DateOfJoining { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string OfficeMobile { get; set; }
        public string OfficeEmail { get; set; }
        public string ReferenceNo { get; set; }
        public string FingerID { get; set; }
        public long? BankId { get; set; }
        public long? BankBranchId { get; set; }
        public string AccountNo { get; set; }
        public string RoutingNumber { get; set; }
        public long? WorkShiftId { get; set; }
        public string BloodGroupName { get; set; }
        public string ReligionName { get; set; }
        public string MaritalStatus { get; set; }
        public string PresentAddress { get; set; }
        public string PermanentAddress { get; set; }
        public long? JobTypeId { get; set; }
        public bool IsResidential { get; set; }
        public string Gender { get; set; }
        public string BankCode { get; set; }
        public string JobTypeName { get; set; }
        public bool IsActive { get; set; }

    }
}
