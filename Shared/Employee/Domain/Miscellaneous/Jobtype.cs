using Microsoft.EntityFrameworkCore;
using Shared.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Shared.Employee.Domain.Miscellaneous
{
    [Table("HR_Jobtypes"), Index("JobTypeName", "CompanyId", "OrganizationId", IsUnique = false, Name = "IX_HR_Jobtypes_NonClusteredIndex")]
    public class Jobtype : BaseModel
    {
        [Key]
        public int JobTypeId { get; set; }
        [Required, StringLength(50)]
        public string JobTypeName { get; set; }
        [StringLength(50)]
        public string JobTypeNameInBengali { get; set; }
        [StringLength(100)]
        public string Duration { get; set; }
        [StringLength(200)]
        public string Remarks { get; set; }
    }
}
