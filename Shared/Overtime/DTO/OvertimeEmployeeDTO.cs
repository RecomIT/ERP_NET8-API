using System;

namespace Shared.Overtime.DTO
{
    public class OvertimeEmployeeDTO
    {
        public long EmployeeId { get; set; }
        public string EmployeeCode { get; set; }
        public string Name { get; set; }
        public string Designation { get; set; }
        public string Department { get; set; }
        public string Division { get; set; }
        public string Branch { get; set; }
        public long OvertimeApproverId { get; set; } = 0;
        public long OvertimeTeamApprovalMappingId { get; set; } = 0;
        public int ApprovalLevel { get; set; } = 0;
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }



    }
}
