using Microsoft.EntityFrameworkCore;
using Shared.BaseModels.For_DomainModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Employee.Domain.Organizational
{
    [Table("HR_EmployeeType"), Index("EmployeeTypeName", "CompanyId", "OrganizationId", IsUnique = false, Name = "IX_HR_EmployeeType_NonClusteredIndex")]
    public class EmployeeType : BaseModel
    {
        [Key]
        public int EmployeeTypeId { get; set; }
        [Required, StringLength(50)]
        public string EmployeeTypeName { get; set; }
        [StringLength(200)]
        public string Remarks { get; set; }
    }
}
