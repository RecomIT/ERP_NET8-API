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
using DAL.Repository.Leave.Interface;
using BLL.Leave.Interface.LeaveSetting;
using Shared.Leave.DTO.Setup;
using Shared.Leave.Filter.Request;
using Shared.Leave.Filter.Setup;
using Shared.Leave.ViewModel.Setup;
using BLL.Base.Implementation;

namespace BLL.Leave.Implementation.LeaveSetting
{
    public class LeaveSettingBusiness : ILeaveSettingBusiness
    {
        private readonly ISysLogger _sysLogger;
        private readonly IDapperData _dapper;
        private readonly ILeaveSettingRepository _leaveSettingRepository;
        private readonly IModuleConfig _moduleConfig;
        public LeaveSettingBusiness(ISysLogger sysLogger, IDapperData dapper, ILeaveSettingRepository leaveSettingRepository, IModuleConfig moduleConfig)
        {
            _sysLogger = sysLogger;
            _dapper = dapper;
            _leaveSettingRepository = leaveSettingRepository;
            _moduleConfig = moduleConfig;
        }
        public async Task<ExecutionStatus> DeleteLeaveSettingAsync(long leaveSettingId, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                var sp_name = "sp_HR_LeaveSetting";
                var parametes = new DynamicParameters();
                parametes.Add("LeaveSettingId", leaveSettingId);
                parametes.Add("CompanyId", user.CompanyId);
                parametes.Add("OrganizationId", user.OrganizationId);
                parametes.Add("Flag", Data.Delete);
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parametes, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                executionStatus = ResponseMessage.Invalid();
                await _sysLogger.SaveHRMSException(ex, user.Database, "LeaveSettingBusiness", "DeleteLeaveSettingAsync", user);
            }
            return executionStatus;
        }

