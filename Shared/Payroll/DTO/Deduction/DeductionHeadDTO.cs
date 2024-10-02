using System.ComponentModel.DataAnnotations;

namespace Shared.Payroll.DTO.Deduction
{
    public class DeductionHeadDTO
    {
        public long DeductionHeadId { get; set; }
        [Required, StringLength(200)]
        public string DeductionHeadName { get; set; }
        [StringLength(20)]
        public string DeductionHeadCode { get; set; }
        [StringLength(200)]
        public string DeductionHeadNameInBengali { get; set; }
        public bool IsActive { get; set; }
    }
}
