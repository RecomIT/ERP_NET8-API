using BLL.Base.Interface;
using BLL.Leave.Interface.LeaveSetting;
using DAL.DapperObject.Interface;
using Dapper;
using Shared.Leave.Domain.Setup;
using Shared.Leave.DTO.Setup;
using Shared.Leave.ViewModel.Setup;
using Shared.OtherModels.DataService;
using Shared.OtherModels.Response;
using Shared.OtherModels.User;
using Shared.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace BLL.Leave.Implementation.LeaveSetting
{
    public class LeaveTypeBusiness : ILeaveTypeBusiness
    {
        private readonly ISysLogger _sysLogger;
        private readonly IDapperData _dapper;
        public LeaveTypeBusiness(ISysLogger sysLogger, IDapperData dapper)
        {
            _sysLogger = sysLogger;
            _dapper = dapper;
        }
        public async Task<ExecutionStatus> DeleteLeaveTypeAsync(long leaveTypeId, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                var sp_name = "sp_HR_LeaveType";
                var parameters = new DynamicParameters();
                parameters.Add("LeaveTypeId", leaveTypeId);
                parameters.Add("Flag", Data.Delete);
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                executionStatus = Utility.Invalid();
                await _sysLogger.SaveHRMSException(ex, user.Database, "LeaveTypeBusiness", "SaveLeaveTypeAsync", user);
            }
            return executionStatus;
        }

        public async Task<LeaveType> GetLeaveTypeById(long id, AppUser user)
        {
            LeaveType leaveType = null;
            try
            {
                var query = $@"SELECT * FROM HR_LeaveTypes Where Id=@Id AND CompanyId=@CompanyId AND OrganizationId=@OrganizationId";
                leaveType = await _dapper.SqlQueryFirstAsync<LeaveType>(user.Database, query, new { Id = id, user.CompanyId, user.OrganizationId });
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "LeaveTypeBusiness", "GetLeaveTypeById", user);
            }
            return leaveType;
        }

        public async Task<IEnumerable<LeaveTypeViewModel>> GetLeaveTypesAsync(LeaveType_Filter filter, AppUser user)
        {
            IEnumerable<LeaveTypeViewModel> data = new List<LeaveTypeViewModel>();
            try
            {
                var query = $@"Select * From HR_LeaveTypes LT
				Where 1=1
				AND (@LeaveTypeId IS NULL OR @LeaveTypeId = 0 OR LT.Id=@LeaveTypeId)
                AND (@LeaveTypeName IS NULL OR @LeaveTypeName = 0 OR LT.Title=@LeaveTypeName)
				AND (@CompanyId =0 OR LT.CompanyId=@CompanyId)
				AND (LT.OrganizationId=@OrganizationId)";
                var parameters = DapperParam.AddParams(filter, user);
                data = await _dapper.SqlQueryListAsync<LeaveTypeViewModel>(user.Database, query, parameters, CommandType.Text);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "LeaveTypeBusiness", "GetLeaveTypesAsync", user);
            }
            return data;
        }
        public async Task<IEnumerable<Select2Dropdown>> GetLeaveTypesDropdownAsync(AppUser user)
        {
            IEnumerable<Select2Dropdown> list = new List<Select2Dropdown>();
            try
            {
                var query = $@"Select Cast(LT.Id as nvarchar(50)) 'Value',Cast(LT.Id as nvarchar(50)) 'Id',LT.Title 'Text' From HR_LeaveTypes LT
				Where 1=1
				AND (LT.CompanyId=@CompanyId)
				AND (LT.OrganizationId=@OrganizationId) Order By LT.Title";
                list = await _dapper.SqlQueryListAsync<Select2Dropdown>(user.Database, query, new { user.CompanyId, user.OrganizationId }, CommandType.Text);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "LeaveTypeBusiness", "GetLeaveTypesDropdownAsync", user);
            }
            return list;
        }
        public async Task<ExecutionStatus> LeaveTypeValidatorAsync(LeaveTypeDTO model, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                var sp_name = "sp_HR_LeaveType";
                var parameters = DapperParam.AddParams(model, user);
                parameters.Add("Flag", Data.Validate);
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                executionStatus = Utility.Invalid();
                await _sysLogger.SaveHRMSException(ex, user.Database, "LeaveTypeBusiness", "LeaveTypeValidatorAsync", user);
            }
            return executionStatus;
        }
        public async Task<ExecutionStatus> SaveLeaveTypeAsync(LeaveTypeDTO model, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                var sp_name = "sp_HR_LeaveType";
                var parameters = DapperParam.AddParams(model, user);
                parameters.Add("@Flag", model.Id > 0 ? Data.Update : Data.Insert);
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                executionStatus = Utility.Invalid();
                await _sysLogger.SaveHRMSException(ex, user.Database, "LeaveTypeBusiness", "SaveLeaveTypeAsync", user);
            }
            return executionStatus;
        }
    }
}
