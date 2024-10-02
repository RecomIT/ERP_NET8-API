using System;

namespace Shared.Leave.ViewModel.History
{
    public class EmployeeLeaveHistoryInfoViewModel
    {
        public int Count { get; set; }
        public string Status { get; set; }
        public DateTime? LeaveDate { get; set; }
        public string DayName { get; set; }
        public DateTime? ReplacementDate { get; set; }
    }
}
