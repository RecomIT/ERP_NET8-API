using Microsoft.EntityFrameworkCore;
using Shared.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Employee.Domain.Organizational
{
    [Table("HR_Grades"), Index("GradeName", "CompanyId", "OrganizationId", IsUnique = false, Name = "IX_HR_Grades_NonClusteredIndex")]
    public class Grade : BaseModel
    {
        [Key]
        public int GradeId { get; set; }
        [Required, StringLength(100)]
        public string GradeName { get; set; }
        public string GradeNameInBengali { get; set; }
        [StringLength(200)]
        public string Remarks { get; set; }
    }
}
