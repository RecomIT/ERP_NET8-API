using Dapper;
using System.Data;
using Shared.Services;
using BLL.Base.Interface;
using Shared.OtherModels.User;
using DAL.DapperObject.Interface;
using Shared.OtherModels.Response;
using BLL.Salary.Deduction.Interface;
using Shared.Payroll.Domain.Deduction;
using Shared.Payroll.ViewModel.Deduction;

namespace BLL.Salary.Deduction.Implementation
{
    public class DeductionConfigBusiness : IDeductionConfigBusiness
    {
        private readonly IDapperData _dapper;
        private readonly ISysLogger _sysLogger;

        public DeductionConfigBusiness(IDapperData dapper, ISysLogger sysLogger)
        {
            _dapper = dapper;
            _sysLogger = sysLogger;
        }

        public async Task<ExecutionStatus> SaveDeductionConfigAsync(DeductionConfiguration deduction, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                var sp_name = "sp_Payroll_DeductionConfig";
                var parameters = new DynamicParameters();
                parameters.Add("ConfigId", deduction.ConfigId);
                parameters.Add("DeductionNameId", deduction.DeductionNameId);
                parameters.Add("IsActive", deduction.IsActive);
                parameters.Add("IsMonthly", deduction.IsMonthly);
                parameters.Add("IsTaxable", deduction.IsTaxable);
                parameters.Add("IsIndividual", deduction.IsIndividual);
                parameters.Add("Gender", deduction.Gender);
                parameters.Add("IsConfirmationRequired", deduction.IsConfirmationRequired);
                parameters.Add("DepandsOnWorkingHour", deduction.DepandsOnWorkingHour);
                parameters.Add("ActivationDate", deduction.ActivationDate);
                parameters.Add("DeactivationDate", deduction.DeactivationDate);
                parameters.Add("ProjectRestYear", deduction.ProjectRestYear);
                parameters.Add("OnceOffDeduction", deduction.OnceOffDeduction);
                parameters.Add("IsOnceOffTax", deduction.IsOnceOffTax);
                parameters.Add("FlatAmount", deduction.FlatAmount);
                parameters.Add("PercentageAmount", deduction.PercentageAmount);
                parameters.Add("MaxAmount", deduction.MaxAmount);
                parameters.Add("MinAmount", deduction.MinAmount);
                parameters.Add("ExemptedAmount", deduction.ExemptedAmount);
                parameters.Add("ExemptedPercentage", deduction.ExemptedPercentage);
                parameters.Add("UserId", user.ActionUserId);
                parameters.Add("BranchId", 0);
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("OrganizationId", user.OrganizationId);
                parameters.Add("ExecutionFlag", deduction.ConfigId > 0 ? Data.Update : Data.Insert);
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                executionStatus = ResponseMessage.Invalid(ResponseMessage.SomthingWentWrong);
                await _sysLogger.SaveHRMSException(ex, user.Database, "DeductionConfigBusiness", "SaveDeductionConfigAsync", user);
            }
            return executionStatus;
        }
        public async Task<IEnumerable<DeductionConfigurationViewModel>> GetDeductionConfigurationsAsync(long deductionNameId, string status, string activationDateFrom, string activationDateTo, string deactivationDateFrom, string deactivationDateTo, AppUser user)
        {
            IEnumerable<DeductionConfigurationViewModel> data = new List<DeductionConfigurationViewModel>();
            try
            {
                var sp_name = "sp_Payroll_DeductionConfig";
                var parameters = new DynamicParameters();
                parameters.Add("ConfigId", 0);
                parameters.Add("DeductionNameId", deductionNameId);
                parameters.Add("StateStatus", status ?? "");
                parameters.Add("ActivationDateFrom", activationDateFrom ?? "");
                parameters.Add("ActivationDateTo", activationDateTo ?? "");
                parameters.Add("DeactivationDateFrom", deactivationDateFrom ?? "");
                parameters.Add("DeactivationDateTo", deactivationDateTo ?? "");
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("OrganizationId", user.OrganizationId);
                parameters.Add("ExecutionFlag", Data.Read);
                data = await _dapper.SqlQueryListAsync<DeductionConfigurationViewModel>(user.Database, sp_name, parameters, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "DeductionConfigBusiness", "GetDeductionConfigurationsAsync", user);
            }
            return data;
        }
        public async Task<DeductionConfigurationViewModel> GetDeductionConfigurationAsync(long configId, long deductionNameId, AppUser user)
        {
            DeductionConfigurationViewModel data = new DeductionConfigurationViewModel();
            try
            {
                var sp_name = "sp_Payroll_DeductionConfig";
                var parameters = new DynamicParameters();
                parameters.Add("ConfigId", configId);
                parameters.Add("DeductionNameId", deductionNameId);
                parameters.Add("StateStatus", "");
                parameters.Add("ActivationDateFrom", "");
                parameters.Add("ActivationDateTo", "");
                parameters.Add("DeactivationDateFrom", "");
                parameters.Add("DeactivationDateTo", "");
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("OrganizationId", user.OrganizationId);
                parameters.Add("ExecutionFlag", Data.Read);
                data = await _dapper.SqlQueryFirstAsync<DeductionConfigurationViewModel>(user.Database, sp_name, parameters, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "DeductionConfigBusiness", "GetDeductionConfigurationAsync", user);
            }
            return data;
        }
        public async Task<ExecutionStatus> SaveDeductionConfigurationStatusAsync(string status, string remarks, long configId, long deductionNameId, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                var sp_name = "sp_Payroll_DeductionConfig";
                var parameters = new DynamicParameters();
                parameters.Add("ConfigId", configId);
                parameters.Add("DeductionNameId", deductionNameId);
                parameters.Add("StateStatus", status);
                parameters.Add("StatusRemarks", remarks);
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("OrganizationId", user.OrganizationId);
                parameters.Add("UserId", user.ActionUserId);
                parameters.Add("ExecutionFlag", Data.Checking);
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                executionStatus = ResponseMessage.Invalid(ResponseMessage.SomthingWentWrong);
                await _sysLogger.SaveHRMSException(ex, user.Database, "DeductionConfigBusiness", "SaveDeductionConfigurationStatusAsync", user);
            }
            return executionStatus;
        }
    }
}
