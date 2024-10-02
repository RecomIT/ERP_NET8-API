using Microsoft.EntityFrameworkCore;
using Shared.BaseModels.For_DomainModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Employee.Domain.Organizational
{
    [Table("HR_JobCategory"), Index("JobCategoryName", "CompanyId", "OrganizationId", IsUnique = false, Name = "IX_HR_JobCategory_NonClusteredIndex")]
    public class JobCategory : BaseModel
    {
        [Key]
        public int JobCategoryId { get; set; }
        [Required, StringLength(50)]
        public string JobCategoryName { get; set; }
        [StringLength(200)]
        public string Remarks { get; set; }
    }
}
