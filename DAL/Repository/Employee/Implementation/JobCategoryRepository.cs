using DAL.Logger.Interface;
using Shared.OtherModels.User;
using DAL.DapperObject.Interface;
using DAL.Repository.Employee.Interface;
using Shared.Employee.Domain.Organizational;

namespace DAL.Repository.Employee.Implementation
{
    public class JobCategoryRepository : IJobCategoryRepository
    {
        private readonly IDALSysLogger _sysLogger;
        private readonly IDapperData _dapper;

        public JobCategoryRepository(IDALSysLogger sysLogger, IDapperData dapper)
        {
            _sysLogger = sysLogger;
            _dapper = dapper;
        }
        public Task<int> DeleteByIdAsync(long id, AppUser user)
        {
            throw new NotImplementedException();
        }
        public async Task<IEnumerable<JobCategory>> GetAllAsync(AppUser user)
        {
            IEnumerable<JobCategory> list = new List<JobCategory>();
            try
            {
                var query = $@"Select * From HR_JobCategory Where 1=1 AND CompanyId=@CompanyId AND OrganizationId=@OrganizationId";
                list = await _dapper.SqlQueryListAsync<JobCategory>(user.Database, query, new { user.CompanyId, user.OrganizationId });
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "JobCategoryRepository", "GetAllAsync", user);
            }
            return list;
        }
        public Task<IEnumerable<JobCategory>> GetAllAsync(object filter, AppUser user)
        {
            throw new NotImplementedException();
        }
        public Task<JobCategory> GetByIdAsync(long id, AppUser user)
        {
            throw new NotImplementedException();
        }
    }
}
