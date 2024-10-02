using Shared.Models;
using System.ComponentModel.DataAnnotations;

namespace Shared.Employee.ViewModel.Organizational
{
    public class GradeViewModel : BaseViewModel2
    {
        public int GradeId { get; set; }
        [Required, StringLength(100)]
        public string GradeName { get; set; }
        [StringLength(100)]
        public string GradeNameInBengali { get; set; }
        [StringLength(200)]
        public string Remarks { get; set; }
    }
}
