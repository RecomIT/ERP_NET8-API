namespace Shared.Employee.ViewModel.Info
{
    public class EmployeeServiceDataViewModel
    {
        public long EmployeeId { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public long? BranchId { get; set; }
        public string BranchName { get; set; }
        public long? GradeId { get; set; }
        public string GradeName { get; set; }
        public long? DesignationId { get; set; }
        public string DesignationName { get; set; }
        public long? ZoneId { get; set; }
        public string ZoneName { get; set; }
        public long? UnitId { get; set; }
        public string UnitName { get; set; }
        public long DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public long SectionId { get; set; }
        public string SectionName { get; set; }
        public long? SubSectionId { get; set; }
        public string SubSectionName { get; set; }
        public bool IsActive { get; set; }
        public string TerminationDate { get; set; }
        public string TerminationStatus { get; set; }
        public string MaritalStatus { get; set; }
        public long? PreviousReviewId { get; set; } = 0;
        public long? WorkShiftId { get; set; } = 0;
        public string WorkShiftName { get; set; }
        public string OfficeEmail { get; set; }
        public string OfficeMobile { get; set; }
        public string LegalName { get; set; }
        public string Gender { get; set; }
        public string Religion { get; set; }
        public string JobType { get; set; }
        public bool IsResidential { get; set; } = false;
        public bool IsPFMember { get; set; } = false;
    }
}
