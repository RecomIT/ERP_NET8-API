using System.ComponentModel.DataAnnotations;

namespace Shared.Employee.DTO.Organizational
{
    public class GradeDTO
    {
        public int GradeId { get; set; }
        [Required, StringLength(100)]
        public string GradeName { get; set; }
        [StringLength(100)]
        public string GradeNameInBengali { get; set; }
        [StringLength(200)]
        public string Remarks { get; set; }
    }
}
