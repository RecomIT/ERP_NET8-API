using System.ComponentModel.DataAnnotations;

namespace Shared.Employee.DTO.Stage
{
    public class PromotionProposalCancellationDTO
    {
        [Range(1, long.MaxValue)]
        public long EmployeeId { get; set; }
        [Range(1, long.MaxValue)]
        public long ProposalId { get; set; }
    }
}
