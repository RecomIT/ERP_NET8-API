using DAL.Logger.Interface;
using Shared.OtherModels.User;
using Shared.Employee.Domain.Education;
using DAL.Repository.Employee.Interface;
using DAL.DapperObject.Interface;

namespace DAL.Repository.Employee.Implementation
{
    public class EmployeeEducationRepository : IEmployeeEducationRepository
    {
        private readonly IDALSysLogger _sysLogger;
        private readonly IDapperData _dapper;

        public EmployeeEducationRepository(IDALSysLogger sysLogger, IDapperData dapper)
        {
            _sysLogger = sysLogger;
            _dapper = dapper;
        }
        public Task<int> DeleteByIdAsync(long id, AppUser user)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<EmployeeEducation>> GetAllAsync(AppUser user)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<EmployeeEducation>> GetAllAsync(object filter, AppUser user)
        {
            throw new NotImplementedException();
        }

        public Task<EmployeeEducation> GetByIdAsync(long id, AppUser user)
        {
            throw new NotImplementedException();
        }

        public async Task<EmployeeEducation> GetEmployeeEducationByEmployeeDegreeId(long employeeId, long degreeId, AppUser user)
        {
            EmployeeEducation employeeEducation = new EmployeeEducation();
            try
            {
                var query = $@"SELECT * FROM HR_EmployeeEducation Where EmployeeId =@Id AND DegreeId=@DegreeId
                AND CompanyId=@CompanyId AND OrganizationId = @OrganizationId";
                employeeEducation = await _dapper.SqlQueryFirstAsync<EmployeeEducation>(user.Database, query, new { Id = employeeId, DegreeId = degreeId, user.CompanyId, user.OrganizationId });
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "DiscontinuedEmployeeRepository", "GetEmployeeEducationByEmployeeDegreeId", user);
            }
            return employeeEducation;
        }

        public async Task<IEnumerable<EmployeeEducation>> GetEmployeeEducationByEmployeeId(long employeeId, AppUser user)
        {
            IEnumerable<EmployeeEducation> list = new List<EmployeeEducation>();
            try
            {
                var query = $@"SELECT * FROM HR_EmployeeEducation Where EmployeeId =@Id
                AND CompanyId=@CompanyId AND OrganizationId = @OrganizationId";
                list = await _dapper.SqlQueryListAsync<EmployeeEducation>(user.Database, query, new { Id = employeeId, user.CompanyId, user.OrganizationId });
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "DiscontinuedEmployeeRepository", "GetEmployeeEducationByEmployeeId", user);
            }
            return list;
        }
    }
}
