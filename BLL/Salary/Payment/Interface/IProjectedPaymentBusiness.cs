using Shared.OtherModels.User;
using Shared.OtherModels.Response;
using System.Data;
using Shared.Payroll.ViewModel.Payment;
using Shared.Payroll.Filter.Payment;
using Shared.Payroll.DTO.Payment;
using Shared.Payroll.Domain.Payment;
using Shared.Payroll.Process.Allowance;

namespace BLL.Salary.Payment.Interface
{
    public interface IProjectedPaymentBusiness
    {
        Task<DBResponse<EmployeeProjectedPaymentViewModel>> GetEmployeeProjectedPaymentsAsync(EmployeeProjectedPayment_Filter filter, AppUser user);
        Task<IEnumerable<ExecutionStatus>> SaveBlukEmployeeProjectedPaymentAsync(List<EmployeeProjectedPaymentDTO> model, AppUser user);
        Task<ExecutionStatus> SaveEmployeeProjectedPaymentAsync(EmployeeProjectedPaymentDTO model, AppUser user);
        Task<ExecutionStatus> ValidatePaymentAsync(List<EmployeeProjectedPaymentDTO> model, AppUser user);
        Task<IEnumerable<EmployeeProjectedPaymentViewModel>> GetEmployeeProjectedPaymentInfosForProcessAsync(EmployeeProjectedPayment_Filter filter, AppUser user);
        Task<ExecutionStatus> SaveProjectedPaymentInProcessAsync(ProjectedPaymentProcessDTO model, AppUser user);
        Task<DBResponse<EmployeeProjectedAllowanceProcessInfoViewModel>> GetEmployeeProjectedAllowanceProcessInfos(EmployeeProjectedAllowanceProcess_Filter filter, AppUser user);
        Task<string> GenerateProjectedAllowanceProcessCodeAsync(AppUser user);
        Task<IEnumerable<EmployeeProjectedPayment>> GetUnProcessedProjectedAllowanceAsync(long employeeId, long fiscalYearId, int year, int month, AppUser user);
        Task<IEnumerable<AllowanceDisbursedAmount>> GetTillProcessedProjectedAllowanceAsync(long employeeId, long fiscalYearId, int year, int month, AppUser user);
        Task<IEnumerable<AllowanceDisbursedAmount>> GetThisMonthProcessedProjectedAllowanceAsync(long employeeId, long fiscalYearId, int year, int month, AppUser user);
        Task<DataTable> GetProjectedPaymentReport(long projectedAllowanceProcessInfoId, long employeeId, long fiscalYearId, short paymentMonth, short paymentYear, AppUser user);
        Task<ExecutionStatus> SaveAsync(List<EmployeeProjectedPaymentDTO> model, AppUser user);
        Task<ExecutionStatus> DeletePendingAllowanceByIdAsync(long id, AppUser user);
        Task<ExecutionStatus> DeleteApprovedAllowanceByIdAsync(long id, AppUser user);
        Task<EmployeeProjectedPaymentViewModel> GetProjectedAllowanceByIdAsync(long id, AppUser user);
        Task<ExecutionStatus> UpdateAsync(EmployeeProjectedPaymentDTO model, AppUser user);
        Task<ExecutionStatus> ApprovalAsync(ProjectedPaymentApprovalDTO model, AppUser user);
        Task<IEnumerable<string>> ListOfPaymentReasonAsync(AppUser user);
    }
}
