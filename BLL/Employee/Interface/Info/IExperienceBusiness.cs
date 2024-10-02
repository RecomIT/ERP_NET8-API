using Shared.OtherModels.User;
using Shared.OtherModels.Response;
using Shared.Employee.ViewModel.Info;
using Shared.Employee.Filter.Info;
using Shared.Employee.DTO.Info;

namespace BLL.Employee.Interface.Info
{
    public interface IExperienceBusiness
    {
        Task<IEnumerable<EmployeeExperienceVM>> GetEmployeeExperiencesAsync(EmployeeExperience_Filter fitler, AppUser user);
        Task<ExecutionStatus> EmployeeExperienceValidatorAsync(EmployeeExperienceDTO model, AppUser user);
        Task<ExecutionStatus> SaveEmployeeExperienceAsync(EmployeeExperienceDTO model, AppUser user);
        Task<ExecutionStatus> DeleteEmployeeExperienceAsync(DeleteEmployeeExperienceDTO model, AppUser user);
    }
}
