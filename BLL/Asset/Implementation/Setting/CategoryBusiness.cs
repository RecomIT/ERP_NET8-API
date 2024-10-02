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
    public class CategoryBusiness : ICategoryBusiness
    {
        private readonly IDapperData _dapper;
        private readonly ISysLogger _sysLogger;
        public CategoryBusiness(IDapperData dapper, ISysLogger sysLogger)
        {
            _dapper = dapper;
            _sysLogger = sysLogger;
        }
        public async Task<IEnumerable<CategoryViewModel>> GetCategoryAsync(Category_Filter filter, AppUser user)
        {
            IEnumerable<CategoryViewModel> list = new List<CategoryViewModel>();
            try {
                var sp_name = "sp_Asset_Category_List";
                var parameters = Utility.DappperParams(filter, user); 
                list = await _dapper.SqlQueryListAsync<CategoryViewModel>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "CategoryBusiness", "GetCategoryAsync", user);
            }
            return list;
        }
        public async Task<ExecutionStatus> SaveCategoryAsync(Category_DTO model, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try {             

                var sp_name = "sp_Asset_Category_Insert_Update";
                var parameters = Utility.DappperParams(model, user);
                parameters.Add("ExecutionFlag", model.CategoryId > 0 ? Data.Update : Data.Insert);
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex) {
                executionStatus = Utility.Invalid(ResponseMessage.InvalidExecution);
                await _sysLogger.SaveHRMSException(ex, user.Database, "CategoryBusiness", "SaveCategoryAsync", user);
            }
            return executionStatus;
        }
        public async Task<ExecutionStatus> ValidatorCategoryAsync(Category_DTO model, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try {
                var sp_name = "sp_Asset_Category_Insert_Update";
                var parameters = Utility.DappperParams(model, user);
                parameters.Add("ExecutionFlag", Data.Validate);
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex) {
                executionStatus = Shared.Services.Utility.Invalid(ResponseMessage.InvalidExecution);
                await _sysLogger.SaveHRMSException(ex, user.Database, "CategoryBusiness", "ValidatorCategoryAsync", user);
            }
            return executionStatus;
        }

        public async Task<IEnumerable<Dropdown>> GetCategoryDropdownAsync(Category_Filter filter, AppUser user)
        {
            List<Dropdown> dropdowns = new List<Dropdown>();
            try {
                var list = await this.GetCategoryAsync(filter, user);
                foreach (var item in list) {
                    dropdowns.Add(new Dropdown() {
                        Id = item.CategoryId,
                        Value = item.CategoryId.ToString(),
                        Text = item.CategoryName.ToString()
                    });
                }
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "CategoryBusiness", "GetCategoryDropdownAsync", user);
            }
            return dropdowns;
        }

    }
}
