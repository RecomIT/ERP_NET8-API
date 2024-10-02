

namespace Shared.Leave.DTO.Balance
{
    public class EmployeeLeaveBalanceDtoForExcel
    {
        public string EmployeeCode { get; set; }

        public string EmployeeName { get; set; }

        public List<LeaveBalanceDtoForExcel> LeaveBalances { get; set; }
    }
}
