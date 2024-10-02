using Shared.OtherModels.User;
using Shared.OtherModels.Response;
using Shared.Payroll.DTO.Variable;
using Shared.Payroll.Filter.Variable;
using Shared.Payroll.Domain.Variable;
using Shared.Payroll.ViewModel.Variable;


namespace BLL.Salary.Variable.Interface
{
    public interface IPeriodicallyVariableAllowanceBusiness
    {
        Task<IEnumerable<PeriodicallyVariableAllowanceInfoViewModel>> GetPeriodicallyVariableAllowanceInfosAsync(long? id, string salaryVariableFor, string amountBaseOn, long? allowanceNameId, AppUser user);
        Task<PeriodicallyVariableAllowanceInfoViewModel> GetPeriodicallyVariableAllowanceInfoAsync(long? id, long? allowanceNameId, AppUser user);
        Task<ExecutionStatus> SavePeriodicallyVariableAllowanceAsync(PeriodicallyVariableAllowanceInfo info, List<PeriodicalDetails> details, AppUser user);
        Task<ExecutionStatus> SavePeriodicallyVariableAllowanceInfoAsync(PeriodicallyVariableAllowanceInfo info, List<PeriodicallyVariableAllowanceDetail> details, AppUser user);
        Task<ExecutionStatus> SavePeriodicallyVariableAllowanceStatusAsync(long periodicallyVariableAllowanceInfoId, string status, string remarks, AppUser user);
        Task<DBResponse<PeriodicallyVariableAllowanceInfoViewModel>> GetAllAsync(PeriodicalAllowance_Filter filter, AppUser user);
        Task<PeriodicallyVariableAllowanceInfoViewModel?> GetByIdAsync(long id, AppUser user);
        Task<ExecutionStatus> SaveAsync(PeriodicalAllowanceInfoDTO model, AppUser user);
        Task<ExecutionStatus> DeletePendingVariableAsync(long id, AppUser user);
        Task<IEnumerable<PeriodicalHeadInfoViewModel>> GetPeriodicalHeadInfosAsync(long id,AppUser user);
        Task<IEnumerable<PrincipleAmountInfoViewModel>> GetPendingPrincipleAmountInfosAsync(long id, AppUser user);
    }
}
