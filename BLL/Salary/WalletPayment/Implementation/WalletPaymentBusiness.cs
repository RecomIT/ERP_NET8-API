using Dapper;
using System.Data;
using Shared.Services;
using BLL.Base.Interface;
using Shared.OtherModels.User;
using DAL.DapperObject.Interface;
using Shared.OtherModels.Response;
using Shared.OtherModels.Pagination;
using Shared.OtherModels.DataService;
using Shared.Payroll.DTO.WalletPayment;
using BLL.Salary.WalletPayment.Interface;
using Shared.Payroll.Filter.WalletPayment;

namespace BLL.Salary.WalletPayment.Implementation
{
    public class WalletPaymentBusiness : IWalletPaymentBusiness
    {
        private IDapperData _dapper;
        private ISysLogger _sysLogger;
        private string sqlQuery;

        public WalletPaymentBusiness(IDapperData dapper, ISysLogger sysLogger)
        {
            _dapper = dapper;
            _sysLogger = sysLogger;
        }
        public async Task<IEnumerable<Select2Dropdown>> GetInternalDesignationExtensionAsync(long? internalDesignationId, AppUser user)
        {
            IEnumerable<Select2Dropdown> data = new List<Select2Dropdown>();
            try
            {
                sqlQuery = "sp_HR_InternalDesignations";
                var parameters = new DynamicParameters();
                parameters.Add("InternalDesignationId", internalDesignationId);
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("OrganizationId", user.OrganizationId);
                parameters.Add("ExecutionFlag", Data.Extension);
                if (!Utility.IsNullEmptyOrWhiteSpace(sqlQuery))
                {
                    data = await _dapper.SqlQueryListAsync<Select2Dropdown>(user.Database, sqlQuery, parameters, CommandType.StoredProcedure);
                }
                return data;
            }
            catch (Exception ex)
            {
                return data;
            }
        }
        public async Task<ExecutionStatus> ValidateWalletPaymentAsync(List<WalletPaymentConfigurationDTO> configurationDTOs, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            executionStatus.Status = true;
            executionStatus.Errors = new Dictionary<string, string>();
            try
            {
                sqlQuery = "sp_Payroll_WalletPaymentConfiguration_Insert_Update";
                string duplicate = string.Empty;
                int itemCount = 0;
                foreach (var item in configurationDTOs)
                {
                    var parameters = Utility.DappperParams(item, user, new string[] { "InternalDesignationId" });
                    parameters.Add("CompanyId", user.CompanyId);
                    parameters.Add("OrganizationId", user.OrganizationId);
                    parameters.Add("ExecutionFlag", Data.Validate);

                    var value = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sqlQuery, parameters, CommandType.StoredProcedure);

                    if (value.Status == false)
                    {
                        executionStatus.Status = false;
                        itemCount = itemCount + 1;
                        executionStatus.Errors.Add(item.InternalDesignationId.ToString(), value.ErrorMsg);
                    }
                }
            }
            catch (Exception ex)
            {
                executionStatus = Utility.Invalid(ResponseMessage.InvalidExecution);
                await _sysLogger.SavePayrollException(ex, user.Database, "WalletPaymentBusiness", "ValidateWalletPaymentAsync", user);
            }
            return executionStatus;
        }
        public async Task<ExecutionStatus> SaveWalletPaymentConfigurationsAsync(List<WalletPaymentConfigurationDTO> configurationDTOs, AppUser user)
        {
            ExecutionStatus executionStatus = null;
            try
            {
                sqlQuery = "sp_Payroll_WalletPaymentConfiguration_Insert_Update";
                var jsonData = Utility.JsonData(configurationDTOs);
                var parameters = new DynamicParameters();
                parameters.Add("JsonData", jsonData);
                parameters.Add("UserId", user.ActionUserId);
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("OrganizationId", user.OrganizationId);
                parameters.Add("BranchId", user.BranchId);
                parameters.Add("ExecutionFlag", Data.Insert);
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sqlQuery, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "WalletPaymentBusiness", "SaveWalletPaymentConfigurationsAsync", user);
            }
            return executionStatus;
        }
        public async Task<DBResponse<WalletPaymentConfigurationViewModel>> GetWalletPaymentConfigurationsAsync(WalletPaymentConfiguration_Filter filter, AppUser user)
        {
            DBResponse<WalletPaymentConfigurationViewModel> data = new DBResponse<WalletPaymentConfigurationViewModel>();
            DBResponse response = new DBResponse();
            try
            {
                sqlQuery = "sp_Payroll_WalletPaymentConfiguration_List";
                var parameters = Utility.DappperParams(filter, user, addBaseProperty: true);
                response = await _dapper.SqlQueryFirstAsync<DBResponse>(user.Database, sqlQuery, parameters, CommandType.StoredProcedure);
                data.ListOfObject = Utility.JsonToObject<IEnumerable<WalletPaymentConfigurationViewModel>>(response.JSONData) ?? new List<WalletPaymentConfigurationViewModel>();
                data.Pageparam = new Pageparam() { PageNumber = response.PageNumber, PageSize = response.PageSize, TotalPages = response.TotalPages, TotalRows = response.TotalRows };
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "WalletPaymentBusiness", "GetWalletPaymentConfigurationsAsync", user);
            }
            return data;
        }
        public async Task<IEnumerable<WalletPaymentConfigurationViewModel>> GetWalletPaymentConfigByIdAsync(long walletConfigId, AppUser user)
        {
            IEnumerable<WalletPaymentConfigurationViewModel> data = new List<WalletPaymentConfigurationViewModel>();
            try
            {
                sqlQuery = "sp_Payroll_WalletPaymentConfiguration_Insert_Update";
                var parameters = new DynamicParameters();
                parameters.Add("WalletConfigId", walletConfigId);
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("OrganizationId", user.OrganizationId);
                parameters.Add("UserId", user.UserId);
                parameters.Add("ExecutionFlag", Data.Read);

                if (!Utility.IsNullEmptyOrWhiteSpace(sqlQuery))
                {
                    data = await _dapper.SqlQueryListAsync<WalletPaymentConfigurationViewModel>(user.Database, sqlQuery, parameters, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
            }
            return data;
        }
        public async Task<ExecutionStatus> UpdateWalletPaymentConfigurationsAsync(WalletPaymentConfigurationDTO dTO, AppUser user)
        {
            ExecutionStatus executionStatus = null;
            try
            {
                sqlQuery = "sp_Payroll_WalletPaymentConfiguration_Insert_Update";
                dTO.COCInWalletTransfer = Math.Round((decimal)(dTO.WalletFlatAmount / 100 * dTO.COCInWalletTransferPercentage));
                var parameters = Utility.DappperParams(dTO, user);
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("OrganizationId", user.OrganizationId);
                parameters.Add("BranchId", user.BranchId);
                parameters.Add("ExecutionFlag", Data.Update);
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sqlQuery, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "WalletPaymentBusiness", "UpdateWalletPaymentConfigurationsAsync", user);
            }
            return executionStatus;
        }
    }
}
