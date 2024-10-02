using Shared.BaseModels.For_DomainModel;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Separation.Models.Domain.Resignation
{
    [Table("HR_EmployeeResignationDetail")]
    public class EmployeeResignationDetail : BaseModel2
    {
        [Key]
        public long ResignationDetailId { get; set; }
        public long? ResignationRequestId { get; set; }
        public bool? RescheduleExitInterviewByHR { get; set; }
        public DateTime? HRExitInterviewDate { get; set; }
        public bool? IsInterviewDoneByHR { get; set; }
        public bool? IsInterviewQuestionsDoneByHR { get; set; }
        public string HRInterviewRemarks { get; set; }
        public bool? IsFilesUpload { get; set; }
        public bool? IsPullback { get; set; }
        public string ReasonforPullback { get; set; }
        public string HRComment { get; set; }
        public string ApprovalStatus { get; set; }
        public bool? IsApproved { get; set; }
    }
}
