using AutoMapper;
using DAL.DapperObject.Interface;
using DAL.Logger.Interface;
using DAL.Payroll.Repository.Interface;
using Shared.OtherModels.User;
using Shared.Payroll.Domain.Allowance;

namespace DAL.Payroll.Repository.Implementation
{
    public class AllowanceNameRepository : IAllowanceNameRepository
    {
        private readonly IDALSysLogger _sysLogger;
        private readonly IDapperData _dapper;
        private readonly IMapper _mapper;

        public AllowanceNameRepository(IDALSysLogger sysLogger, IDapperData dapper, IMapper mapper)
        {
            _sysLogger = sysLogger;
            _dapper = dapper;
            _mapper = mapper;
        }

        public Task<int> DeleteByIdAsync(long id, AppUser user)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<AllowanceName>> GetAllAsync(AppUser user)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<AllowanceName>> GetAllAsync(object filter, AppUser user)
        {
            throw new NotImplementedException();
        }

        public async Task<AllowanceName> GetByIdAsync(long id, AppUser user)
        {
            AllowanceName allowanceName = null;
            try {
                var query = $@"SELECT * FROM Payroll_AllowanceName Where AllowanceNameId=@Id AND CompanyId=@CompanyId AND OrganizationId=@OrganizationId";
                allowanceName = await _dapper.SqlQueryFirstAsync<AllowanceName>(user.Database, query, new { Id = id, CompanyId = user.CompanyId, OrganizationId = user.OrganizationId });
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "AllowanceNameRepository", "GetByIdAsync", user);
            }
            return allowanceName;
        }
    }
}
