using Shared.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Leave.Domain.Setup
{
    [Table("HR_LeaveTypes"), Index("Title", "CompanyId", "OrganizationId", IsUnique = false, Name = "IX_HR_LeaveTypes_NonClusteredIndex")]
    public class LeaveType : BaseModel
    {
        [Key]
        public long Id { get; set; }
        [Required, StringLength(100)]
        public string Title { get; set; }
        [StringLength(100)]
        public string TitleInBengali { get; set; }
        [StringLength(15)]
        public string ShortName { get; set; }
        [StringLength(30)]
        public string ShortNameInBangali { get; set; }
        [StringLength(200)]
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public int SerialNo { get; set; } = 0;
        public ICollection<LeaveSetting> LeaveSettings { get; set; }
    }
}
