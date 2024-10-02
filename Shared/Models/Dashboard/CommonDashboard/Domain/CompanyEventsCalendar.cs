using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Models.Dashboard.CommonDashboard.Domain
{
    [Table("HR_CompanyEventsCalendar")]
    public class CompanyEventsCalendar : BaseModel2
    {
        [Key]
        public long CompanyEventsCalendarId { get; set; }
        [StringLength(255)]
        public string EventTitle { get; set; }
        [StringLength(255)]
        public string EventTitleInBengali { get; set; }
        public string Description { get; set; }
        [StringLength(50)]
        public string Category { get; set; }
        public Nullable<DateTime> StartDate { get; set; }
        public Nullable<DateTime> EndDate { get; set; }
        public Nullable<DateTime> EventDate { get; set; }
        public Nullable<TimeSpan> EventTime { get; set; }
        [StringLength(50)]
        public string StateStatus { get; set; }
        public bool IsApproved { get; set; }
        [ForeignKey("CompanyEvent")]
        public long? CompanyEventsId { get; set; }
        public CompanyEvents CompanyEvent { get; set; }

    }
}
