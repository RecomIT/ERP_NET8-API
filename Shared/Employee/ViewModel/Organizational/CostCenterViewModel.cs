using Shared.Models;
using System.ComponentModel.DataAnnotations;

namespace Shared.Employee.ViewModel.Organizational
{
    public class CostCenterViewModel : BaseViewModel2
    {
        public int CostCenterId { get; set; }
        [Required, StringLength(150)]
        public string CostCenterName { get; set; }
        [StringLength(150)]
        public string CostCenterNameInBengali { get; set; }
        [StringLength(150)]
        public string CostCenterCode { get; set; }
        public bool IsActive { get; set; }
        [StringLength(200)]
        public string Remarks { get; set; }
    }
}
