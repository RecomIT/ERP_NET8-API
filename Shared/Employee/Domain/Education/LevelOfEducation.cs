using Microsoft.EntityFrameworkCore;
using Shared.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Shared.Employee.Domain.Education
{
    [Table("HR_LevelOfEducation"), Index("Name", "CompanyId", "OrganizationId", IsUnique = false, Name = "IX_HR_LevelOfEducation_NonClusteredIndex")]
    public class LevelOfEducation : BaseModel
    {
        [Key]
        public int LevelOfEducationId { get; set; }
        [Required, StringLength(150)]
        public string Name { get; set; }
        [StringLength(150)]
        public string NameInBengali { get; set; }
        public ICollection<EducatioalDegree> Degrees { get; set; }
    }
}
