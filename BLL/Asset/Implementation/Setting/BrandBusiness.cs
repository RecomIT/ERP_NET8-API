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
    public class BrandBusiness : IBrandBusiness
    {
        private readonly IDapperData _dapper;
        private readonly ISysLogger _sysLogger;
        public BrandBusiness(IDapperData dapper, ISysLogger sysLogger)
        {
            _dapper = dapper;
            _sysLogger = sysLogger;
        }
        public async Task<IEnumerable<BrandViewModel>> GetBrandAsync(Brand_Filter filter, AppUser user)
        {
            IEnumerable<BrandViewModel> list = new List<BrandViewModel>();
            try {
                var sp_name = "sp_Asset_Brand_List";
                var parameters = Utility.DappperParams(filter, user);
                list = await _dapper.SqlQueryListAsync<BrandViewModel>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "BrandBusiness", "GetBrandAsync", user);
            }
            return list;
        }
        public async Task<ExecutionStatus> SaveBrandAsync(Brand_DTO model, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try {
                var sp_name = "sp_Asset_Brand_Insert_Update";
                var parameters = Utility.DappperParams(model, user);
                parameters.Add("ExecutionFlag", model.BrandId > 0 ? Data.Update : Data.Insert);
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex) {
                executionStatus = Utility.Invalid(ResponseMessage.InvalidExecution);
                await _sysLogger.SaveHRMSException(ex, user.Database, "BrandBusiness", "SaveBrandAsync", user);
            }
            return executionStatus;
        }
        public async Task<ExecutionStatus> ValidatorBrandAsync(Brand_DTO model, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try {
                var sp_name = "sp_Asset_Brand_Insert_Update";
                var parameters = Utility.DappperParams(model, user);
                parameters.Add("ExecutionFlag", Data.Validate);
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex) {
                executionStatus = Shared.Services.Utility.Invalid(ResponseMessage.InvalidExecution);
                await _sysLogger.SaveHRMSException(ex, user.Database, "BrandBusiness", "ValidatorBrandAsync", user);
            }
            return executionStatus;
        }

        public async Task<IEnumerable<Dropdown>> GetBrandDropdownAsync(Brand_Filter filter, AppUser user)
        {
            List<Dropdown> dropdowns = new List<Dropdown>();
            try {
                var list = await this.BrandDropdownAsync(filter, user);
                foreach (var item in list) {
                    dropdowns.Add(new Dropdown() {
                        Id = item.BrandId,
                        Value = item.BrandId.ToString(),
                        Text = item.BrandName.ToString()
                    });
                }
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "BrandBusiness", "GetBrandDropdownAsync", user);
            }
            return dropdowns;
        }

        public async Task<IEnumerable<BrandViewModel>> BrandDropdownAsync(Brand_Filter filter, AppUser user)
        {
            IEnumerable<BrandViewModel> list = new List<BrandViewModel>();
            try {
                var sp_name = $@"Select Distinct BrandId,BrandName=[Name] From Asset_Brand
                Where 1=1 AND (@SubCategoryId IS NULL OR @SubCategoryId = 0 OR SubCategoryId = @SubCategoryId)  AND (CompanyId = @CompanyId) AND (OrganizationId = @OrganizationId)";
                var parameters = DapperParam.AddParams(filter, user);
                list = await _dapper.SqlQueryListAsync<BrandViewModel>(user.Database, sp_name, parameters, CommandType.Text);

            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "BrandBusiness", "BrandDropdownAsync", user);
            }
            return list;
        }
    }
}
