using Shared.Models;
using System.ComponentModel.DataAnnotations;

namespace Shared.Employee.ViewModel.Miscellaneous
{
    public class LevelViewModel : BaseViewModel2
    {
        public int LevelId { get; set; }
        [StringLength(200)]
        public string LevelName { get; set; }
        [StringLength(200)]
        public string LevelNameInBengali { get; set; }
        [StringLength(200)]
        public string Remarks { get; set; }
    }
}
