using Shared.Models;
using System.ComponentModel.DataAnnotations;

namespace Shared.Employee.ViewModel.Miscellaneous
{
    public class JobtypeViewModel : BaseViewModel2
    {
        public int JobTypeId { get; set; }
        [Required, StringLength(50)]
        public string JobTypeName { get; set; }
        [StringLength(50)]
        public string JobTypeNameInBengali { get; set; }
        [StringLength(100)]
        public string Duration { get; set; }
        [StringLength(200)]
        public string Remarks { get; set; }
    }
}
