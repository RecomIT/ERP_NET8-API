using System.ComponentModel.DataAnnotations;

namespace Shared.Payroll.DTO.Configuration
{
    public class SalaryAllowanceConfigurationInfoDTO
    {
        public long SalaryAllowanceConfigId { get; set; }
        [StringLength(100)]
        public string ConfigCategory { get; set; }
        public bool IsActive { get; set; }
        [StringLength(50)]
        public string StateStatus { get; set; }
        public short HeadCount { get; set; }
        public string HeadDetails { get; set; }
        [StringLength(200)]
        public string Remarks { get; set; }
        [StringLength(100)]
        public string BaseType { get; set; }
        public IEnumerable<SalaryAllowanceConfigurationDetailDTO> SalaryAllowanceConfigurationDetails { get; set; }
    }
}
