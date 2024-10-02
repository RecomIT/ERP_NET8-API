using System;
using Dapper;
using System.Data;
using Shared.Services;
using DAL.DapperObject;
using BLL.Base.Interface;
using System.Threading.Tasks;
using Shared.OtherModels.User;
using System.Collections.Generic;
using Shared.OtherModels.Response;
using DAL.DapperObject.Interface;
using BLL.Attendance.Interface.Attendance;
using Shared.Attendance.DTO.AttendanceDataPulling.AgaKhan;

namespace BLL.Attendance.Implementation.Attendance
{
    public class AttendanceDataPullingService : IAttendanceDataPullingService
    {
        private readonly IDapperData _dapper;
        private readonly ISysLogger _sysLogger;
        public AttendanceDataPullingService(IDapperData dapper, ISysLogger sysLogger)
        {
            _dapper = dapper;
            _sysLogger = sysLogger;
        }

        #region Aga Khan Attendance Data Pulling Service
        public async Task<ExecutionStatus> AgakhanDataPulling(AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                var parameterValues = await GetAgakhanDataPullingParameter(user);
                if (!Utility.IsNullEmptyOrWhiteSpace(parameterValues.ConnectionString))
                {
                    var attn_users = await GetAgakhanAttendanceDeviceUsersDataAsync(parameterValues.ConnectionString, parameterValues.UserId, user);
                    var attn_data = await GetAgakhanAttendanceDataAsync(parameterValues.ConnectionString, parameterValues.EventLogId, user);

                    var sp_name = "sp_AgakhanAttendanceDataInserting";

                    var attn_users_json = Utility.JsonData(attn_users);
                    var attn_data_json = Utility.JsonData(attn_data);

                    var parameters = new DynamicParameters();
                    parameters.Add("UserJsonData", attn_users_json);
                    parameters.Add("AttendanceJsonData", attn_data_json);
                    parameters.Add("UserId", user.UserId);

                    executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(Database.ControlPanel, sp_name, parameters, CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                executionStatus = ResponseMessage.Invalid();
                await _sysLogger.SaveHRMSException(ex, user.Database, "AttendanceDataPullingService", "AgakhanDataPulling", user);
            }
            return executionStatus;
        }
        public async Task<IEnumerable<AgakhanEventLogDTO>> GetAgakhanAttendanceDataAsync(string deviceConnectionString, int lastId, AppUser user)
        {
            IEnumerable<AgakhanEventLogDTO> list = new List<AgakhanEventLogDTO>();
            try
            {
                var query = @"SELECT * FROM TB_EVENT_LOG WHERE nEventLogIdn > @id";
                list = await _dapper.RemoteSqlQueryListAsync<AgakhanEventLogDTO>(deviceConnectionString, query, new { id = lastId }, CommandType.Text);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "AttendanceDataPullingService", "GetAgakhanAttendanceDataAsync", user);
            }
            return list;
        }
        public async Task<IEnumerable<AgakhanUserDTO>> GetAgakhanAttendanceDeviceUsersDataAsync(string deviceConnectionString, int lastId, AppUser user)
        {
            IEnumerable<AgakhanUserDTO> list = new List<AgakhanUserDTO>();
            try
            {
                var query = @"SELECT * FROM TB_USER WHERE nUserIdn > @id";
                list = await _dapper.RemoteSqlQueryListAsync<AgakhanUserDTO>(deviceConnectionString, query, new { id = lastId }, CommandType.Text);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "AttendanceDataPullingService", "GetAgakhanAttendanceDeviceUsersDataAsync", user);
            }
            return list;
        }
        public async Task<AgakhanDataPullingParameterDTO> GetAgakhanDataPullingParameter(AppUser user)
        {
            AgakhanDataPullingParameterDTO data = new AgakhanDataPullingParameterDTO();
            try
            {
                var query = @"sp_AgaKhanDataPullingParameters";
                data = await _dapper.SqlQueryFirstAsync<AgakhanDataPullingParameterDTO>(Database.ControlPanel, query, null, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "AttendanceDataPullingService", "GetAgakhanAttendanceDeviceUsersDataAsync", user);
            }
            return data;
        }
        #endregion
    }
}
