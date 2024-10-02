using Microsoft.EntityFrameworkCore;
using Shared.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Employee.Domain.Miscellaneous
{
    [Table("HR_Levels"), Index("LevelName", "CompanyId", "OrganizationId", IsUnique = false, Name = "IX_HR_Levels_NonClusteredIndex")]
    public class Level : BaseModel
    {
        [Key]
        public int LevelId { get; set; }
        [Required, StringLength(200)]
        public string LevelName { get; set; }
        [StringLength(200)]
        public string LevelNameInBengali { get; set; }
        [StringLength(200)]
        public string Remarks { get; set; }
    }
}
