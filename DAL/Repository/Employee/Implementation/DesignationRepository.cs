
using DAL.Logger.Interface;
using Shared.OtherModels.User;
using DAL.DapperObject.Interface;
using DAL.Repository.Employee.Interface;
using Shared.Employee.Domain.Organizational;


namespace DAL.Repository.Employee.Implementation
{
    public class DesignationRepository : IDesignationRepository
    {
        private readonly IDALSysLogger _sysLogger;
        private readonly IDapperData _dapper;

        public DesignationRepository(IDALSysLogger sysLogger, IDapperData dapper)
        {
            _sysLogger = sysLogger;
            _dapper = dapper;
        }
        public Task<int> DeleteByIdAsync(long id, AppUser user)
        {
            throw new NotImplementedException();
        }
        public Task<IEnumerable<Designation>> GetAllAsync(AppUser user)
        {
            throw new NotImplementedException();
        }
        public Task<IEnumerable<Designation>> GetAllAsync(object filter, AppUser user)
        {
            throw new NotImplementedException();
        }
        public async Task<Designation> GetByIdAsync(long id, AppUser user)
        {
            Designation designation = null;
            try
            {
                var query = "Select * From HR_Designations Where DesignationId=@Id AND  CompanyId=@CompanyId AND OrganizationId=@OrganizationId";
                designation = await _dapper.SqlQueryFirstAsync<Designation>(user.Database, query, new { Id = id, user.CompanyId, user.OrganizationId });
            }
            catch (Exception ex)
            {
            }
            return designation;
        }
    }
}
