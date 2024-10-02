using Dapper;
using System.Data;
using Shared.Services;
using BLL.Base.Interface;
using Shared.OtherModels.User;
using DAL.DapperObject.Interface;
using DAL.Context.Employee;
using Shared.Employee.Domain.Logger;


namespace BLL.Base.Implementation
{
    public class SysLogger : ISysLogger
    {
        private string sqlQuery = null;
        private readonly IDapperData _dapper;
        //private readonly EmployeeModuleDbContext _employeeModuleDb;
        public SysLogger(IDapperData dapper)
        {
            _dapper = dapper;
            //_employeeModuleDb = employeeModuleDb;
            //EmployeeModuleDbContext employeeModuleDb
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
            parameters.Add("EMPCode", userId);
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
            try
            {
                var query = $@"INSERT INTO HR_ExceptionLogger([StackTrace],[Source],[Message],[HelpLink],[FileName],[MethodName],[LineNo],[ErrorDateTime],[OrganizationId],[CompanyId],[BranchId],[CreatedBy],[CreatedDate])";
                query += @"VALUES
	(@StackTrace,@Source,@Message,@HelpLink,@FileName,@MethodName,@LineNo,GETDATE(),@OrganizationId,@CompanyId,@BranchId,@UserId,GETDATE())";

                await _dapper.SqlExecuteNonQuery(db, query, new
                {
                    StackTrace = ex.StackTrace,
                    Source = ex.Source,
                    Message = ex.Message,
                    HelpLink = ex.HelpLink??"",
                    FileName = businessClassName,
                    MethodName = methodName,
                    LineNo = 0,
                    OrganizationId = user.OrganizationId,
                    CompanyId = user.CompanyId,
                    BranchId = user.BranchId,
                    UserId = user.ActionUserId
                }, CommandType.Text);
            }
            catch (Exception msg)
            {
                Console.WriteLine(msg);
            }
            
        }

        public async Task SavePayrollException(Exception ex, string db, string businessClassName, string methodName, AppUser user)
        {
            var query = $@"INSERT INTO HR_ExceptionLogger([StackTrace],[Source],[Message],[HelpLink],[FileName],[MethodName],[LineNo],[ErrorDateTime],[OrganizationId],[CompanyId],[BranchId],[CreatedBy],[CreatedDate])";
            query += @"VALUES
	(@StackTrace,@Source,@Message,@HelpLink,@FileName,@MethodName,@LineNo,GETDATE(),@OrganizationId,@CompanyId,@BranchId,@UserId,GETDATE())";

            await _dapper.SqlExecuteNonQuery(db, query, new {
                StackTrace = ex.StackTrace,
                Source = ex.Source,
                Message = ex.Message,
                HelpLink = ex.HelpLink,
                FileName = businessClassName,
                MethodName = methodName,
                LineNo = 0,
                OrganizationId = user.OrganizationId,
                CompanyId = user.CompanyId,
                BranchId = user.BranchId,
                UserId = user.ActionUserId
            }, CommandType.Text);
        }

        public async Task SaveControlPanelException(Exception ex, string db, string businessClassName, string methodName, AppUser user)
        {
            var query = $@"INSERT INTO tblExceptionLogger([StackTrace],[Source],[Message],[HelpLink],[FileName],[MethodName],[LineNo],[ErrorDateTime],[OrganizationId],[CompanyId],[BranchId],[CreatedBy],[CreatedDate])";
            query += $@"VALUES
	(@StackTrace,@Source,@Message,@HelpLink,@FileName,@MethodName,@LineNo,GETDATE(),@OrganizationId,@CompanyId,@BranchId,@UserId,GETDATE())";

            await _dapper.SqlExecuteNonQuery(db, query, new {
                StackTrace = ex.StackTrace,
                Source = ex.Source,
                Message = ex.Message,
                HelpLink = ex.HelpLink,
                FileName = businessClassName,
                MethodName = methodName,
                LineNo = 0,
                OrganizationId = user.OrganizationId,
                CompanyId = user.CompanyId,
                BranchId = user.BranchId,
                UserId = user.ActionUserId
            }, CommandType.Text);
        }

        public async Task SaveUserActivity(string targetedTable, string db, string previousDataInJsonFormat, string presentDataInJsonFormat, string primaryKey, string actionMethod, string action, long employeeId, AppUser user)
        {
            HRActivityLogger activityLogger = new HRActivityLogger();
            
        }
    }
}
