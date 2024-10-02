using System;
using System.ComponentModel.DataAnnotations;
using Shared.BaseModels.For_DomainModel;

namespace Shared.Separation.Models.Domain
{
    public class SupervisorResignationRequestApproval : BaseModel3
    {
        [Key]
        public long SupervisorResignationRequestApprovalId { get; set; }
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
        public string StateStatus { get; set; }
        public bool? IsApproved { get; set; }
    }
}
