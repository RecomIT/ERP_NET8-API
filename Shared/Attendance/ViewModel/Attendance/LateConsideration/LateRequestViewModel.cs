using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Shared.Models.HR.DomainModels.LateConsideration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Models;

namespace Shared.Attendance.ViewModel.Attendance.LateConsideration
{
    public class LateRequestViewModel : BaseModel4
    {

        public long LateRequestsId { get; set; }
        public string FullName { get; set; }

        public long? EmployeeId { get; set; }
        public DateTime? AppliedDate { get; set; }
        public string EmailNotificationStatus { get; set; }
        public long? SupervisorId { get; set; }
        public string SupervisorName { get; set; }

        public long LateRequestsDetailId { get; set; }
        public string RequestedForDate { get; set; }
        public string Reason { get; set; }
        public string OtherReason { get; set; }
        public string Status { get; set; }
        public long? InMinute { get; set; }

        public bool IsApproved { get; set; }
        // public bool Check { get; set; }
        public List<LateRequestsDetailViewModel> LateRequestsDetailViewModel { get; set; }


    }

    public class LateRequestsDetailViewModel
    {
        public long LateRequestsId { get; set; }
        public long LateRequestsDetailId { get; set; }
        public string RequestedForDate { get; set; }
        public string Reason { get; set; }
        public string OtherReason { get; set; }
        public string Status { get; set; }
        public long? InMinute { get; set; }
        public string Comment { get; set; }
        public bool IsApproved { get; set; }
        public long? AttendanceId { get; set; }
        public string FullName { get; set; }

        public string EmployeeCode { get; set; }
        public DateTime? AppliedDate { get; set; }
        public string OfficeEmail { get; set; }
        public string SupervisorEmail { get; set; }



    }
    public class LateRequestFilter
    {
        public string CompanyId { get; set; }
        public string OrganizationId { get; set; }
        public string BranchId { get; set; }
        public string EmployeeId { get; set; }

    }

    public class feedbackdata
    {
        public long LateRequestsId { get; set; }

        public string FullName { get; set; }
        public string EmployeeCode { get; set; }

        public DateTime? AppliedDate { get; set; }
        public string OfficeEmail { get; set; }
        public string SupervisorEmail { get; set; }
        public List<feedBackDetail> feedBackDetails { get; set; }

    }
    public class feedBackDetail
    {
        public long LateRequestsDetailId { get; set; }
        public string RequestedForDate { get; set; }
        public string Comment { get; set; }
        public string Flag { get; set; }
    }
}

