using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Shared.Models;

namespace Shared.Attendance.Domain.Attendance.LateConsideration
{
    [Table("HR_LateRequests")]
    public class LateRequests : BaseModel5
    {
        [Key]
        public long LateRequestsId { get; set; }
        public long? EmloyeeId { get; set; }
        [Column(TypeName = "date")]
        public DateTime? AppliedDate { get; set; }
        public string EmailNotificationStatus { get; set; }
        public long? SupervisorId { get; set; }
        public ICollection<LateRequestsDetail> LateRequestsDetail { get; set; }
    }

    [Table("HR_LateRequestsDetail")]
    public class LateRequestsDetail : BaseModel3
    {
        [Key]
        public long LateRequestsDetailId { get; set; }
        public DateTime? RequestedForDate { get; set; }
        public long? LateReasonId { get; set; }
        public string OtherReason { get; set; }
        public string Status { get; set; }
        public string Comment { get; set; }
        public bool IsApproved { get; set; }
        [ForeignKey("LateRequests")]
        public long LateRequestsId { get; set; }
        public LateRequests LateRequests { get; set; }
    }

}
