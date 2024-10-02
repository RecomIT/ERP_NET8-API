using DAL.Logger.Interface;
using Shared.OtherModels.User;
using DAL.DapperObject.Interface;
using DAL.Repository.Employee.Interface;
using Shared.Employee.Domain.Termination;

namespace DAL.Repository.Employee.Implementation
{
    public class DiscontinuedEmployeeRepository : IDiscontinuedEmployeeRepository
    {
        private readonly IDALSysLogger _sysLogger;
        private readonly IDapperData _dapper;

        public DiscontinuedEmployeeRepository(IDALSysLogger sysLogger, IDapperData dapper)
        {
            _sysLogger = sysLogger;
            _dapper = dapper;
        }
        public Task<int> DeleteByIdAsync(long id, AppUser user)
        {
            throw new NotImplementedException();
        }
        public Task<IEnumerable<DiscontinuedEmployee>> GetAllAsync(AppUser user)
        {
            throw new NotImplementedException();
        }
        public Task<IEnumerable<DiscontinuedEmployee>> GetAllAsync(object filter, AppUser user)
        {
            throw new NotImplementedException();
        }
        public Task<DiscontinuedEmployee> GetByIdAsync(long id, AppUser user)
        {
            throw new NotImplementedException();
        }
        public async Task<DiscontinuedEmployee> GetPendingOrApprovedDiscontinuedByEmployeeIdAsync(long employeeId, AppUser user)
        {
            DiscontinuedEmployee discontinuedEmployee = null;
            try
            {
                var query = $@"SELECT * FROM HR_DiscontinuedEmployee Where EmployeeId=@EmployeeId AND CompanyId=@CompanyId AND OrganizationId=@OrganizationId AND StateStatus IN ('Pending','Approved')";
                discontinuedEmployee = await _dapper.SqlQueryFirstAsync<DiscontinuedEmployee>(user.Database, query, new { EmployeeId = employeeId, user.CompanyId, user.OrganizationId });
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "DiscontinuedEmployeeRepository", "GetPendingOrApprovedDiscontinuedByEmployeeIdAsync", user);
            }
            return discontinuedEmployee;
        }
    }
}
