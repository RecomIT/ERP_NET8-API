using Shared.OtherModels.User;
using DAL.Repository.Base.Interface;
using Shared.Employee.Domain.Termination;


namespace DAL.Repository.Employee.Interface
{
    public interface IDiscontinuedEmployeeRepository : IDapperBaseRepository<DiscontinuedEmployee>
    {
        Task<DiscontinuedEmployee> GetPendingOrApprovedDiscontinuedByEmployeeIdAsync(long employeeId, AppUser user);
    }
}
