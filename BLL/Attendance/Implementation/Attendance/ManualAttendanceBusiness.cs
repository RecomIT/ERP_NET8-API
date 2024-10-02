using System;
using System.Data;
using Shared.Services;
using System.Threading.Tasks;
using Shared.OtherModels.User;
using System.Collections.Generic;
using Shared.OtherModels.Response;
using BLL.Base.Interface;
using DAL.DapperObject.Interface;
using BLL.Attendance.Interface.Attendance;
using Shared.Attendance.ViewModel.Attendance;
using Shared.Attendance.DTO.Attendance;
using Shared.Attendance.Filter.Attendance;

namespace BLL.Attendance.Implementation.Attendance
{
    public class ManualAttendanceBusiness : IManualAttendanceBusiness
    {
        private readonly ISysLogger _sysLogger;
        private readonly IDapperData _dapper;
        public ManualAttendanceBusiness(IDapperData dapper, ISysLogger sysLogger)
        {
            _dapper = dapper;
            _sysLogger = sysLogger;
        }
        public async Task<IEnumerable<EmployeeManualAttendanceViewModel>> GetEmployeeManualAttendancesAsync(EmployeeManualAttendance_Filter filter, AppUser user)
        {
            IEnumerable<EmployeeManualAttendanceViewModel> data = new List<EmployeeManualAttendanceViewModel>();
            try
            {
                var sp_name = "sp_HR_ManualAttendance";
                var parameters = DapperParam.AddParams(filter, user);
                parameters.Add("ExecutionFlag", Data.Read);
                data = await _dapper.SqlQueryListAsync<EmployeeManualAttendanceViewModel>(user.Database, sp_name, parameters, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ManualAttendanceBusiness", "GetEmployeeManualAttendancesAsync", user);
            }
            return data;
        }
        public async Task<ExecutionStatus> SaveManualAttendanceAsync(EmployeeManualAttendanceDTO model, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                var sp_name = "sp_HR_ManualAttendance";
                var parameters = DapperParam.AddParams(model, user);
                parameters.Add("ExecutionFlag", model.ManualAttendanceId > 0 ? Data.Update : Data.Insert);
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                executionStatus = Utility.Invalid(ResponseMessage.SomthingWentWrong);
                await _sysLogger.SaveHRMSException(ex, user.Database, "ManualAttendanceBusiness", "SaveManualAttendanceAsync", user);
            }
            return executionStatus;
        }
        public async Task<ExecutionStatus> SaveManualAttendanceStatusAsync(ManualAttendanceStatusDTO model, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                var sp_name = "sp_HR_ManualAttendance";
                var parameters = DapperParam.AddParams(model, user);
                parameters.Add("ExecutionFlag", Data.Checking);
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                executionStatus = Utility.Invalid(ResponseMessage.SomthingWentWrong);
                await _sysLogger.SaveHRMSException(ex, user.Database, "ManualAttendanceBusiness", "SaveManualAttendanceStatusAsync", user);
            }
            return executionStatus;
        }
        public async Task<ExecutionStatus> DeleteManualAttendanceAsync(DeleteManualAttendanceDTO model, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                var sp_name = "sp_HR_ManualAttendance";
                var parameters = DapperParam.AddParams(model, user);
                parameters.Add("ExecutionFlag", Data.Delete);
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                executionStatus = Utility.Invalid(ResponseMessage.SomthingWentWrong);
                await _sysLogger.SaveHRMSException(ex, user.Database, "ManualAttendanceBusiness", "SaveManualAttendanceStatusAsync", user);
            }
            return executionStatus;
        }
        public async Task<IEnumerable<EmployeeManualAttendanceViewModel>> GetSubordinatesManualAttendancesRequestsAsync(SubordinatesManualAttendances_Filter filter, AppUser user)
        {
            IEnumerable<EmployeeManualAttendanceViewModel> data = new List<EmployeeManualAttendanceViewModel>();
            try
            {
                var sp_name = "sp_HR_ManualAttendance";
                var parameters = DapperParam.AddParams(filter, user);
                parameters.Add("SupervisorId", user.EmployeeId);
                parameters.Add("ExecutionFlag", Data.Read);
                data = await _dapper.SqlQueryListAsync<EmployeeManualAttendanceViewModel>(user.Database, sp_name, parameters, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ManualAttendanceBusiness", "GetSubordinatesManualAttendancesRequestsAsync", user);
            }
            return data;
        }
    }
}
