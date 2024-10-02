using Shared.OtherModels.User;
using Shared.OtherModels.Response;
using Shared.Employee.ViewModel.Organizational;
using Shared.Employee.Filter.Organizational;
using Shared.Employee.DTO.Organizational;

namespace BLL.Employee.Interface.Organizational
{
    public interface ILineBusiness
    {
        Task<IEnumerable<LineViewModel>> GetLinesAsync(Line_Filter filter, AppUser user);
        Task<ExecutionStatus> SaveLineAsync(LineDTO model, AppUser user);
        Task<ExecutionStatus> ValidateLineAsync(LineDTO model, AppUser user);
    }
}
