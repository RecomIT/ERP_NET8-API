using Shared.OtherModels.User;
using Shared.OtherModels.Response;
using Shared.OtherModels.DataService;
using Shared.Payroll.DTO.WalletPayment;
using Shared.Payroll.Filter.WalletPayment;
using BLL.Salary.WalletPayment.Implementation;

namespace BLL.Salary.WalletPayment.Interface
{
    public interface IWalletPaymentBusiness
    {
        Task<IEnumerable<Select2Dropdown>> GetInternalDesignationExtensionAsync(long? internalDesignationId, AppUser user);
        Task<ExecutionStatus> ValidateWalletPaymentAsync(List<WalletPaymentConfigurationDTO> configurationDTOs, AppUser user);
        Task<ExecutionStatus> SaveWalletPaymentConfigurationsAsync(List<WalletPaymentConfigurationDTO> configurationDTOs, AppUser user);
        Task<DBResponse<WalletPaymentConfigurationViewModel>> GetWalletPaymentConfigurationsAsync(WalletPaymentConfiguration_Filter filter, AppUser user);
        Task<IEnumerable<WalletPaymentConfigurationViewModel>> GetWalletPaymentConfigByIdAsync(long walletConfigId, AppUser user);
        Task<ExecutionStatus> UpdateWalletPaymentConfigurationsAsync(WalletPaymentConfigurationDTO dTO, AppUser user);
    }
}
