using DAL.Repository.Base.Interface;
using Shared.OtherModels.User;
using Shared.Payroll.Domain.Payment;

namespace DAL.Payroll.Repository.Interface
{
    public interface IConditionalProjectedPaymentExcludeParameterRepository : IDapperBaseRepository<ConditionalProjectedPaymentExcludeParameter>
    {
        Task<ConditionalProjectedPaymentExcludeParameter> ConditionalProjectedPaymentExcludeParameterById(long id, string flag, long fiscalYearId, long configId, AppUser user);
    }
}
