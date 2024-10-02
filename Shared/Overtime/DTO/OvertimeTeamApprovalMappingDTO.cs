
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Shared.Overtime.DTO
{
    public class OvertimeTeamApprovalMappingDTO
    {
        public OvertimeApproverDTO Approver { get; set; } = new();

        public int ApprovalLevel { get; set; } = 1;

        public List<OvertimeEmployeeDTO> TeamMembers { get; set; } = new();
    }


}