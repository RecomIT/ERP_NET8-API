using Shared.OtherModels.User;
using Shared.OtherModels.Response;
using Shared.Employee.ViewModel.Organizational;
using Shared.Employee.Filter.Organizational;
using Shared.Employee.DTO.Organizational;

namespace BLL.Employee.Interface.Organizational
{
    public interface IUnitBusiness
    {
        Task<IEnumerable<UnitViewModel>> GetUnitsAsync(Unit_Filter filter, AppUser user);
        Task<ExecutionStatus> SaveUnitAsync(UnitDTO model, AppUser user);
        Task<ExecutionStatus> ValidateUnitAsync(UnitDTO model, AppUser user);
    }
}
