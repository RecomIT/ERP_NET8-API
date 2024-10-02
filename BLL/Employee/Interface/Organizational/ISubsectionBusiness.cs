using Shared.OtherModels.User;
using Shared.OtherModels.Response;
using Shared.OtherModels.DataService;
using Shared.Employee.ViewModel.Organizational;
using Shared.Employee.Filter.Organizational;
using Shared.Employee.DTO.Organizational;

namespace BLL.Employee.Interface.Organizational
{
    public interface ISubSectionBusiness
    {
        Task<IEnumerable<SubSectionViewModel>> GetSubSectionsAsync(SubSection_Filter filter, AppUser user);
        Task<ExecutionStatus> SaveSubSectionAsync(SubSectionDTO model, AppUser user);
        Task<ExecutionStatus> ValidateSubSectionAsync(SubSectionDTO model, AppUser user);
        Task<IEnumerable<Dropdown>> GetSubSectionDropdownAsync(SubSection_Filter filter, AppUser user);
    }
}
