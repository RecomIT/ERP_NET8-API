using Shared.OtherModels.User;
using Shared.OtherModels.Response;
using Shared.OtherModels.DataService;
using Shared.Employee.ViewModel.Organizational;
using Shared.Employee.Filter.Organizational;
using Shared.Employee.DTO.Organizational;

namespace BLL.Employee.Interface.Organizational
{
    public interface IDesignationBusiness
    {
        Task<IEnumerable<DesignationViewModel>> GetDesignationsAsync(Designation_Filter filter, AppUser user);
        Task<ExecutionStatus> SaveDesignationAsync(DesignationDTO model, AppUser user);
        Task<ExecutionStatus> ValidateDesignationAsync(DesignationDTO model, AppUser user);
        Task<IEnumerable<Dropdown>> GetDesignationDropdownAsync(AppUser user);
        Task<IEnumerable<Dropdown>> GetDesignationItemsAsync(List<string> items, AppUser user);
    }
}
