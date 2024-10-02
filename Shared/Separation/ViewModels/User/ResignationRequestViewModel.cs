using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Separation.ViewModels.User
{
    public class ResignationRequestViewModel
    {
        public long ResignationRequestId { get; set; }
        public string ResignCode { get; set; }
        public long EmployeeId { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string FullName { get; set; }
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
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string CancelledBy { get; set; }
        public DateTime? CancelledDate { get; set; }
        public string CancelRemarks { get; set; }
        public long? SupervisorId { get; set; }
        public bool? RescheduleExitInterviewBySupervisor { get; set; }
        public DateTime? SupervisorExitInterviewDate { get; set; }
        public string SupervisorStatus { get; set; }
        public bool? RescheduleExitInterviewByHR { get; set; }
        public DateTime? HRExitInterviewDate { get; set; }
        public string HRStatus { get; set; }
        public string StateStatus { get; set; }
        public long? CompanyId { get; set; }
        public long? OrganizationId { get; set; }
        public string CategoryName { get; set; }
        public string SubCategoryName { get; set; }
        public string SupervisorName { get; set; }




        public string DesignationName { get; set; }
        public string DepartmentName { get; set; }
        public string GradeName { get; set; }
        public string BranchName { get; set; }
        public string JobType { get; set; }
        public string PhotoPath { get; set; }
        public string Photo { get; set; }
        public string OfficeEmail { get; set; }
        public string PersonalEmailAddress { get; set; }
        public string OfficeMobile { get; set; }
        public string PersonalMobileNo { get; set; }




        [DisplayFormat(DataFormatString = "{0:dd MMM, yyyy}")]
        public DateTime DateOfJoining { get; set; }

        public bool IsActive { get; set; }


    }
}
