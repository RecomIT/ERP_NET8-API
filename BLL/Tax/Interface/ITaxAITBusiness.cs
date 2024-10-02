using Shared.Models;
using Shared.OtherModels.User;
using Shared.OtherModels.Response;
using Shared.OtherModels.DataService;
using Shared.Payroll.ViewModel.Tax;
using Shared.Payroll.Filter.Tax;
using Shared.Payroll.DTO.Tax;

namespace BLL.Tax.Interface
{
    public interface ITaxAITBusiness
    {
        Task<ExecutionStatus> SaveAITAsync(TaxDocumentSubmissionDTO model, AppUser user);
        Task<DBResponse<TaxDocumentSubmissionViewModel>> GetEmployeeAITDocumentsAsync(TaxDocumentQuery query, AppUser User);
        Task<IEnumerable<Select2Dropdown>> GetAITFiscalYearsExtensionAsync(string fiscalYearRange, AppUser User);
        Task<decimal> GetCarAITAmountInTaxProcessAsync(long employeeId, long fiscalYearId, AppUser user);
        Task<decimal> GetTaxRefundAmountInTaxProcessAsync(long employeeId, long fiscalYearId, AppUser user);
        Task<ExecutionStatus> SaveAsync(AITSubmissionDTO model, AppUser user);
        Task<ExecutionStatus> CheckingAsync(string model, AppUser user);
        Task<ExecutionStatus> BlukCheckingAsync(List<CheckingModel> models, AppUser user);
        Task<DBResponse<TaxDocumentSubmissionViewModel>> GetAllAsync(TaxDocumentQuery filter, AppUser user);
        Task<IEnumerable<TaxDocumentSubmissionViewModel>> GetIndividualEmployeeAsync(TaxDocumentQuery filter, AppUser user);
        Task<TaxDocumentSubmissionViewModel> GetSingleAsync(TaxDocumentQuery filter, AppUser user);
        Task<ExecutionStatus> UploadAsync(List<TaxDocumentSubmissionDTO> mode, AppUser user);
        Task<ExecutionStatus> DeleteAsync(long id, AppUser user);
    }
}
