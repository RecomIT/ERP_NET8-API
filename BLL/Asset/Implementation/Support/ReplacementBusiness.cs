using BLL.Base.Interface;
using Shared.OtherModels.Pagination;
using Shared.OtherModels.Response;
using Shared.OtherModels.User;
using Shared.Services;
using System.Data;
using Shared.Asset.Filter.Support;
using Shared.Asset.DTO.Support;
using Shared.Asset.ViewModel.Support;
using BLL.Asset.Interface.Support;
using DAL.DapperObject.Interface;



namespace BLL.Asset.Implementation.Support
{
    public class ReplacementBusiness : IReplacementBusiness
    {
        private readonly IDapperData _dapper;
        private readonly ISysLogger _sysLogger;
        public ReplacementBusiness(IDapperData dapper, ISysLogger sysLogger)
        {
            _dapper = dapper;
            _sysLogger = sysLogger;
        } 
        public async Task<DBResponse<ReplacementViewModel>> GetAssetAsync(Replacement_Filter filter, AppUser user)
        {
            DBResponse<ReplacementViewModel> data = new DBResponse<ReplacementViewModel>();
            DBResponse response = new DBResponse();
            try {
                var sp_name = "sp_Asset_Replacement_List";
                var parameters = DapperParam.AddParams(filter, user, addBaseProperty: true);       
                response = await _dapper.SqlQueryFirstAsync<DBResponse>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
                data.ListOfObject = Utility.JsonToObject<IEnumerable<ReplacementViewModel>>(response.JSONData) ?? new List<ReplacementViewModel>();
                data.Pageparam = new Pageparam() { PageNumber = response.PageNumber, PageSize = response.PageSize, TotalPages = response.TotalPages, TotalRows = response.TotalRows };
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ReplacementBusiness", "GetAssetAsync", user);
            }
            return data;
        }
        public async Task<ExecutionStatus> SaveReplacementAsync(Replacement_DTO model, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try {
                var sp_name = "sp_Asset_Replacement_Insert";
                var parameters = Utility.DappperParams(model, user);
                parameters.Add("ExecutionFlag", Data.Insert);
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex) {
                executionStatus = Utility.Invalid(ResponseMessage.InvalidExecution);
                await _sysLogger.SaveHRMSException(ex, user.Database, "ReplacementBusiness", "SaveReplacementAsync", user);
            }
            return executionStatus;
        }
        public async Task<ExecutionStatus> SaveReceivedAsync(Received_DTO model, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try {
                var sp_name = "sp_Asset_Received_Insert";
                var parameters = Utility.DappperParams(model, user);
                parameters.Add("ExecutionFlag", Data.Insert);
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex) {
                executionStatus = Utility.Invalid(ResponseMessage.InvalidExecution);
                await _sysLogger.SaveHRMSException(ex, user.Database, "ReplacementBusiness", "SaveReceivedAsync", user);
            }
            return executionStatus;
        }
        public async Task<ExecutionStatus> UpdateProductAsync(Replacement_DTO model, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try {
                var sp_name = "sp_Asset_Product_Update";
                var parameters = Utility.DappperParams(model, user);
                parameters.Add("ExecutionFlag", Data.Update);
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex) {
                executionStatus = Utility.Invalid(ResponseMessage.InvalidExecution);
                await _sysLogger.SaveHRMSException(ex, user.Database, "ReplacementBusiness", "UpdateProductAsync", user);
            }
            return executionStatus;
        }


    }
}
