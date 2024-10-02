using Microsoft.EntityFrameworkCore;
using Shared.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Shared.Employee.Domain.Organizational
{
    [Table("HR_SubSections"), Index("SubSectionName", "IsActive", "CompanyId", "OrganizationId", IsUnique = false, Name = "IX_HR_SubSections_NonClusteredIndex")]
    public class SubSection : BaseModel
    {
        [Key]
        public int SubSectionId { get; set; }
        [Required, StringLength(100)]
        public string SubSectionName { get; set; }
        [StringLength(100)]
        public string SubSectionNameInBengali { get; set; }
        public bool IsActive { get; set; }
        [StringLength(200)]
        public string Remarks { get; set; }
        [ForeignKey("Section")]
        public int? SectionId { get; set; }
        public Section Section { get; set; }
    }
}