        public async Task<IEnumerable<LeaveSettingInfoDTO>> GetLeavePeriodAsync(long employeeId, AppUser user)
        {
            IEnumerable<LeaveSettingInfoDTO> list = new List<LeaveSettingInfoDTO>();
            try
            {
                var query = $@"Select distinct convert(varchar,LeavePeriodStart,106) as LeavePeriodStart,convert(varchar,LeavePeriodEnd,106) as LeavePeriodEnd From HR_EmployeeLeaveBalance
				Where 1=1
				and (EmployeeId=@EmployeeId)
				and (CAST(GETDATE() AS DATE) BETWEEN LeavePeriodStart AND LeavePeriodEnd)
				and (CompanyId =@CompanyId)
				and (OrganizationId = @OrganizationId)";
                var parameters = new DynamicParameters();
                parameters.Add("EmployeeId", employeeId);
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("OrganizationId", user.OrganizationId);
                list = await _dapper.SqlQueryListAsync<LeaveSettingInfoDTO>(user.Database, query, parameters, CommandType.Text);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "LeaveSettingBusiness", "GetLeavePeriodAsync", user);
            }
            return list;
        }




        public async Task<IEnumerable<LeaveSettingInfoDTO>> GetLeaveTypeSettingAsync(long leaveTypeId, long? employeeId, AppUser user)
        {
            IEnumerable<LeaveSettingInfoDTO> list = new List<LeaveSettingInfoDTO>();
            try
            {
                var query = $@"Select 
	                            RequestDaysBeforeTakingLeave,
	                            AcquiredViaOffDayWork,
	                            FileAttachedOption,
	                            MaxDaysLeaveAtATime,
	                            LeaveTypeName=LT.Title,
	                            convert(varchar,BLN.LeavePeriodStart,106) as LeavePeriodStart,
	                            convert(varchar,BLN.LeavePeriodEnd,106) as LeavePeriodEnd,
	                            ls.ShowFullCalender,
	                            IsMinimumDaysRequiredForFileAttached,
	                            RequiredDaysForFileAttached,
                                RequiredDaysBeforeEDD,
                                MandatoryNumberOfDays,
                                BLN.TotalLeave
      
                    
                            From HR_LeaveSetting ls
	                            INNER JOIN HR_LeaveTypes LT
	                            ON ls.LeaveTypeId=lt.Id
                            LEFT JOIN  HR_EmployeeLeaveBalance BLN 
	                            ON lt.Id = BLN.LeaveTypeId AND ls.LeaveSettingId= BLN.LeaveSettingId
                            Where 1=1
	                            and (@LeaveTypeId =0 OR @LeaveTypeId IS NULL OR lt.Id=@LeaveTypeId)
	                            AND (CAST(GETDATE() AS DATE) BETWEEN LeavePeriodStart AND LeavePeriodEnd)
	                            AND (@EmployeeId IS NULL OR @EmployeeId =0 OR BLN.EmployeeId = @EmployeeId)
	                            and (ls.CompanyId =@CompanyId)
	                            and (ls.OrganizationId = @OrganizationId)";

                var parameters = new DynamicParameters();
                parameters.Add("LeaveTypeId", leaveTypeId);
                parameters.Add("EmployeeId", employeeId ?? 0);
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("OrganizationId", user.OrganizationId);

                list = await _dapper.SqlQueryListAsync<LeaveSettingInfoDTO>(user.Database, query, parameters, CommandType.Text);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "LeaveSettingBusiness", "GetLeaveSettingAsync", user);
            }
            return list;
        }






        public async Task<IEnumerable<LeaveSettingViewModel>> GetLeaveSettingsAsync(LeaveSetting_Filter filter, AppUser user)
        {
            IEnumerable<LeaveSettingViewModel> data = new List<LeaveSettingViewModel>();
            try
            {
                var query = $@"Select LS.*,LT.Title 'LeaveTypeName',LT.ShortName From HR_LeaveSetting LS
				INNER JOIN HR_LeaveTypes LT ON LS.LeaveTypeId = LT.Id
				Where 1=1
				AND (@LeaveSettingId IS NULL OR @LeaveSettingId = 0 OR LS.LeaveSettingId=@LeaveSettingId)
				AND (@LeaveTypeId IS NULL OR @LeaveTypeId = 0 OR LS.LeaveTypeId=@LeaveTypeId)
				AND (LS.CompanyId=@CompanyId)
				AND (LS.OrganizationId=@OrganizationId)";
                var parameters = DapperParam.AddParams(filter, user);
                data = await _dapper.SqlQueryListAsync<LeaveSettingViewModel>(user.Database, query, parameters, CommandType.Text);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "LeaveSettingBusiness", "GetLeaveSettingsAsync", user);
            }
            return data;
        }



        public async Task<ExecutionStatus> LeaveSettingValidatorAsync(LeaveSettingDTO model, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                var sp_name = "sp_HR_LeaveSetting";
                var parameters = new DynamicParameters();
                parameters.Add("LeaveSettingId", model.LeaveSettingId);
                parameters.Add("LeaveTypeId", model.LeaveTypeId);
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("OrganizationId", user.OrganizationId);
                parameters.Add("Flag", Data.Validate);
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                executionStatus = ResponseMessage.Invalid();
                await _sysLogger.SaveHRMSException(ex, user.Database, "LeaveSettingBusiness", "LeaveSettingValidatorAsync", user);
            }
            return executionStatus;
        }




        public async Task<ExecutionStatus> SaveLeaveSettingAsync(LeaveSettingDTO model, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                executionStatus = await _leaveSettingRepository.SaveAsync(model, user);
            }
            catch (Exception ex)
            {
                executionStatus = ResponseMessage.Invalid();
                await _sysLogger.SaveHRMSException(ex, user.Database, "LeaveSettingBusiness", "SaveLeaveSettingAsync", user);
            }
            return executionStatus;
        }
        public async Task<ExecutionStatus> GetTotalRequestDaysAsync(TotalRequestDays_Filter filter, AppUser user)
        {
            ExecutionStatus executionStatus = null;
            try
            {
                var query = "sp_HR_EmployeeLeaveRequest_2";
                var parameters = DapperParam.AddParams(filter, user);
                parameters.Add("ExecutionFlag", "Total_Request_Days");
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, query, parameters, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                executionStatus = ResponseMessage.Invalid();
                await _sysLogger.SaveHRMSException(ex, user.Database, "LeaveSettingBusiness", "GetTotalRequestDaysAsync", user);
            }
            return executionStatus;
        }

        public async Task<LeavePeriodRange> GetLeavePeriodDateRange(DateTime? joiningDate, int leaveYear, AppUser user)
        {
            LeavePeriodRange leavePeriodRange = null;
            try
            {
                var moduleConfig = await _moduleConfig.HRModuleConfig(user);
                if (moduleConfig != null)
                {
                    leavePeriodRange =
                        DateTimeExtension.GetLeavePeriodDateRange(joiningDate, moduleConfig.LeaveStartMonth ?? 0, moduleConfig.LeaveEndMonth ?? 0, leaveYear);
                }
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "LeaveSettingBusiness", "GetLeavePeriodDateRange", user);
            }
            return leavePeriodRange;
        }
    }
}
