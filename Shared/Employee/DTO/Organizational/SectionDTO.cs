using System.ComponentModel.DataAnnotations;

namespace Shared.Employee.DTO.Organizational
{
    public class SectionDTO
    {
        public int SectionId { get; set; }
        [Required, StringLength(100)]
        public string SectionName { get; set; }
        [StringLength(100)]
        public string SectionNameInBengali { get; set; }
        public bool IsActive { get; set; }
        [StringLength(200)]
        public string Remarks { get; set; }
        public long? DepartmentId { get; set; }
    }
}
