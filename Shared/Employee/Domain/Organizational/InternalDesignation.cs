using Microsoft.EntityFrameworkCore;
using Shared.BaseModels.For_DomainModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Employee.Domain.Organizational
{
    [Table("HR_InternalDesignations"), Index("InternalDesignationName", "CompanyId", "OrganizationId", IsUnique = false, Name = "IX_HR_InternalDesignation_NonClusteredIndex")]
    public class InternalDesignation : BaseModel
    {
        [Key]
        public long InternalDesignationId { get; set; }
        [StringLength(150)]
        public string InternalDesignationName { get; set; }
        public bool? IsActive { get; set; }
        [StringLength(200)]
        public string Remarks { get; set; }
    }
}
