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
    public class ServicingBusiness : IServicingBusiness
    {
        private readonly IDapperData _dapper;
        private readonly ISysLogger _sysLogger;
        public ServicingBusiness(IDapperData dapper, ISysLogger sysLogger)
        {
            _dapper = dapper;
            _sysLogger = sysLogger;
        } 
        public async Task<DBResponse<ServicingViewModel>> GetServicingDataAsync(Servicing_Filter filter, AppUser user)
        {
            DBResponse<ServicingViewModel> data = new DBResponse<ServicingViewModel>();
            DBResponse response = new DBResponse();
            try {
                var sp_name = "sp_Asset_Servicing_List";
                var parameters = DapperParam.AddParams(filter, user, addBaseProperty: true);       
                response = await _dapper.SqlQueryFirstAsync<DBResponse>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
                data.ListOfObject = Utility.JsonToObject<IEnumerable<ServicingViewModel>>(response.JSONData) ?? new List<ServicingViewModel>();
                data.Pageparam = new Pageparam() { PageNumber = response.PageNumber, PageSize = response.PageSize, TotalPages = response.TotalPages, TotalRows = response.TotalRows };
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ServicingBusiness", "GetServicingDataAsync", user);
            }
            return data;
        }
        public async Task<ExecutionStatus> SaveServicingAsync(Servicing_DTO model, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try {
                var sp_name = "sp_Asset_Servicing_Insert";
                var parameters = Utility.DappperParams(model, user);
                parameters.Add("ExecutionFlag", Data.Insert);
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex) {
                executionStatus = Utility.Invalid(ResponseMessage.InvalidExecution);
                await _sysLogger.SaveHRMSException(ex, user.Database, "ServicingBusiness", "SaveServicingAsync", user);
            }
            return executionStatus;
        }
        public async Task<DBResponse<ReceivedViewModel>> GetReceivedAssetAsync(Servicing_Filter filter, AppUser user)
        {
            DBResponse<ReceivedViewModel> data = new DBResponse<ReceivedViewModel>();
            DBResponse response = new DBResponse();
            try {
                var sp_name = "sp_Asset_Received_List";
                var parameters = DapperParam.AddParams(filter, user, addBaseProperty: true);                
                response = await _dapper.SqlQueryFirstAsync<DBResponse>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
                data.ListOfObject = Utility.JsonToObject<IEnumerable<ReceivedViewModel>>(response.JSONData) ?? new List<ReceivedViewModel>();
                data.Pageparam = new Pageparam() { PageNumber = response.PageNumber, PageSize = response.PageSize, TotalPages = response.TotalPages, TotalRows = response.TotalRows };
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ResignationBusiness", "GetReceivedAssetAsync", user);
            }
            return data;
        }

        

    }
}
