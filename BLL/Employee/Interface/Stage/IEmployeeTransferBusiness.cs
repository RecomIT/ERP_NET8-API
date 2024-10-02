using Shared.OtherModels.User;
using Shared.OtherModels.Response;
using Shared.Employee.ViewModel.Stage;
using Shared.Employee.Filter.Stage;
using Shared.Employee.DTO.Stage;

namespace BLL.Employee.Interface.Stage
{
    public interface IEmployeeTransferBusiness
    {
        Task<DBResponse<EmployeeTransferProposalViewModel>> GetEmployeeTransferProposalsAsync(EmployeeTransfer_Filter query, AppUser user);
        Task<ExecutionStatus> SaveTransferProposalAsync(EmployeeTransferProposalDTO model, AppUser user);
        Task<ExecutionStatus> UploadTransferProposalAsync(List<TransferProposalReadExcelDTO> readExcelDTOs, AppUser user);
    }
}
