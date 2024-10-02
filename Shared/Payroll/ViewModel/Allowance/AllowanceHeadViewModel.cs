using Shared.Models;
using System.ComponentModel.DataAnnotations;

namespace Shared.Payroll.ViewModel.Allowance
{
    public class AllowanceHeadViewModel : BaseViewModel1
    {
        public long AllowanceHeadId { get; set; }
        [Required, StringLength(200)]
        public string AllowanceHeadName { get; set; }
        [StringLength(20)]
        public string AllowanceHeadCode { get; set; }
        public bool IsActive { get; set; }
        [StringLength(200)]
        public string AllowanceHeadNameInBengali { get; set; }
    }
}
