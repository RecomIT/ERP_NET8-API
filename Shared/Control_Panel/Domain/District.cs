using Shared.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Control_Panel.Domain
{
    [Table("tblDistricts")]
    public class District : BaseModel
    {
        [Key]
        public long DistrictId { get; set; }
        [Required, StringLength(100)]
        public string DistrictName { get; set; }
        [Required, StringLength(5)]
        public string ShortName { get; set; }
        [StringLength(30)]
        public string DISCode { get; set; }
        public bool IsActive { get; set; }

        [ForeignKey("DivisionId")]
        public long DivisionId { get; set; }
        public Division Division { get; set; }
        public ICollection<Zone> Zones { get; set; }
    }
}
