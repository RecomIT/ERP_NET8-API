using Microsoft.EntityFrameworkCore;
using Shared.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Shared.Employee.Domain.Organizational
{
    [Table("HR_Sections"), Index("SectionName", "IsActive", "CompanyId", "OrganizationId", IsUnique = false, Name = "IX_HR_Sections_NonClusteredIndex")]
    public class Section : BaseModel
    {
        [Key]
        public int SectionId { get; set; }
        [Required, StringLength(100)]
        public string SectionName { get; set; }
        [StringLength(100)]
        public string SectionNameInBengali { get; set; }
        public bool IsActive { get; set; }
        [StringLength(200)]
        public string Remarks { get; set; }
        //[ForeignKey("Department")]
        public int? DepartmentId { get; set; }
        //public Department Department { get; set; }
        public ICollection<SubSection> SubSections { get; set; }
    }
}
