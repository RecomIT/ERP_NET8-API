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
    public class DistrictBusiness : IDistrictBusiness
    {
        private readonly IDapperData _dapper;
        private readonly ISysLogger _sysLogger;

        public DistrictBusiness(IDapperData dapper, ISysLogger sysLogger)
        {
            _dapper = dapper;
            _sysLogger = sysLogger;
        }

        public async Task<IEnumerable<DistrictViewModel>> GetDistrictsAsync(District_Filter filter, AppUser user)
        {
            IEnumerable<DistrictViewModel> list = new List<DistrictViewModel>();
            try
            {
                var sp_name = "sp_HR_District_List";
                var parameters = DapperParam.AddParams(filter, user);
                list = await _dapper.SqlQueryListAsync<DistrictViewModel>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "DistrictBusiness", "GetDistrictsAsync", user);
            }
            return list;
        }
        public async Task<ExecutionStatus> SaveDistrictAsync(DistrictDTO model, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                var sp_name = "sp_HR_District_Insert_Update";
                var parameters = DapperParam.AddParams(model, user);
                parameters.Add("ExecutionFlag", model.DistrictId > 0 ? Data.Update : Data.Insert);
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                executionStatus = ResponseMessage.Invalid(ResponseMessage.InvalidParameters);
                await _sysLogger.SaveHRMSException(ex, user.Database, "DistrictBusiness", "SaveDistrictAsync", user);
            }
            return executionStatus;
        }
        public async Task<ExecutionStatus> ValidateDistrictAsync(DistrictDTO model, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                var sp_name = "sp_HR_District_Insert_Update";
                var parameters = DapperParam.AddParams(model, user);
                parameters.Add("ExecutionFlag", model.DistrictId > 0 ? Data.Update : Data.Insert);
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                executionStatus = ResponseMessage.Invalid(ResponseMessage.InvalidParameters);
                await _sysLogger.SaveHRMSException(ex, user.Database, "DistrictBusiness", "ValidateDistrictAsync", user);
            }
            return executionStatus;
        }

        
    }
}
