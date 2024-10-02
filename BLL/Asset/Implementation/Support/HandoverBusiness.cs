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
    public class HandoverBusiness : IHandoverBusiness
    {
        private readonly IDapperData _dapper;
        private readonly ISysLogger _sysLogger;
        public HandoverBusiness(IDapperData dapper, ISysLogger sysLogger)
        {
            _dapper = dapper;
            _sysLogger = sysLogger;
        } 
        public async Task<DBResponse<HandoverViewModel>> GetHandoverDataAsync(Handover_Filter filter, AppUser user)
        {
            DBResponse<HandoverViewModel> data = new DBResponse<HandoverViewModel>();
            DBResponse response = new DBResponse();
            try {
                var sp_name = "sp_Asset_Handover_List";
                var parameters = DapperParam.AddParams(filter, user, addBaseProperty: true);       
                response = await _dapper.SqlQueryFirstAsync<DBResponse>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
                data.ListOfObject = Utility.JsonToObject<IEnumerable<HandoverViewModel>>(response.JSONData) ?? new List<HandoverViewModel>();
                data.Pageparam = new Pageparam() { PageNumber = response.PageNumber, PageSize = response.PageSize, TotalPages = response.TotalPages, TotalRows = response.TotalRows };
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "HandoverBusiness", "GetHandoverDataAsync", user);
            }
            return data;
        }
        public async Task<ExecutionStatus> SaveHandoverAsync(Handover_DTO model, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try {
                var sp_name = "sp_Asset_Handover_Insert";
                var parameters = Utility.DappperParams(model, user);
                parameters.Add("ExecutionFlag", Data.Insert);
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex) {
                executionStatus = Utility.Invalid(ResponseMessage.InvalidExecution);
                await _sysLogger.SaveHRMSException(ex, user.Database, "HandoverBusiness", "SaveHandoverAsync", user);
            }
            return executionStatus;
        }       


        public async Task<IEnumerable<HandoverViewModel>> GetAssignedDataAsync(Handover_Filter filter, AppUser user)
        {
            IEnumerable<HandoverViewModel> list = new List<HandoverViewModel>();
            try {
                var sp_name = @"Select TransactionDate,Category,SubCategory,Brand,AssetName,ProductId,Condition,Number,WarrantyDate,Remarks,Type,AssetId
                From Vw_Asset_Employee_Handover_List
	            Where 1=1
	            AND (@EmployeeId IS NULL OR @EmployeeId=0 OR EmployeeId=@EmployeeId)
	            AND CompanyId=@CompanyId
	            AND OrganizationId=@OrganizationId
	            Order By EmployeeId";
                var parameters = DapperParam.AddParams(filter, user, addUserId: false);
                list = await _dapper.SqlQueryListAsync<HandoverViewModel>(user.Database, sp_name.Trim(), parameters, CommandType.Text);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "HandoverBusiness", "GetAssignedDataAsync", user);
            }
            return list;
        }

    }
}
