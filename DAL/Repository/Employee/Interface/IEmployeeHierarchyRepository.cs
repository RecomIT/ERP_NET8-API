using Shared.OtherModels.User;
using Shared.Employee.Domain.Info;
using DAL.Repository.Base.Interface;


namespace DAL.Repository.Employee.Interface
{
    public interface IEmployeeHierarchyRepository : IDapperBaseRepository<EmployeeHierarchy>
    {
        Task<EmployeeHierarchy> GetEmployeeActiveHierarchy(long employeeId, AppUser user);
    }
}
