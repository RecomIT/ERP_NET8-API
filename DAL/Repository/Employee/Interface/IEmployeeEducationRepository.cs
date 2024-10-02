using Shared.OtherModels.User;
using DAL.Repository.Base.Interface;
using Shared.Employee.Domain.Education;

namespace DAL.Repository.Employee.Interface
{
    public interface IEmployeeEducationRepository : IDapperBaseRepository<EmployeeEducation>
    {
        Task<IEnumerable<EmployeeEducation>> GetEmployeeEducationByEmployeeId(long employeeId, AppUser user);
        Task<EmployeeEducation> GetEmployeeEducationByEmployeeDegreeId(long employeeId, long degreeId, AppUser user);
    }
}
