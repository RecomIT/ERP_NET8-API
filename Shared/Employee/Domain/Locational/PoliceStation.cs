using Shared.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Employee.Domain.Locational
{
    [Table("HR_PoliceStations"), Index("PoliceStationName", "CompanyId", "OrganizationId", IsUnique = false, Name = "IX_HR_PoliceStations_NonClusteredIndex")]
    public class PoliceStation : BaseModel
    {
        [Key]
        public int PoliceStationId { get; set; }
        [Required, StringLength(100)]
        public string PoliceStationName { get; set; }
        [StringLength(100)]
        public string PoliceStationNameInBengali { get; set; }
        [StringLength(200)]
        public string Remarks { get; set; }
        [ForeignKey("DistrictId")]
        public int DistrictId { get; set; }
        public District District { get; set; }
    }
}
