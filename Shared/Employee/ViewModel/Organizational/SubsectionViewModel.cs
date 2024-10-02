using Shared.Models;
using System.ComponentModel.DataAnnotations;

namespace Shared.Employee.ViewModel.Organizational
{
    public class SubSectionViewModel : BaseViewModel2
    {
        public int SubSectionId { get; set; }
        [Required, StringLength(100)]
        public string SubSectionName { get; set; }
        [StringLength(100)]
        public string SubSectionNameInBengali { get; set; }
        public bool IsActive { get; set; }
        [StringLength(200)]
        public string Remarks { get; set; }
        public int SectionId { get; set; }
        [StringLength(100)]
        public string SectionName { get; set; }
    }
}
