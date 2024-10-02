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
using BLL.Attendance.Interface.Holiday;
using Shared.Attendance.ViewModel.Holiday;
using Shared.Attendance.DTO.Holiday;


namespace BLL.Attendance.Implementation.Holiday
{
    public class YearlyHolidayBusiness : IYearlyHolidayBusiness
    {
        private readonly ISysLogger _sysLogger;
        private readonly IDapperData _dapper;
        public YearlyHolidayBusiness(ISysLogger sysLogger, IDapperData dapper)
        {
            _sysLogger = sysLogger;
            _dapper = dapper;
        }
        public async Task<IEnumerable<YearlyHolidayViewModel>> AssignYearlyHolidayAsync(AppUser user)
        {
            IEnumerable<YearlyHolidayViewModel> data = new List<YearlyHolidayViewModel>();
            try
            {
                var sp_name = "sp_HR_PublicHoliday";
                var parameters = new DynamicParameters();
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("OrganizationId", user.OrganizationId);
                parameters.Add("Flag", "AssignYearlyHoliday");
                data = await _dapper.SqlQueryListAsync<YearlyHolidayViewModel>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
                return data;
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "YearlyHolidayBusiness", "AssignYearlyHolidayAsync", user);
            }
            return data;
        }
        public async Task<IEnumerable<YearlyHolidayViewModel>> GetYearlyHolidaysAsync(long yearlyHolidayId, AppUser user)
        {
            IEnumerable<YearlyHolidayViewModel> data = new List<YearlyHolidayViewModel>();
            try
            {
                var sp_name = "sp_HR_YearlyHoliday";
                var parameters = new DynamicParameters();
                parameters.Add("YearlyHolidayId", yearlyHolidayId);
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("OrganizationId", user.OrganizationId);
                parameters.Add("Flag", Data.Read);
                data = await _dapper.SqlQueryListAsync<YearlyHolidayViewModel>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "YearlyHolidayBusiness", "AssignYearlyHolidayAsync", user);
            }
            return data;
        }
        public async Task<ExecutionStatus> SaveYearlyHolidayAsync(YearlyHolidayDTO model, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                var query = "sp_HR_YearlyHoliday";
                var parameters = DapperParam.AddParams(model, user);
                parameters.Add("@Flag", model.YearlyHolidayId > 0 ? Data.Update : Data.Insert);
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, query, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                executionStatus = Utility.Invalid(ResponseMessage.SomthingWentWrong);
                await _sysLogger.SaveHRMSException(ex, user.Database, "YearlyHolidayBusiness", "SaveYearlyHolidayAsync", user);
            }
            return executionStatus;
        }
        public async Task<ExecutionStatus> SaveYearlyPublicHolidayAsync(List<YearlyHolidayDTO> models, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                string jsonData = Utility.JsonData(models);
                var sp_name = "sp_HR_YearlyHoliday";
                var parameters = new DynamicParameters();
                parameters.Add("@JsonData", jsonData);
                parameters.Add("@UserId", user.ActionUserId);
                parameters.Add("@CompanyId", user.CompanyId);
                parameters.Add("@OrganizationId", user.OrganizationId);
                parameters.Add("@Flag", "JsonInsert");
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                executionStatus = Utility.Invalid("Server responsed with error");
                await _sysLogger.SaveHRMSException(ex, user.Database, "YearlyHolidayBusiness", "SaveYearlyHolidayAsync", user);
            }
            return executionStatus;
        }
    }
}
