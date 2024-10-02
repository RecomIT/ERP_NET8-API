using Shared.OtherModels.User;
using Shared.Payroll.DTO.Payment;
using Shared.OtherModels.Response;
using Shared.Payroll.Domain.Payment;
using DAL.Repository.Base.Interface;

namespace DAL.Payroll.Repository.Interface
{
    public interface IDepositAllowancePaymentHistoryRepository : IDapperBaseRepository<DepositAllowancePaymentHistory>
    {
        Task<ExecutionStatus> SavePaymentOfDepositAmountAsync(List<PaymentOfDepositAmountByConfigDTO> model, AppUser user);
        Task<ExecutionStatus> UpdatePaymentOfDepositAmountAsync(PaymentOfDepositAmountByConfigDTO model, AppUser user);
    }
}
