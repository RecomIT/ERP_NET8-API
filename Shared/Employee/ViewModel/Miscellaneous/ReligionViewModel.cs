using Shared.Models;
using System.ComponentModel.DataAnnotations;

namespace Shared.Employee.ViewModel.Miscellaneous
{
    public class ReligionViewModel : BaseViewModel2
    {
        public int ReligionId { get; set; }
        [Required, StringLength(50)]
        public string ReligionName { get; set; }
        [StringLength(100)]
        public string ReligionNameInBengali { get; set; }
        [StringLength(20)]
        public string ReligionCode { get; set; }
    }
}
