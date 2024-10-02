using Shared.OtherModels.User;
using Shared.Payroll.Domain.Payment;
using Shared.Payroll.Filter.Payment;

namespace BLL.Salary.Payment.Interface
{
    public interface IDepositeAllowanceHistoryBusiness
    {
        Task<IEnumerable<DepositAllowanceHistory>> GetEmployeeDepositAllowanceHistoriesExceptPaymentMonthAsync(FindDepositAllowanceHistory_Filter filter, AppUser user);
    }
}
