using BLL.Asset.Interface.Setting;
using BLL.Base.Interface;
using DAL.DapperObject.Interface;
using Shared.Asset.DTO.Setting;
using Shared.Asset.Filter.Setting;
using Shared.Asset.ViewModel.Setting;
using Shared.OtherModels.DataService;
using Shared.OtherModels.Response;
using Shared.OtherModels.User;
using Shared.Services;
using System.Data;


namespace BLL.Asset.Implementation.Setting
{
    public class SubCategoryBusiness : ISubCategoryBusiness
    {
        private readonly IDapperData _dapper;
        private readonly ISysLogger _sysLogger;
        public SubCategoryBusiness(IDapperData dapper, ISysLogger sysLogger)
        {
            _dapper = dapper;
            _sysLogger = sysLogger;
        }
        public async Task<IEnumerable<SubCategoryViewModel>> GetSubCategoryAsync(SubCategory_Filter filter, AppUser user)
        {
            IEnumerable<SubCategoryViewModel> list = new List<SubCategoryViewModel>();
            try {
                var sp_name = "sp_Asset_SubCategory_List";
                var parameters = Utility.DappperParams(filter, user);
                list = await _dapper.SqlQueryListAsync<SubCategoryViewModel>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "SubCategoryBusiness", "GetSubCategoryAsync", user);
            }
            return list;
        }
        public async Task<ExecutionStatus> SaveSubCategoryAsync(SubCategory_DTO model, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try {
                var sp_name = "sp_Asset_SubCategory_Insert_Update";
                var parameters = Utility.DappperParams(model, user);
                parameters.Add("ExecutionFlag", model.CategoryId > 0 ? Data.Update : Data.Insert);
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex) {
                executionStatus = Utility.Invalid(ResponseMessage.InvalidExecution);
                await _sysLogger.SaveHRMSException(ex, user.Database, "SubCategoryBusiness", "SaveSubCategoryAsync", user);
            }
            return executionStatus;
        }
        public async Task<ExecutionStatus> ValidatorSubCategoryAsync(SubCategory_DTO model, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try {
                var sp_name = "sp_Asset_SubCategory_Insert_Update";
                var parameters = Utility.DappperParams(model, user);
                parameters.Add("ExecutionFlag", Data.Validate);
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex) {
                executionStatus = Shared.Services.Utility.Invalid(ResponseMessage.InvalidExecution);
                await _sysLogger.SaveHRMSException(ex, user.Database, "SubCategoryBusiness", "ValidatorSubCategoryAsync", user);
            }
            return executionStatus;
        }
        public async Task<IEnumerable<SubCategoryViewModel>> SubCategoryDropdownAsync(SubCategory_Filter filter, AppUser user)
        {
            IEnumerable<SubCategoryViewModel> list = new List<SubCategoryViewModel>();
            try {
                var sp_name = $@"Select Distinct SubCategoryId,SubCategoryName=[Name] From Asset_SubCategory 
                Where 1=1 AND (@CategoryId IS NULL OR @CategoryId =0 OR CategoryId = @CategoryId) AND (CompanyId = @CompanyId) AND (OrganizationId = @OrganizationId)";
                var parameters = DapperParam.AddParams(filter, user);
                list = await _dapper.SqlQueryListAsync<SubCategoryViewModel>(user.Database, sp_name, parameters, CommandType.Text);
       
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "SubCategoryBusiness", "SubCategoryDropdownAsync", user);
            }
            return list;
        }
        public async Task<IEnumerable<Dropdown>> GetSubCategoryDropdownAsync(SubCategory_Filter filter, AppUser user)
        {
            List<Dropdown> dropdowns = new List<Dropdown>();
            try {
                var list = await this.SubCategoryDropdownAsync(filter, user);
                foreach (var item in list) {
                    dropdowns.Add(new Dropdown() {
                        Id = item.SubCategoryId,
                        Value = item.SubCategoryId.ToString(),
                        Text = item.SubCategoryName.ToString()
                    });
                }
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "SubCategoryBusiness", "GetSubCategoryDropdownAsync", user);
            }
            return dropdowns;
        }
    }
}
