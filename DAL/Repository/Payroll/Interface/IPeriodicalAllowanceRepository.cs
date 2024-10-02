using DAL.Repository.Base.Interface;
using Shared.Payroll.Domain.Variable;

namespace DAL.Payroll.Repository.Interface
{
    public interface IPeriodicalAllowanceRepository : IDapperBaseRepository<PeriodicallyVariableAllowanceInfo>
    {
        //Task<bool> ValidateAysnc(, AppUser user);
        //Task<ExecutionStatus> SaveAsync(,AppUser user);
    }
}
