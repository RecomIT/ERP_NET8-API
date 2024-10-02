using Dapper;
using System.Data;
using Shared.Services;
using BLL.Base.Interface;
using Shared.OtherModels.User;
using BLL.Salary.Bonus.Interface;
using Shared.OtherModels.Response;
using Shared.OtherModels.DataService;
using DAL.DapperObject.Interface;
using Shared.Payroll.ViewModel.Bonus;
using Shared.Payroll.Filter.Bonus;

namespace BLL.Salary.Bonus.Implementation
{
    public class BonusBusiness : IBonusBusiness
    {
        private readonly IDapperData _dapper;
        private readonly ISysLogger _sysLogger;
        public BonusBusiness(IDapperData dapper, ISysLogger sysLogger)
        {
            _dapper = dapper;
            _sysLogger = sysLogger;
        }
        public async Task<ExecutionStatus> SaveBonusAsync(BonusViewModel bonus, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                var sp_name = "sp_Payroll_Bonus";
                var parameters = new DynamicParameters();
                parameters.Add("BonusId", bonus.BonusId);
                parameters.Add("BonusName", bonus.BonusName);
                parameters.Add("IsActive", bonus.IsActive);

                parameters.Add("Remarks", bonus.Remarks);
                parameters.Add("UserId", user.UserId);
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("OrganizationId", user.OrganizationId);
                parameters.Add("ExecutionFlag", bonus.BonusId > 0 ? Data.Update : Data.Insert);
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                executionStatus = ResponseMessage.Invalid(ResponseMessage.SomthingWentWrong);
                await _sysLogger.SaveHRMSException(ex, user.Database, "BonusBusiness", "SaveBonusAsync", user);
            }
            return executionStatus;
        }
        public async Task<IEnumerable<BonusViewModel>> GetBonusesAsync(string bonusName, long? bonusId, AppUser user)
        {
            IEnumerable<BonusViewModel> bonusList = new List<BonusViewModel>();
            try
            {
                var sp_name = "sp_Payroll_Bonus";
                var parameters1 = new DynamicParameters();
                parameters1.Add("BonusName", bonusName ?? "");
                parameters1.Add("BonusId", bonusId ?? 0);
                parameters1.Add("companyId", user.CompanyId);
                parameters1.Add("organizationId", user.OrganizationId);
                parameters1.Add("executionFlag", Data.Read);
                bonusList = await _dapper.SqlQueryListAsync<BonusViewModel>(user.Database, sp_name, parameters1, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "BonusBusiness", "GetBonusesAsync", user);
            }
            return bonusList;
        }
        public async Task<ExecutionStatus> SaveBonusConfigAsync(BonusConfigViewModel bonus, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                var sp_name = "sp_Payroll_BonusConfig";
                var parameters = Utility.DappperParams(bonus, user, new string[] { "BonusName", "FiscalYearRange" }, addBaseProperty: false);
                parameters.Add("ExecutionFlag", bonus.BonusConfigId > 0 ? Data.Update : Data.Insert);
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                executionStatus = ResponseMessage.Invalid(ResponseMessage.SomthingWentWrong);
                await _sysLogger.SaveHRMSException(ex, user.Database, "BankBranchController", "SaveBonusConfigAsync", user);
            }
            return executionStatus;
        }
        public async Task<IEnumerable<BonusConfigViewModel>> GetBonusConfigsAsync(BonusQuery filter, AppUser user)
        {
            IEnumerable<BonusConfigViewModel> list = new List<BonusConfigViewModel>();
            try
            {
                var sp_name = "sp_Payroll_BonusConfig";
                var parameters = Utility.DappperParams(filter, user);
                parameters.Add("ExecutionFlag", Data.Read);
                list = await _dapper.SqlQueryListAsync<BonusConfigViewModel>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveSystemException(ex, user.Database, "BonusBusiness", "GetBonusConfigAsync", "", user.OrganizationId, user.CompanyId, 0);
            }
            return list;
        }
        public async Task<IEnumerable<Dropdown>> GetBonusExtensionAsync(AppUser user)
        {
            List<Dropdown> list = new List<Dropdown>();
            try
            {
                var items = await GetBonusesAsync(string.Empty, 0, user);
                foreach (var item in items)
                {
                    list.Add(new Dropdown()
                    {
                        Id = item.BonusId,
                        Value = item.BonusId.ToString(),
                        Text = item.BonusName.ToString(),
                    });
                }
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveSystemException(ex, user.Database, "BonusBusiness", "GetBonusExtensionAsync", "", user.OrganizationId, user.CompanyId, 0);
            }
            return list;
        }
        public async Task<IEnumerable<Dropdown>> GetBonusAndConfigInThisFiscalYearExtensionAsync(AppUser user)
        {
            IEnumerable<Dropdown> list = new List<Dropdown>();
            try
            {
                var sp_name = "sp_Payroll_BonusConfig_Extension";
                var parameters = new DynamicParameters();
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("OrganizationId", user.OrganizationId);
                parameters.Add("Flag", "Bonus&ConfigInThisFiscalYear");
                list = await _dapper.SqlQueryListAsync<Dropdown>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {

                await _sysLogger.SaveSystemException(ex, user.Database, "BonusBusiness", "GetBonusAndConfigInThisFiscalYearExtensionAsync", "", user.OrganizationId, user.CompanyId, 0);
            }
            return list;
        }

        //Added by Monzur 20-SEP-2023
        public async Task<IEnumerable<Select2Dropdown>> GetLFAYearlyAllowanceExtensionAsync(long? allowanceNameId, AppUser appUser)
        {
            IEnumerable<Select2Dropdown> data = new List<Select2Dropdown>();
            try
            {
                var sp_name = string.Format(@"sp_Payroll_BonusConfig_Extension");
                var parameters = new DynamicParameters();
                parameters.Add("AllowanceNameId", allowanceNameId);
                parameters.Add("CompanyId", appUser.CompanyId);
                parameters.Add("OrganizationId", appUser.OrganizationId);
                parameters.Add("Flag", "AllowanceName_Extension");
                if (!Utility.IsNullEmptyOrWhiteSpace(sp_name))
                {
                    data = await _dapper.SqlQueryListAsync<Select2Dropdown>(appUser.Database, sp_name, parameters, CommandType.StoredProcedure);
                }
                return data;
            }
            catch (Exception ex)
            {
                return data;
            }
        }
    }
}
