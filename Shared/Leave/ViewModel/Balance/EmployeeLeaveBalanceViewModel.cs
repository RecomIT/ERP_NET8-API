using Shared.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Leave.ViewModel.Balance
{
    public class EmployeeLeaveBalanceViewModel : BaseViewModel2
    {
        public long EmployeeLeaveBalanceId { get; set; }
        public long EmployeeId { get; set; }
        public long LeaveTypeId { get; set; }
        public long? LeaveSettingId { get; set; }
        public short LeaveYear { get; set; }
        [StringLength(100)]
        public string LeaveTypeName { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalLeave { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? LeaveApplied { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? LeaveEnjoyed { get; set; }
        [StringLength(200)]
        public string Remarks { get; set; }
        [StringLength(50)]
        public string StateStatus { get; set; }
        public bool IsApproved { get; set; }
        // Custom Properties
        public string EmployeeName { get; set; }
    }
}
