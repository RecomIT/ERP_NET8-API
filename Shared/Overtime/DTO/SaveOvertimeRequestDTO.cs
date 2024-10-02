using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace Shared.Overtime.DTO
{
    public class SaveOvertimeRequestDTO
    {
        [Range(minimum: 1, maximum: int.MaxValue)]
        public long EmployeeId { get; set; }


        [Range(minimum: 1, maximum: int.MaxValue)]
        public long OvertimeId { get; set; }

        [Required]
        public DateTime RequestDate { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public string Remarks { get; set; } = "-";
        public List<OvertimeRequestApprover> Approvers { get; set; } = new();
    }

    public class OvertimeRequestApprover
    {
        [Range(minimum: 1, maximum: int.MaxValue)]
        public long OvertimeApproverId { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string EmployeeCode { get; set; } = string.Empty;

        [Range(minimum: 1, maximum: int.MaxValue)]
        public int ApprovalOrder { get; set; } = 1;
    }

}
