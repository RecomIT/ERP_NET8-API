using Shared.OtherModels.User;
using Shared.OtherModels.Response;
using Shared.Payroll.DTO.Allowance;
using Shared.Payroll.ViewModel.Configuration;
using Shared.Payroll.Filter.Allowance;

namespace BLL.Salary.Allowance.Interface
{
    public interface IAllowanceConfigBusiness
    {
        Task<ExecutionStatus> SaveAllowanceConfigAsync(AllowanceConfigurationDTO model, AppUser user);
        Task<IEnumerable<AllowanceConfigurationViewModel>> GetAllownaceConfigurationsAsync(AllowanceConfig_Filter filter, AppUser user);
        Task<AllowanceConfigurationViewModel> GetAllownaceConfigurationByAllowanceIdAsync(AllowanceConfig_Filter filter, AppUser user);
        Task<ExecutionStatus> SaveAllowanceConfigStatusAsync(AllowanceConfigStatusDTO model, AppUser user);
    }
}
