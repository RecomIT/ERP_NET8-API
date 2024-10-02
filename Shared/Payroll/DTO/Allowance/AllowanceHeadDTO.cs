using System.ComponentModel.DataAnnotations;

namespace Shared.Payroll.DTO.Allowance
{
    public class AllowanceHeadDTO
    {
        public long AllowanceHeadId { get; set; }
        [Required, StringLength(200)]
        public string AllowanceHeadName { get; set; }
        [StringLength(20)]
        public string AllowanceHeadCode { get; set; }
        [StringLength(200)]
        public string AllowanceHeadNameInBengali { get; set; }
        public bool IsActive { get; set; }
    }
}
