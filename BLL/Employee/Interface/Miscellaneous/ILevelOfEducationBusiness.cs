using Shared.OtherModels.User;
using Shared.OtherModels.Response;
using Shared.OtherModels.DataService;
using Shared.Employee.ViewModel.Education;
using Shared.Employee.Filter.Education;
using Shared.Employee.DTO.Eudcation;

namespace BLL.Employee.Interface.Miscellaneous
{
    public interface ILevelOfEducationBusiness
    {
        Task<IEnumerable<LevelOfEducationViewModel>> GetLevelOfEducationsAsync(LevelOfEducation_Fiter filter, AppUser user);
        Task<IEnumerable<Dropdown>> GetLevelOfEducationsDropdownAsync(AppUser user);
        Task<ExecutionStatus> SaveLevelOfEducationAsync(LevelOfEducationDTO model, AppUser user);
        Task<ExecutionStatus> ValidateLevelOfEducationAsync(LevelOfEducationDTO model, AppUser user);
    }
}
