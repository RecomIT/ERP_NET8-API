

using System.ComponentModel.DataAnnotations;

namespace Shared.Payroll.DTO.Allowance
{
    public class AllowanceConfigStatusDTO
    {
        [Required,StringLength(50)]
        public string StateStatus { get; set; }
        [StringLength(200)]
        public string StatusRemarks { get; set; }
        [Range(1,long.MaxValue)]
        public long ConfigId { get; set; }
        [Range(1, long.MaxValue)]
        public long AllowanceNameId { get; set; }
    }
}
