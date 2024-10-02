using Dapper;
using System;
using System.Data;
using Shared.Services;
using BLL.Base.Interface;
using System.Threading.Tasks;
using Shared.OtherModels.User;
using System.Collections.Generic;
using Shared.OtherModels.Response;
using Shared.OtherModels.DataService;
using DAL.DapperObject.Interface;
using BLL.Attendance.Interface.WorkShift;
using Shared.Attendance.ViewModel.Workshift;
using Shared.Attendance.DTO.Workshift;
using Shared.Attendance.Filter.Shift;

namespace BLL.Attendance.Implementation.WorkShift
{
    public class WorkShiftBusiness : IWorkShiftBusiness
    {
        private readonly IDapperData _dapper;
        private readonly ISysLogger _sysLogger;
        public WorkShiftBusiness(IDapperData dapper, ISysLogger sysLogger)
        {
            _dapper = dapper;
            _sysLogger = sysLogger;
        }
        public async Task<IEnumerable<Select2Dropdown>> GetWorkShiftDropdownAsync(AppUser user)
        {
            IEnumerable<Select2Dropdown> list = new List<Select2Dropdown>();
            try
            {
                var query = @"
    SELECT 
        CONVERT(NVarchar(20), WorkShiftId) AS Id,
        (Title + ' [' + NameDetail + ']') AS Text
    FROM 
        HR_WorkShifts
    WHERE 
        CompanyId = @CompanyId
        AND OrganizationId = @OrganizationId Order By Title";
                var parameter = new DynamicParameters();
                parameter.Add("CompanyId", user.CompanyId);
                parameter.Add("OrganizationId", user.OrganizationId);

                list = await _dapper.SqlQueryListAsync<Select2Dropdown>(user.Database, query, parameter, CommandType.Text);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "WorkShiftBusiness", "GetWorkShiftExtensionAsync", user);
            }
            return list;
        }
        public async Task<WorkShiftViewModel> GetWorkShiftsByIdAsync(long workShiftId, AppUser user)
        {
            WorkShiftViewModel shift = new WorkShiftViewModel();
            try
            {
                var query = @"Select ws.* From HR_WorkShifts ws
			Where 1=1
			AND (ws.WorkShiftId=@WorkShiftId)
			AND (ws.CompanyId=@CompanyId)
			AND (ws.OrganizationId=@OrganizationId)";

                var parameters = new DynamicParameters();
                parameters.Add("@WorkShiftId", workShiftId);
                parameters.Add("@CompanyId", user.CompanyId);
                parameters.Add("@OrganizationId", user.OrganizationId);

                shift = await _dapper.SqlQueryFirstAsync<WorkShiftViewModel>(user.Database, query, parameters, CommandType.Text);

            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "WorkShiftBusiness", "GetWorkShiftsAsync", user);
            }
            return shift;
        }
        public async Task<IEnumerable<WorkShiftViewModel>> GetWorkShiftsAsync(WorkShift_Filter filter, AppUser user)
        {
            IEnumerable<WorkShiftViewModel> list = new List<WorkShiftViewModel>();
            try
            {
                var query = @"Select ws.* From HR_WorkShifts ws
			Where 1=1
			AND (@WorkShiftId IS NULL OR @WorkShiftId = 0 OR ws.WorkShiftId=@WorkShiftId)
			AND (ws.CompanyId=@CompanyId)
			AND (ws.OrganizationId=@OrganizationId)";
                var parameters = DapperParam.AddParams(filter, user, new string[] { "WorkshiftName" }, addUserId: false);
                list = await _dapper.SqlQueryListAsync<WorkShiftViewModel>(user.Database, query, parameters, CommandType.Text);

            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "WorkShiftBusiness", "GetWorkShiftsAsync", user);
            }
            return list;
        }
        public async Task<ExecutionStatus> SaveWorkShiftAsync(WorkShiftDTO model, List<string> Weekends, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                model.WeekendDayName = string.Join(',', Weekends);
                var sp_name = "sp_HR_WorkShifts";
                var parameters = DapperParam.AddParams(model, user, new string[] { "Weekends" });
                parameters.Add("@JsonData", Utility.JsonData(Weekends));
                parameters.Add("@Flag", model.WorkShiftId > 0 ? Data.Update : Data.Insert);
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                executionStatus = Utility.Invalid();
                await _sysLogger.SaveHRMSException(ex, user.Database, "WorkShiftBusiness", "SaveWorkShiftAsync", user);
            }
            return executionStatus;
        }
        public async Task<ExecutionStatus> SaveWorkShiftChecking(WorkShiftCheckingDTO model, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                var sp_name = "sp_HR_WorkShifts";
                var parameters = DapperParam.AddParams(model, user);
                parameters.Add("@Flag", Data.Checking);
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                executionStatus = Utility.Invalid();
                await _sysLogger.SaveHRMSException(ex, user.Database, "WorkShiftBusiness", "SaveWorkShiftAsync", user);
            }
            return executionStatus;
        }
        public Task<ExecutionStatus> WorkShiftValidatorAsync(WorkShiftDTO model, AppUser appUser)
        {
            throw new NotImplementedException();
        }
    }
}
