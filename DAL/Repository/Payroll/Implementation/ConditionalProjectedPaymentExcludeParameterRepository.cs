using DAL.Logger.Interface;
using Shared.OtherModels.User;
using DAL.Payroll.Repository.Interface;
using DAL.DapperObject.Interface;
using Shared.Payroll.Domain.Payment;

namespace DAL.Payroll.Repository.Implementation
{
    public class ConditionalProjectedPaymentExcludeParameterRepository : IConditionalProjectedPaymentExcludeParameterRepository
    {
        private readonly IDALSysLogger _sysLogger;
        private readonly IDapperData _dapper;

        public ConditionalProjectedPaymentExcludeParameterRepository(IDALSysLogger sysLogger, IDapperData dapper)
        {
            _sysLogger = sysLogger;
            _dapper = dapper;
        }
        public Task<int> DeleteByIdAsync(long id, AppUser user)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<ConditionalProjectedPaymentExcludeParameter>> GetAllAsync(AppUser user)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<ConditionalProjectedPaymentExcludeParameter>> GetAllAsync(object filter, AppUser user)
        {
            throw new System.NotImplementedException();
        }

        public Task<ConditionalProjectedPaymentExcludeParameter> GetByIdAsync(long id, AppUser user)
        {
            throw new System.NotImplementedException();
        }

        public async Task<ConditionalProjectedPaymentExcludeParameter> ConditionalProjectedPaymentExcludeParameterById(long id, string flag, long fiscalYearId, long configId, AppUser user)
        {
            ConditionalProjectedPaymentExcludeParameter item = new ConditionalProjectedPaymentExcludeParameter();
            try {
                var query = $@"SELECT * FROM Payroll_ConditionalProjectedPaymentExcludeParameter
                Where 1=1
                AND ConditionalProjectedPaymentId=@ConfigId
                AND Flag=@Flag
                AND FiscalYearId=@FiscalYearId
                AND ParameterId=@Id
                AND CompanyId=@CompanyId
                AND OrganizationId=@OrganizationId";

                item = await _dapper.SqlQueryFirstAsync<ConditionalProjectedPaymentExcludeParameter>(user.Database, query, new {
                    ConfigId = configId,
                    Flag = flag,
                    FiscalYearId = fiscalYearId,
                    Id = id,
                    CompanyId = user.CompanyId,
                    OrganizationId = user.OrganizationId
                });

            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ConditionalProjectedPaymentExcludeParameterRepository", "ConditionalProjectedPaymentExcludeParameterById", user);
            }
            return item;
        }
    }
}
