using System.ComponentModel.DataAnnotations;

namespace Shared.Employee.DTO.Organizational
{
    public class UnitDTO
    {
        public int UnitId { get; set; }
        [StringLength(100)]
        public string UnitName { get; set; }
        [StringLength(100)]
        public string UnitNameInBengali { get; set; }
        public bool IsActive { get; set; }
        [StringLength(200)]
        public string Remarks { get; set; }
        public int SubSectionId { get; set; }
    }
}
