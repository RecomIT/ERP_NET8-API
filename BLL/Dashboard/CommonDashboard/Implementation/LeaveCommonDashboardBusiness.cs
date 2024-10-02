using System;
using System.Threading.Tasks;
using Shared.OtherModels.User;
using Shared.Models.Dashboard.CommonDashboard.ViewModels.Leave;
using Shared.Models.Dashboard.SubordinatesLeave.Approval.ViewModel;
using Shared.Services;
using System.Collections.Generic;
using System.Data;
using BLL.Base.Interface;
using DAL.DapperObject;
using BLL.Base.Implementation;
using DAL.DapperObject.Interface;
using BLL.Dashboard.CommonDashboard.Interface;
using BLL.Dashboard.DataService.Interface;

namespace BLL.Dashboard.CommonDashboard.Implementation
{
    public class LeaveCommonDashboardBusiness : ILeaveCommonDashboardBusiness
    {
        private readonly IDataGetService _dataGetService;

        private readonly IDapperData _dapper;
        private ISysLogger _sysLogger;


        public LeaveCommonDashboardBusiness(IDataGetService dataGetService, IDapperData dapper, ISysLogger sysLogger)
        {
            _dataGetService = dataGetService;
            _dapper = dapper;
            _sysLogger = sysLogger;
        }



        // ----------------------------------------- >>> GetMyLeaveSummaryAsync
        public async Task<object> GetMyLeaveSummaryAsync(dynamic filter, AppUser user)
        {
            try
            {
                string sqlQuery = "sp_GetMyLeaveSummary";
                string businessClassName = "LeaveCommonDashboardBusiness";
                string methodName = "GetMyLeaveSummaryAsync";

                var data = await _dataGetService.GetDataAsync<MyLeaveSummaryViewModel>(user, sqlQuery, businessClassName, methodName, filter);

                return data;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }
        // ----------------------------------------- >>> GetMyLeaveTypeSummaryAsync
        public async Task<object> GetMyLeaveTypeSummaryAsync(AppUser user)
        {
            try
            {
                string sqlQuery = "sp_GetMyLeaveTypeSummary";
                string businessClassName = "LeaveCommonDashboardBusiness";
                string methodName = "GetMyLeaveTypeSummaryAsync";
                var data = await _dataGetService.GetDataAsync<MyLeaveTypeSummaryViewModel>(user, sqlQuery, businessClassName, methodName);

                return data;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }






        public async Task<IEnumerable<object>> GetMyLeaveAppliedRecordsAsync(dynamic filter, AppUser user)
        {
            IEnumerable<LeaveApprovalViewModel> data = new List<LeaveApprovalViewModel>();
            try
            {
                var sp_name = "sp_GetMyLeaveAppliedRecords";
                var parameters = DapperParam.AddParams(filter, user, addEmployee: false);

                data = await _dapper.SqlQueryListAsync<LeaveApprovalViewModel>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, Database.ControlPanel, "EmployeeAttendanceDataBusiness", "GetMyLeaveAppliedRecordsAsync", user);
            }
            return data;
        }



    }
}
