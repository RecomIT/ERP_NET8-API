using System.ComponentModel.DataAnnotations;

namespace Shared.Payroll.DTO.Configuration
{
    public class SalaryAllowanceConfigurationStatusDTO
    {
        [Range(1,long.MaxValue)]
        public long SalaryAllowanceConfigId { get; set; }
        public string ConfigCategory { get; set; }
        [StringLength(100)]
        public string BaseType { get; set; }
        public bool IsActive { get; set; }
        [Required,StringLength(50)]
        public string StateStatus { get; set; }
        [StringLength(200)]
        public string Remarks { get; set; }
    }
}
