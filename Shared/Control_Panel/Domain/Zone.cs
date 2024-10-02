using Shared.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Control_Panel.Domain
{
    [Table("tblZones")]
    public class Zone : BaseModel
    {
        [Key]
        public long ZoneId { get; set; }
        [Required, StringLength(100)]
        public string ZoneName { get; set; }
        [Required, StringLength(5)]
        public string ShortName { get; set; }
        [StringLength(30)]
        public string ZoneCode { get; set; }
        public bool IsActive { get; set; }
        [ForeignKey("District")]
        public long DistrictId { get; set; }
        public District District { get; set; }
        public long DivisionId { get; set; }
        public ICollection<Branch> Branches { get; set; }
    }
}
