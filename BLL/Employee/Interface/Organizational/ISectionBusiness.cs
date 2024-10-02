using Shared.Employee.DTO.Organizational;
using Shared.Employee.Filter.Organizational;
using Shared.Employee.ViewModel.Organizational;
using Shared.OtherModels.DataService;
using Shared.OtherModels.Response;
using Shared.OtherModels.User;

namespace BLL.Employee.Interface.Organizational
{
    public interface ISectionBusiness
    {
        Task<IEnumerable<SectionViewModel>> GetSectionsAsync(Section_Filter filter, AppUser user);
        Task<ExecutionStatus> SaveSectionAsync(SectionDTO model, AppUser user);
        Task<ExecutionStatus> ValidateSectionAsync(SectionDTO model, AppUser user);
        Task<IEnumerable<Dropdown>> GetSectionDropdownAsync(Section_Filter filter, AppUser user);
    }
}
