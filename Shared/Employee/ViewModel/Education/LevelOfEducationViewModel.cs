using Shared.Models;
using System.ComponentModel.DataAnnotations;


namespace Shared.Employee.ViewModel.Education
{
    public class LevelOfEducationViewModel : BaseViewModel2
    {
        public int LevelOfEducationId { get; set; }
        [StringLength(150)]
        public string Name { get; set; }
        [StringLength(150)]
        public string NameInBengali { get; set; }
    }
}
