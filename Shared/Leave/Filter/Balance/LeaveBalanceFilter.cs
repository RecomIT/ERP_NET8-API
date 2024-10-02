
using Shared.OtherModels.Pagination;

namespace Shared.Leave.Filter.Balance
{
    public class LeaveBalanceFilter
    {
        public long? EmployeeId { get; set; }
        public long? LeaveTypeId { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

    }
}
