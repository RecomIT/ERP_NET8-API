using Shared.OtherModels.User;
using Shared.Payroll.DTO.Tax;
using Shared.Payroll.Filter.Tax;
using Shared.OtherModels.Response;
using Shared.Payroll.ViewModel.Tax;

namespace BLL.Tax.Interface
{
    public interface ITaxReturnSubmissionBusiness
    {
        Task<ExecutionStatus> SaveEmployeeTaxReturnSubmissionAsync(EmployeeTaxReturnSubmissionDTO model, AppUser user);
        Task<IEnumerable<EmployeeTaxReturnSubmissionViewModel>> GetEmployeeTaxReturnSubmissionAsync(EmployeeTaxReturnSubmission_Filter model, AppUser user);
        Task<EmployeeTaxReturnSubmissionViewModel> GetEmployeeTaxReturnSubmissionByIdAsync(long id, AppUser user);
    }
}
