
namespace Shared.Leave.DTO.Balance.Insert
{
    public class LeaveBalanceDTO
    {

        public long LeaveTypeId { get; set; }
        public decimal? TotalLeave { get; set; }
        public long EmployeeId { get; set; }
    }
}
