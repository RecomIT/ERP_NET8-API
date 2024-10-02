using Shared.OtherModels.Pagination;
using System.ComponentModel.DataAnnotations;

namespace Shared.Employee.Filter.Info
{
    public class EmployeeQuery : Sortparam
    {
        public string EmployeeId { get; set; }
        [StringLength(50)]
        public string EmployeeCode { get; set; }
        [StringLength(50)]
        public string FullName { get; set; }
        [StringLength(50)]
        public string DesignationId { get; set; }
        [StringLength(50)]
        public string DepartmentId { get; set; }
        [StringLength(50)]
        public string BranchId { get; set; }
        [StringLength(50)]
        public string Gender { get; set; }
        [StringLength(50)]
        public string DateOfJoining { get; set; }
        [StringLength(50)]
        public string StateStatus { get; set; }
        public string JobStatus { get; set; }
    }
}
