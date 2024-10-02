using Shared.Services;
using BLL.Base.Interface;
using Shared.OtherModels.User;
using DAL.DapperObject.Interface;
using Shared.OtherModels.Response;
using Shared.Employee.DTO.Organizational;
using Shared.Employee.Filter.Organizational;
using BLL.Employee.Interface.Organizational;
using Shared.Employee.ViewModel.Organizational;

using System.Data;

namespace BLL.Employee.Implementation.Organizational
{
    public class LineBusiness : ILineBusiness
    {
        private readonly IDapperData _dapper;
        private readonly ISysLogger _sysLogger;
        public LineBusiness(IDapperData dapper, ISysLogger sysLogger)
        {
            _dapper = dapper;
            _sysLogger = sysLogger;
        }
        public async Task<IEnumerable<LineViewModel>> GetLinesAsync(Line_Filter filter, AppUser user)
        {
            IEnumerable<LineViewModel> list = new List<LineViewModel>();
            try
            {
                var sp_name = "sp_HR_Line_List";
                var parameters = DapperParam.AddParams(filter, user);
                list = await _dapper.SqlQueryListAsync<LineViewModel>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "LineBusiness", "GetLinesAsync", user);
            }
            return list;
        }
        public async Task<ExecutionStatus> SaveLineAsync(LineDTO model, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                var sp_name = "sp_HR_Line_Insert_Update";
                var parameters = DapperParam.AddParams(model, user);
                parameters.Add("ExecutionFlag", model.LineId > 0 ? Data.Update : Data.Insert);
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                executionStatus = ResponseMessage.Invalid();
                await _sysLogger.SaveHRMSException(ex, user.Database, "LineBusiness", "SaveLineAsync", user);
            }
            return executionStatus;
        }
        public async Task<ExecutionStatus> ValidateLineAsync(LineDTO model, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                var sp_name = "sp_HR_Line_Insert_Update";
                var parameters = DapperParam.AddParams(model, user);
                parameters.Add("ExecutionFlag", Data.Validate);
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                executionStatus = ResponseMessage.Invalid();
                await _sysLogger.SaveHRMSException(ex, user.Database, "LineBusiness", "ValidateLineAsync", user);
            }
            return executionStatus;
        }
    }
}
