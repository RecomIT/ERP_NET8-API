
using BLL.Base.Interface;
using BLL.Dashboard.SubordiantesLeave.Interface;
using DAL.DapperObject;
using DAL.DapperObject.Interface;
using NLog.Filters;
using Shared.Models.Dashboard.SubordinatesLeave.Approval.ViewModel;
using Shared.Models.Dashboard.SubordinatesLeave.LeaveHistory.ViewModel;
using Shared.OtherModels.User;
using Shared.Services;
using System.Data;


namespace BLL.Dashboard.SubordiantesLeave.Implementation
{
    public class SubordiantesLeaveBusiness : ISubordinatesLeaveBusiness
    {
        private readonly IDapperData _dapper;
        private ISysLogger _sysLogger;

        public SubordiantesLeaveBusiness(IDapperData dapper, ISysLogger sysLogger)
        {
            _dapper = dapper;
            _sysLogger = sysLogger;
        }


        public async Task<object> IsSupervisorOrFinalApprovalAsync(AppUser user)
        {
            try
            {

                var sp_name = "sp_IsSupervisorOrFinalApproval";

                var parameters = DapperParam.AddParams(user, addEmployee: false);

                var data = await _dapper.SqlQueryListAsync<object>(user.Database, sp_name, parameters, CommandType.StoredProcedure);

                return data;

            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "SubordiantesLeaveBusiness", "IsSupervisorAsync", user);

                return null;
            }
        }



        public async Task<object> GetSubordinatesEmployeesAsync(AppUser user)
        {
            try
            {

                var sp_name = "sp_GetSubordinateList";

                var parameters = DapperParam.AddParams(user, addEmployee: false);

                var data = await _dapper.SqlQueryListAsync<object>(user.Database, sp_name, parameters, CommandType.StoredProcedure);

                return data;

            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "SubordiantesLeaveBusiness", "GetSubordinatesEmployeesAsync", user);

                return null;
            }
        }



        public async Task<IEnumerable<object>> GetSubordinatesLeaveAsync(dynamic filter, AppUser user)
        {
            IEnumerable<SubordinatesLeaveHistoryViewModel> data = new List<SubordinatesLeaveHistoryViewModel>();
            try
            {
                var sp_name = "sp_GetSubordinateLeaveInfo";
                var parameters = DapperParam.AddParams(filter, user);

                data = await _dapper.SqlQueryListAsync<SubordinatesLeaveHistoryViewModel>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, Database.ControlPanel, "EmployeeAttendanceDataBusiness", "GetSubordinatesLeaveAsync", user);
            }
            return data;
        }



        public async Task<IEnumerable<object>> GetSubordinatesLeaveApprovalAsync(dynamic filter, AppUser user)
        {
            IEnumerable<LeaveApprovalViewModel> data = new List<LeaveApprovalViewModel>();
            try
            {
                var sp_name = "sp_GetSubordinateLeaveApproval";
                var parameters = DapperParam.AddParams(filter, user);

                data = await _dapper.SqlQueryListAsync<LeaveApprovalViewModel>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, Database.ControlPanel, "EmployeeAttendanceDataBusiness", "GetSubordinatesLeaveApprovalAsync", user);
            }
            return data;
        }



    }
}
