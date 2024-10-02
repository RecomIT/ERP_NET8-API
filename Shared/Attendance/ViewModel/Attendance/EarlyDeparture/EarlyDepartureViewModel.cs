using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Attendance.ViewModel.Attendance.EarlyDeparture
{
    public class EarlyDepartureViewModel
    {

        public long EarlyDepartureId { get; set; }
        public long? EmployeeId { get; set; }
        public DateTime? AppliedDate { get; set; }
        public TimeSpan? AppliedTime { get; set; }
        public string EmpEmailNotificationStatus { get; set; }
        public string AdminEmailNotificationStatus { get; set; }
        public long? SupervisorId { get; set; }
        public long? LateReasonId { get; set; }
        public string OtherReason { get; set; }
        public string Reason { get; set; }
        public string Status { get; set; }
        public string Comment { get; set; }
        public string FullName { get; set; }
        public string EmployeeCode { get; set; }
        public bool IsApproved { get; set; }
        public string Flag { get; set; }
        public DateTime? RequestedForDate { get; set; }
        public string OfficeEmail { get; set; }
        public string SupervisorEmail { get; set; }
        public string SupervisorName { get; set; }

    }


    public class EarlyDepartureMail
    {
        public long EarlyDepartureId { get; set; }
        public string OfficeEmail { get; set; }
        public string SupervisorEmail { get; set; }
        public string SupervisorName { get; set; }
    }
    public class EarlyDepartureFeedbackdata
    {
        public long EarlyDepartureId { get; set; }

        public string FullName { get; set; }
        public string EmployeeCode { get; set; }

        public DateTime? AppliedDate { get; set; }
        public string OfficeEmail { get; set; }
        public string SupervisorEmail { get; set; }
        public List<EarlyDepartureFeedBackDetail> EarlyDepartureFeedBackDetail { get; set; }

    }
    public class EarlyDepartureFeedBackDetail
    {
        public long EarlyDepartureId { get; set; }
        public string RequestedForDate { get; set; }
        public string Comment { get; set; }
        public string Flag { get; set; }
    }

    public class EmployeeDTO
    {
        public long EmployeeId { get; set; }
        public string EmployeeCode { get; set; } = "-";
        public string Name { get; set; } = "-";
        public string Designation { get; set; } = "-";
        public string Department { get; set; } = "-";
        public string Division { get; set; } = "-";
        public string Branch { get; set; } = "-";
        public string OfficeEmail { get; set; }
        public string SupervisorEmail { get; set; }
        public string SupervisorName { get; set; }
    }
}
