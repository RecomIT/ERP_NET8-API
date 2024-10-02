using Shared.OtherModels.User;
using Shared.OtherModels.Response;
using Shared.OtherModels.DataService;
using Shared.Employee.ViewModel.Organizational;
using Shared.Employee.Filter.Organizational;
using Shared.Employee.DTO.Organizational;

namespace BLL.Employee.Interface.Organizational
{
    public interface IDepartmentBusiness
    {
        Task<IEnumerable<DepartmentViewModel>> GetDepartmentsAsync(Department_Filter filter, AppUser user);
        Task<ExecutionStatus> SaveDepartmentAsync(DepartmentDTO model, AppUser user);
        Task<ExecutionStatus> ValidateDepartmentAsync(DepartmentDTO model, AppUser user);
        Task<IEnumerable<Dropdown>> GetDepartmentDropdownAsync(AppUser user);
        Task<IEnumerable<Dropdown>> GetDepartmentItemsAsync(List<string> items, AppUser user);

    }
}
