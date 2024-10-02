using Microsoft.EntityFrameworkCore;
using Shared.BaseModels.For_DomainModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Employee.Domain.Organizational
{
    [Table("HR_FunctionalDivision"), Index("FunctionalDivisionName", "IsActive", "CompanyId", "OrganizationId", IsUnique = false, Name = "IX_HR_FunctionalDivision_NonClusteredIndex")]
    public class FunctionalDivision : BaseModel
    {
        [Key]
        public int FunctionalDivisionId { get; set; }
        [Required, StringLength(100)]
        public string FunctionalDivisionName { get; set; }
        [StringLength(100)]
        public string FunctionalDivisionNameInBengali { get; set; }
        public bool IsActive { get; set; }
        [StringLength(200)]
        public string Remarks { get; set; }
    }
}
