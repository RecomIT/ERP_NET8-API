using System;
using System.ComponentModel.DataAnnotations;
using Shared.BaseModels.For_DomainModel;

namespace Shared.Separation.Models.Domain
{
    public class EmployeeResignationRequest : BaseModel3
    {
        [Key]
        public long ResignationRequestId { get; set; }
        public string ResignCode { get; set; }
        public long? EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public long? ResignationCategoryId { get; set; }
        public long? ResignationSubCategoryId { get; set; }
        public string ResignationReason { get; set; }
        public string SecondaryReason { get; set; }
        public int? NoticePeriod { get; set; }
        public DateTime? NoticeDate { get; set; }
        public DateTime? RequestLastWorkingDate { get; set; }
        public DateTime? AcceptedLastWorkingDate { get; set; }
        public DateTime? ActualLastWorkingDate { get; set; }
        public bool? NotifiedWithinNoticePeriod { get; set; }
        public int? CreatedShortfall { get; set; }
        public int? ActualShortfall { get; set; }
        public string EmployeeComment { get; set; }
        public DateTime? EmployeeExitInterviewDate { get; set; }
        public bool? IsResignationLetterUpload { get; set; }
        public string FileType { get; set; }
        public string FileSize { get; set; }
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public string ActualFileName { get; set; }
        public long? SupervisorId { get; set; }
        public bool? RescheduleExitInterviewBySupervisor { get; set; }
        public DateTime? SupervisorExitInterviewDate { get; set; }
        public string SupervisorStatus { get; set; }
        public bool? RescheduleExitInterviewByHR { get; set; }
        public DateTime? HRExitInterviewDate { get; set; }
        public string HRStatus { get; set; }
        public string StateStatus { get; set; }
    }
}
