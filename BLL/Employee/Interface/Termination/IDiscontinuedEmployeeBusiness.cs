using Shared.OtherModels.User;
using Shared.OtherModels.Response;
using Shared.Employee.DTO.Termination;
using Shared.Employee.Filter.Termination;
using Shared.Employee.ViewModel.Termination;
using Shared.Employee.Domain.Termination;


namespace BLL.Employee.Interface.Termination
{
    public interface IDiscontinuedEmployeeBusiness
    {
        Task<ExecutionStatus> SaveDiscontinuedEmployeeAsync(DiscontinuedEmployeeDTO model, AppUser user);
        Task<IEnumerable<DiscontinuedEmployeeViewModel>> GetDiscontinuedEmployeesAsync(Termination_Filter filter, AppUser user);
        Task<ExecutionStatus> DeleteDiscontinuedEmployeeAsync(Termination_Filter filter, AppUser user);
        Task<ExecutionStatus> ValidateDiscontinuedEmployeeAsync(DiscontinuedEmployeeDTO filter, AppUser user);
        Task<ExecutionStatus> ApprovalDiscontinuedEmployeeAsync(DiscontinuedEmployeeApprovalDTO model, AppUser user);
        Task<DiscontinuedEmployee> GetDiscontinuedEmployeeById(long employeeId, AppUser user);
    }
}
