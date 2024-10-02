using Shared.OtherModels.User;
using Shared.OtherModels.Response;
using Shared.OtherModels.DataService;
using Shared.Employee.DTO.Organizational;
using Shared.Employee.Filter.Organizational;
using Shared.Employee.ViewModel.Organizational;

namespace BLL.Employee.Interface.Organizational
{
    public interface IGradeBusiness
    {
        Task<IEnumerable<GradeViewModel>> GetGradesAsync(Grade_Filter filter, AppUser user);
        Task<ExecutionStatus> SaveGradeAsync(GradeDTO model, AppUser user);
        Task<ExecutionStatus> ValidateGradeAsync(GradeDTO model, AppUser user);
        Task<IEnumerable<Dropdown>> GetGradeDropdownAsync(AppUser user);
        Task<IEnumerable<Dropdown>> GetGradeItemsAsync(List<string> items, AppUser user);
    }
}
