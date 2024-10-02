
using DAL.Repository.Base.Interface;
using Shared.Employee.Domain.Info;
using Shared.Employee.DTO.Info;
using Shared.Employee.Filter.Info;
using Shared.Employee.ViewModel.Info;
using Shared.OtherModels.Response;
using Shared.OtherModels.User;

namespace DAL.Repository.Employee.Interface
{
    public interface IEmployeeRepository : IDapperBaseRepository<EmployeeInformation>
    {
        Task<IEnumerable<EmployeeServiceDataViewModel>> GetEmployeeServiceDataAsync(EmployeeService_Filter filter, AppUser user);
        Task<ExecutionStatus> UpdateByUploaderAsync(EmployeeUploadInformation employee, AppUser user);
        Task<EmployeeDetail> GetEmployeeDetailByIdAsync(long id, AppUser user);
        Task<EmployeeInformation> GetByCodeAsync(string code, AppUser user);
    }
}
