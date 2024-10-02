using Shared.OtherModels.User;
using Shared.OtherModels.Response;
using Shared.Payroll.Filter.Allowance;
using Shared.Payroll.DTO.Configuration;
using Shared.Payroll.ViewModel.Configuration;
using Shared.OtherModels.DataService;
using Shared.Payroll.DTO.Allowance;

namespace BLL.Salary.Allowance.Interface
{
    public interface ISalaryAllowanceConfigBusiness
    {
        Task<ExecutionStatus> SaveSalaryAllowanceConfigAsync(SalaryAllowanceConfigurationInfoDTO model, AppUser user);
        Task<IEnumerable<SalaryAllowanceConfigurationInfoViewModel>> GetSalaryAllowanceConfigurationInfosAsync(SalaryAllowanceConfig_Filter filter, AppUser user);
        Task<IEnumerable<SalaryAllowanceConfigurationDetailViewModel>> GetSalaryAllowanceConfigurationDetailsAsync(long salaryAllowanceConfigId, AppUser user);
        Task<ExecutionStatus> SaveSalaryAllowanceConfigStatusAsync(SalaryAllowanceConfigurationStatusDTO model, AppUser user);
        Task<ExecutionStatus> SaveAsync(SalaryAllowanceConfigurationInfoDTO model, AppUser user);
        Task<IEnumerable<SalaryAllowanceConfigurationInfoViewModel>> GetAllAsync(SalaryAllowanceConfig_Filter filter, AppUser user);
        Task<IEnumerable<KeyValue>> GetHeadsInfoAsync(Breakhead_Filter filter, AppUser user);
        Task<ExecutionStatus> DeletePendingConfigAsync(long id, AppUser user);
        Task<ExecutionStatus> ApprovedPendingConfigAsync(ApprovedPendingSalaryAllowanceConfigDTO model, AppUser user);
    }
}
