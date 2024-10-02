using DAL.Repository.Base.Interface;
using Shared.OtherModels.Response;
using Shared.OtherModels.User;
using Shared.Payroll.Domain.Variable;
using Shared.Payroll.ViewModel.Variable;

namespace DAL.Payroll.Repository.Interface
{
    public interface IMonthlyVariableAllowanceRepository: IDapperBaseRepository<MonthlyVariableAllowance>
    {
        Task<ExecutionStatus> UpdateApprovedAllowanceAysnc(MonthlyVariableAllowanceViewModel model, AppUser user);
    }
}
