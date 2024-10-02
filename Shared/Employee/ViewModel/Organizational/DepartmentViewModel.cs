using Shared.Models;
using System.ComponentModel.DataAnnotations;


namespace Shared.Employee.ViewModel.Organizational
{
    public class DepartmentViewModel : BaseViewModel2
    {
        public int DepartmentId { get; set; }
        [Required, StringLength(150)]
        public string DepartmentName { get; set; }
        [StringLength(150)]
        public string DepartmentNameInBengali { get; set; }
        public bool IsActive { get; set; }
        [StringLength(200)]
        public string Remarks { get; set; }
    }
}
