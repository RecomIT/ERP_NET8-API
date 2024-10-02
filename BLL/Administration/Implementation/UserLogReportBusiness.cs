using System.Data;
using DAL.DapperObject;
using BLL.Base.Interface;
using Shared.OtherModels.User;
using BLL.Administration.Interface;
using DAL.DapperObject.Interface;
using Shared.Control_Panel.Filter;

namespace BLL.Administration.Implementation
{
    public class UserLogReportBusiness : IUserLogReportBusiness
    {
        private readonly IDapperData _dapper;
        private readonly ISysLogger _sysLogger;
        public UserLogReportBusiness(IDapperData dapper, ISysLogger sysLogger)
        {
            _dapper = dapper;
            _sysLogger = sysLogger;
        }
        public async Task<DataTable> GetUserAccessStatusInfoAsync(UserLogReport_Filter filter, AppUser user)
        {
            DataTable dataTable = new DataTable();
            try {
                var sp_name = "sp_UserAccessStatusInfo";
                Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
                keyValuePairs.Add("EmployeeId", filter.EmployeeId.ToString());
                keyValuePairs.Add("BranchId", filter.BranchId.ToString());
                keyValuePairs.Add("CompanyId", user.CompanyId.ToString());
                keyValuePairs.Add("OrganizationId", user.OrganizationId.ToString());
                dataTable = await _dapper.SqlDataTable(Database.ControlPanel, sp_name, keyValuePairs, CommandType.StoredProcedure);
            }
            catch (Exception ex) {
                await _sysLogger.SaveControlPanelException(ex, Database.ControlPanel, "UserLogReportBusiness", "GetUserAccessStatusInfoAsync", user);
            }
            return dataTable;
        }

        public async Task<DataTable> GetUserPrivilegeInfoAsync(UserLogReport_Filter filter, AppUser user)
        {
            DataTable dataTable = new DataTable();
            try {
                var sp_name = "sp_UserPrivilegeReport";
                Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
                keyValuePairs.Add("EmployeeId", filter.EmployeeId.ToString());
                keyValuePairs.Add("BranchId", filter.BranchId.ToString());
                keyValuePairs.Add("CompanyId", user.CompanyId.ToString());
                keyValuePairs.Add("OrganizationId", user.OrganizationId.ToString());
                dataTable = await _dapper.SqlDataTable(Database.ControlPanel, sp_name, keyValuePairs, CommandType.StoredProcedure);
            }
            catch (Exception ex) {
                await _sysLogger.SaveControlPanelException(ex, Database.ControlPanel, "UserLogReportBusiness", "GetUserPrivilegeInfoAsync",user);
            }
            return dataTable;
        }

        public async Task<DataTable> GetUserRolePrivilegeInfoAsync(UserLogReport_Filter filter, AppUser user)
        {
            DataTable dataTable = new DataTable();
            try {
                var sp_name = "sp_UserRolePrivilegeReport";
                Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
                keyValuePairs.Add("RoleId", filter.RoleId);
                keyValuePairs.Add("BranchId", filter.BranchId.ToString());
                keyValuePairs.Add("CompanyId", user.CompanyId.ToString());
                keyValuePairs.Add("OrganizationId", user.OrganizationId.ToString());
                dataTable = await _dapper.SqlDataTable(Database.ControlPanel, sp_name, keyValuePairs, CommandType.StoredProcedure);
            }
            catch (Exception ex) {
                await _sysLogger.SaveControlPanelException(ex, Database.ControlPanel, "UserLogReportBusiness", "GetUserPrivilegeInfoAsync", user);
            }
            return dataTable;
        }
    }
}
