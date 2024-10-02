using Shared.OtherModels.User;
using Shared.OtherModels.Response;
using Shared.OtherModels.DataService;
using Shared.Employee.Domain.Organizational;
using Shared.Employee.Filter.Organizational;
using Shared.Employee.DTO.Organizational;

namespace BLL.Employee.Interface.Organizational
{
    public interface IEmployeeTypeBusiness
    {
        Task<IEnumerable<EmployeeType>> GetEmployeeTypesAsync(EmployeeType_Filter filter, AppUser user);
        Task<IEnumerable<Dropdown>> GetEmployeeTypeDropdownAsync(AppUser user);
        Task<ExecutionStatus> SaveEmployeeTypeAsync(EmployeeTypeDTO model, AppUser user);
    }
}
