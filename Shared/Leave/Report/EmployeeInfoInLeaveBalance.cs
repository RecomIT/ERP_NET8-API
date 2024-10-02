using System;

namespace Shared.Leave.Report
{
    public class EmployeeInfoInLeaveBalance
    {
        public long EmployeeId { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string DesignationName { get; set; }
        public string DepartmentName { get; set; }
        public long DepartmentId { get; set; }
        public long SectionId { get; set; }
        public long SubSectionId { get; set; }
        public DateTime? LeaveStartDate { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? LeaveEndDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string LeaveTypeName { get; set; }
        public decimal TotalLeave { get; set; }
        public decimal LeaveAvailed { get; set; }
        public decimal LeaveBalance { get; set; }
    }
}
