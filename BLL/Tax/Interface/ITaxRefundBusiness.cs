using Shared.Models;
using Shared.OtherModels.User;
using Shared.OtherModels.Response;
using Shared.Payroll.ViewModel.Tax;
using Shared.Payroll.Filter.Tax;
using Shared.Payroll.DTO.Tax;

namespace BLL.Tax.Interface
{
    public interface ITaxRefundBusiness
    {
        Task<DBResponse<TaxDocumentSubmissionViewModel>> GetAllAsync(TaxDocumentQuery filter, AppUser user);
        Task<IEnumerable<TaxDocumentSubmissionViewModel>> GetIndividualEmployeeAsync(TaxDocumentQuery filter, AppUser user);
        Task<TaxDocumentSubmissionViewModel> GetSingleAsync(TaxDocumentQuery filter, AppUser user);
        Task<ExecutionStatus> SaveAsync(TaxRefundSubmissionDTO model, AppUser user);
        Task<ExecutionStatus> CheckingAsync(string model, AppUser user);
        Task<ExecutionStatus> BlukCheckingAsync(List<CheckingModel> models, AppUser user);
        Task<ExecutionStatus> UploadAsync(List<TaxDocumentSubmissionDTO> mode, AppUser user);
        Task<ExecutionStatus> DeleteAsync(long id, AppUser user);
    }
}
