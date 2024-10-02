
using Shared.Leave.Domain.Setup;

namespace Shared.Leave.ViewModel.Encashment
{
    public class EncashmentBalanceViewModel
    {
        public long LeaveTypeId { get; set; }
        public decimal TotalLeaveBalance { get; set; }
        public LeaveSetting LeaveSetting { get; set; }
    }
}
