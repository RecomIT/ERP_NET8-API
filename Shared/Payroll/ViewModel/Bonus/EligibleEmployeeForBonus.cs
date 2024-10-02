using System;

namespace Shared.Payroll.ViewModel.Bonus
{
    public class EligibleEmployeeForBonus
    {
        public long EmployeeId { get; set; }
        public string EmployeeCode { get; set; }
        public string FullName { get; set; }
        public long GradeId { get; set; }
        public string GradeName { get; set; }
        public long DesignationId { get; set; }
        public string DesignationName { get; set; }
        public long DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public long? SectionId { get; set; }
        public string SectionName { get; set; }
        public long? SubSectionId { get; set; }
        public string SubSectionName { get; set; }
        public string ReligionName { get; set; }
        public DateTime? DateOfJoining { get; set; }
        public DateTime? DateOfConfirmation { get; set; }
        public DateTime? TerminationDate { get; set; }
        public string TerminationStatus { get; set; }
        public bool IsActive { get; set; }
    }
}
