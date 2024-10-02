
using Shared.OtherModels.User;
using Shared.OtherModels.Response;
using Shared.Employee.ViewModel.Stage;
using Shared.Employee.Filter.Stage;


namespace BLL.Employee.Interface.Stage
{
    public interface IEmploymentProbationaryExtensionBusiness
    {
        Task<ExecutionStatus> SaveEmploymentProbationaryExtensionAsync(EmploymentProbationaryExtensionDTO model, AppUser user);
        Task<DBResponse<EmploymentProbationaryExtensionViewModel>> GetEmploymentProbationaryExtensionAsync(ProbationaryExtension_Filter model, AppUser user);
        Task<ExecutionStatus> SaveEmploymentProbationaryExtensionStatusAsync(EmploymentProbationaryStatusDTO model, AppUser user);

    }
}
