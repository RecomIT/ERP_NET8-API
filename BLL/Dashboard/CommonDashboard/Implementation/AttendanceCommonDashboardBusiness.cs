using BLL.Base.Interface;
using BLL.Dashboard.CommonDashboard.Interface;
using BLL.Dashboard.DataService.Interface;
using DAL.DapperObject;
using DAL.DapperObject.Interface;
using Microsoft.AspNetCore.Mvc;
using Shared.Models.Dashboard.CommonDashboard.Attendance.ViewModel;
using Shared.Models.Dashboard.CommonDashboard.CompanyEvents;
using Shared.Models.Dashboard.CommonDashboard.QueryParam.Attendance;
using Shared.Models.Dashboard.SubordinatesLeave.Approval.ViewModel;
using Shared.OtherModels.User;
using Shared.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace BLL.Dashboard.CommonDashboard.Implementation
{
    public class AttendanceCommonDashboardBusiness : IAttendanceCommonDashboardBusiness
    {

        private readonly IDataGetService _dataGetService;

        private readonly IDapperData _dapper;
        private ISysLogger _sysLogger;

        public AttendanceCommonDashboardBusiness(IDataGetService dataGetService, IDapperData dapper, ISysLogger sysLogger)
        {
            _dataGetService = dataGetService;
            _dapper = dapper;
            _sysLogger = sysLogger;
        }



        // ----------------------- >>> GetAttendanceMonthWithDataByYear
        public async Task<object> GetAttendanceMonthWithDataByYearAsync([FromBody] dynamic filter, AppUser user)
        {
            try
            {
                string sqlQuery = "sp_GetAttendanceMonthWithDataByYear";
                string businessClassName = "AttendanceCommonDashboardBusiness";
                string methodName = "GetAttendanceMonthWithDataByYearAsync";

                var data = await _dataGetService.GetDataAsync<object>(user, sqlQuery, businessClassName, methodName, filter);

                return data;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }


        // ----------------------- >>> GetEmployeeAttendanceYears
        public async Task<object> GetEmployeeAttendanceYearsAsync(AppUser user)
        {
            try
            {
                string sqlQuery = "sp_GetEmployeeAttendanceYears";
                string businessClassName = "AttendanceCommonDashboardBusiness";
                string methodName = "GetEmployeeAttendanceYears";

                var data = await _dataGetService.GetDataAsync<object>(user, sqlQuery, businessClassName, methodName);

                return data;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }


        public async Task<object> GetGeoLocationAttendanceAsync(GeoLocationAttendance filter, AppUser user)
        {
            try
            {
                var sp_name = "sp_GetGeoLocationAttendanceData";
                var parameters = DapperParam.AddParams(filter, user, addEmployee: false);

                var list = await _dapper.SqlQueryListAsync<object>(user.Database, sp_name, parameters, CommandType.StoredProcedure);

                return list;
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "AttendanceCommonDashboardBusiness", "GetGeoLocationAttendanceAsync", user);

                return null;
            }
        }



        // ----------------------- >>> MyAttendanceSummeryAsync
        public async Task<object> GetMyAttendanceSummaryAsync([FromBody] dynamic filter, AppUser user)
        {

            try
            {
                string sqlQuery = "sp_GetMyAttendanceSummary";
                string businessClassName = "AttendanceCommonDashboardBusiness";
                string methodName = "MyAttendanceSummeryAsync";

                var data = await _dataGetService.GetDataAsync<object>(user, sqlQuery, businessClassName, methodName, filter);

                return data;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }



        // ----------------------- >>> GetMyRecentAttendanceSummaryAsync
        public async Task<object> GetMyRecentAttendanceSummaryAsync(AppUser user)
        {
            try
            {
                string sqlQuery = "sp_GetMyRecentAttendanceSummary";
                string businessClassName = "AttendanceCommonDashboardBusiness";
                string methodName = "GetMyRecentAttendanceSummaryAsync";

                var data = await _dataGetService.GetDataAsync<object>(user, sqlQuery, businessClassName, methodName);

                return data;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }










        // --------------------------------------- 13 February, 2024

        public async Task<object> GetEmployeeWorkShiftAsync(AppUser user)
        {
            try
            {

                var sp_name = "sp_GetEmployeeWorkShift";
                var parameters = DapperParam.AddParams(user, addEmployee: false);

                var list = await _dapper.SqlQueryListAsync<EmployeeWorkShiftViewModel>(user.Database, sp_name, parameters, CommandType.StoredProcedure);

                return list;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                await _sysLogger.SaveHRMSException(ex, user.Database, "HrDashboardBusiness", "GetTotalEmployeeAsync", user);

                return null;
            }
        }





        public async Task<object> GetCheckPunchInPunchOutAsync(AppUser user)
        {
            try
            {

                var sp_name = "sp_CheckAttendanceStatus";
                var parameters = DapperParam.AddParams(user, addEmployee: false);

                var list = await _dapper.SqlQueryListAsync<CheckPunchInPunchOutViewModel>(user.Database, sp_name, parameters, CommandType.StoredProcedure);

                return list;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                await _sysLogger.SaveHRMSException(ex, user.Database, "HrDashboardBusiness", "GetTotalEmployeeAsync", user);

                return null;
            }
        }





        public async Task<IEnumerable<object>> GetMyGeoLocationAttendanceAsync(dynamic filter, AppUser user)
        {
            IEnumerable<GeoLocationAttendanceViewModel> data = new List<GeoLocationAttendanceViewModel>();
            try
            {
                var sp_name = "sp_GetGeoLocationAttendanceData";
                var parameters = DapperParam.AddParams(filter, user);

                data = await _dapper.SqlQueryListAsync<GeoLocationAttendanceViewModel>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, Database.ControlPanel, "AttendanceCommonDashboardBusiness", "GetMyGeoLocationAttendanceAsync", user);
            }
            return data;
        }


    }
}
