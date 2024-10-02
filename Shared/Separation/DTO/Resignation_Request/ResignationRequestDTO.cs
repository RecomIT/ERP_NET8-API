using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Separation.DTO.Resignation_Request
{

    public class ResignationRequestDTO
    {
        public long? ResignationRequestId { get; set; }
        public long EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public long? SupervisorId { get; set; }
        public long ResignationCategoryId { get; set; }
        public long ResignationSubCategoryId { get; set; }
        public long NoticePeriod { get; set; }

        public DateTime? NoticeDate { get; set; }
        public DateTime? RequestLastWorkingDate { get; set; }

        public DateTime? AcceptedLastWorkingDate { get; set; }


        public string ResignationReason { get; set; }
        public string SecondaryReason { get; set; }

        public long CreatedShortfall { get; set; }
        public string EmployeeComment { get; set; }


        public DateTime? EmployeeExitInterviewDate { get; set; }
        public DateTime? HRExitInterviewDate { get; set; }


        public string TakeInterview { get; set; }
        public DateTime? SupervisorExitInterviewDate { get; set; }


        public IFormFile File { get; set; }

        public string FileSize { get; set; }
        [StringLength(300)]
        public string FilePath { get; set; }

        [StringLength(300)]
        public string ExistsFilePath { get; set; }

        [StringLength(100)]
        public string FileType { get; set; }

        [StringLength(200)]
        public string FileName { get; set; }

        [StringLength(200)]
        public string ActualFileName { get; set; }


        [StringLength(200)]
        public string ExistsFileName { get; set; }

        public string CancelRemarks { get; set; }

        public string ApprovalRemarks { get; set; }
        public string RejectedRemarks { get; set; }

        public string ExecutionFlag { get; set; }
    }

}
