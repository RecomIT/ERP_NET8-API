
using DAL.Logger.Interface;
using Shared.OtherModels.User;
using DAL.Payroll.Repository.Interface;
using Shared.Services;
using Dapper;
using Shared.OtherModels.Response;
using DAL.DapperObject.Interface;
using Shared.Payroll.Domain.Variable;
using Shared.Payroll.ViewModel.Variable;

namespace DAL.Payroll.Repository.Implementation
{
    public class MonthlyVariableAllowanceRepository : IMonthlyVariableAllowanceRepository
    {
        private readonly IDALSysLogger _sysLogger;
        private readonly IDapperData _dapper;
        public MonthlyVariableAllowanceRepository(IDALSysLogger sysLogger, IDapperData dapper)
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
                                    var query = $@"DELETE FROM Payroll_MonthlyVariableAllowance Where MonthlyVariableAllowanceId=@Id
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
                                    await _sysLogger.SaveHRMSException(ex, user.Database, "MonthlyVariableAllowanceRepository", "DeleteByIdAsync", user);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "MonthlyVariableAllowanceRepository", "DeleteByIdAsync", user);
            }
            return rowCount;
        }
        public Task<IEnumerable<MonthlyVariableAllowance>> GetAllAsync(AppUser user)
        {
            throw new NotImplementedException();
        }
        public Task<IEnumerable<MonthlyVariableAllowance>> GetAllAsync(object filter, AppUser user)
        {
            throw new NotImplementedException();
        }
        public async Task<MonthlyVariableAllowance> GetByIdAsync(long id, AppUser user)
        {
            MonthlyVariableAllowance monthlyVariableAllowance = new MonthlyVariableAllowance();
            try {
                var query = $@"SELECT * FROM Payroll_MonthlyVariableAllowance Where MonthlyVariableAllowanceId=@Id
                AND CompanyId=@CompanyId AND OrganizationId=@OrganizationId";

                monthlyVariableAllowance = await _dapper.SqlQueryFirstAsync<MonthlyVariableAllowance>(user.Database, query, new { Id = id, CompanyId = user.CompanyId, OrganizationId = user.OrganizationId });
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "MonthlyVariableAllowanceRepository", "GetByIdAsync", user);
            }
            return monthlyVariableAllowance;
        }

        public async Task<ExecutionStatus> UpdateApprovedAllowanceAysnc(MonthlyVariableAllowanceViewModel model, AppUser user)
        {
            ExecutionStatus executionStatus = null;
            try {
                var itemInDb = await GetByIdAsync(model.MonthlyVariableAllowanceId, user);
                if (itemInDb != null && itemInDb.StateStatus == StateStatus.Approved) {
                    using (var connection = await _dapper.SqlConnectionForTransactionalAsync(user.Database)) {
                        connection.Open();
                        using (var transaction = connection.BeginTransaction()) {
                            try {
                                string query = $@"Update Payroll_MonthlyVariableAllowance
                                SET Amount=@Amount, UpdatedBy=@UserId,UpdatedDate=GETDATE()
                                Where MonthlyVariableAllowanceId=@Id AND StateStatus='Approved' 
                                AND CompanyId=@CompanyId AND OrganizationId=@OrganizationId";

                                var rowCount = await connection.ExecuteAsync(query, new {
                                    Id = model.MonthlyVariableAllowanceId,
                                    Amount = model.Amount,
                                    UserId = user.ActionUserId,
                                    CompanyId = user.CompanyId,
                                    OrganizationId = user.OrganizationId
                                }, transaction);

                                if (rowCount > 0) {
                                    executionStatus = ResponseMessage.Message(true, ResponseMessage.Successfull);
                                    transaction.Commit();
                                }
                                else {
                                    throw new Exception(ResponseMessage.Unsuccessful);
                                }
                            }
                            catch (Exception ex) {
                                if (ex.InnerException != null) {
                                    executionStatus = ResponseMessage.Invalid(ResponseMessage.ServerResponsedWithError);
                                    await _sysLogger.SaveHRMSException(ex, user.Database, "MonthlyVariableAllowanceRepository", "UpdateApprovedAllowanceAysnc", user);
                                }
                                else {
                                    executionStatus = ResponseMessage.Invalid(ex.Message);
                                }
                            }
                            finally {
                                connection.Close();
                            }
                        }
                    }
                }
                else {
                    executionStatus = ResponseMessage.Invalid("Status of the data has been changed");
                }
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "MonthlyVariableAllowanceRepository", "UpdateApprovedAllowanceAysnc", user);
            }
            return executionStatus;
        }
    }
}
