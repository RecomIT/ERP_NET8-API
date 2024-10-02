namespace Shared.Leave.ViewModel.Balance
{
    public class LeaveBalanceViewModel
    {
        public long EmployeeId { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public long LeaveTypeId { get; set; }
        public string LeaveTypeName { get; set; }
        public int Allocated { get; set; }
        public decimal Applied { get; set; }
        public decimal Balance { get; set; }
        public decimal Pending { get; set; }
        public decimal Availed { get; set; }
        public decimal Rejected { get; set; }
    }
}
