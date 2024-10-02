using Shared.OtherModels.User;
using Shared.OtherModels.Response;
using Shared.Employee.ViewModel.Stage;
using Shared.Employee.Filter.Stage;
using Shared.Employee.DTO.Stage;
using Shared.Employee.Domain.Stage;

namespace BLL.Employee.Interface.Stage
{
    public interface IEmployeePromotionBusiness
    {
        Task<DBResponse<EmployeePromotionProposalViewModel>> GetEmployeePromotionProposalsAsync(EmployeePromotion_Filter query, AppUser user);
        Task<ExecutionStatus> SavePromotionProposalAsync(EmployeePromotionProposalDTO model, AppUser user);
        Task<ExecutionStatus> UploadPromotionProposalAsync(List<PromotionProposalReadExcelDTO> readExcelDTOs, AppUser user);
        Task<ExecutionStatus> DeleteEmployeePendingProposalAsync(PromotionProposalCancellationDTO model, AppUser user);
        Task<ExecutionStatus> ApprovalProposalAsync(long id, long employeeId, AppUser user);
        Task<EmployeePromotionProposal> SingleEmployeePendingProposalAsync(long id, long employeeId, AppUser user);

    }
}
