using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations;

namespace Shared.Overtime.DTO
{
    public class GetOvertimeRequestDTO
    {
        public long OvertimeRequestId { get; set; }
        public EmployeeDTO Employee { get; set; } = new();
        public GetOvertimePolicyDTO OvertimeType { get; set; } = new();
        public DateTime RequestDate { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public DateTime ApplicationDate { get; set; }
        public string Remarks { get; set; } = "-";
        public string Status { get; set; }
        public string WaitingStage { get; set; } = "-";
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public List<OvertimeRequestDetailsDTO> OvertimeRequestDetails { get; set; } = new();
    }

    public class OvertimeRequestDetailsDTO
    {
        public long OvertimeRequestDetailsId { get; set; }
        public long OvertimeApproverId { get; set; }
        public OvertimeApproverDTO Approver { get; set; } = new();
        public int ApprovalOrder { get; set; } = 1;
        public bool ActionRequired { get; set; } = false;
        public bool IsReverted { get; set; } = false;
        public string Remarks { get; set; } = "-";
        public string Status { get; set; }
        public DateTime? ProcessAt { get; set; }
    }



}
