using DAL.Logger.Interface;
using Shared.OtherModels.User;
using DAL.DapperObject.Interface;
using Shared.Employee.Domain.Info;
using DAL.Repository.Employee.Interface;

namespace DAL.Repository.Employee.Implementation
{
    public class EmployeeHierarchyRepository : IEmployeeHierarchyRepository
    {
        private readonly IDALSysLogger _sysLogger;
        private readonly IDapperData _dapper;

        public EmployeeHierarchyRepository(IDALSysLogger sysLogger, IDapperData dapper)
        {
            _sysLogger = sysLogger;
            _dapper = dapper;
        }
        public Task<int> DeleteByIdAsync(long id, AppUser user)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<EmployeeHierarchy>> GetAllAsync(AppUser user)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<EmployeeHierarchy>> GetAllAsync(object filter, AppUser user)
        {
            throw new NotImplementedException();
        }

        public Task<EmployeeHierarchy> GetByIdAsync(long id, AppUser user)
        {
            throw new NotImplementedException();
        }

        public async Task<EmployeeHierarchy> GetEmployeeActiveHierarchy(long employeeId, AppUser user)
        {
            EmployeeHierarchy hierarchy = null;
            try
            {
                var query = $@"Select TOP 1 * from HR_EmployeeHierarchy 
                            Where EmployeeId=@EmployeeId and IsActive=1
                            And CompanyId = @CompanyId And OrganizationId = @OrganizationId";
                hierarchy = await _dapper.SqlQueryFirstAsync<EmployeeHierarchy>(user.Database, query, new { EmployeeId = employeeId, user.CompanyId, user.OrganizationId });
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeHierarchyRepository", "GetEmployeeActiveHierarchy", user);
            }
            return hierarchy;
        }
    }
}
