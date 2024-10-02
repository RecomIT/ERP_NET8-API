using System.Data;
using BLL.Base.Interface;
using BLL.Employee.Interface.Locational;
using DAL.DapperObject.Interface;
using Shared.Employee.DTO.Locational;
using Shared.Employee.Filter.Locational;
using Shared.Employee.ViewModel.Locational;
using Shared.OtherModels.Response;
using Shared.OtherModels.User;
using Shared.Services;

namespace BLL.Employee.Implementation.Locational
{
    public class LocationBusiness : ILocationBusiness
    {
        private readonly IDapperData _dapper;
        private readonly ISysLogger _sysLogger;

        public LocationBusiness(IDapperData dapper, ISysLogger sysLogger)
        {
            _dapper = dapper;
            _sysLogger = sysLogger;
        }
        public async Task<IEnumerable<LocationViewModel>> GetLocationsAsync(Location_Filter filter, AppUser user)
        {
            IEnumerable<LocationViewModel> list = new List<LocationViewModel>();
            try
            {
                var sp_name = "sp_HR_Location_List";
                var parameters = DapperParam.AddParams(filter, user);
                list = await _dapper.SqlQueryListAsync<LocationViewModel>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "LocationBusiness", "GetLocationsAsync", user);
            }
            return list;
        }

        public async Task<ExecutionStatus> SaveLocationAsync(LocationDTO model, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                var sp_name = string.Format(@"sp_HR_Location_Insert_Update");
                var parameters = DapperParam.AddParams(model, user);
                parameters.Add("ExecutionFlag", model.LocationId > 0 ? Data.Update : Data.Insert);
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                executionStatus = ResponseMessage.Invalid(ResponseMessage.InvalidParameters);
                await _sysLogger.SaveHRMSException(ex, user.Database, "LocationBusiness", "SaveLocationAsync", user);
            }
            return executionStatus;
        }

        public async Task<ExecutionStatus> ValidateLocationAsync(LocationDTO model, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                var sp_name = "sp_HR_Location_Insert_Update";
                var parameters = DapperParam.AddParams(model, user);
                parameters.Add("ExecutionFlag", model.LocationId > 0 ? Data.Update : Data.Insert);
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                executionStatus = ResponseMessage.Invalid(ResponseMessage.InvalidParameters);
                await _sysLogger.SaveHRMSException(ex, user.Database, "LocationBusiness", "ValidateLocationAsync", user);
            }
            return executionStatus;
        }
    }
}
