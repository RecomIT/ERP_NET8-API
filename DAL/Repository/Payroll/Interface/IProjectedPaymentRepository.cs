
using Shared.OtherModels.User;
using Shared.Payroll.DTO.Payment;
using Shared.OtherModels.Response;
using DAL.Repository.Base.Interface;
using Shared.Payroll.Domain.Payment;
using Shared.Payroll.ViewModel.Payment;


namespace DAL.Payroll.Repository.Interface
{
    public interface IProjectedPaymentRepository : IDapperBaseRepository<EmployeeProjectedPayment>
    {
        Task<ExecutionStatus> SaveAysnc(List<EmployeeProjectedPaymentDTO> model, AppUser user);
        Task<ExecutionStatus> DeletePendingAllowanceByIdAsync(long id, AppUser user);
        Task<ExecutionStatus> DeleteApprovedAllowanceByIdAsync(long id, AppUser user);
        Task<EmployeeProjectedPaymentViewModel> GetProjectedAllowanceByIdAsync(long id, AppUser user);
    }
}
