using Shared.OtherModels.User;
using Shared.OtherModels.Response;
using Shared.Payroll.ViewModel.Variable;
using Shared.Payroll.Domain.Variable;


namespace BLL.Salary.Variable.Interface
{
    public interface IPeriodicallyVariableDeductionBusiness
    {
        Task<IEnumerable<PeriodicallyVariableDeductionInfoViewModel>> GetPeriodicallyVariableDeductionInfosAsync(long? id, string salaryVariableFor, string amountBaseOn, long? deductionNameId, AppUser user);
        Task<PeriodicallyVariableDeductionInfoViewModel> GetPeriodicallyVariableDeductionInfoAsync(long? id, long? deductionNameId, AppUser user);
        Task<ExecutionStatus> SavePeriodicallyVariableDeductionAsync(PeriodicallyVariableDeductionInfo info, List<PeriodicalDetails> details, AppUser user);
        Task<ExecutionStatus> SavePeriodicallyVariableDeductionInfoAsync(PeriodicallyVariableDeductionInfo info, List<PeriodicalDetails> details, AppUser user);
        Task<ExecutionStatus> SavePeriodicallyVariableDeductionStatusAsync(long periodicallyVariableDeductionInfoId, string status, string remarks, AppUser user);
    }
}
