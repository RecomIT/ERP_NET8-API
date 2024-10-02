using System.ComponentModel.DataAnnotations;

namespace Shared.Employee.DTO.Organizational
{
    public class DepartmentDTO
    {
        public int DepartmentId { get; set; }
        [Required, StringLength(150)]
        public string DepartmentName { get; set; }
        [StringLength(150)]
        public string DepartmentNameInBengali { get; set; }
        public bool IsActive { get; set; }
        [StringLength(200)]
        public string Remarks { get; set; }
    }
}
