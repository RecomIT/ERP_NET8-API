using Dapper;
using Shared.Services;
using DAL.Logger.Interface;
using Shared.OtherModels.User;
using DAL.DapperObject.Interface;
using Shared.Payroll.Domain.Variable;
using DAL.Payroll.Repository.Interface;

namespace DAL.Payroll.Repository.Implementation
{
    public class MonthlyVariableDeductionRepository : IMonthlyVariableDeductionRepository
    {
        private readonly IDALSysLogger _sysLogger;
        private readonly IDapperData _dapper;
        public MonthlyVariableDeductionRepository(IDALSysLogger sysLogger, IDapperData dapper)
        {
            _sysLogger = sysLogger;
            _dapper = dapper;
        }
        public async Task<int> DeleteByIdAsync(long id, AppUser user)
        {
            int rowCount = 0;
            try {
                var itemInDb = await GetByIdAsync(id, user);
                if (itemInDb != null && itemInDb.StateStatus == StateStatus.Pending) {
                    using (var connection = await _dapper.SqlConnectionForTransactionalAsync(user.Database)) {
                        connection.Open();
                        {
                            using (var transaction = connection.BeginTransaction()) {
                                try {
                                    var query = $@"DELETE FROM Payroll_MonthlyVariableDeduction Where MonthlyVariableDeductionId=@Id
                                    AND CompanyId=@CompanyId AND OrganizationId=@OrganizationId";

                                    rowCount = await connection.ExecuteAsync(query, new { Id = id, CompanyId = user.CompanyId, OrganizationId = user.OrganizationId }, transaction);

                                    if (rowCount > 0) {
                                        transaction.Commit();
                                    }
                                    else {
                                        transaction.Rollback();
                                    }
                                }
                                catch (Exception ex) {
                                    await _sysLogger.SaveHRMSException(ex, user.Database, "MonthlyVariableDeductionRepository", "DeleteByIdAsync", user);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "MonthlyVariableDeductionRepository", "DeleteByIdAsync", user);
            }
            return rowCount;
        }
        public Task<IEnumerable<MonthlyVariableDeduction>> GetAllAsync(AppUser user)
        {
            throw new NotImplementedException();
        }
        public Task<IEnumerable<MonthlyVariableDeduction>> GetAllAsync(object filter, AppUser user)
        {
            throw new NotImplementedException();
        }
        public async Task<MonthlyVariableDeduction> GetByIdAsync(long id, AppUser user)
        {
            MonthlyVariableDeduction monthlyVariableDeduction = new MonthlyVariableDeduction();
            try {
                var query = $@"SELECT * FROM Payroll_MonthlyVariableDeduction Where MonthlyVariableDeductionId=@Id
                AND CompanyId=@CompanyId AND OrganizationId=@OrganizationId";

                monthlyVariableDeduction = await _dapper.SqlQueryFirstAsync<MonthlyVariableDeduction>(user.Database, query, new { Id = id, CompanyId = user.CompanyId, OrganizationId = user.OrganizationId });
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "MonthlyVariableDeductionRepository", "GetByIdAsync", user);
            }
            return monthlyVariableDeduction;
        }
    }
}
