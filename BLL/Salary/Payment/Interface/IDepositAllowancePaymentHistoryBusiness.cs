using Shared.OtherModels.User;
using Shared.Payroll.Domain.Payment;
using Shared.Payroll.Filter.Payment;
using Shared.Payroll.Process.Salary;
using Shared.Payroll.ViewModel.Payment;

namespace BLL.Salary.Payment.Interface
{
    public interface IDepositAllowancePaymentHistoryBusiness
    {
        Task<IEnumerable<DepositAllowancePaymentHistory>> GetThisMonthDepositAllowancePaymentProposalsAsync(FindDepositAllowancePaymentProposal_Filter filter, AppUser user);
        Task<IEnumerable<DepositAllowancePaymentHistoryViewModel>> GetPreviousDepositAllowancePaymentProposalsAsync(FindDepositAllowancePaymentProposal_Filter filter, AppUser user);
        Task<IEnumerable<DepositAllowancePaymentHistory>> ThisMonthEmployeeDepositAllowanceCalculationInSalaryAsync(EligibleEmployeeForSalaryType employee, List<DepositAllowanceHistory> listOfNewDepositAmount, int month, int year, long fiscalYearId, AppUser user);
    }
}
