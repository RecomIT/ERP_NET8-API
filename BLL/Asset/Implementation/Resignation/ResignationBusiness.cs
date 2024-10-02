
using BLL.Asset.Interface.Resignation;
using BLL.Base.Interface;
using DAL.DapperObject.Interface;
using Shared.Asset.DTO.Resignation;
using Shared.Asset.Filter.Resignation;
using Shared.Asset.ViewModel.Resignation;
using Shared.OtherModels.Pagination;
using Shared.OtherModels.Response;
using Shared.OtherModels.User;
using Shared.Services;
using System.Data;




namespace BLL.Asset.Implementation.Resignation
{
    public class ResignationBusiness : IResignationBusiness
    {
        private readonly IDapperData _dapper;
        private readonly ISysLogger _sysLogger;
        public ResignationBusiness(IDapperData dapper, ISysLogger sysLogger)
        {
            _dapper = dapper;
            _sysLogger = sysLogger;
        }
        public async Task<DBResponse<ResignationViewModel>> GetEmployeeResignationAsync(Resignation_Filter filter, AppUser user)
        {
            DBResponse<ResignationViewModel> data = new DBResponse<ResignationViewModel>();
            DBResponse response = new DBResponse();
            try {
                var sp_name = "sp_Asset_Employee_Resignation_List";
                var parameters = DapperParam.AddParams(filter, user, addBaseProperty: true);
                response = await _dapper.SqlQueryFirstAsync<DBResponse>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
                data.ListOfObject = Utility.JsonToObject<IEnumerable<ResignationViewModel>>(response.JSONData) ?? new List<ResignationViewModel>();
                data.Pageparam = new Pageparam() { PageNumber = response.PageNumber, PageSize = response.PageSize, TotalPages = response.TotalPages, TotalRows = response.TotalRows };
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ResignationBusiness", "GetEmployeeResignationAsync", user);
            }
            return data;
        }

        public async Task<DBResponse<AssetListViewModel>> GetAssignedDataAsync(AssetList_Filter filter, AppUser user)
        {
            DBResponse<AssetListViewModel> data = new DBResponse<AssetListViewModel>();
            DBResponse response = new DBResponse();
            try {
                var sp_name = "sp_Asset_Employee_List";
                var parameters = DapperParam.AddParams(filter, user, addBaseProperty: true);
                parameters.Add("ExecutionFlag", "Assigning");
                response = await _dapper.SqlQueryFirstAsync<DBResponse>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
                data.ListOfObject = Utility.JsonToObject<IEnumerable<AssetListViewModel>>(response.JSONData) ?? new List<AssetListViewModel>();
                data.Pageparam = new Pageparam() { PageNumber = response.PageNumber, PageSize = response.PageSize, TotalPages = response.TotalPages, TotalRows = response.TotalRows };
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ResignationBusiness", "GetAssignedDataAsync", user);
            }
            return data;
        }

        public async Task<ExecutionStatus> SaveAssetAsync(Resignation_DTO model, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try {
                var sp_name = "sp_Asset_Resignation_Insert";
                var parameters = Utility.DappperParams(model, user);
                parameters.Add("ExecutionFlag", Data.Insert);
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex) {
                executionStatus = Utility.Invalid(ResponseMessage.InvalidExecution);
                await _sysLogger.SaveHRMSException(ex, user.Database, "ResignationBusiness", "SaveAssetAsync", user);
            }
            return executionStatus;
        }

    }
}
