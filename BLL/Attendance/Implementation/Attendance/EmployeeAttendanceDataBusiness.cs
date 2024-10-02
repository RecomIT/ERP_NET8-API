using System;
using System.Data;
using Shared.Services;
using DAL.DapperObject;
using System.Threading.Tasks;
using Shared.OtherModels.User;
using System.Collections.Generic;
using BLL.Base.Interface;
using DAL.DapperObject.Interface;
using BLL.Attendance.Interface.Attendance;
using Shared.Attendance.ViewModel.Attendance;
using Shared.Attendance.Filter.Attendance;

namespace BLL.Attendance.Implementation.Attendance
{
    public class EmployeeAttendanceDataBusiness : IEmployeeAttendanceDataBusiness
    {
        private readonly ISysLogger _sysLogger;
        private readonly IDapperData _dapper;
        public EmployeeAttendanceDataBusiness(ISysLogger sysLogger, IDapperData dapper)
        {
            _sysLogger = sysLogger;
            _dapper = dapper;
        }

        public async Task<IEnumerable<AttendanceSummeryViewModel>> GetEmployeesAttendanceSummeryAsync(AttendanceSummary_Filter filter, AppUser user)
        {
            IEnumerable<AttendanceSummeryViewModel> data = new List<AttendanceSummeryViewModel>();
            try
            {
                var sp_name = "sp_HR_EmployeeAttendance";
                var parameters = DapperParam.AddParams(filter, user);
                parameters.Add("ExecuationFlag", "Summery");
                data = await _dapper.SqlQueryListAsync<AttendanceSummeryViewModel>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeAttendanceDataBusiness", "GetEmployeesAttendanceSummeryAsync", user);
            }
            return data;
        }

        public async Task<IEnumerable<EmployeeDailyAttendanceViewModel>> GetEmployeesDailyAttendanceAsync(DailyAttendance_Filter filter, AppUser user)
        {
            IEnumerable<EmployeeDailyAttendanceViewModel> data = new List<EmployeeDailyAttendanceViewModel>();
            try
            {
                var sp_name = "sp_HR_EmployeeAttendance";
                var parameters = DapperParam.AddParams(filter, user);
                parameters.Add("ExecuationFlag", "Daily");
                data = await _dapper.SqlQueryListAsync<EmployeeDailyAttendanceViewModel>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, Database.ControlPanel, "EmployeeAttendanceDataBusiness", "GetEmployeesDailyAttendanceAsync", user);
            }
            return data;
        }
    }
}
