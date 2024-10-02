using Shared.Models;
using System.ComponentModel.DataAnnotations;

namespace Shared.Employee.ViewModel.Organizational
{
    public class UnitViewModel : BaseViewModel2
    {
        public int UnitId { get; set; }
        [StringLength(100)]
        public string UnitName { get; set; }
        [StringLength(100)]
        public string UnitNameInBengali { get; set; }
        public bool IsActive { get; set; }
        [StringLength(200)]
        public string Remarks { get; set; }
        public int SubSectionId { get; set; }
        [StringLength(100)]
        public string SubSectionName { get; set; }
    }
}
