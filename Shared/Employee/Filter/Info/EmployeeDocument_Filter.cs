using System.ComponentModel.DataAnnotations;

namespace Shared.Employee.Filter.Info
{
    public class EmployeeDocument_Filter
    {
        public string DocumentId { get; set; }
        public string EmployeeId { get; set; }
        [StringLength(50)]
        public string EmployeeCode { get; set; }
    }
}
