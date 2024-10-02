using Shared.OtherModels.User;
using Shared.OtherModels.Response;
using Shared.Employee.ViewModel.Info;
using Shared.Employee.Filter.Info;
using Shared.Employee.DTO.Info;

namespace BLL.Employee.Interface.Info
{
    public interface ISkillBusiness
    {
        Task<IEnumerable<EmployeeSkillVM>> GetEmployeeSkillsAsync(Skill_Filter filter, AppUser user);
        Task<ExecutionStatus> EmployeeSkillValidatorAsync(EmployeeSkillDTO model, AppUser user);
        Task<ExecutionStatus> SaveEmployeeSkillAsync(EmployeeSkillDTO model, AppUser user);
        Task<ExecutionStatus> DeleteEmployeeSkillAsync(DeleteEmployeeSkillDTO model, AppUser user);
    }
}
