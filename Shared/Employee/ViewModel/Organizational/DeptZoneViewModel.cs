using Shared.Models;
using System.ComponentModel.DataAnnotations;


namespace Shared.Employee.ViewModel.Organizational
{
    public class DeptZoneViewModel : BaseViewModel2
    {
        public int DeptZoneId { get; set; }
        [Required, StringLength(100)]
        public string ZoneName { get; set; }
        [StringLength(100)]
        public string ZoneNameInBengali { get; set; }
        public bool IsActive { get; set; }
        [StringLength(200)]
        public string Remarks { get; set; }
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
    }
}
