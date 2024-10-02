using DAL.DapperObject.Interface;
using DAL.Logger.Interface;
using Dapper;
using Shared.OtherModels.User;
using Shared.Services;
using System.Data;

namespace DAL.Logger.Implementation
{
    public class DALSysLogger : IDALSysLogger
    {
        private string sqlQuery;
        private readonly IDapperData _dapper;
        public DALSysLogger(IDapperData dapper)
        {
            _dapper = dapper;
        }

        public async Task SaveSystemException(Exception ex, string db, string businessClassName, string methodName, string username, long organizationId, long companyId, long? branchId)
        {
            sqlQuery = "sp_HR_ExceptionLogger";
            var parameters = new DynamicParameters();
            parameters.Add("StackTrace", ex.StackTrace);
            parameters.Add("Source", ex.Source);
            parameters.Add("Message", ex.Message);
            parameters.Add("HelpLink", ex.HelpLink);
            parameters.Add("FileName", businessClassName);
            parameters.Add("MethodName", methodName);
            parameters.Add("LineNo", 0);
            parameters.Add("OrgId", organizationId);
            parameters.Add("ComId", companyId);
            parameters.Add("BrId", branchId);
            parameters.Add("EntryUser", username);

            if (!Utility.IsNullEmptyOrWhiteSpace(sqlQuery)) {
                await _dapper.SqlExecuteNonQuery(db, sqlQuery, parameters, CommandType.StoredProcedure);
            }
        }
        public async Task SaveUserActivity(string targetedTable, string db, string previousDataInJsonFormat, string presentDataInJsonFormat, string primaryKey, string actionMethod, string action, string userId, long organizationId, long companyId, long branchId)
        {
            sqlQuery = "sp_HR_ActivityLogger";
            var parameters = new DynamicParameters();
            parameters.Add("UserId", userId);
            parameters.Add("ActionMethod", actionMethod);
            parameters.Add("PK", primaryKey);
            parameters.Add("ActionName", action);
            parameters.Add("ImpactTables", targetedTable);
            parameters.Add("PreviousValues", previousDataInJsonFormat);
            parameters.Add("PresentValues", presentDataInJsonFormat);
            parameters.Add("Org", organizationId);
            parameters.Add("Com", companyId);
            parameters.Add("Br", branchId);
            if (!Utility.IsNullEmptyOrWhiteSpace(sqlQuery)) {
                await _dapper.SqlExecuteNonQuery(db, sqlQuery, parameters, CommandType.StoredProcedure);
            }
        }
        public Task SaveAccessActivity(string username, string ip, string deviceModel, string devicename, string deviceType, bool? isMobile, bool? isTablet, bool? isDesktop, string deviceOS, string deviceOsVersion, string browser, string browserVersion, string browserMajorVersion)
        {
            throw new NotImplementedException();
        }
        public async Task SaveControlPanelException(Exception ex, string db, string businessClassName, string methodName, string username, long organizationId, long companyId, long? branchId)
        {
            sqlQuery = "sp_ExceptionLogger";
            var parameters = new DynamicParameters();
            parameters.Add("StackTrace", ex.StackTrace);
            parameters.Add("Source", ex.Source);
            parameters.Add("Message", ex.Message);
            parameters.Add("HelpLink", ex.HelpLink);
            parameters.Add("FileName", businessClassName);
            parameters.Add("MethodName", methodName);
            parameters.Add("LineNo", 0);
            parameters.Add("OrgId", organizationId);
            parameters.Add("ComId", companyId);
            parameters.Add("BrId", branchId);
            parameters.Add("EntryUser", username);

            if (!Utility.IsNullEmptyOrWhiteSpace(sqlQuery)) {
                await _dapper.SqlExecuteNonQuery(db, sqlQuery, parameters, CommandType.StoredProcedure);
            }
        }
        public async Task SaveHRMSException(Exception ex, string db, string businessClassName, string methodName, AppUser user)
        {
            sqlQuery = "sp_HR_ExceptionLogger";
            var parameters = new DynamicParameters();
            parameters.Add("StackTrace", ex.StackTrace);
            parameters.Add("Source", ex.Source);
            parameters.Add("Message", ex.Message);
            parameters.Add("HelpLink", ex.HelpLink);
            parameters.Add("FileName", businessClassName);
            parameters.Add("MethodName", methodName);
            parameters.Add("LineNo", 0);
            parameters.Add("OrgId", user.OrganizationId);
            parameters.Add("ComId", user.CompanyId);
            parameters.Add("BrId", user.BranchId);
            parameters.Add("EntryUser", user.Username);

            if (!Utility.IsNullEmptyOrWhiteSpace(sqlQuery)) {
                await _dapper.SqlExecuteNonQuery(db, sqlQuery, parameters, CommandType.StoredProcedure);
            }
        }
        public async Task SavePayrollException(Exception ex, string db, string businessClassName, string methodName, AppUser user)
        {
            sqlQuery = "sp_HR_ExceptionLogger";
            var parameters = new DynamicParameters();
            parameters.Add("StackTrace", ex.StackTrace);
            parameters.Add("Source", ex.Source);
            parameters.Add("Message", ex.Message);
            parameters.Add("HelpLink", ex.HelpLink);
            parameters.Add("FileName", businessClassName);
            parameters.Add("MethodName", methodName);
            parameters.Add("LineNo", 0);
            parameters.Add("OrgId", user.OrganizationId);
            parameters.Add("ComId", user.CompanyId);
            parameters.Add("BrId", user.BranchId);
            parameters.Add("EntryUser", user.Username);

            if (!Utility.IsNullEmptyOrWhiteSpace(sqlQuery)) {
                await _dapper.SqlExecuteNonQuery(db, sqlQuery, parameters, CommandType.StoredProcedure);
            }
        }
        public async Task SaveControlPanelException(Exception ex, string db, string businessClassName, string methodName, AppUser user)
        {
            sqlQuery = "sp_ExceptionLogger";
            var parameters = new DynamicParameters();
            parameters.Add("StackTrace", ex.StackTrace);
            parameters.Add("Source", ex.Source);
            parameters.Add("Message", ex.Message);
            parameters.Add("HelpLink", ex.HelpLink);
            parameters.Add("FileName", businessClassName);
            parameters.Add("MethodName", methodName);
            parameters.Add("LineNo", 0);
            parameters.Add("OrgId", user.OrganizationId);
            parameters.Add("ComId", user.CompanyId);
            parameters.Add("BrId", user.BranchId);
            parameters.Add("EntryUser", user.Username);

            if (!Utility.IsNullEmptyOrWhiteSpace(sqlQuery)) {
                await _dapper.SqlExecuteNonQuery(db, sqlQuery, parameters, CommandType.StoredProcedure);
            }
        }

