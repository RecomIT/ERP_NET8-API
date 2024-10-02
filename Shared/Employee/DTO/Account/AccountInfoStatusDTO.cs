using Shared.Helpers.ValidationFilters;
using System.ComponentModel.DataAnnotations;

namespace Shared.Employee.DTO.Account
{
    public class AccountInfoStatusDTO
    {
        public long AccountInfoId { get; set; }
        public long EmployeeId { get; set; }
        [StringLength(50), RequiredValue(new string[] { "Approved", "Recheck", "Cancelled" })]
        public string StateStatus { get; set; }
    }
}
