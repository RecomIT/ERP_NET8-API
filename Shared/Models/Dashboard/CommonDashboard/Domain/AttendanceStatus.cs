using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Models.Dashboard.CommonDashboard.Domain
{
    [Table("HR_AttendanceStatus")]
    public class AttendanceStatus
    {
        [Key]
        [StringLength(5)]
        public char StatusCode { get; set; }
        [StringLength(50)]
        public string StatusDescription { get; set; }
    }
}
