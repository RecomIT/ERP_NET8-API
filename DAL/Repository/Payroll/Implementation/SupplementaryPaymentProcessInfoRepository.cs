using AutoMapper;
using DAL.DapperObject.Interface;
using DAL.Logger.Interface;
using DAL.Payroll.Repository.Interface;
using Shared.OtherModels.User;
using Shared.Payroll.Domain.Payment;
using System.Data;

namespace DAL.Payroll.Repository.Implementation
{
    public class SupplementaryPaymentProcessInfoRepository : ISupplementaryPaymentProcessInfoRepository
    {
        private readonly IDALSysLogger _sysLogger;
        private readonly IDapperData _dapper;
        private readonly IMapper _mapper;

        public SupplementaryPaymentProcessInfoRepository(IDALSysLogger sysLogger, IDapperData dapper, IMapper mapper)
        {
            _sysLogger = sysLogger;
            _dapper = dapper;
            _mapper = mapper;
        }
        public Task<int> DeleteByIdAsync(long id, AppUser user)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<SupplementaryPaymentProcessInfo>> GetAllAsync(AppUser user)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<SupplementaryPaymentProcessInfo>> GetAllAsync(object filter, AppUser user)
        {
            throw new NotImplementedException();
        }

        public async Task<SupplementaryPaymentProcessInfo> GetByIdAsync(long id, AppUser user)
        {
            SupplementaryPaymentProcessInfo info = null;
            try {
                var query = $@"SELECT * FROM Payroll_SupplementaryPaymentProcessInfo Where PaymentProcessInfoId=@Id AND CompanyId=@CompanyId AND OrganizationId=@OrganizationId";
                info = await _dapper.SqlQueryFirstAsync<SupplementaryPaymentProcessInfo>(user.Database, query, new {
                    Id = id,
                    CompanyId = user.CompanyId,
                    OrganizationId = user.OrganizationId
                }, CommandType.Text);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "SupplementaryPaymentProcessInfoRepository", "GetByIdAsync", user);
            }
            return info;
        }
    }
}
