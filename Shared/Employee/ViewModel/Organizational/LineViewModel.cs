using Shared.Models;
using System.ComponentModel.DataAnnotations;

namespace Shared.Employee.ViewModel.Organizational
{
    public class LineViewModel : BaseViewModel2
    {
        public long LineId { get; set; }
        [Required, StringLength(100)]
        public string LineName { get; set; }
        [StringLength(100)]
        public string LineNameInBengali { get; set; }
        [StringLength(100)]
        public string ShortName { get; set; }
        [StringLength(10)]
        public string LineCode { get; set; } //LN-0001
        public bool IsActive { get; set; }
        [StringLength(200)]
        public string Remarks { get; set; }
        public int? DepartmentId { get; set; }
        public int? SectionId { get; set; }
        // Custom Property
        public string DepartmentName { get; set; }
        public string SectionName { get; set; }
    }
}
