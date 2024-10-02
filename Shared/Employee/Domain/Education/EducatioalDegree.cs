
using Microsoft.EntityFrameworkCore;
using Shared.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Shared.Employee.Domain.Education
{
    [Table("HR_EducationalDegrees"), Index("DegreeName", "CompanyId", "OrganizationId", IsUnique = false, Name = "IX_HR_EducationalDegrees_NonClusteredIndex")]
    public class EducatioalDegree : BaseModel
    {
        [Key]
        public int EducatioalDegreeId { get; set; }
        [Required, StringLength(200)]
        public string DegreeName { get; set; }
        [StringLength(200)]
        public string DegreeNameInBengali { get; set; }
        [ForeignKey("LevelOfEducation")]
        public int LevelOfEducationId { get; set; }
        public LevelOfEducation LevelOfEducation { get; set; }
    }
}
