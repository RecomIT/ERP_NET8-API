using System.ComponentModel.DataAnnotations;

namespace Shared.Employee.DTO.Organizational
{
    public class EmployeeTypeDTO
    {
        public int EmployeeTypeId { get; set; }
        [Required, StringLength(100)]
        public string EmployeeTypeName { get; set; }
        [StringLength(200)]
        public string Remarks { get; set; }
    }
}
