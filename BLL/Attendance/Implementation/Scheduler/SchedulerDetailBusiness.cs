using System;
using System.Data;
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
    public class SchedulerDetailBusiness : ISchedulerDetailBusiness
    {
        private readonly ISysLogger _sysLogger;
        private readonly IDapperData _dapper;
        public SchedulerDetailBusiness(ISysLogger sysLogger, IDapperData dapper)
        {
            _sysLogger = sysLogger;
            _dapper = dapper;
        }
        public async Task<IEnumerable<SchedulerDetailListViewModel>> GetSchedulerDetailListAsync(SchedulerDetail_Filter filter, AppUser user)
        {
            IEnumerable<SchedulerDetailListViewModel> data = new List<SchedulerDetailListViewModel>();
            try
            {
                var sp_name = "sp_HR_SchedulerDetail";
                var parameters = DapperParam.AddParams(filter, user);
                parameters.Add("ExecutionFlag", Data.Read);
                data = await _dapper.SqlQueryListAsync<SchedulerDetailListViewModel>(user.Database, sp_name, parameters, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "SchedulerDetailBusiness", "GetSchedulerDetailListAsync", user);
            }
            return data;
        }
        public async Task<IEnumerable<SchedulerDetailViewModel>> GetSchedulerDetailsAsync(SchedulerDetail_Filter filter, AppUser user)
        {
            IEnumerable<SchedulerDetailViewModel> data = new List<SchedulerDetailViewModel>();
            try
            {
                var sp_name = "sp_HR_SchedulerDetail";
                var parameters = DapperParam.AddParams(filter, user);
                parameters.Add("ExecutionFlag", Data.Read);
                data = await _dapper.SqlQueryListAsync<SchedulerDetailViewModel>(user.Database, sp_name, parameters, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "SchedulerDetailBusiness", "GetSchedulerDetailsAsync", user);
            }
            return data;
        }
        public async Task<ExecutionStatus> SaveSchedulerParticipantStatusAsync(SchedulerParticipantStatusDTO model, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                var sp_name = "sp_HR_SchedulerDetail";
                var parameters = DapperParam.AddParams(model, user);
                parameters.Add("ExecutionFlag", Data.Update);
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "SchedulerDetailBusiness", "SaveSchedulerParticipantStatusAsync", user);
            }
            return executionStatus;
        }
    }
}
