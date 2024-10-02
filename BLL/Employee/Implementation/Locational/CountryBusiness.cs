using System.Data;
using Shared.Services;
using BLL.Base.Interface;
using Shared.OtherModels.User;
using DAL.DapperObject.Interface;
using Shared.OtherModels.Response;
using Shared.Employee.DTO.Locational;
using BLL.Employee.Interface.Locational;
using Shared.Employee.Filter.Locational;
using Shared.Employee.ViewModel.Locational;

namespace BLL.Employee.Implementation.Locational
{
    public class CountryBusiness : ICountryBusiness
    {
        private readonly IDapperData _dapper;
        private readonly ISysLogger _sysLogger;

        public CountryBusiness(IDapperData dapper, ISysLogger sysLogger)
        {
            _dapper = dapper;
            _sysLogger = sysLogger;
        }

        public async Task<IEnumerable<CountryViewModel>> GetCountriesAsync(Country_Filter filter, AppUser user)
        {
            IEnumerable<CountryViewModel> list = new List<CountryViewModel>();
            try
            {
                var sp_name = "sp_HR_Country_List";
                var parameters = DapperParam.AddParams(filter, user);
                list = await _dapper.SqlQueryListAsync<CountryViewModel>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "CountryBusiness", "GetCountriessAsync", user);
            }
            return list;
        }

        public async Task<ExecutionStatus> SaveCountryAsync(CountryDTO model, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                var sp_name = "sp_HR_Country_Insert_Update";
                var parameters = DapperParam.AddParams(model, user);
                parameters.Add("ExecutionFlag", model.CountryId > 0 ? Data.Update : Data.Insert);
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                executionStatus = ResponseMessage.Invalid(ResponseMessage.InvalidParameters);
                await _sysLogger.SaveHRMSException(ex, user.Database, "CountryBusiness", "SaveCountryAsync", user);
            }
            return executionStatus;
        }

        public async Task<ExecutionStatus> ValidateCountryAsync(CountryDTO model, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                var sp_name = "sp_HR_Country_Insert_Update";
                var parameters = DapperParam.AddParams(model, user);
                parameters.Add("ExecutionFlag", Data.Validate);
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                executionStatus = ResponseMessage.Invalid(ResponseMessage.InvalidParameters);
                await _sysLogger.SaveHRMSException(ex, user.Database, "CountryBusiness", "ValidateCountryAsync", user);
            }
            return executionStatus;
        }
    }
}
