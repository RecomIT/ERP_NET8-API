using Shared.OtherModels.User;
using Shared.OtherModels.Response;
using Shared.Employee.ViewModel.Education;
using Shared.Employee.Filter.Info;
using Shared.Employee.DTO.Info;

namespace BLL.Employee.Interface.Education
{
    public interface IEducationBusiness
    {
        Task<IEnumerable<EmployeeEducationVM>> GetEmployeeEducationsAsync(Education_Filter filter, AppUser user);
        Task<ExecutionStatus> SaveEmployeeEducationAsync(EmployeeEducationDTO model, AppUser user);
        Task<ExecutionStatus> DeleteEmployeeEducationAsync(DeleteEmployeeEducationDTO model, AppUser user);
    }
}
