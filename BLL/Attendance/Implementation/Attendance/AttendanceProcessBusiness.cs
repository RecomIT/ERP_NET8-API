using Dapper;
using System;
using System.Data;
using Shared.Services;
using BLL.Base.Interface;
using System.Threading.Tasks;
using Shared.OtherModels.User;
using System.Collections.Generic;
using Shared.OtherModels.Response;
using DAL.DapperObject.Interface;
using BLL.Attendance.Interface.Attendance;
using Shared.Attendance.ViewModel.Attendance;

namespace BLL.Attendance.Implementation.Attendance
{
    public class AttendanceProcessBusiness : IAttendanceProcessBusiness
    {
        private readonly ISysLogger _sysLogger;
        private readonly IDapperData _dapper;
        public AttendanceProcessBusiness(ISysLogger sysLogger, IDapperData dapper)
        {
            _sysLogger = sysLogger;
            _dapper = dapper;
        }

        public async Task<ExecutionStatus> AttendanceProcessAsync(AttendanceProcessViewModel model, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                var sp_name = "sp_HR_AttendanceProcessInfo"; // 
                var parameter1 = new DynamicParameters();
                parameter1.Add("Month", model.Month);
                parameter1.Add("Year", model.Year);
                parameter1.Add("BranchId", model.BranchId);
                parameter1.Add("UserId", user.UserId);
                parameter1.Add("CompanyId", user.CompanyId);
                parameter1.Add("OrganizationId", user.OrganizationId);
                parameter1.Add("ExecutionFlag", Data.Insert);
                await _dapper.SqlExecuteNonQuery(user.Database, sp_name, parameter1, CommandType.StoredProcedure);

                sp_name = string.Format("sp_HR_AttendanceProcess"); // Main Attendance Process
                var parameter2 = new DynamicParameters();
                parameter2.Add("SelectedEmployees", model.SelectedEmployees ?? "");
                parameter2.Add("AttendanceFromDate", model.FromDate);
                parameter2.Add("AttendanceToDate", model.ToDate);
                parameter2.Add("UserId", user.UserId);
                parameter2.Add("CompanyId", user.CompanyId);
                parameter2.Add("OrganizationId", user.OrganizationId);

                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameter2, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                executionStatus = Utility.Invalid(ResponseMessage.ServerResponsedWithError);
                await _sysLogger.SaveHRMSException(ex, user.Database, "AttendanceProcessBusiness", "AttendanceProcessAsync", user);
            }
            return executionStatus;
        }
        public async Task<IEnumerable<AttendanceProcessViewModel>> GetAttendanceProcessInfosAsync(short? month, short? year, AppUser user)
        {
            IEnumerable<AttendanceProcessViewModel> data = new List<AttendanceProcessViewModel>();
            try
            {
                var sp_name = "sp_HR_AttendanceProcessInfo";
                var parameter = new DynamicParameters();
                parameter.Add("Month", month ?? 0);
                parameter.Add("Year", year ?? 0);
                parameter.Add("CompanyId", user.CompanyId);
                parameter.Add("OrganizationId", user.OrganizationId);
                parameter.Add("ExecutionFlag", Data.Read);
                data = await _dapper.SqlQueryListAsync<AttendanceProcessViewModel>(user.Database, sp_name, parameter, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "AttendanceProcessBusiness", "GetAttendanceProcessInfosAsync", user);
            }
            return data;
        }
        public async Task<ExecutionStatus> LockAttendanceProcessAsync(AttendanceProcessLockUnlock model, AppUser user)
        {
            ExecutionStatus executionStatus = null;
            try
            {
                var sp_name = "sp_HR_AttendanceProcessInfo";
                var parameters = new DynamicParameters();
                parameters.Add("AttendanceProcessId", model.AttendanceProcessId);
                parameters.Add("Month", model.Month);
                parameters.Add("Year", model.Year);
                parameters.Add("IsLocked", model.IsLocked);
                parameters.Add("UserId", user.UserId);
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("OrganizationId", user.OrganizationId);
                parameters.Add("ExecutionFlag", Data.Update);
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "AttendanceProcessBusiness", "GetEmployeesDailyAttendanceAsync", user);
            }
            return executionStatus;
        }
        public async Task<ExecutionStatus> UnLockAttendanceProcessAsync(AttendanceProcessLockUnlock model, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                var sp_name = "sp_HR_AttendanceProcessInfo";
                var parameters = new DynamicParameters();
                parameters.Add("AttendanceProcessId", model.AttendanceProcessId);
                parameters.Add("Month", model.Month);
                parameters.Add("Year", model.Year);
                parameters.Add("IsLocked", model.IsLocked);
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("OrganizationId", user.OrganizationId);
                parameters.Add("ExecutionFlag", Data.Update);
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, CommandType.StoredProcedure);

            }
            catch (Exception ex)
            {
                executionStatus = Utility.Invalid();
                await _sysLogger.SaveHRMSException(ex, user.Database, "AttendanceProcessBusiness", "UnLockAttendanceProcessAsync", user);
            }
            return executionStatus;
        }
        public async Task<ExecutionStatus> UploadRowAttendanceData(List<UploadAttendanceViewModel> attendances, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                var sp_name = "sp_HR_AttendanceRowData";
                var jsonData = Utility.JsonData(attendances);
                var paramaters = new DynamicParameters();
                paramaters.Add("JsonData", jsonData);
                paramaters.Add("UserId", user.UserId);
                paramaters.Add("CompanyId", user.CompanyId);
                paramaters.Add("OrganizationId", user.OrganizationId);
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, paramaters, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                executionStatus = Utility.Invalid();
                await _sysLogger.SaveHRMSException(ex, user.Database, "AttendanceProcessBusiness", "UploadRowAttendanceData", user);
            }
            return executionStatus;
        }
        public async Task<ExecutionStatus> ValidateAttendanceProcessAsync(short month, short year, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                var sp_name = "sp_HR_AttendanceProcessInfo";
                var parameters = new DynamicParameters();
                parameters.Add("Month", month);
                parameters.Add("Year", year);
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("OrganizationId", user.OrganizationId);
                parameters.Add("ExecutionFlag", Data.Validate);
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "AttendanceProcessBusiness", "ValidateAttendanceProcessAsync", user);
            }
            return executionStatus;
        }
    }
}
