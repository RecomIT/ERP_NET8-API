namespace Shared.Attendance.ViewModel.Attendance
{
    public class EmployeeLateEntryEmailNotification
    {
        public long EmployeeId { get; set; }
        public string EmployeeCode { get; set; }
        public string FullName { get; set; }
        public long DesignationId { get; set; }
        public string DesignationName { get; set; }
        public string OfficeEmail { get; set; }
        public long SupervisorId { get; set; }
        public string SupervisorName { get; set; }
        public string SupervisorDesignation { get; set; }
        public string SupervisorEmail { get; set; }
        public short ContinuousLateCount { get; set; }
        public string LateSetId { get; set; }
        public string TransactionDate { get; set; }
        public string DayName { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string InTime { get; set; }
        public string OutTime { get; set; }
        public string TotalLateTime { get; set; }
        public string TotalWorkingHours { get; set; }
    }
}
