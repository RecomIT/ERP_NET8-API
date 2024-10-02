using BLL.Asset.Interface.IT;
using BLL.Base.Interface;
using DAL.DapperObject.Interface;
using Shared.Asset.DTO.IT;
using Shared.Asset.Filter.IT;
using Shared.Asset.ViewModel.IT;
using Shared.OtherModels.Pagination;
using Shared.OtherModels.Response;
using Shared.OtherModels.User;
using Shared.Services;
using System.Data;


namespace BLL.Asset.Implementation.IT
{

    public class ITSupportBusiness : IITSupportBusiness
    {
        private readonly IDapperData _dapper;
        private readonly ISysLogger _sysLogger;
        public ITSupportBusiness(IDapperData dapper, ISysLogger sysLogger)
        {
            _dapper = dapper;
            _sysLogger = sysLogger;
        }
 
        public async Task<DBResponse<ITSupportViewModel>> GetAssetAsync(ITSupport_Filter filter, AppUser user)
        {
            DBResponse<ITSupportViewModel> data = new DBResponse<ITSupportViewModel>();
            DBResponse response = new DBResponse();
            try {
                var sp_name = "sp_Asset_IT_List";
                var parameters = DapperParam.AddParams(filter, user, addBaseProperty: true);  
                response = await _dapper.SqlQueryFirstAsync<DBResponse>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
                data.ListOfObject = Utility.JsonToObject<IEnumerable<ITSupportViewModel>>(response.JSONData) ?? new List<ITSupportViewModel>();
                data.Pageparam = new Pageparam() { PageNumber = response.PageNumber, PageSize = response.PageSize, TotalPages = response.TotalPages, TotalRows = response.TotalRows };
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ITSupportBusiness", "GetAssetAsync", user);
            }
            return data;
        }

        public async Task<ExecutionStatus> UpdateProductAsync(ITSupport_DTO model, AppUser user)
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
                await _sysLogger.SaveHRMSException(ex, user.Database, "ITSupportBusiness", "UpdateProductAsync", user);
            }
            return executionStatus;
        }


    }
}
