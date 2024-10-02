using DAL.Logger.Interface;
using Shared.OtherModels.User;
using DAL.Payroll.Repository.Interface;
using DAL.DapperObject.Interface;
using Shared.Payroll.Domain.Payment;

namespace DAL.Payroll.Repository.Implementation
{
    public class ConditionalProjectedPaymentParameterRepository : IConditionalProjectedPaymentParameterRepository
    {
        private readonly IDALSysLogger _sysLogger;
        private readonly IDapperData _dapper;

        public ConditionalProjectedPaymentParameterRepository(IDALSysLogger sysLogger, IDapperData dapper)
        {
            _sysLogger = sysLogger;
            _dapper = dapper;
        }

        public async Task<ConditionalProjectedPaymentParameter> ConditionalProjectedPaymentParameterById(long id, string flag, long fiscalYearId, long configId, AppUser user)
        {
            ConditionalProjectedPaymentParameter item = new ConditionalProjectedPaymentParameter();
            try {
                var query = $@"SELECT * FROM Payroll_ConditionalProjectedPaymentParameter
                Where 1=1
                AND ConditionalProjectedPaymentId=@ConfigId
                AND Flag=@Flag
                AND FiscalYearId=@FiscalYearId
                AND ParameterId=@Id
                AND CompanyId=@CompanyId
                AND OrganizationId=@OrganizationId";

                item = await _dapper.SqlQueryFirstAsync<ConditionalProjectedPaymentParameter>(user.Database, query, new {
                    ConfigId = configId,
                    Flag = flag,
                    FiscalYearId = fiscalYearId,
                    Id = id,
                    CompanyId = user.CompanyId,
                    OrganizationId = user.OrganizationId
                });

            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ConditionalProjectedPaymentParameterRepository", "ConditionalProjectedPaymentParameterById", user);
            }
            return item;
        }

        public Task<int> DeleteByIdAsync(long id, AppUser user)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ConditionalProjectedPaymentParameter>> GetAllAsync(AppUser user)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ConditionalProjectedPaymentParameter>> GetAllAsync(object filter, AppUser user)
        {
            throw new NotImplementedException();
        }

        public Task<ConditionalProjectedPaymentParameter> GetByIdAsync(long id, AppUser user)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<ConditionalProjectedPaymentParameter>> GetConditionalProjectedPaymentsAsync(long configId, long fiscalYearId, string flag, AppUser user)
        {
            IEnumerable<ConditionalProjectedPaymentParameter> list = new List<ConditionalProjectedPaymentParameter>();
            try {
                var query = $@"SELECT * FROM Payroll_ConditionalProjectedPaymentParameter
                Where 1=1
                AND Flag = @Flag
                AND FiscalYearId=@FiscalYearId
                AND ConditionalProjectedPaymentId=@ConfigId
                AND CompanyId=@CompanyId
                AND OrganizationId=@OrganizationId";
                list = await _dapper.SqlQueryListAsync<ConditionalProjectedPaymentParameter>(user.Database, query.Trim(), new { Flag = flag, FiscalYearId = fiscalYearId, ConfigId = configId, CompanyId = user.CompanyId, OrganizationId = user.OrganizationId });
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ConditionalProjectedPaymentBusiness", "GetConditionalProjectedPaymentsAsync", user);
            }
            return list;
        }
    }
}
