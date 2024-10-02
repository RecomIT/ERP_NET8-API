using Shared.Employee.DTO.Locational;
using Shared.Employee.Filter.Locational;
using Shared.Employee.ViewModel.Locational;
using Shared.OtherModels.Response;
using Shared.OtherModels.User;


namespace BLL.Employee.Interface.Locational
{
    public interface IDivisionBusiness
    {
        Task<IEnumerable<DivisionViewModel>> GetDivisionsAsync(Division_Filter filter, AppUser user);
        Task<ExecutionStatus> SaveDivisionAsync(DivisionDTO model, AppUser user);
        Task<ExecutionStatus> ValidateDivisionAsync(DivisionDTO model, AppUser user);
    }
}
