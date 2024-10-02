using Shared.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Employee.Domain.Organizational
{
    [Table("HR_Departments"), Index("DepartmentName", "IsActive", "CompanyId", "OrganizationId", IsUnique = false, Name = "IX_HR_Departments_NonClusteredIndex")]
    public class Department : BaseModel
    {
        [Key]
        public int DepartmentId { get; set; }
        [Required, StringLength(150)]
        public string DepartmentName { get; set; }
        [StringLength(150)]
        public string DepartmentNameInBengali { get; set; }
        public bool IsActive { get; set; }
        [StringLength(200)]
        public string Remarks { get; set; }
        public int? FunctionalDivisionId { get; set; }
    }
}
