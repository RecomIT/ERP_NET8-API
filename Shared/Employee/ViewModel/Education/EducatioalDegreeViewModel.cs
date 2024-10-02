using Shared.Models;
using System.ComponentModel.DataAnnotations;

namespace Shared.Employee.ViewModel.Education
{
    public class EducatioalDegreeViewModel : BaseViewModel2
    {
        public int EducatioalDegreeId { get; set; }
        [Required, StringLength(50)]
        public string DegreeName { get; set; }
        [StringLength(200)]
        public string DegreeNameInBengali { get; set; }
        public int LevelOfEducationId { get; set; }
    }
}
