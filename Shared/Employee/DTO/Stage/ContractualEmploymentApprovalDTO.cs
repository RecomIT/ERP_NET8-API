using Shared.Helpers.ValidationFilters;
using System;
using System.ComponentModel.DataAnnotations;

namespace Shared.Employee.DTO.Stage
{
    public class ContractualEmploymentApprovalDTO
    {
        [Range(1, long.MaxValue)]
        public long ContractId { get; set; }
        [Range(1, long.MaxValue)]
        public long EmployeeId { get; set; }
        public long LastContractId { get; set; }
        [StringLength(50), RequiredValue(new string[] { "Approved", "Recheck", "Cancelled" })]
        public string StateStatus { get; set; }
        public DateTime? LastContractEndDate { get; set; }
    }
}
