using Microsoft.EntityFrameworkCore;
using Shared.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Shared.Employee.Domain.Locational
{
    [Table("HR_Locations"), Index("LocationName", "CompanyId", "OrganizationId", IsUnique = false, Name = "IX_HR_Locations_NonClusteredIndex")]
    public class Location : BaseModel
    {
        [Key]
        public int LocationId { get; set; }
        [Required, StringLength(100)]
        public string LocationName { get; set; }
        [StringLength(100)]
        public string LocationNameInBengali { get; set; }
        public int PoliceStationId { get; set; }
    }
}
