using Shared.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Employee.Domain.Organizational
{
    [Table("HR_Designations"), Index("DesignationName", "CompanyId", "OrganizationId", IsUnique = false, Name = "IX_HR_Designations_NonClusteredIndex")]
    public class Designation : BaseModel
    {
        [Key]
        public int DesignationId { get; set; }
        [Required, StringLength(100)]
        public string DesignationName { get; set; }
        [StringLength(20)]
        public string ShortName { get; set; }
        [StringLength(200)]
        public string DesignationNameInBengali { get; set; }
        [StringLength(100)]
        public string DesignationGroup { get; set; }
        [StringLength(100)]
        public string SalaryGroup { get; set; }
        [StringLength(200)]
        public string Remarks { get; set; }
        [ForeignKey("Grade")]
        public int? GradeId { get; set; }
        public Grade Grade { get; set; }
    }
}
