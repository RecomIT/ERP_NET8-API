using Shared.Models;
using System.ComponentModel.DataAnnotations;

namespace Shared.Employee.ViewModel.Miscellaneous
{
    public class BloodGroupViewModel : BaseViewModel2
    {
        public int BloodGroupId { get; set; }
        [Required, StringLength(10)]
        public string BloodGroupName { get; set; }
        [StringLength(100)]
        public string BloodGroupNameInBengali { get; set; }
    }
}
