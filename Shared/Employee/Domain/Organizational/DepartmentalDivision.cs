using Microsoft.EntityFrameworkCore;
using Shared.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Shared.Employee.Domain.Organizational
{
    [Table("HR_DepartmentalDivision"), Index("DepartmentalDivisionName", "CompanyId", "OrganizationId", IsUnique = false, Name = "IX_HR_FunctionalDivision_NonClusteredIndex")]
    public class DepartmentalDivision : BaseModel
    {
        [Key]
        public int DepartmentalDivisionId { get; set; }
        [Required, StringLength(100)]
        public string DepartmentalDivisionName { get; set; }
        [StringLength(100)]
        public string DepartmentalDivisionNameInBengali { get; set; }
        public bool IsActive { get; set; }
        [StringLength(200)]
        public string Remarks { get; set; }
        [ForeignKey("Department")]
        public int? DepartmentId { get; set; }
        public Department Department { get; set; }
    }
}
