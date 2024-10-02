

using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Leave.DTO.Balance
{
    public class LeaveBalanceDto
    {
        public long LeaveTypeId { get; set; }
        public string LeaveTypeName { get; set; }
        public int TotalLeave { get; set; }
        public decimal? Balance { get; set; }
        public decimal? Applied { get; set; }
        public int LeaveYear { get; set; }


        [Column(TypeName = "date")]
        public DateTime? LeavePeriodStart { get; set; }

        [Column(TypeName = "date")]
        public DateTime? LeavePeriodEnd { get; set; }
    }
}
