
using DAL.Repository.Base.Interface;
using Shared.Employee.Domain.Stage;
using Shared.Employee.ViewModel.Stage;
using Shared.OtherModels.DataService;
using Shared.OtherModels.Response;
using Shared.OtherModels.User;


namespace DAL.Repository.Employee.Interface
{
    public interface IEmploymentConfirmationRepository : IDapperBaseRepository<EmployeeConfirmationProposal>
    {
        Task<IEnumerable<Select2Dropdown>> GetUnconfirmedEmployeeInfosInApplyAsync(AppUser user);
        Task<IEnumerable<Select2Dropdown>> GetUnconfirmedEmployeeInfosInUpdateAsync(AppUser user);
        Task<ExecutionStatus> SaveAsync(EmploymentConfirmationDTO model, AppUser user);
        Task<DBResponse> GetEmploymentConfirmationsAsync(object filter, AppUser user);
        Task<ExecutionStatus> ConfirmationApprovalAsync(EmploymentConfirmationStatusDTO model, AppUser user);
    }
}
