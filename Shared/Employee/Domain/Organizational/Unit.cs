using Shared.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Shared.Employee.Domain.Organizational
{
    [Table("HR_Units"), Index("UnitName", "IsActive", "CompanyId", "OrganizationId", IsUnique = false, Name = "IX_HR_Units_NonClusteredIndex")]
    public class Unit : BaseModel
    {
        [Key]
        public int UnitId { get; set; }
        [Required, StringLength(100)]
        public string UnitName { get; set; }
        [StringLength(100)]
        public string UnitNameInBengali { get; set; }
        public bool IsActive { get; set; }
        [StringLength(200)]
        public string Remarks { get; set; }
        public int SubSectionId { get; set; }
    }
}
