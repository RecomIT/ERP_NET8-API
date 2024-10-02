
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
    public class VendorBusiness : IVendorBusiness
    {
        private readonly IDapperData _dapper;
        private readonly ISysLogger _sysLogger;
        public VendorBusiness(IDapperData dapper, ISysLogger sysLogger)
        {
            _dapper = dapper;
            _sysLogger = sysLogger;
        }
        public async Task<IEnumerable<VendorViewModel>> GetVendorAsync(Vendor_Filter filter, AppUser user)
        {
            IEnumerable<VendorViewModel> list = new List<VendorViewModel>();
            try {
                var sp_name = "sp_Asset_Vendor_List";
                var parameters = Utility.DappperParams(filter, user); 
                list = await _dapper.SqlQueryListAsync<VendorViewModel>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "VendorBusiness", "GetVendorAsync", user);
            }
            return list;
        }
        public async Task<ExecutionStatus> SaveVendorAsync(Vendor_DTO model, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try {             

                var sp_name = "sp_Asset_Vendor_Insert_Update";
                var parameters = Utility.DappperParams(model, user);
                parameters.Add("ExecutionFlag", model.VendorId > 0 ? Data.Update : Data.Insert);
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex) {
                executionStatus = Utility.Invalid(ResponseMessage.InvalidExecution);
                await _sysLogger.SaveHRMSException(ex, user.Database, "VendorBusiness", "SaveVendorAsync", user);
            }
            return executionStatus;
        }
        public async Task<ExecutionStatus> ValidatorVendorAsync(Vendor_DTO model, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try {
                var sp_name = "sp_Asset_Vendor_Insert_Update";
                var parameters = Utility.DappperParams(model, user);
                parameters.Add("ExecutionFlag", Data.Validate);
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex) {
                executionStatus = Shared.Services.Utility.Invalid(ResponseMessage.InvalidExecution);
                await _sysLogger.SaveHRMSException(ex, user.Database, "VendorBusiness", "ValidatorVendorAsync", user);
            }
            return executionStatus;
        }

        public async Task<IEnumerable<Dropdown>> GetVendorDropdownAsync(Vendor_Filter filter, AppUser user)
        {
            List<Dropdown> dropdowns = new List<Dropdown>();
            try {
                var list = await this.GetVendorAsync(filter, user);
                foreach (var item in list) {
                    dropdowns.Add(new Dropdown() {
                        Id = item.VendorId,
                        Value = item.VendorId.ToString(),
                        Text = item.Name.ToString()
                    });
                }
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "VendorBusiness", "GetVendorDropdownAsync", user);
            }
            return dropdowns;
        }

    }
}
