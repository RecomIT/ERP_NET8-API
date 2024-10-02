using Shared.OtherModels.User;
using Shared.Employee.DTO.Stage;
using Shared.OtherModels.Response;
using Shared.Employee.Domain.Stage;
using DAL.Repository.Base.Interface;

namespace DAL.Repository.Employee.Interface
{
    public interface IEmployeePromotionProposalRepository : IDapperBaseRepository<EmployeePromotionProposal>
    {
        Task<ExecutionStatus> DeletePendingProposalAsync(PromotionProposalCancellationDTO model, AppUser user);
        Task<ExecutionStatus> ApprovalProposalAsync(long id, long employeeId, AppUser user);
        Task<EmployeePromotionProposal> SingleEmployeePendingProposalAsync(long id, long employeeId, AppUser user);
    }
}
