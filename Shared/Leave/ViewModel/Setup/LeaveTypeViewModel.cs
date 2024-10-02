using Shared.Models;
using System.ComponentModel.DataAnnotations;

namespace Shared.Leave.ViewModel.Setup
{
    public class LeaveTypeViewModel : BaseViewModel2
    {
        public long Id { get; set; }
        [Required, StringLength(100)]
        public string Title { get; set; }
        [StringLength(100)]
        public string TitleInBengali { get; set; }
        [Required, StringLength(10)]
        public string ShortName { get; set; }
        [StringLength(30)]
        public string ShortNameInBangali { get; set; }
        public bool IsActive { get; set; }
    }
}
