using Shared.OtherModels.User;
using Shared.OtherModels.Response;
using Shared.OtherModels.DataService;
using Shared.Payroll.ViewModel.Payment;
using Shared.Payroll.Filter.Payment;
using Shared.Payroll.DTO.Payment;

namespace BLL.Salary.Payment.Interface
{
    public interface IConditionalDepositAllowanceConfigBusiness
    {
        Task<IEnumerable<ConditionalDepositAllowanceConfigViewModel>> GetConditionalDepositAllowanceConfigsAsync(ConditionalDepositAllowanceConfig_Filter filter, AppUser user);
        Task<ConditionalDepositAllowanceConfigViewModel> GetEmployeeConditionalDepositAllowanceConfigById(long id, AppUser user);
        Task<IEnumerable<ConditionalDepositAllowanceConfigViewModel>> GetEmployeeConditionalDepositAllowanceConfigsAsync(EligibilityInConditionalDeposit_Filter filter, AppUser user);
        Task<ExecutionStatus> SaveAsync(ConditionalDepositAllowanceConfigDTO model, AppUser user);
        Task<IEnumerable<EmployeeDepositPaymentViewModel>> GetEmployeeEligibleDepositPaymentAsync(EmployeeEligibleDepositPayment_Filter filter, AppUser user);
        Task<IEnumerable<Select2Dropdown>> GetEligibleEmployeesByConfigIdAsync(long configId, AppUser user);
        Task<EmployeeDepositPaymentViewModel> GetEmployeeAllowanceAccuredAmountInTaxProcessAsync(long employeeId, short year, short month, AppUser user);
    }
}
