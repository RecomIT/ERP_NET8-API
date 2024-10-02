using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models.Dashboard.SubordinatesLeave.Approval.ViewModel
{
    public class LeaveApprovalViewModel
    {
        public short TotalRowCount { get; set; }

        public long EmployeeLeaveRequestId { get; set; }
        public long EmployeeId { get; set; }
        public string Name { get; set; }
        public string DesignationName { get; set; }
        public long LeaveTypeId { get; set; }
        public string Title { get; set; }
        public decimal LeaveCount { get; set; }
        public DateTime AppliedFromDate { get; set; }
        public DateTime AppliedToDate { get; set; }
        public decimal AppliedTotalDays { get; set; }
        public short TotalEmployees { get; set; }


        public string DayLeaveType { get; set; }
        public string LeavePurpose { get; set; }

        public decimal TotalLeave { get; set; }
        public decimal LeaveAvailed { get; set; }
        public decimal LeaveApplied { get; set; }
        public decimal RemainingLeave { get; set; }
        public string SupervisorName { get; set; }
        public string HODName { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }


        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }


        public string CancelledBy { get; set; }
        public DateTime CancelledDate { get; set; }
        public string CancelRemarks { get; set; }


        public string CheckedBy { get; set; }
        public DateTime CheckedDate { get; set; }
        public string CheckRemarks { get; set; }



        public string RejectedBy { get; set; }
        public DateTime RejectedDate { get; set; }
        public string RejectedRemarks { get; set; }


        public string ApprovedBy { get; set; }
        public DateTime ApprovedDate { get; set; }
        public string ApprovalRemarks { get; set; }

        public string StateStatus { get; set; }


        public string FileName { get; set; }
        public string FilePath { get; set; }


    }
}
