

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Payroll.DTO.Payment
{
    public class SupplementaryProcessDetailDTO
    {
        [Range(1,long.MaxValue)]
        public long EmployeeId { get; set; }
        public string EmployeeCode { get; set; }
        public string Designation { get; set; }
        [Required,Column(TypeName ="decimal(18,2)")]
        public decimal Amount { get; set; }
    }
}
