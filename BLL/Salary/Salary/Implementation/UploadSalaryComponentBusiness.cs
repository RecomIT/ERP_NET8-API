using Dapper;
using System.Data;
using Shared.Services;
using BLL.Base.Interface;
using Shared.OtherModels.User;
using BLL.Salary.Salary.Interface;
using Shared.OtherModels.Response;
using DAL.DapperObject.Interface;
using Shared.Payroll.ViewModel.Salary;

namespace BLL.Salary.Salary.Implementation
{
    public class UploadSalaryComponentBusiness : IUploadSalaryComponentBusiness
    {

        private readonly IDapperData _dapper;
        private readonly ISysLogger _sysLogger;
        public UploadSalaryComponentBusiness(IDapperData dapper, ISysLogger sysLogger)
        {
            _dapper = dapper;
            _sysLogger = sysLogger;
        }
        public async Task<IEnumerable<UploadAllowanceViewModel>> GetUploadAllowancesAsync(long? uploadId, long? allowanceNameId, long? employeeId, short? month, short? year, AppUser user)
        {
            IEnumerable<UploadAllowanceViewModel> data = new List<UploadAllowanceViewModel>();
            try
            {
                var sp_name = "sp_Payroll_UploadSalaryComponent";
                var parameters = new DynamicParameters();
                parameters.Add("UploadId", uploadId ?? 0);
                parameters.Add("AllowanceNameId", allowanceNameId ?? 0);
                parameters.Add("EmployeeId", employeeId ?? 0);
                parameters.Add("Month", month ?? 0);
                parameters.Add("Year", year ?? 0);
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("OrganizationId", user.OrganizationId);
                parameters.Add("ExecutionFlag", "R_Allowance");
                data = await _dapper.SqlQueryListAsync<UploadAllowanceViewModel>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "UploadSalaryComponentBusiness", "GetUploadAllowancesAsync", user);
            }
            return data;
        }
        public async Task<IEnumerable<UploadDeductionViewModel>> GetUploadDeductionsAsync(long? uploadId, long? deductionNameId, long? employeeId, short? month, short? year, AppUser user)
        {
            IEnumerable<UploadDeductionViewModel> data = new List<UploadDeductionViewModel>();
            try
            {
                var sp_name = "sp_Payroll_UploadSalaryComponent";
                var parameters = new DynamicParameters();
                parameters.Add("UploadId", uploadId ?? 0);
                parameters.Add("EmployeeId", employeeId ?? 0);
                parameters.Add("Month", month ?? 0);
                parameters.Add("Year", year ?? 0);
                parameters.Add("OrganizationId", user.OrganizationId);
                parameters.Add("AllowanceNameId", deductionNameId ?? 0);
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("ExecutionFlag", "R_Deduction");
                data = await _dapper.SqlQueryListAsync<UploadDeductionViewModel>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "UploadSalaryComponentBusiness", "GetUploadDeductionsAsync", user);
            }
            return data;
        }
        public async Task<ExecutionStatus> UpdateAllowanceAsync(long? uploadId, long allowanceNameId, long? employeeId, short? month, short? year, decimal amount, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                var sp_name = "sp_Payroll_UploadSalaryComponent";
                var paramters = new DynamicParameters();
                paramters.Add("UserId", user.ActionUserId);
                paramters.Add("UploadId", uploadId);
                paramters.Add("AllowanceNameId", allowanceNameId);
                paramters.Add("EmployeeId", employeeId);
                paramters.Add("Month", month);
                paramters.Add("Year", year);
                paramters.Add("Amount", amount);
                paramters.Add("CompanyId", user.CompanyId);
                paramters.Add("OrganizationId", user.OrganizationId);
                paramters.Add("ExecutionFlag", "U_Allowance");
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, paramters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                executionStatus = ResponseMessage.Invalid(ResponseMessage.ServerResponsedWithError);
                await _sysLogger.SaveHRMSException(ex, user.Database, "UploadSalaryComponentBusiness", "UpdateAllowanceAsync", user);
            }
            return executionStatus;
        }
        public async Task<ExecutionStatus> UpdateDeductionAsync(long? uploadId, long deductionNameId, long? employeeId, short? month, short? year, decimal amount, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                var sp_name = "sp_Payroll_UploadSalaryComponent";
                var paramters = new DynamicParameters();
                paramters.Add("UserId", user.ActionUserId);
                paramters.Add("UploadId", uploadId);
                paramters.Add("deductionNameId", deductionNameId);
                paramters.Add("EmployeeId", employeeId);
                paramters.Add("Month", month);
                paramters.Add("Year", year);
                paramters.Add("Amount", amount);
                paramters.Add("CompanyId", user.CompanyId);
                paramters.Add("OrganizationId", user.OrganizationId);
                paramters.Add("ExecutionFlag", "U_Deduction");
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, paramters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                executionStatus = ResponseMessage.Invalid(ResponseMessage.ServerResponsedWithError);
                await _sysLogger.SaveHRMSException(ex, user.Database, "UploadSalaryComponentBusiness", "UpdateAllowanceAsync", user);
            }
            return executionStatus;
        }
        public async Task<ExecutionStatus> UploadAllowanceAsync(List<SalaryAllowanceViewModel> allowances, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                var sp_name = "sp_Payroll_UploadSalaryComponent";
                var jsonData = Utility.JsonData(allowances);
                var paramaters = new DynamicParameters();
                paramaters.Add("JsonData", jsonData);
                paramaters.Add("UserId", user.ActionUserId);
                paramaters.Add("CompanyId", user.CompanyId);
                paramaters.Add("OrganizationId", user.OrganizationId);
                paramaters.Add("BranchId", 0);
                paramaters.Add("ExecutionFlag", "Allowance_Upload");
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, paramaters, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                executionStatus = ResponseMessage.Invalid(ResponseMessage.ServerResponsedWithError);
                await _sysLogger.SaveHRMSException(ex, user.Database, "UploadSalaryComponentBusiness", "UploadAllowanceAsync", user);
            }
            return executionStatus;
        }
        public async Task<ExecutionStatus> UploadDeductionAsync(List<SalaryDeductionViewModel> deductions, AppUser user)
        {
            ExecutionStatus executionStatus = null;
            try
            {
                var sp_name = "sp_Payroll_UploadSalaryComponent";
                var jsonData = Utility.JsonData(deductions);
                var paramaters = new DynamicParameters();
                paramaters.Add("JsonData", jsonData);
                paramaters.Add("UserId", user.ActionUserId);
                paramaters.Add("CompanyId", user.CompanyId);
                paramaters.Add("OrganizationId", user.OrganizationId);
                paramaters.Add("BranchId", 0);
                paramaters.Add("ExecutionFlag", "Deduction_Upload");
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, paramaters, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                executionStatus = ResponseMessage.Invalid(ResponseMessage.SomthingWentWrong);
                await _sysLogger.SaveHRMSException(ex, user.Database, "UploadSalaryComponentBusiness", "UploadDeductionAsync", user);
            }
            return executionStatus;
        }
        public async Task<ExecutionStatus> UploadSalaryAllowanceAsync(List<SalaryAllowanceViewModel> allowances, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                var sp_name = "sp_Payroll_SalaryComponent";
                var jsonData = Utility.JsonData(allowances);
                var paramaters = new DynamicParameters();
                paramaters.Add("JsonData", jsonData);
                paramaters.Add("UserId", user.ActionUserId);
                paramaters.Add("CompanyId", user.CompanyId);
                paramaters.Add("OrganizationId", user.OrganizationId);
                paramaters.Add("BranchId", 0);
                paramaters.Add("ExecutionFlag", "Allowance");

                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, paramaters, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                executionStatus = ResponseMessage.Invalid();
                await _sysLogger.SaveHRMSException(ex, user.Database, "UploadSalaryAllowanceAsync", "UploadSalaryAllowanceAsync", user);
            }
            return executionStatus;
        }
        public async Task<ExecutionStatus> UploadSalaryDeductionAsync(List<SalaryDeductionViewModel> deductions, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                var sp_name = "sp_Payroll_SalaryComponent";
                var jsonData = Utility.JsonData(deductions);
                var paramaters = new DynamicParameters();
                paramaters.Add("JsonData", jsonData);
                paramaters.Add("UserId", user.ActionUserId);
                paramaters.Add("CompanyId", user.CompanyId);
                paramaters.Add("OrganizationId", user.OrganizationId);
                paramaters.Add("BranchId", 0);
                paramaters.Add("ExecutionFlag", "Deduction");
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, paramaters, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                executionStatus = ResponseMessage.Invalid(ResponseMessage.SomthingWentWrong);
                await _sysLogger.SaveHRMSException(ex, user.Database, "UploadSalaryAllowanceAsync", "UploadSalaryAllowanceAsync", user);
            }
            return executionStatus;
        }
    }
}
