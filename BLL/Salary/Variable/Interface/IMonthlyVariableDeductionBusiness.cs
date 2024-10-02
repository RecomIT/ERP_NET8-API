using Shared.OtherModels.User;
using Shared.OtherModels.Response;
using Shared.Payroll.DTO.Variable;
using Shared.Payroll.ViewModel.Variable;


namespace BLL.Salary.Variable.Interface
{
    public interface IMonthlyVariableDeductionBusiness
    {
        Task<IEnumerable<MonthlyVariableDeductionViewModel>> GetMonthlyVariableDeductionsAsync(long? monthlyVariableDeductionId, long? employeeId, long? deductionNameId, short salaryMonth, short salaryYear, string stateStatus, AppUser user);
        Task<MonthlyVariableDeductionViewModel> GetMonthlyVariableDeductionAsync(long monthlyVariableDeductionId, long? employeeId, long? deductionNameId, AppUser user);
        Task<ExecutionStatus> SaveMonthlyVariableDeductionsAsync(List<MonthlyVariableDeductionViewModel> model, AppUser user);
        Task<ExecutionStatus> UploadMonthlyVariableDeductionsAsync(List<MonthlyVariableDeductionViewModel> model, AppUser user);
        Task<ExecutionStatus> MonthlyVariableDeductionValidatorAsync(List<MonthlyVariableDeductionViewModel> model, AppUser user);
        Task<ExecutionStatus> UpdateMonthlyVariableDeductionAsync(MonthlyVariableDeductionViewModel model, AppUser user);
        Task<ExecutionStatus> SaveMonthlyVariableDeductionStatusAsync(MonthlyVariableDeductionStatusDTO model, AppUser user);
    }
}
