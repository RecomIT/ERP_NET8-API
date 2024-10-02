using DAL.Logger.Interface;
using Shared.OtherModels.User;
using Shared.OtherModels.DataService;
using Shared.Employee.Domain.Education;
using DAL.Repository.Employee.Interface;
using DAL.DapperObject.Interface;

namespace DAL.Repository.Employee.Implementation
{
    public class EducationDegreeRepository : IEducationDegreeRepository
    {
        private readonly IDALSysLogger _sysLogger;
        private readonly IDapperData _dapper;

        public EducationDegreeRepository(IDALSysLogger sysLogger, IDapperData dapper)
        {
            _sysLogger = sysLogger;
            _dapper = dapper;
        }
        public Task<int> DeleteByIdAsync(long id, AppUser user)
        {
            throw new NotImplementedException();
        }
        public Task<IEnumerable<EducatioalDegree>> GetAllAsync(AppUser user)
        {
            throw new NotImplementedException();
        }
        public Task<IEnumerable<EducatioalDegree>> GetAllAsync(object filter, AppUser user)
        {
            throw new NotImplementedException();
        }
        public Task<EducatioalDegree> GetByIdAsync(long id, AppUser user)
        {
            throw new NotImplementedException();
        }
        public async Task<IEnumerable<Dropdown>> GetEducationDegreeByEductionLevelAsync(long levelOfEductionId, AppUser user)
        {
            IEnumerable<Dropdown> list = new List<Dropdown>();
            try
            {
                var query = $@"SELECT [Id]=EducatioalDegreeId,
                [Value]=EducatioalDegreeId,
                [Text]=DegreeName
                FROM HR_EducationalDegrees Where LevelOfEducationId=@Id AND OrganizationId=@OrganizationId";
                list = await _dapper.SqlQueryListAsync<Dropdown>(user.Database, query, new { Id = levelOfEductionId, user.OrganizationId });
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EducationDegreeRepository", "GetEducationDegreeByEductionLevelAsync", user);
            }
            return list;
        }
    }
}
