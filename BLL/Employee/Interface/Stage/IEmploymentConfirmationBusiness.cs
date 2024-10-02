
using Shared.OtherModels.User;
using Shared.OtherModels.Response;
using Shared.OtherModels.DataService;
using Shared.Employee.ViewModel.Stage;
using Shared.Employee.Filter.Stage;

namespace BLL.Employee.Interface.Stage
{
    public interface IEmploymentConfirmationBusiness
    {
        Task<ExecutionStatus> SaveEmploymentConfirmationAsync(EmploymentConfirmationDTO model, AppUser user);
        Task<DBResponse<EmploymentConfirmationViewModel>> GetEmploymentConfirmationsAsync(Confimation_Filter filter, AppUser user);
        Task<IEnumerable<EmploymentConfirmationViewModel>> GetEmploymentConfirmationsDropdownAsync(Confimation_Filter filter, AppUser user);
        Task<ExecutionStatus> SaveEmploymentConfirmationStatusAsync(EmploymentConfirmationStatusDTO model, AppUser user);
        Task<IEnumerable<Select2Dropdown>> GetUnconfirmedEmployeeInfosInApplyAsync(AppUser user);
        Task<IEnumerable<Select2Dropdown>> GetUnconfirmedEmployeeInfosInUpdateAsync(AppUser user);
    }
}
