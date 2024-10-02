using Dapper;
using System.Data;
using Shared.Services;
using BLL.Base.Interface;
using Shared.OtherModels.User;
using DAL.DapperObject.Interface;
using Shared.OtherModels.Response;
using BLL.Salary.Variable.Interface;
using Shared.Payroll.Domain.Variable;
using Shared.Payroll.ViewModel.Variable;

namespace BLL.Salary.Variable.Implementation
{
    public class PeriodicallyVariableDeductionBusiness : IPeriodicallyVariableDeductionBusiness
    {
        private readonly IDapperData _dapper;
        private readonly ISysLogger _sysLogger;
        public PeriodicallyVariableDeductionBusiness(IDapperData dapper, ISysLogger sysLogger)
        {
            _dapper = dapper;
            _sysLogger = sysLogger;
        }

        public async Task<IEnumerable<PeriodicallyVariableDeductionInfoViewModel>> GetPeriodicallyVariableDeductionInfosAsync(long? id, string salaryVariableFor, string amountBaseOn, long? deductionNameId, AppUser user)
        {
            IEnumerable<PeriodicallyVariableDeductionInfoViewModel> data = new List<PeriodicallyVariableDeductionInfoViewModel>();
            try
            {
                var sp_name = "sp_Payroll_PeriodicallyVariableAllowance";
                var parameters = new DynamicParameters();
                parameters.Add("PeriodicallyVariableAllowanceInfoId", id ?? 0);
                parameters.Add("SalaryVariableFor", salaryVariableFor ?? "");
                parameters.Add("AmountBaseOn", amountBaseOn ?? "");
                parameters.Add("AllowanceNameId", deductionNameId ?? 0);
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("OrganizationId", user.OrganizationId);
                parameters.Add("ExecutionFlag", Data.Read);
                data = await _dapper.SqlQueryListAsync<PeriodicallyVariableDeductionInfoViewModel>(user.Database, sp_name, parameters, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "PeriodicallyVariableDeductionBusiness", "GetPeriodicallyVariableDeductionInfosAsync", user);
            }
            return data;
        }
        public async Task<PeriodicallyVariableDeductionInfoViewModel> GetPeriodicallyVariableDeductionInfoAsync(long? id, long? deductionNameId, AppUser user)
        {
            PeriodicallyVariableDeductionInfoViewModel data = new PeriodicallyVariableDeductionInfoViewModel();
            try
            {
                var sp_name = "sp_Payroll_PeriodicallyVariableAllowance";
                var parameters = new DynamicParameters();
                parameters.Add("PeriodicallyVariableDeductionInfoId", id);
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("OrganizationId", user.OrganizationId);
                parameters.Add("ExecutionFlag", Data.Detail);

                data = await _dapper.SqlQueryFirstAsync<PeriodicallyVariableDeductionInfoViewModel>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
                if (data != null)
                {
                    sp_name = "sp_Payroll_PeriodicallyVariableDeductionDetail";
                    data.PeriodicalDetails = (await _dapper.SqlQueryListAsync<PeriodicalDetails>(user.Database, sp_name, parameters, CommandType.StoredProcedure)).ToList();
                }
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "PeriodicallyVariableDeductionBusiness", "GetPeriodicallyVariableDeductionInfoAsync", user);
            }
            return data;
        }
        public async Task<ExecutionStatus> SavePeriodicallyVariableDeductionAsync(PeriodicallyVariableDeductionInfo info, List<PeriodicalDetails> details, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                var sp_name = "sp_Payroll_PeriodicallyVariableDeduction";
                var parameters = new DynamicParameters();
                parameters.Add("PeriodicallyVariableDeductionInfoId", info.PeriodicallyVariableDeductionInfoId);
                parameters.Add("SalaryVariableFor", info.SalaryVariableFor);
                parameters.Add("AmountBaseOn", info.AmountBaseOn);
                parameters.Add("PrincipalAmount", info.PrincipalAmount);
                parameters.Add("Amount", info.Amount);
                parameters.Add("Percentage", info.Percentage);
                parameters.Add("DurationType", info.DurationType);
                parameters.Add("FiscalYearId", info.FiscalYearId);
                parameters.Add("EffectiveFrom", info.EffectiveFrom);
                parameters.Add("EffectiveTo", info.EffectiveTo);
                parameters.Add("StateStatus", info.StateStatus);
                parameters.Add("Remarks", info.Remarks);
                parameters.Add("DeductionNameId", info.DeductionNameId);
                parameters.Add("JsonData", Utility.JsonData(details));
                parameters.Add("UserId", user.ActionUserId);
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("OrganizationId", user.OrganizationId);
                parameters.Add("ExecutionFlag", info.PeriodicallyVariableDeductionInfoId > 0 ? Data.Update : Data.Insert);

                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                executionStatus = ResponseMessage.Invalid();
                await _sysLogger.SaveHRMSException(ex, user.Database, "PeriodicallyVariableDeductionBusiness", "SavePeriodicallyVariableDeductionAsync", user);
            }
            return executionStatus;
        }
        public async Task<ExecutionStatus> SavePeriodicallyVariableDeductionStatusAsync(long periodicallyVariableDeductionInfoId, string status, string remarks, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                string sqlQuery = string.Format(@"sp_Payroll_PeriodicallyVariableDeduction");
                var parameters = new DynamicParameters();
                parameters.Add("PeriodicallyVariableDeductionInfoId", periodicallyVariableDeductionInfoId);
                parameters.Add("StateStatus", status);
                parameters.Add("Remarks", status);
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("Organizationid", user.OrganizationId);
                parameters.Add("ExecutionFlag", Data.Checking);

                if (!Utility.IsNullEmptyOrWhiteSpace(sqlQuery))
                {
                    executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sqlQuery, parameters, commandType: CommandType.StoredProcedure);
                }
                else
                {
                    executionStatus = Utility.Invalid();
                }
            }
            catch (Exception ex)
            {
                executionStatus = ResponseMessage.Invalid();
                await _sysLogger.SaveHRMSException(ex, user.Database, "PeriodicallyVariableDeductionBusiness", "SavePeriodicallyVariableDeductionStatusAsync", user);
            }
            return executionStatus;
        }
        public Task<ExecutionStatus> SavePeriodicallyVariableDeductionInfoAsync(PeriodicallyVariableDeductionInfo info, List<PeriodicalDetails> details, AppUser user)
        {
            throw new NotImplementedException();
        }
    }
}
