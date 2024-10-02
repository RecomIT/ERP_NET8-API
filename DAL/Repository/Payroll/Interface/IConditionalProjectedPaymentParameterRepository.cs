using DAL.Repository.Base.Interface;
using Shared.OtherModels.User;
using Shared.Payroll.Domain.Payment;

namespace DAL.Payroll.Repository.Interface
{
    public interface IConditionalProjectedPaymentParameterRepository : IDapperBaseRepository<ConditionalProjectedPaymentParameter>
    {
        Task<ConditionalProjectedPaymentParameter> ConditionalProjectedPaymentParameterById(long id, string flag,long fiscalYearId, long configId, AppUser user);
        Task<IEnumerable<ConditionalProjectedPaymentParameter>> GetConditionalProjectedPaymentsAsync(long configId, long fiscalYearId, string flag, AppUser user);
    }
}
