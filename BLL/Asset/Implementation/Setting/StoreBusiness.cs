

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
    public class StoreBusiness : IStoreBusiness
    {
        private readonly IDapperData _dapper;
        private readonly ISysLogger _sysLogger;
        public StoreBusiness(IDapperData dapper, ISysLogger sysLogger)
        {
            _dapper = dapper;
            _sysLogger = sysLogger;
        }
        public async Task<IEnumerable<StoreViewModel>> GetStoreAsync(Store_Filter filter, AppUser user)
        {
            IEnumerable<StoreViewModel> list = new List<StoreViewModel>();
            try {
                var sp_name = "sp_Asset_Store_List";
                var parameters = Utility.DappperParams(filter, user); 
                list = await _dapper.SqlQueryListAsync<StoreViewModel>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "StoreBusiness", "GetStoreAsync", user);
            }
            return list;
        }
        public async Task<ExecutionStatus> SaveStoreAsync(Store_DTO model, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try {             

                var sp_name = "sp_Asset_Store_Insert_Update";
                var parameters = Utility.DappperParams(model, user);
                parameters.Add("ExecutionFlag", model.StoreId > 0 ? Data.Update : Data.Insert);
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex) {
                executionStatus = Utility.Invalid(ResponseMessage.InvalidExecution);
                await _sysLogger.SaveHRMSException(ex, user.Database, "StoreBusiness", "SaveStoreAsync", user);
            }
            return executionStatus;
        }
        public async Task<ExecutionStatus> ValidatorStoreAsync(Store_DTO model, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try {
                var sp_name = "sp_Asset_Store_Insert_Update";
                var parameters = Utility.DappperParams(model, user);
                parameters.Add("ExecutionFlag", Data.Validate);
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex) {
                executionStatus = Shared.Services.Utility.Invalid(ResponseMessage.InvalidExecution);
                await _sysLogger.SaveHRMSException(ex, user.Database, "StoreBusiness", "ValidatorStoreAsync", user);
            }
            return executionStatus;
        }

        public async Task<IEnumerable<Dropdown>> GetStoreDropdownAsync(Store_Filter filter, AppUser user)
        {
            List<Dropdown> dropdowns = new List<Dropdown>();
            try {
                var list = await this.GetStoreAsync(filter, user);
                foreach (var item in list) {
                    dropdowns.Add(new Dropdown() {
                        Id = item.StoreId,
                        Value = item.StoreId.ToString(),
                        Text = item.StoreName.ToString()
                    });
                }
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "BrandBusiness", "GetBrandDropdownAsync", user);
            }
            return dropdowns;
        }

    }
}
