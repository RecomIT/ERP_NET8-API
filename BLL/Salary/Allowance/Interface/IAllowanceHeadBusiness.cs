using Shared.OtherModels.DataService;
using Shared.OtherModels.Response;
using Shared.OtherModels.User;
using Shared.Payroll.DTO.Allowance;
using Shared.Payroll.Filter.Allowance;
using Shared.Payroll.ViewModel.Allowance;

namespace BLL.Salary.Allowance.Interface
{
    public interface IAllowanceHeadBusiness
    {
        Task<ExecutionStatus> SaveAllowanceHeadAsync(AllowanceHeadDTO model, AppUser user);
        Task<IEnumerable<AllowanceHeadViewModel>> GetAllowanceHeadsAsync(AllowanceHead_Filter filter, AppUser user);
        Task<IEnumerable<Select2Dropdown>> GetAllowanceHeadExtensionAsync(AppUser user);
        Task<ExecutionStatus> AllowanceHeadValidatorAsync(AllowanceHeadDTO model, AppUser user);
        Task<IEnumerable<Dropdown>> GetAllowanceHeadDropdownAsync(AppUser user);
    }
}
