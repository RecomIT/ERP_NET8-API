using System;
using Dapper;
using System.Data;
using System.Linq;
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
    public class PublicHolidayBusiness : IPublicHolidayBusiness
    {
        private readonly IDapperData _dapper;
        private readonly ISysLogger _sysLogger;
        public PublicHolidayBusiness(IDapperData dapper, ISysLogger sysLogger)
        {
            _dapper = dapper;
            _sysLogger = sysLogger;
        }
        public async Task<IEnumerable<PublicHolidayViewModel>> GetPublicHolidaysAsync(long publicHolidayId, AppUser user)
        {
            IEnumerable<PublicHolidayViewModel> data = new List<PublicHolidayViewModel>();
            try
            {
                var sp_name = "sp_HR_PublicHoliday";
                var parameters = new DynamicParameters();
                parameters.Add("PublicHolidayId", publicHolidayId);
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("OrganizationId", user.OrganizationId);
                parameters.Add("Flag", Data.Read);
                data = await _dapper.SqlQueryListAsync<PublicHolidayViewModel>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "PublicHolidayBusiness", "GetPublicHolidaysAsync", user);
            }
            return data;
        }
        public async Task<ExecutionStatus> PublicHolidayValidatorAsync(PublicHolidayDTO model, AppUser user)
        {
            ExecutionStatus executionStatus = null;
            try
            {
                var list = await GetPublicHolidaysAsync(0, user);
                var isDuplicatePublicHoliday = list.Where(p => p.PublicHolidayId != model.PublicHolidayId && p.Title == model.Title && p.CompanyId == user.CompanyId && p.OrganizationId == user.OrganizationId) != null;
                if (isDuplicatePublicHoliday)
                {
                    executionStatus = new ExecutionStatus();
                    executionStatus.Status = false;
                    executionStatus.Msg = "Validation Error";
                    executionStatus.Errors = new Dictionary<string, string>();
                    executionStatus.Errors.Add("duplicatePublicHoliday", "Duplicate Public Holiday");
                }
            }
            catch (Exception ex)
            {
                executionStatus = Utility.Invalid();
                await _sysLogger.SaveHRMSException(ex, user.Database, "PublicHolidayBusiness", "GetPublicHolidaysAsync", user);
            }
            return executionStatus;
        }
        public async Task<ExecutionStatus> SavePublicHolidayAsync(PublicHolidayDTO model, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                var sp_name = "sp_HR_PublicHoliday";
                model.Type = model.IsDepandentOnMoon ? "Religious" : "National";
                var parameters = DapperParam.AddParams(model, user);
                parameters.Add("Flag", model.PublicHolidayId > 0 ? Data.Update : Data.Insert);

                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                executionStatus = Utility.Invalid();
                await _sysLogger.SaveHRMSException(ex, user.Database, "PublicHolidayBusiness", "SavePublicHolidayAsync", user);
            }
            return executionStatus;
        }
    }
}