        public async Task SaveUserActivity(AppUser user, string targetedTable, string db, string previousDataInJsonFormat, string presentDataInJsonFormat, string primaryKey, string actionMethod, string actionName, long employeeId = 0)
        {
            try {
                var query = $@"INSERT INTO HR_ActivityLogger
([ActionMethod],[ActionName],[ImpactTables],[PreviousValue],[PresentValue],[PK],[OrganizationId],[CompanyId],[BranchId],[UserId],[CreatedDate],[EmployeeId])
VALUES(@ActionMethod,@ActionName,@ImpactTables,@PreviousValue,@PresentValue,@PK,@OrganizationId,@CompanyId,@BranchId,@UserId,GETDATE(),@EmployeeId)";
                await _dapper.SqlExecuteNonQuery(db, query, new {
                    ActionMethod = actionMethod,
                    ActionName = actionName,
                    ImpactTables = targetedTable,
                    PreviousValue = previousDataInJsonFormat,
                    PresentValue = previousDataInJsonFormat,
                    PK = primaryKey,
                    OrganizationId = user.OrganizationId,
                    CompanyId = user.CompanyId,
                    BranchId = user.BranchId,
                    UserId = user.ActionUserId,
                    EmployeeId = employeeId
                }, CommandType.Text);
            }
            catch (Exception ex) {

                throw;
            }

        }
    }
}
