using Shared.Employee.DTO.Info;
using Shared.Employee.Filter.Info;
using Shared.Employee.ViewModel.Info;
using Shared.OtherModels.DataService;
using Shared.OtherModels.Response;
using Shared.OtherModels.User;

namespace BLL.Employee.Interface.Info
{
    public interface IHierarchyBusiness
    {
        Task<IEnumerable<EmployeeHierarchyViewModel>> GetEmployeeHierarchyAsync(EmployeeHierarchy_Filter filter, AppUser user);
        Task<EmployeeHierarchyViewModel> GetEmployeeActiveHierarchyAsync(long employeeId, AppUser user);
        Task<ExecutionStatus> SaveEmployeeHierarchyAsync(EmployeeHierarchyDTO EmployeeHierarchy, AppUser user);
        Task<ExecutionStatus> EmployeeHierarchyValidatorAsync(EmployeeHierarchyDTO EmployeeHierarchy, AppUser user);
        Task<IEnumerable<Select2Dropdown>> GetSubordinatesAsync(long id, AppUser user);
    }
}
