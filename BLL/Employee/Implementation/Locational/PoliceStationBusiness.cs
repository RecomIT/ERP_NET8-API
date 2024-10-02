using BLL.Base.Interface;
using BLL.Employee.Interface.Locational;
using DAL.DapperObject.Interface;
using Shared.Employee.DTO.Locational;
using Shared.Employee.Filter.Locational;
using Shared.Employee.ViewModel.Locational;
using Shared.OtherModels.Response;
using Shared.OtherModels.User;
using Shared.Services;
using System.Data;

namespace BLL.Employee.Implementation.Locational
{
    public class PoliceStationBusiness : IPoliceStationBusiness
    {
        private readonly IDapperData _dapper;
        private readonly ISysLogger _sysLogger;

        public PoliceStationBusiness(IDapperData dapper, ISysLogger sysLogger)
        {
            _dapper = dapper;
            _sysLogger = sysLogger;
        }

        public async Task<IEnumerable<PoliceStationViewModel>> GetPoliceStationsAsync(PoliceStation_Filter filter, AppUser user)
        {
            IEnumerable<PoliceStationViewModel> list = new List<PoliceStationViewModel>();
            try
            {
                var sp_name = "sp_HR_PoliceStation_List";
                var parameters = DapperParam.AddParams(filter, user);
                list = await _dapper.SqlQueryListAsync<PoliceStationViewModel>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "PoliceStationBusiness", "GetPoliceStationsAsync", user);
            }
            return list;
        }

        public async Task<ExecutionStatus> SavePoliceStationAsync(PoliceStationDTO model, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                var sp_name = "sp_HR_PoliceStation_Insert_Update";
                var parameters = DapperParam.AddParams(model, user);
                parameters.Add("ExecutionFlag", model.PoliceStationId > 0 ? Data.Update : Data.Insert);
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                executionStatus = ResponseMessage.Invalid(ResponseMessage.InvalidParameters);
                await _sysLogger.SaveHRMSException(ex, user.Database, "PoliceStationBusiness", "SavePoliceStationAsync", user);
            }
            return executionStatus;
        }

        public async Task<ExecutionStatus> ValidatePoliceStationAsync(PoliceStationDTO model, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                var sp_name = "sp_HR_PoliceStation_Insert_Update";
                var parameters = DapperParam.AddParams(model, user);
                parameters.Add("ExecutionFlag", model.PoliceStationId > 0 ? Data.Update : Data.Insert);
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                executionStatus = ResponseMessage.Invalid(ResponseMessage.InvalidParameters);
                await _sysLogger.SaveHRMSException(ex, user.Database, "PoliceStationBusiness", "ValidatePoliceStationAsync", user);
            }
            return executionStatus;
        }
    }
}
