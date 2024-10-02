using Shared.BaseModels.For_DomainModel;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Separation.Models.Domain
{
    [Table("HR_AdminResignationRequestApproval")]
    public class AdminResignationRequestApproval : BaseModel3
    {
        [Key]
        public long AdminResignationRequestApprovalId { get; set; }
        public long? ResignationRequestId { get; set; }
        public string ResignCode { get; set; }
        public long? EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public DateTime? EmployeeExitInterviewDate { get; set; }
        public bool? TakeInterview { get; set; }
        public bool? RescheduleExitInterview { get; set; }
        public DateTime? ExitInterviewDate { get; set; }
        public bool? IsInterviewDone { get; set; }
        public string InterviewRemarks { get; set; }
        public long? SupervisorId { get; set; }
        public bool? IsPullback { get; set; }
        public string ReasonforPullback { get; set; }
        public string HRComment { get; set; }
        public bool? IsFilesUpload { get; set; }
        public string StateStatus { get; set; }
        public bool? IsApproved { get; set; }
        public bool? IsSetupComplete { get; set; }
    }
}
