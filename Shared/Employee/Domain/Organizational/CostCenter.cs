using Shared.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Employee.Domain.Organizational
{
    [Table("HR_Costcenter"), Index("CostCenterName", "IsActive", "CompanyId", "OrganizationId", IsUnique = false, Name = "IX_HR_Costcenter_NonClusteredIndex")]
    public class CostCenter : BaseModel
    {
        [Key]
        public int CostCenterId { get; set; }
        [Required, StringLength(150)]
        public string CostCenterName { get; set; }
        [Required, StringLength(150)]
        public string CostCenterCode { get; set; }
        [StringLength(150)]
        public string CostcenterNameInBengali { get; set; }
        public bool IsActive { get; set; }
        [StringLength(200)]
        public string Remarks { get; set; }
    }
}
