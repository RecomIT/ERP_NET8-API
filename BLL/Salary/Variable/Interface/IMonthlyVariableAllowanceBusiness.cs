using Shared.OtherModels.User;
using Shared.OtherModels.Response;
using Shared.Payroll.ViewModel.Variable;
using Shared.Payroll.DTO.Variable;

namespace BLL.Salary.Variable.Interface
{
    public interface IMonthlyVariableAllowanceBusiness
    {
        Task<IEnumerable<MonthlyVariableAllowanceViewModel>> GetMonthlyVariableAllowancesAsync(long? monthlyVariableAllowanceId, long? employeeId, long? allowanceNameId, short salaryMonth, short salaryYear, string stateStatus, AppUser user);
        Task<MonthlyVariableAllowanceViewModel> GetMonthlyVariableAllowanceAsync(long monthlyVariableAllowanceId, long? employeeId, long? allowanceNameId, AppUser user);
        Task<ExecutionStatus> SaveMonthlyVariableAllowancesAsync(List<MonthlyVariableAllowanceViewModel> model, AppUser user);
        Task<ExecutionStatus> UploadMonthlyVariableAllowancesAsync(List<MonthlyVariableAllowanceViewModel> model, AppUser user);
        Task<ExecutionStatus> MonthlyVariableAllowanceValidatorAsync(List<MonthlyVariableAllowanceViewModel> model, AppUser user);
        Task<ExecutionStatus> UpdateMonthlyVariableAllowanceAsync(MonthlyVariableAllowanceViewModel model, AppUser user);
        Task<ExecutionStatus> SaveMonthlyVariableAllowanceStatusAsync(MonthlyVariableAllowanceStatusDTO model, AppUser user);
        Task<ExecutionStatus> DeleteMonthlyVariableAsync(long id, AppUser user);
        Task<MonthlyVariableAllowanceViewModel> GetByIdAsync(long id, AppUser user);
        Task<ExecutionStatus> UpdateApprovedAllowanceAysnc(MonthlyVariableAllowanceViewModel model, AppUser user);

    }
}
