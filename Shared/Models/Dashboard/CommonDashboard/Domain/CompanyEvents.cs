using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Models.Dashboard.CommonDashboard.Domain
{
    [Table("HR_CompanyEvents")]
    public class CompanyEvents : BaseModel
    {
        [Key]
        public long CompanyEventsId { get; set; }
        [StringLength(255)]
        public string EventTitle { get; set; }
        [StringLength(255)]
        public string EventTitleInBengali { get; set; }
        public string Description { get; set; }
        [StringLength(50)]
        public string Category { get; set; }
        public ICollection<CompanyEventsCalendar> CompanyEventsCalendars { get; set; }
    }
}
