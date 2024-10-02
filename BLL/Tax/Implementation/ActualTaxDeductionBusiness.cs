using System.Data;
using Shared.Services;
using BLL.Base.Interface;
using Shared.OtherModels.User;
using Shared.OtherModels.Response;
using DAL.DapperObject.Interface;
using Shared.Payroll.ViewModel.Tax;
using Shared.Payroll.Filter.Tax;
using Shared.Payroll.DTO.Tax;
using BLL.Tax.Interface;

namespace BLL.Tax.Implementation
{
    public class ActualTaxDeductionBusiness : IActualTaxDeductionBusiness
    {
        private readonly ISysLogger _sysLogger;
        private readonly IDapperData _dapper;
        public ActualTaxDeductionBusiness(ISysLogger sysLogger, IDapperData dapper)
        {
            _sysLogger = sysLogger;
            _dapper = dapper;
        }

        public async Task<IEnumerable<ActualTaxDeductionViewModel>> GetActualTaxDeductionInfosAsync(ActualTaxDeduction_Filter filter, AppUser user)
        {
            IEnumerable<ActualTaxDeductionViewModel> list = new List<ActualTaxDeductionViewModel>();
            try
            {
                var sp_name = "sp_Payroll_ActualTaxDeduction_List";
                var parameters = DapperParam.AddParams(filter, user, addUserId: false);
                list = await _dapper.SqlQueryListAsync<ActualTaxDeductionViewModel>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ActualTaxDeductionBusiness", "GetActualTaxDeductionInfos", user);
            }
            return list;
        }
        public async Task<ExecutionStatus> SaveActualTaxDeductionAsync(ActualTaxDeductionDTO model, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                var sp_name = "sp_Payroll_ActualTaxDeduction_Insert_Update";
                var parameters = DapperParam.AddParams(model, user);
                parameters.Add("ExecutionFlag", model.ActualTaxDeductionId > 0 ? Data.Update : Data.Insert);
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                executionStatus = ResponseMessage.Invalid();
                await _sysLogger.SaveHRMSException(ex, user.Database, "ActualTaxDeductionBusiness", "SaveActualTaxDeductionAsync", user);
            }
            return executionStatus;
        }
        public async Task<IEnumerable<ExecutionStatus>> SaveApprovalAsync(ActualTaxDeductionApprovalDTO model, AppUser user)
        {
            List<ExecutionStatus> executionList = new List<ExecutionStatus>();
            try
            {
                var sp_name = "sp_Payroll_ActualTaxDeduction_Insert_Update";
                foreach (var item in model.Employees)
                {
                    var parameters = DapperParam.AddParams(model, user, new string[] { "Employees" });
                    parameters.AddDynamicParams(item);
                    parameters.Add("ExecutionFlag", Data.Checking);
                    var execution = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
                    executionList.Add(execution);
                }
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ActualTaxDeductionBusiness", "SaveApprovalAsync", user);
            }
            return executionList;
        }
        public async Task<IEnumerable<ExecutionStatus>> SaveUploadInfosAsync(List<ActualTaxDeductionDTO> model, AppUser user)
        {
            List<ExecutionStatus> list = new List<ExecutionStatus>();
            try
            {
                foreach (var item in model)
                {
                    var validate = await ValidatorAsync(item, user);
                    if (validate != null && validate.Status == true)
                    {
                        ExecutionStatus executionStatus = await SaveActualTaxDeductionAsync(item, user);
                        list.Add(executionStatus);
                    }
                }
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ActualTaxDeductionBusiness", "SaveUploadInfosAsync", user);
            }
            return list;
        }
        public async Task<ExecutionStatus> UpdateActaulTaxDeductedInSalaryAndTaxAsync(UpdateActaulTaxDeductedDTO model, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                var sp_name = "sp_Payroll_UpdateActualTaxDeductionAmountInTaxAndSalary";
                var parameters = DapperParam.AddParams(model, user);
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                executionStatus = ResponseMessage.Invalid();
                await _sysLogger.SaveHRMSException(ex, user.Database, "ActualTaxDeductionBusiness", "UpdateActaulTaxDeductedInSalaryAndTaxAsync", user);
            }
            return executionStatus;
        }
        public async Task<ExecutionStatus> ValidatorAsync(ActualTaxDeductionDTO model, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                var sp_name = "sp_Payroll_ActualTaxDeduction_Insert_Update";
                var parameters = DapperParam.AddParams(model, user);
                parameters.Add("ExecutionFlag", Data.Validate);
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ActualTaxDeductionBusiness", "ValidatorAsync", user);
                executionStatus = ResponseMessage.Invalid();
            }
            return executionStatus;
        }
    }
}