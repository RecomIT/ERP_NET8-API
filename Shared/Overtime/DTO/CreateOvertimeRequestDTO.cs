
using System.Collections.Generic;

namespace Shared.Overtime.DTO
{
    public class CreateOvertimeRequestDTO
    {
        public List<GetOvertimePolicyDTO> OvertimeTypes { get; set; } = new();
        public int MaxApprovalLevel { get; set; } = 1;
        public int MinApprovalLevel { get; set; } = 1;
        public List<OvertimeApproverDTO> Approvers { get; set; } = new();
        public List<OvertimeEmployeeDTO> TeamMembers { get; set; } = new();
    }
}
