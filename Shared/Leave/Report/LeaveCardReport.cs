namespace Shared.Leave.Report
{
    public class LeaveCardEmployeeInformation
    {
        public long EmployeeId { get; set; }
        public string EmployeeCode { get; set; }
        public string FullName { get; set; }
        public string Designation { get; set; }
        public string Department { get; set; }
        public string JobType { get; set; }
        public string Gender { get; set; }
        public string MaritalStatus { get; set; }
        public Nullable<DateTime> JoiningDate { get; set; }
        public Nullable<DateTime> LastWrokingDate { get; set; }
        public Nullable<DateTime> LeaveStartDate { get; set; }
        public Nullable<DateTime> LeaveEndDate { get; set; }
        public int LeaveYear { get; set; }
        public long BranchId { get; set; }
        public string BranchName { get; set; }
        public string BranchAddress { get; set; }
        public byte[] BranchLogo { get; set; }
        public long CompanyId { get; set; }
        public byte[] ReportLogo { get; set; }
        public long OrganizationId { get; set; }
        public string OrganizationName { get; set; }
        public string ServiceLength { get; set; }
    }
    public class LeaveCardLeaveBalanceSummary
    {
        public long EmployeeId { get; set; }
        public string EmployeeCode { get; set; }
        public string FullName { get; set; }
        public Nullable<DateTime> JoiningDate { get; set; }
        public Nullable<DateTime> LastWrokingDate { get; set; }
        public Nullable<DateTime> LeaveStartDate { get; set; }
        public Nullable<DateTime> LeaveEndDate { get; set; }
        public long LeaveTypeId { get; set; }
        public string LeaveType { get; set; }
        public decimal Allocated { get; set; } = 0;
        public decimal Pending { get; set; } = 0;
        public decimal Approved { get; set; } = 0;
        public decimal Availed { get; set; } = 0;
        public decimal Applied { get; set; } = 0;
        public decimal Balance { get; set; } = 0;
        public decimal Applicable { get; set; } = 0;
    }
    public class LeaveCardAppliedLeaveInformation
    {
        public long EmployeeId { get; set; }
        public string EmployeeCode { get; set; }
        public string FullName { get; set; }
        public Nullable<DateTime> LastWrokingDate { get; set; }
        public Nullable<DateTime> LeaveStartDate { get; set; }
        public Nullable<DateTime> LeaveDate { get; set; }
        public long LeaveTypeId { get; set; }
        public string DayLeaveType { get; set; }
        public string PartOfDay { get; set; }
        public string Count { get; set; }
        public string Purpose { get; set; }
        public Nullable<DateTime> AppliedDate { get; set; }
        public string ActivityLog { get; set; }
        public string LeaveDuration { get; set; }
        public string LeaveType { get; set; }
        public string Status { get; set; }
    }
    public class LeaveCardFilter
    {
        public long EmployeeId { get; set; } = 0;
        public Nullable<DateTime> LeaveStartDate { get; set; }
        public Nullable<DateTime> LeaveEndDate { get; set; }
    }
}
