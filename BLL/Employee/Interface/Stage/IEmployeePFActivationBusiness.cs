using Shared.Employee.DTO.Info;
using Shared.Employee.DTO.Stage;
using Shared.Employee.Filter.Info;
using Shared.Employee.Filter.Stage;
using Shared.Employee.ViewModel.Stage;
using Shared.OtherModels.DataService;
using Shared.OtherModels.Response;
using Shared.OtherModels.User;

namespace BLL.Employee.Interface.Stage
{
    public interface IEmployeePFActivationBusiness
    {
        Task<IEnumerable<Select2Dropdown>> GetConfirmedEmployeesToAssignPFAsync(ConfirmedEmployeesToAssignPF_Filter filter, AppUser user);
        Task<ExecutionStatus> SavePFActivationAsync(EmployeePFActivationDTO model, AppUser user);
        Task<IEnumerable<EmployeePFActivationViewModel>> GetEmployeePFActivationListAsync(PFActivation_Filter filter, AppUser user);
        Task<IEnumerable<Select2Dropdown>> GetPFBasedAmountDropdownAsync(string baseAmount, AppUser user);
        Task<ExecutionStatus> SavePFActivationApprovalAsync(EmployeePFActivationApprovalDTO model, AppUser user);
        Task<ExecutionStatus> UploadPFActivationExcelAsync(List<EmployeePFActivationViewModel> models, AppUser user);
        Task<EmployeePFActivationViewModel> EmployeePFActionInfoAysnc(long employeeId, AppUser user);
    }
}
