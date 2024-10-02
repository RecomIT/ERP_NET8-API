using Microsoft.EntityFrameworkCore;
using Shared.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Shared.Employee.Domain.Miscellaneous
{
    [Table("HR_JobStatus"), Index("StatusId", "CompanyId", "OrganizationId", IsUnique = false, Name = "IX_HR_JobStatus_NonClusteredIndex")]
    public class JobStatus : BaseModel
    {
        [Key]
        public long StatusId { get; set; }
        [Required, StringLength(100)]
        public string JobStatusName { get; set; }
        [StringLength(100)]
        public string JobStatusNameInBengali { get; set; }
        [StringLength(200)]
        public string Remarks { get; set; }
    }
}
