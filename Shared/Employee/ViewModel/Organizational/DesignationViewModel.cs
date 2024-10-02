using Shared.Models;
using System.ComponentModel.DataAnnotations;

namespace Shared.Employee.ViewModel.Organizational
{
    public class DesignationViewModel : BaseViewModel2
    {
        public int DesignationId { get; set; }
        [StringLength(100)]
        public string DesignationName { get; set; }
        [StringLength(20)]
        public string ShortName { get; set; }
        [StringLength(100)]
        public string DesignationNameInBengali { get; set; }
        [StringLength(100)]
        public string DesignationGroup { get; set; }
        [StringLength(100)]
        public string SalaryGroup { get; set; }
        [StringLength(200)]
        public string Remarks { get; set; }
        public int GradeId { get; set; }
        [StringLength(100)]
        public string GradeName { get; set; }
    }
}
