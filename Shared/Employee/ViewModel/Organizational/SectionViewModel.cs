using Shared.Models;
using System.ComponentModel.DataAnnotations;

namespace Shared.Employee.ViewModel.Organizational
{
    public class SectionViewModel : BaseViewModel2
    {
        public int SectionId { get; set; }
        [Required, StringLength(100)]
        public string SectionName { get; set; }
        [StringLength(100)]
        public string SectionNameInBengali { get; set; }
        public bool IsActive { get; set; }
        [StringLength(200)]
        public string Remarks { get; set; }
        public int DeptZoneId { get; set; }
        public string ZoneName { get; set; }
        public long DepartmentId { get; set; }
        public string DepartmentName { get; set; }
    }
}
