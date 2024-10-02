using System;


namespace Shared.Overtime.DTO
{
    public class GetOvertimeApprovalLevelDTO
    {
        public long OvertimeApprovalLevelId { get; set; }
        public int MaximumLevel { get; set; } = 1;
        public int MinimumLevel { get; set; } = 1;
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
