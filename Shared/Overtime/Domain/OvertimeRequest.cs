using Shared.BaseModels.For_DomainModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Overtime.Domain
{
    [Table("Payroll_OvertimeRequest")]
    public class OvertimeRequest : BaseModel1
    {

        [Key]
        public long OvertimeRequestId { get; set; }
        public long EmployeeId { get; set; } = 0;
        public long AuthorityId { get; set; } = 0;
        public long OvertimeId { get; set; } = 0;

        [Required]
        public DateTime RequestDate { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public DateTime ApplicationDate { get; set; }
        public string Remarks { get; set; } = "-";
        public string Status { get; set; }
        public string WaitingStage { get; set; } = "-";


        public List<OvertimeRequestDetails> OvertimeRequestDetails { get; set; } = new();

    }

    [Table("Payroll_OvertimeRequestDetails")]
    public class OvertimeRequestDetails
    {
        [Key]
        public long OvertimeRequestDetailsId { get; set; }
        public long OvertimeApproverId { get; set; } = 0;
        public int ApprovalOrder { get; set; } = 1;
        public bool ActionRequired { get; set; } = false;
        public bool IsReverted { get; set; } = false;
        public string Remarks { get; set; } = "-";
        public string Status { get; set; }
        public DateTime? ProcessAt { get; set; }
        public long OvertimeRequestId { get; set; }
    }
}
