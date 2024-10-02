using Shared.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Shared.Employee.Domain.Miscellaneous
{
    [Table("HR_Religions")]
    public class Religion : BaseModel
    {
        [Key]
        public int ReligionId { get; set; }
        [Required, StringLength(50)]
        public string ReligionName { get; set; }
        [StringLength(100)]
        public string ReligionNameInBengali { get; set; }
        [StringLength(20)]
        public string ReligionCode { get; set; }
    }
}
