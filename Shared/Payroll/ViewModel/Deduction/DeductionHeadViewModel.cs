using Shared.Models;
using System.ComponentModel.DataAnnotations;


namespace Shared.Payroll.ViewModel.Deduction
{
    public class DeductionHeadViewModel : BaseViewModel1
    {
        public long DeductionHeadId { get; set; }
        [Required, StringLength(200)]
        public string DeductionHeadName { get; set; }
        [StringLength(20)]
        public string DeductionHeadCode { get; set; }
        public bool IsActive { get; set; }
        [StringLength(200)]
        public string DeductionHeadNameInBengali { get; set; }
    }
}
