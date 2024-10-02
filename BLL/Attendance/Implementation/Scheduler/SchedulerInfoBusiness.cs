using System;
using System.Data;
using System.Linq;
using Shared.Services;
using System.Threading.Tasks;
using Shared.OtherModels.User;
using System.Collections.Generic;
using Shared.OtherModels.Response;
using BLL.Base.Interface;
using DAL.DapperObject.Interface;
using BLL.Attendance.Interface.Scheduler;
using Shared.Attendance.ViewModel.Scheduler;
using Shared.Attendance.DTO.Scheduler;
using Shared.Attendance.Filter.Scheduler;

namespace BLL.Attendance.Implementation.Scheduler
{
    public class SchedulerInfoBusiness : ISchedulerInfoBusiness
    {
        private readonly ISysLogger _sysLogger;
        private readonly IDapperData _dapper;
        private readonly ISchedulerDetailBusiness _schedulerDetailBusiness;
        public SchedulerInfoBusiness(ISysLogger sysLogger, IDapperData dapper, ISchedulerDetailBusiness schedulerDetailBusiness)
        {
            _sysLogger = sysLogger;
            _dapper = dapper;
            _schedulerDetailBusiness = schedulerDetailBusiness;
        }
        public async Task<ExecutionStatus> DeleteSchedulerInfoAsync(DeleteSchedulerInfoDTO model, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                var sp_name = "sp_HR_SchedulerInfo";
                var parameters = DapperParam.AddParams(model, user);
                parameters.Add("ExecutionFlag", Data.Delete);
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                executionStatus = Utility.Invalid(ResponseMessage.SomthingWentWrong);
                await _sysLogger.SaveHRMSException(ex, user.Database, "SchedulerInfoBusiness", "DeleteSchedulerInfoAsync", user);
            }
            return executionStatus;
        }
        public async Task<IEnumerable<SchedulerInfoViewModel>> GetSchedulerInfosAsync(SchedulerInfo_Filter filter, AppUser user)
        {
            IEnumerable<SchedulerInfoViewModel> data = new List<SchedulerInfoViewModel>();
            try
            {
                var sp_name = "sp_HR_SchedulerInfo";
                var parameters = DapperParam.AddParams(filter, user);
                parameters.Add("ExecutionFlag", Data.Read);
                data = await _dapper.SqlQueryListAsync<SchedulerInfoViewModel>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "SchedulerInfoBusiness", "GetSchedulerInfosAsync", user);
            }
            return data;
        }
        public async Task<SchedulerInfoViewModel> GetSchedulerInfoWithDetailsAsync(SchedulerInfo_Filter model, AppUser user)
        {
            SchedulerInfoViewModel data = new SchedulerInfoViewModel();
            try
            {
                data = (await GetSchedulerInfosAsync(model, user)).FirstOrDefault();
                if (data != null)
                {
                    data.SchedulerDetails = (await _schedulerDetailBusiness.GetSchedulerDetailsAsync(new SchedulerDetail_Filter()
                    {
                        SchedulerCode = model.ScheduleCode,
                        SchedulerInfoId = model.SchedulerInfoId,
                        SchedulerDetailId = "0",
                        HostEmployeeId = "0"
                    }, user)).ToList();
                }
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "SchedulerInfoBusiness", "GetSchedulerInfoWithDetailsAsync", user);
            }
            return data;
        }
        public async Task<ExecutionStatus> SaveSchedulerAsync(SchedulerInfoDTO model, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                List<string> employeeIds = new List<string>() { model.HostEmployeeId.ToString() };
                // Need to copy from previous source
            }
            catch (Exception ex)
            {
                executionStatus = Utility.Invalid(ResponseMessage.SomthingWentWrong);
                await _sysLogger.SaveHRMSException(ex, user.Database, "SchedulerInfoBusiness", "SaveSchedulerAsync", user);
            }
            return executionStatus;
        }
        public async Task<ExecutionStatus> SaveSchedulerStatusAsync(SchedulerInfoStatusDTO model, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                var sp_name = "sp_HR_SchedulerInfo";
                var parameters = DapperParam.AddParams(model, user);
                parameters.Add("ExecutionFlag", Data.Checking);
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                executionStatus = Utility.Invalid(ResponseMessage.SomthingWentWrong);
                await _sysLogger.SaveHRMSException(ex, user.Database, "SchedulerInfoBusiness", "SaveSchedulerStatusAsync", user);
            }
            return executionStatus;
        }
    }
}
