using AutoMapper;
using BLL.Base.Interface;
using BLL.Tax.Interface;
using DAL.DapperObject;
using DAL.DapperObject.Interface;
using Dapper;
using Shared.OtherModels.Response;
using Shared.OtherModels.User;
using Shared.Payroll.Domain.Tax;
using Shared.Payroll.DTO.Tax;
using Shared.Payroll.Process.Tax;
using Shared.Payroll.ViewModel.Tax;
using Shared.Services;
using System.Data;

namespace BLL.Tax.Implementation
{
    public class TaxSettingBusiness : ITaxSettingBusiness
    {
        private string sqlQuery = null;
        private readonly IDapperData _dapper;
        private readonly IMapper _mapper;
        private readonly ISysLogger _sysLogger;

        public TaxSettingBusiness(IDapperData dapper, IMapper mapper, ISysLogger sysLogger)
        {
            _dapper = dapper;
            _mapper = mapper;
            _sysLogger = sysLogger;
        }

        public async Task<ExecutionStatus> ValidateIncomeTaxSettingAsync(TaxSettingDTO setting, AppUser user)
        {
            ExecutionStatus executionStatus = null;
            try
            {
                sqlQuery = string.Format(@"sp_Payroll_TaxSetting");
                var parameter = Utility.DappperParams(setting.IncomeTaxSetting, user);
                parameter.Add("ExecutionFlag", Data.Validate);

                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sqlQuery, parameter, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "TaxBusiness", "ValidateIncomeTaxSetting", user);
            }
            return executionStatus;
        }

