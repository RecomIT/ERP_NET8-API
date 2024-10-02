using System.ComponentModel.DataAnnotations;

namespace Shared.Payroll.DTO.Allowance
{
    public class AllowanceNameDTO
    {
        [Range(1, long.MaxValue)]
        public long AllowanceHeadId { get; set; }
        [StringLength(200)]
        public string AllowanceHeadName { get; set; }
        public long AllowanceNameId { get; set; }
        [Required, StringLength(200)]
        public string Name { get; set; }
        [StringLength(200)]
        public string AllowanceNameInBengali { get; set; }
        [StringLength(20)]
        public string GLCode { get; set; }
        [StringLength(100)]
        public string AllowanceFlag { get; set; } // Mark AS [Basic, HR, Conveynace]
        [StringLength(50)]
        public string AllowanceType { get; set; }
        public bool IsFixed { get; set; }
        public bool IsTaxble { get; set; }
        [StringLength(50)]
        public string TaxableType { get; set; }
    }
}
