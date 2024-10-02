using Shared.OtherModels.User;
using Shared.OtherModels.Response;
using Shared.OtherModels.DataService;
using DAL.Repository.Base.Interface;
using Shared.Payroll.Domain.Payment;
using Shared.Payroll.DTO.Payment;
using Shared.Payroll.ViewModel.Payment;
using Shared.Payroll.Filter.Payment;

namespace DAL.Payroll.Repository.Interface
{
    public interface IConditionalDepositAllowanceConfigRepository : IDapperBaseRepository<ConditionalDepositAllowanceConfig>
    {
        Task<ExecutionStatus> SaveAsync(ConditionalDepositAllowanceConfigDTO model, AppUser user);
        Task<IEnumerable<EmployeeDepositPaymentViewModel>> GetEmployeeEligibleDepositPaymentAsync(EmployeeEligibleDepositPayment_Filter filter, AppUser user);
        Task<IEnumerable<Select2Dropdown>> GetEligibleEmployeesByConfigIdAsync(long id, AppUser user);
        Task<EmployeeDepositPaymentViewModel> GetEmployeeAllowanceAccuredAmountInTaxProcessAsync(long employeeId, long allowanceNameId, short year, short month, long fiscalYearId, AppUser user);
    }
}
