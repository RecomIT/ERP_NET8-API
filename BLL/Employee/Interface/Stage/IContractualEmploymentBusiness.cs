using Shared.OtherModels.User;
using Shared.OtherModels.Response;
using Shared.Employee.DTO.Stage;
using Shared.Employee.ViewModel.Stage;
using Shared.Employee.Filter.Stage;
using Shared.Employee.Domain.Stage;

namespace BLL.Employee.Interface.Stage
{
    public interface IContractualEmploymentBusiness
    {
        Task<ExecutionStatus> TerminateContractAysnc(AppUser user);
        Task<ExecutionStatus> SaveRenewContractAysnc(ContractualEmploymentDTO model, AppUser user);
        Task<ExecutionStatus> UpdateRenewContractAysnc(ContractualEmploymentDTO model, AppUser user);
        Task<DBResponse<ContractualEmploymentViewModel>> GetContractualEmploymentsInfoAsync(ContractualEmployment_Filter filter, AppUser user);
        Task<ContractualEmploymentViewModel> GetEmployeeLastContractInfo(long employeeId, AppUser user);
        Task<IEnumerable<ExecutionStatus>> UploadEmployeeContractAsync(IEnumerable<ContractualEmploymentDTO> models, AppUser user);
        Task<ExecutionStatus> SaveEmployeeContractApprovalAsync(ContractualEmploymentApprovalDTO model, AppUser user);
        Task<ContractualEmployment> GetContractualEmploymentById(long id, AppUser user);
    }
}
