using Shared.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Employee.Domain.Miscellaneous
{
    [Table("HR_BloodGroups")]
    public class BloodGroup : BaseModel
    {
        [Key]
        public int BloodGroupId { get; set; }
        [StringLength(10)]
        public string BloodGroupName { get; set; }
        [StringLength(100)]
        public string BloodGroupNameInBengali { get; set; }
    }
}
