using Shared.OtherModels.User;
using Shared.Payroll.Process.Tax;
using Shared.Payroll.Domain.Payment;
using Shared.Payroll.Process.Allowance;
using Shared.OtherModels.Response;
using Shared.Payroll.Filter.Payment;
using Shared.Payroll.ViewModel.Payment;
using Shared.Payroll.DTO.Payment;

namespace BLL.Salary.Payment.Interface
{
    public interface IConditionalProjectedPaymentBusiness
    {
        Task<DBResponse<ConditionalProjectedPaymentViewModel>> GetConditionalProjectedPaymentsAsync(ConditionalProjected_Filter filter, AppUser user);
        Task<IEnumerable<ConditionalProjectedPayment>> GetUnProcessedConditionalProjectedPaymentsAsync(EligibilityInConditionalProjectedPayment_Filter filter, EligibleEmployeeForTaxType employee, AppUser user);
        Task<IEnumerable<ConditionalProjectedPaymentParameter>> GetConditionalProjectedPaymentsAsync(long configId, long fiscalYearId, string flag, AppUser user);
        Task<IEnumerable<AllowanceDisbursedAmount>> GetAllowanceTillDisbursedAmountsAsync(long employeeId, long fiscalYearId, int year, int month, AppUser user);
        Task<IEnumerable<AllowanceDisbursedAmount>> GetAllowanceThisMonthDisbursedAmountsAsync(long employeeId, long fiscalYearId, int year, int month, AppUser user);
        Task<ExecutionStatus> SaveAsync(ConditionalProjectedPaymentDTO model, AppUser user);
        Task<ConditionalProjectedPaymentViewModel> GetById(long id, AppUser user);
        Task<ExecutionStatus> ApprovalAsync(ConditionalProjectedPaymentApprovalDTO model, AppUser user);
    }
}
