using Shared.OtherModels.User;
using Shared.OtherModels.Response;
using Shared.Payroll.Domain.Deduction;
using Shared.Payroll.ViewModel.Deduction;

namespace BLL.Salary.Deduction.Interface
{
    public interface IDeductionConfigBusiness
    {
        Task<ExecutionStatus> SaveDeductionConfigAsync(DeductionConfiguration deduction, AppUser user);
        Task<IEnumerable<DeductionConfigurationViewModel>> GetDeductionConfigurationsAsync(long deductionNameId, string status, string activationDateFrom, string activationDateTo, string deactivationDateFrom, string deactivationDateTo, AppUser user);
        Task<DeductionConfigurationViewModel> GetDeductionConfigurationAsync(long configId, long deductionNameId, AppUser user);
        Task<ExecutionStatus> SaveDeductionConfigurationStatusAsync(string status, string remarks, long configId, long deductionNameId, AppUser user);
    }
}
