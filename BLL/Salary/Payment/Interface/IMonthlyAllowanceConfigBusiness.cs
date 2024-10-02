using Shared.OtherModels.Response;
using Shared.OtherModels.User;
using Shared.Payroll.Domain.Payment;
using Shared.Payroll.Filter.Payment;
using Shared.Payroll.ViewModel.Payment;

namespace BLL.Salary.Payment.Interface
{
    public interface IMonthlyAllowanceConfigBusiness
    {
        Task<IEnumerable<MonthlyAllowanceConfig>> GetMonthlyAllowanceConfigsAsync(long employeeId, string activationdate, AppUser user);
        Task<DBResponse<MonthlyAllowanceConfigViewModel>> GetMonthlyAllowanceConfigsAsync(MonthlyAllowanceConfig_Filter filter, AppUser user);
    }
}
