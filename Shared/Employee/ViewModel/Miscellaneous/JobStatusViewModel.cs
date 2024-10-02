using Shared.Models;
using System.ComponentModel.DataAnnotations;

namespace Shared.Employee.ViewModel.Miscellaneous
{
    public class JobStatusViewModel : BaseViewModel2
    {
        public long StatusId { get; set; }
        [Required, StringLength(100)]
        public string JobStatusName { get; set; }
        [StringLength(100)]
        public string JobStatusNameInBengali { get; set; }
        [StringLength(200)]
        public string Remarks { get; set; }
    }
}
