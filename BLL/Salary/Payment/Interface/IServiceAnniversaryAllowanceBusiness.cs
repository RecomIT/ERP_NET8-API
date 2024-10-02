using Shared.OtherModels.User;
using Shared.Payroll.Domain.Payment;
using Shared.Payroll.Filter.Payment;
using System;

namespace BLL.Salary.Payment.Interface
{
    public interface IServiceAnniversaryAllowanceBusiness
    {
        Task<IEnumerable<ServiceAnniversaryAllowanceViewModel>> GetServiceAnniversaryAllowancesAsync(ServiceAnniversaryAllowance_Filter filter, AppUser user);
        Task<IEnumerable<ServiceAnniversaryAllowance>> GetEmployeeServiceAnniversaryAllowancesAsync(EligibilityInServiceAnniversary_Filter filter, int month, int year, DateTime dateOfJoining, AppUser user);
    }
}
