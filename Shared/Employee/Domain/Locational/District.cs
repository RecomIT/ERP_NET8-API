using Shared.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Employee.Domain.Locational
{
    [Table("HR_Districts"), Index("DistrictName", "DistrictCode", "CompanyId", "OrganizationId", IsUnique = false, Name = "IX_HR_Districts_NonClusteredIndex")]
    public class District : BaseModel
    {
        [Key]
        public int DistrictId { get; set; }
        [Required, StringLength(100)]
        public string DistrictName { get; set; }
        [StringLength(100)]
        public string DistrictNameInBengali { get; set; }
        [StringLength(100)]
        public string DistrictCode { get; set; }
        [ForeignKey("DivisionId")]
        public int DivisionId { get; set; }
        public Division Division { get; set; }
    }
}
