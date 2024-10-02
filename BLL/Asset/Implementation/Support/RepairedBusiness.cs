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
    public class RepairedBusiness : IRepairedBusiness
    {
        private readonly IDapperData _dapper;
        private readonly ISysLogger _sysLogger;
        public RepairedBusiness(IDapperData dapper, ISysLogger sysLogger)
        {
            _dapper = dapper;
            _sysLogger = sysLogger;
        } 
        public async Task<DBResponse<RepairedViewModel>> GetRepairedDataAsync(Servicing_Filter filter, AppUser user)
        {
            DBResponse<RepairedViewModel> data = new DBResponse<RepairedViewModel>();
            DBResponse response = new DBResponse();
            try {
                var sp_name = "sp_Asset_Repaired_List";
                var parameters = DapperParam.AddParams(filter, user, addBaseProperty: true);       
                response = await _dapper.SqlQueryFirstAsync<DBResponse>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
                data.ListOfObject = Utility.JsonToObject<IEnumerable<RepairedViewModel>>(response.JSONData) ?? new List<RepairedViewModel>();
                data.Pageparam = new Pageparam() { PageNumber = response.PageNumber, PageSize = response.PageSize, TotalPages = response.TotalPages, TotalRows = response.TotalRows };
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "RepairedBusiness", "GetRepairedDataAsync", user);
            }
            return data;
        }
        public async Task<ExecutionStatus> SaveRepairedAsync(Repaired_DTO model, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try {
                var sp_name = "sp_Asset_Repaired_Insert";
                var parameters = Utility.DappperParams(model, user);
                parameters.Add("ExecutionFlag", Data.Insert);
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex) {
                executionStatus = Utility.Invalid(ResponseMessage.InvalidExecution);
                await _sysLogger.SaveHRMSException(ex, user.Database, "RepairedBusiness", "SaveRepairedAsync", user);
            }
            return executionStatus;
        }
        public async Task<DBResponse<ServicingViewModel>> GetServicingAssetAsync(Servicing_Filter filter, AppUser user)
        {
            DBResponse<ServicingViewModel> data = new DBResponse<ServicingViewModel>();
            DBResponse response = new DBResponse();
            try {
                var sp_name = "sp_Asset_Repair_List";
                var parameters = DapperParam.AddParams(filter, user, addBaseProperty: true);                
                response = await _dapper.SqlQueryFirstAsync<DBResponse>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
                data.ListOfObject = Utility.JsonToObject<IEnumerable<ServicingViewModel>>(response.JSONData) ?? new List<ServicingViewModel>();
                data.Pageparam = new Pageparam() { PageNumber = response.PageNumber, PageSize = response.PageSize, TotalPages = response.TotalPages, TotalRows = response.TotalRows };
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ResignationBusiness", "GetServicingAssetAsync", user);
            }
            return data;
        }

        

    }
}
