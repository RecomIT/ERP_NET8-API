using System.ComponentModel.DataAnnotations;

namespace Shared.Employee.DTO.Organizational
{
    public class CostCenterDTO
    {
        public int CostCenterId { get; set; }
        [Required, StringLength(150)]
        public string CostCenterName { get; set; }
        [Required, StringLength(150)]
        public string CostCenterCode { get; set; }
        [StringLength(150)]
        public string CostcenterNameInBengali { get; set; }
        public bool IsActive { get; set; }
        [StringLength(200)]
        public string Remarks { get; set; }
    }
}
