
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Leave.DTO.Balance
{
    public class LeaveBalanceRequestModel
    {
        public int EmployeeId { get; set; }

        [Column(TypeName = "date")]
        public DateTime? LeavePeriodStart { get; set; }

        [Column(TypeName = "date")]
        public DateTime? LeavePeriodEnd { get; set; }

        public string ExecutionMode { get; set; }

        public List<LeaveBalanceModel> LeaveBalances { get; set; }

    }
}
