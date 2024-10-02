using Shared.OtherModels.DataService;
using System.Collections.Generic;

namespace Shared.Employee.Filter.Info
{
    public class EmployeeInfoReport_Filter
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string BranchId { get; set; }
        public string DesignationId { get; set; }
        public string DepartmentId { get; set; }
        public string DateOfJoiningFrom { get; set; }
        public string DateOfJoiningTo { get; set; } = null;
        public string Status { get; set; }
        public string JobStatus { get; set; }
        public string ServiceLength { get; set; }
        public string Gender { get; set; }
        public string MaritalStatus { get; set; }
        public string Religion { get; set; }
        public string Age { get; set; }
        public string LastWorkingDateFrom { get; set; } = null;
        public string LastWorkingDateTo { get; set; } = null;
        public string DateOfBirthFrom { get; set; } = null;
        public string DateOfBirthTo { get; set; } = null;
        public string IsResdential { get; set; }
        public string BloodGroup { get; set; }
        public List<KeyValue> Columns { get; set; }
        public string ColumnsJson { get; set; }
    }
}
