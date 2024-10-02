using System.Data;
using Shared.Services;
using BLL.Base.Interface;
using Shared.OtherModels.User;
using Shared.OtherModels.Response;
using BLL.Salary.Allowance.Interface;
using DAL.DapperObject.Interface;
using Shared.Payroll.DTO.Allowance;
using Shared.Payroll.ViewModel.Configuration;
using Shared.Payroll.Filter.Allowance;

namespace BLL.Salary.Allowance.Implementation
{
    public class AllowanceConfigBusiness : IAllowanceConfigBusiness
    {
        private readonly IDapperData _dapper;
        private readonly ISysLogger _sysLogger;
        public AllowanceConfigBusiness(IDapperData dapper, ISysLogger sysLogger)
        {
            _dapper = dapper;
            _sysLogger = sysLogger;
        }

        public async Task<ExecutionStatus> SaveAllowanceConfigAsync(AllowanceConfigurationDTO model, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                var sp_name = "sp_Payroll_AllowanceConfig";
                var parameters = DapperParam.AddParams(model, user, new string[] { "IsPerquisite", "TaxConditionType", "IsTaxDistributed", "IsApproved", "Remarks" });
                parameters.Add("ExecutionFlag", model.ConfigId > 0 ? Data.Update : Data.Insert);
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                executionStatus = ResponseMessage.Invalid(ResponseMessage.SomthingWentWrong);
                await _sysLogger.SaveHRMSException(ex, user.Database, "AllowanceConfigBusiness", "SaveAllowanceConfigAsync", user);

            }
            return executionStatus;
        }
        public async Task<IEnumerable<AllowanceConfigurationViewModel>> GetAllownaceConfigurationsAsync(AllowanceConfig_Filter filter, AppUser user)
        {
            IEnumerable<AllowanceConfigurationViewModel> data = new List<AllowanceConfigurationViewModel>();
            try
            {
                var query = $@"Select ac.*,an.[Name] 'AllowanceName',[AllowanceFlag]=an.Flag From Payroll_AllowanceConfiguration ac
			INNER JOIN Payroll_AllowanceName an on ac.AllowanceNameId = an.AllowanceNameId AND ac.CompanyId = an.CompanyId AND ac.OrganizationId = an.OrganizationId
			Where 1=1 
			AND (@ConfigId IS NULL OR @ConfigId =0 OR ac.ConfigId =@ConfigId)
			AND (@AllowanceNameId IS NULL OR @AllowanceNameId =0 OR ac.AllowanceNameId =@AllowanceNameId)
			AND (@StateStatus IS NULL OR @StateStatus = '' OR ac.StateStatus =@StateStatus)
			AND (
				((@ActivationDateFrom <> '' AND @ActivationDateTo <> '') 
				AND ActivationDate Between Convert(date,@ActivationDateFrom) AND Convert(date,@ActivationDateTo)) 
				OR
				(@ActivationDateFrom <> '' AND ActivationDate = Convert(date,@ActivationDateFrom))
				OR	
				(@ActivationDateTo <> '' AND ActivationDate =Convert(date,@ActivationDateTo))
				OR
				(@ActivationDateFrom ='' AND @ActivationDateTo = '')
				OR
				(@ActivationDateFrom IS NULL AND @ActivationDateTo IS NULL)
			)
			AND (
				((@DeactivationDateFrom <> '' AND @DeactivationDateTo <> '') 
				AND DeactivationDate Between Convert(date,@DeactivationDateFrom) AND Convert(date,@DeactivationDateTo)) 
				OR
				(@DeactivationDateFrom <> '' AND DeactivationDate = Convert(date,@DeactivationDateFrom))
				OR	
				(@DeactivationDateTo <> '' AND DeactivationDate =Convert(date,@DeactivationDateTo))
				OR
				(@DeactivationDateFrom ='' AND @DeactivationDateTo = '')
				OR
				(@DeactivationDateFrom IS NULL AND @DeactivationDateTo IS NULL)
			)
			AND (ac.CompanyId =@CompanyId)
			AND (ac.OrganizationId =@OrganizationId)";
                var parameters = DapperParam.AddParams(filter, user);
                data = await _dapper.SqlQueryListAsync<AllowanceConfigurationViewModel>(user.Database, query, parameters, commandType: CommandType.Text);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "AllowanceConfigBusiness", "GetAllownaceConfigurationsAsync", user);
            }
            return data;
        }
        public async Task<ExecutionStatus> SaveAllowanceConfigStatusAsync(AllowanceConfigStatusDTO model, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                var sp_name = "sp_Payroll_AllowanceConfig";
                var parameters = DapperParam.AddParams(model, user);
                parameters.Add("ExecutionFlag", Data.Checking);
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                executionStatus = ResponseMessage.Invalid(ResponseMessage.ServerResponsedWithError);
                await _sysLogger.SaveHRMSException(ex, user.Database, "AllowanceConfigBusiness", "SaveAllowanceConfigStatusAsync", user);
            }
            return executionStatus;
        }

        public async Task<AllowanceConfigurationViewModel> GetAllownaceConfigurationByAllowanceIdAsync(AllowanceConfig_Filter filter, AppUser user)
        {
            AllowanceConfigurationViewModel data = new AllowanceConfigurationViewModel();
            try
            {
                var query = $@"Select ac.*,an.[Name] 'AllowanceName',[AllowanceFlag]=an.Flag From Payroll_AllowanceConfiguration ac
			INNER JOIN Payroll_AllowanceName an on ac.AllowanceNameId = an.AllowanceNameId AND ac.CompanyId = an.CompanyId AND ac.OrganizationId = an.OrganizationId
			Where 1=1 
			AND (@AllowanceNameId IS NULL OR @AllowanceNameId =0 OR ac.AllowanceNameId =@AllowanceNameId)
			AND (ac.StateStatus ='Approved')
			AND (ac.CompanyId =@CompanyId)
			AND (ac.OrganizationId =@OrganizationId)";
                data = await _dapper.SqlQueryFirstAsync<AllowanceConfigurationViewModel>(user.Database, query, new
                {
                    filter.AllowanceNameId,
                    user.CompanyId,
                    user.OrganizationId
                }, commandType: CommandType.Text);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "AllowanceConfigBusiness", "GetAllownaceConfigurationsAsync", user);
            }
            return data;
        }
    }
}
