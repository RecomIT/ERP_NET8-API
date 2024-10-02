using Shared.OtherModels.Pagination;
using System.ComponentModel.DataAnnotations;

namespace Shared.Employee.Filter.Info
{
    public class EmployeePersonalInfoQuery : Sortparam
    {
        public string EmployeeId { get; set; }
        [StringLength(50)]
        public string EmployeeCode { get; set; }
        [StringLength(50)]
        public string FullName { get; set; }
        public string FamilyPersonName { get; set; }
        public string Gender { get; set; }
        public string DateOfBirthFrom { get; set; }
        public string DateOfBirthTo { get; set; }
        public string BloodGroup { get; set; }
        public string Religion { get; set; }
    }
}