        public async Task<ExecutionStatus> SaveTaxSettingAsync(TaxSettingDTO setting, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                sqlQuery = string.Format(@"sp_Payroll_TaxSetting");
                var exemptionSettingJSON = Utility.JsonData(setting.TaxExemptionSettings);
                var taxInvestmentSettingJSON = Utility.JsonData(setting.TaxInvestmentSettings);
                var parameters = Utility.DappperParams(setting.IncomeTaxSetting, user);
                parameters.Add("ExemptionSettingJSON", exemptionSettingJSON);
                parameters.Add("TaxInvestmentSettingJSON", taxInvestmentSettingJSON);
                parameters.Add("ExecutionFlag", setting.IncomeTaxSetting.IncomeTaxSettingId > 0 ? Data.Update : Data.Insert);

                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sqlQuery, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                executionStatus = Utility.Invalid(ResponseMessage.InvalidExecution);
                await _sysLogger.SaveSystemException(ex, Database.ControlPanel, "TaxBusiness", "SaveTaxSettingAsync", user.UserId, user.OrganizationId, user.CompanyId, 0);
            }
            return executionStatus;
        }
        public async Task<IEnumerable<IncomeTaxSettingViewModel>> GetTaxSettingsAsync(long? IncomeTaxSettingId, long? FiscalYearId, string ImpliedCondition, AppUser user)
        {
            IEnumerable<IncomeTaxSettingViewModel> data = new List<IncomeTaxSettingViewModel>();
            try
            {
                sqlQuery = string.Format(@"sp_Payroll_TaxSetting");
                var parameters = new DynamicParameters();
                parameters.Add("IncomeTaxSettingId", IncomeTaxSettingId ?? 0);
                parameters.Add("FiscalYearId", FiscalYearId ?? 0);
                parameters.Add("ImpliedCondition", ImpliedCondition);
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("OrganizationId", user.OrganizationId);
                parameters.Add("ExecutionFlag", Data.Read);

                data = await _dapper.SqlQueryListAsync<IncomeTaxSettingViewModel>(user.Database, sqlQuery, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveSystemException(ex, Database.Payroll, "TaxBusiness", "GetTaxSettingsAsync", "", user.OrganizationId, user.CompanyId, 0);
            }
            return data;
        }
        public async Task<TaxSetting> GetTaxSettingAsync(long IncomeTaxSettingId, AppUser user)
        {
            TaxSetting taxSetting = new TaxSetting();
            try
            {
                sqlQuery = string.Format(@"sp_Payroll_TaxSetting");
                var IncomeTaxSettingParam = new DynamicParameters();
                IncomeTaxSettingParam.Add("IncomeTaxSettingId", IncomeTaxSettingId);
                IncomeTaxSettingParam.Add("CompanyId", user.CompanyId);
                IncomeTaxSettingParam.Add("OrganizationId", user.OrganizationId);
                IncomeTaxSettingParam.Add("ExecutionFlag", "R");
                taxSetting.IncomeTaxSetting = await _dapper.SqlQueryFirstAsync<IncomeTaxSettingViewModel>(user.Database, sqlQuery, IncomeTaxSettingParam, CommandType.StoredProcedure);

                var ExemptionsParam = new DynamicParameters();
                ExemptionsParam.Add("IncomeTaxSettingId", IncomeTaxSettingId);
                ExemptionsParam.Add("CompanyId", user.CompanyId);
                ExemptionsParam.Add("OrganizationId", user.OrganizationId);
                ExemptionsParam.Add("ExecutionFlag", "Exemptions");

                taxSetting.TaxExemptionSettings = (await _dapper.SqlQueryListAsync<TaxExemptionSettingViewModel>(user.Database, sqlQuery, ExemptionsParam, CommandType.StoredProcedure)).ToList();

                var InvestmentsParam = new DynamicParameters();
                InvestmentsParam.Add("IncomeTaxSettingId", IncomeTaxSettingId);
                InvestmentsParam.Add("CompanyId", user.CompanyId);
                InvestmentsParam.Add("OrganizationId", user.OrganizationId);
                InvestmentsParam.Add("ExecutionFlag", "Investments");

                taxSetting.TaxInvestmentSettings = (await _dapper.SqlQueryListAsync<TaxInvestmentSettingViewModel>(user.Database, sqlQuery, InvestmentsParam, CommandType.StoredProcedure)).ToList();
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveSystemException(ex, Database.ControlPanel, "TaxBusiness", "GetTaxSettingAsync", "", user.OrganizationId, user.CompanyId, 0);
            }
            return taxSetting;
        }

        public async Task<TaxSettingInTaxProcess> GetTaxSettingByFiscalYearIdAsync(long FiscalYearId, AppUser user)
        {
            TaxSettingInTaxProcess taxSetting = new TaxSettingInTaxProcess();
            try
            {
                var query = $@"Select setting.*
                From Payroll_IncomeTaxSetting setting
                Inner Join Payroll_FiscalYear fs On setting.FiscalYearId = fs.FiscalYearId
                Where 1=1
                AND (setting.FiscalYearId=@FiscalYearId)
                AND setting.CompanyId=@CompanyId
                AND setting.OrganizationId=@OrganizationId";

                taxSetting.IncomeTaxSetting = await _dapper.SqlQueryFirstAsync<IncomeTaxSetting>(user.Database, query.Trim(), new { FiscalYearId, user.CompanyId, user.OrganizationId });

                query = $@"SELECT exemption.* FROM Payroll_TaxExemptionSetting exemption
                INNER JOIN Payroll_IncomeTaxSetting setting On setting.IncomeTaxSettingId= exemption.IncomeTaxSettingId
                Inner Join Payroll_FiscalYear fs On setting.FiscalYearId = fs.FiscalYearId
                Where 1=1
                AND (setting.FiscalYearId=@FiscalYearId)
                AND (setting.IncomeTaxSettingId=@IncomeTaxSettingId)
                AND setting.CompanyId=@CompanyId
                AND setting.OrganizationId=@OrganizationId";

                taxSetting.TaxExemptionSettings = (await _dapper.SqlQueryListAsync<TaxExemptionSetting>(user.Database, query.Trim(), new { FiscalYearId, user.CompanyId, user.OrganizationId, taxSetting.IncomeTaxSetting.IncomeTaxSettingId })).ToList();

                query = $@"SELECT investment.* FROM Payroll_TaxInvestmentSetting investment
                INNER JOIN Payroll_IncomeTaxSetting setting On investment.IncomeTaxSettingId= setting.IncomeTaxSettingId
                Where 1=1
                AND (setting.FiscalYearId=@FiscalYearId)
                AND (setting.IncomeTaxSettingId=@IncomeTaxSettingId)
                AND setting.CompanyId=@CompanyId
                AND setting.OrganizationId=@OrganizationId";

                taxSetting.TaxInvestmentSetting = await _dapper.SqlQueryFirstAsync<TaxInvestmentSetting>(user.Database, query.Trim(), new { FiscalYearId, user.CompanyId, user.OrganizationId, taxSetting.IncomeTaxSetting.IncomeTaxSettingId });
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveSystemException(ex, Database.ControlPanel, "TaxBusiness", "GetTaxSettingAsync", "", user.OrganizationId, user.CompanyId, 0);
            }
            return taxSetting;
        }

        public async Task<TaxSetting> GetTaxSettingAsync(long? IncomeTaxSettingId, AppUser user)
        {
            TaxSetting taxSetting = new TaxSetting();
            try
            {
                sqlQuery = string.Format(@"sp_Payroll_TaxSetting");
                var IncomeTaxSettingParam = new DynamicParameters();
                IncomeTaxSettingParam.Add("IncomeTaxSettingId", IncomeTaxSettingId);
                IncomeTaxSettingParam.Add("CompanyId", user.CompanyId);
                IncomeTaxSettingParam.Add("OrganizationId", user.OrganizationId);
                IncomeTaxSettingParam.Add("ExecutionFlag", "R");
                taxSetting.IncomeTaxSetting = await _dapper.SqlQueryFirstAsync<IncomeTaxSettingViewModel>(user.Database, sqlQuery, IncomeTaxSettingParam, CommandType.StoredProcedure);

                var ExemptionsParam = new DynamicParameters();
                ExemptionsParam.Add("IncomeTaxSettingId", IncomeTaxSettingId);
                ExemptionsParam.Add("CompanyId", user.CompanyId);
                ExemptionsParam.Add("OrganizationId", user.OrganizationId);
                ExemptionsParam.Add("ExecutionFlag", "Exemptions");

                taxSetting.TaxExemptionSettings = (await _dapper.SqlQueryListAsync<TaxExemptionSettingViewModel>(user.Database, sqlQuery, ExemptionsParam, CommandType.StoredProcedure)).ToList();

                var InvestmentsParam = new DynamicParameters();
                InvestmentsParam.Add("IncomeTaxSettingId", IncomeTaxSettingId);
                InvestmentsParam.Add("CompanyId", user.CompanyId);
                InvestmentsParam.Add("OrganizationId", user.OrganizationId);
                InvestmentsParam.Add("ExecutionFlag", "Investments");

                taxSetting.TaxInvestmentSettings = (await _dapper.SqlQueryListAsync<TaxInvestmentSettingViewModel>(user.Database, sqlQuery, InvestmentsParam, CommandType.StoredProcedure)).ToList();
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveSystemException(ex, Database.ControlPanel, "TaxBusiness", "GetTaxSettingAsync", "", user.OrganizationId, user.CompanyId, 0);
            }
            return taxSetting;
        }
    }
}
