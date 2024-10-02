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
    public class DivisionBusiness : IDivisionBusiness
    {
        private readonly IDapperData _dapper;
        private readonly ISysLogger _sysLogger;

        public DivisionBusiness(IDapperData dapper, ISysLogger sysLogger)
        {
            _dapper = dapper;
            _sysLogger = sysLogger;
        }

        public async Task<IEnumerable<DivisionViewModel>> GetDivisionsAsync(Division_Filter filter, AppUser user)
        {
            IEnumerable<DivisionViewModel> list = new List<DivisionViewModel>();
            try
            {
                var sp_name = "sp_HR_Division_List";
                var parameters = Utility.DappperParams(filter, user);
                list = await _dapper.SqlQueryListAsync<DivisionViewModel>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "CountryBusiness", "GetCountriessAsync", user);
            }
            return list;
        }
        public async Task<ExecutionStatus> SaveDivisionAsync(DivisionDTO model, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                var sp_name = "sp_HR_Division_Insert_Update";
                var parameters = Utility.DappperParams(model, user);
                parameters.Add("ExecutionFlag", model.DivisionId > 0 ? Data.Update : Data.Insert);
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                executionStatus = ResponseMessage.Invalid(ResponseMessage.InvalidParameters);
                await _sysLogger.SaveHRMSException(ex, user.Database, "CountryBusiness", "SaveCountryAsync", user);
            }
            return executionStatus;
        }
        public async Task<ExecutionStatus> ValidateDivisionAsync(DivisionDTO model, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                var sp_name = "sp_HR_Division_Insert_Update";
                var parameters = Utility.DappperParams(model, user);
                parameters.Add("ExecutionFlag", model.DivisionId > 0 ? Data.Update : Data.Insert);
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                executionStatus = ResponseMessage.Invalid(ResponseMessage.InvalidParameters);
                await _sysLogger.SaveHRMSException(ex, user.Database, "CountryBusiness", "SaveCountryAsync", user);
            }
            return executionStatus;
        }
    }
}
