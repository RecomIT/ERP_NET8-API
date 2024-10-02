using Shared.OtherModels.User;
using Shared.OtherModels.Response;
using Shared.OtherModels.DataService;
using Shared.Employee.DTO.Organizational;
using Shared.Employee.Filter.Organizational;
using Shared.Employee.ViewModel.Organizational;

namespace BLL.Employee.Interface.Organizational
{
    public interface ICostCenterBusiness
    {
        Task<IEnumerable<CostCenterViewModel>> GetCostCentersAsync(CostCenter_Filter filter, AppUser user);
        Task<ExecutionStatus> SaveCostCenterAsync(CostCenterDTO model, AppUser user);
        Task<ExecutionStatus> SaveAsync(CostCenterDTO model, AppUser user);
        Task<ExecutionStatus> ValidateCostCenterAsync(CostCenterDTO model, AppUser user);
        Task<IEnumerable<Dropdown>> GetCostCenterDropdownAsync(AppUser user);
    }
}
