using System.Threading.Tasks;
using Shared.OtherModels.User;
using Shared.OtherModels.Response;
using Shared.Employee.Domain.Stage;
using Shared.Employee.DTO.Info;
using Shared.Employee.DTO.Stage;
using DAL.Repository.Base.Interface;

namespace DAL.Repository.Employee.Interface
{
    public interface IEmployeePFActivationRepository : IDapperBaseRepository<EmployeePFActivation>
    {
        Task<EmployeePFActivation> GetEmployeePFActivationByConfirmationId(long confirmationId, AppUser user);
        Task<ExecutionStatus> SaveAsync(EmployeePFActivationDTO model, AppUser user);
        Task<ExecutionStatus> PFApprovalAsync(EmployeePFActivationApprovalDTO model, AppUser user);
    }
}
