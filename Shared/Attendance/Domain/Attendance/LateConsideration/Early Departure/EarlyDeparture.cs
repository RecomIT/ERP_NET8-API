using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models.HR.DomainModels.LateConsideration.Early_Departure
{
    [Table("HR_EarlyDeparture")]
    public class EarlyDeparture : BaseModel5
    {
        
     
            [Key]
            public long EarlyDepartureId { get; set; }
            public long? EmployeeId { get; set; }
            [Column(TypeName = "date")]
            public Nullable<DateTime> AppliedDate { get; set; }
            public Nullable<TimeSpan> AppliedTime { get; set; }
            public string EmpEmailNotificationStatus { get; set; }
            public string AdminEmailNotificationStatus { get; set; }
            public long? SupervisorId { get; set; }
            public long? LateReasonId { get; set; }
            public string OtherReason { get; set; }
            public string Status { get; set; }
            public string Comment { get; set; }
            public bool IsApproved { get; set; }
            public Nullable<DateTime> RequestedForDate { get; set; }


    }
}
