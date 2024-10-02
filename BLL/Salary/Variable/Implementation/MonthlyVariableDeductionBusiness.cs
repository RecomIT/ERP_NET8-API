using Dapper;
using System.Data;
using System.Text;
using Shared.Services;
using BLL.Base.Interface;
using Shared.OtherModels.User;
using Shared.OtherModels.Response;
using DAL.DapperObject.Interface;
using BLL.Employee.Interface.Info;
using Shared.Payroll.DTO.Variable;
using BLL.Salary.Variable.Interface;
using Shared.Payroll.ViewModel.Variable;
using Shared.Employee.Filter.Info;

namespace BLL.Salary.Variable.Implementation
{
    public class MonthlyVariableDeductionBusiness : IMonthlyVariableDeductionBusiness
    {
        private readonly IDapperData _dapper;
        private readonly ISysLogger _sysLogger;
        private readonly IInfoBusiness _employeeInfoBusiness;

        public MonthlyVariableDeductionBusiness(IDapperData dapper, IInfoBusiness employeeInfoBusiness, ISysLogger sysLogger)
        {
            _dapper = dapper;
            _employeeInfoBusiness = employeeInfoBusiness;
            _sysLogger = sysLogger;
        }
        public async Task<IEnumerable<MonthlyVariableDeductionViewModel>> GetMonthlyVariableDeductionsAsync(long? monthlyVariableDeductionId, long? employeeId, long? deductionNameId, short salaryMonth, short salaryYear, string stateStatus, AppUser user)
        {
            IEnumerable<MonthlyVariableDeductionViewModel> data = new List<MonthlyVariableDeductionViewModel>();
            try
            {
                var sp_name = "sp_Payroll_MonthlyVariableDeduction";
                var parameters = new DynamicParameters();
                parameters.Add("MonthlyVariableDeductionId", monthlyVariableDeductionId ?? 0);
                parameters.Add("EmployeeId", employeeId ?? 0);
                parameters.Add("DesignationId", 0);
                parameters.Add("SalaryMonth", salaryMonth);
                parameters.Add("SalaryYear", salaryYear);
                parameters.Add("StateStatus", stateStatus);
                parameters.Add("DeductionNameId", deductionNameId ?? 0);
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("OrganizationId", user.OrganizationId);
                parameters.Add("ExecutionFlag", Data.Read);
                data = await _dapper.SqlQueryListAsync<MonthlyVariableDeductionViewModel>(user.Database, sp_name, parameters, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "MonthlyVariableDeductionBusiness", "GetMonthlyVariableDeductionsAsync", user);
            }
            return data;
        }
        public async Task<MonthlyVariableDeductionViewModel> GetMonthlyVariableDeductionAsync(long monthlyVariableDeductionId, long? employeeId, long? deductionNameId, AppUser user)
        {
            MonthlyVariableDeductionViewModel data = new MonthlyVariableDeductionViewModel();
            try
            {
                var sp_name = "sp_Payroll_MonthlyVariableDeduction";
                var parameters = new DynamicParameters();
                parameters.Add("MonthlyVariableAllowanceId", monthlyVariableDeductionId);
                parameters.Add("NotMonthlyVariableAllowanceId", null);
                parameters.Add("EmployeeId", employeeId ?? 0);
                parameters.Add("DesignationId", 0);
                parameters.Add("SalaryMonth", 0);
                parameters.Add("SalaryYear", 0);
                parameters.Add("DeductionNameId", deductionNameId ?? 0);
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("OrganizationId", user.OrganizationId);
                parameters.Add("ExecutionFlag", Data.Read);
                data = await _dapper.SqlQueryFirstAsync<MonthlyVariableDeductionViewModel>(user.Database, sp_name, parameters, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "MonthlyVariableDeductionBusiness", "GetMonthlyVariableDeductionAsync", user);
            }
            return data;
        }
        public async Task<ExecutionStatus> SaveMonthlyVariableDeductionsAsync(List<MonthlyVariableDeductionViewModel> model, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                var employeesInDeduction = model.Select(s => s.EmployeeId.ToString()).AsEnumerable().ToArray();
                var employeeIds = string.Join(',', employeesInDeduction);

                var employees = await _employeeInfoBusiness.GetEmployeeServiceDataAsync(new EmployeeService_Filter()
                {
                    IncludedEmployeeId = employeeIds
                }, user);

                foreach (var item in model)
                {
                    var emp = employees.FirstOrDefault(i => i.EmployeeId == item.EmployeeId);
                    if (emp != null)
                    {
                        item.DesignationId = emp.DesignationId;
                        var salaryMonthYearNumber = item.DeductionForYearOfMonth.Split('-');
                        item.SalaryMonth = Convert.ToInt16(salaryMonthYearNumber[0]);
                        item.SalaryYear = Convert.ToInt16(salaryMonthYearNumber[1]);
                        item.SalaryMonthYear = new DateTime(item.SalaryYear ?? 2090, item.SalaryMonth ?? 12, 1);
                    }
                }

                var sp_name = "sp_Payroll_MonthlyVariableDeduction";
                var parameters = new DynamicParameters();
                parameters.Add("MonthlyVariableDeductionId", 0);
                parameters.Add("JsonData", Utility.JsonData(model));
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("Organizationid", user.OrganizationId);
                parameters.Add("ExecutionFlag", Data.Insert);
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                executionStatus = ResponseMessage.Invalid();
                await _sysLogger.SaveHRMSException(ex, user.Database, "MonthlyVariableDeductionBusiness", "SaveMonthlyVariableDeductionsAsync", user);
            }
            return executionStatus;
        }
        public async Task<ExecutionStatus> UpdateMonthlyVariableDeductionAsync(MonthlyVariableDeductionViewModel model, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                model.SalaryMonthYear = new DateTime(model.SalaryYear ?? 2090, model.SalaryMonth ?? 12, 1);
                var sp_name = "sp_Payroll_MonthlyVariableDeduction";
                var parameters = new DynamicParameters();
                parameters.Add("MonthlyVariableDeductionId", model.MonthlyVariableDeductionId);
                parameters.Add("DeductionNameId", model.DeductionNameId);
                parameters.Add("SalaryMonthYear", model.SalaryMonthYear);
                parameters.Add("SalaryMonth", model.SalaryMonth);
                parameters.Add("SalaryYear", model.SalaryYear);
                parameters.Add("Amount", model.Amount);
                parameters.Add("UserId", user.ActionUserId);
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("OrganizationId", user.OrganizationId);
                parameters.Add("ExecutionFlag", Data.Update);
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                executionStatus = ResponseMessage.Invalid();
                await _sysLogger.SaveHRMSException(ex, user.Database, "MonthlyVariableDeductionBusiness", "UpdateMonthlyVariableDeductionAsync", user);
            }
            return executionStatus;
        }
        public async Task<ExecutionStatus> MonthlyVariableDeductionValidatorAsync(List<MonthlyVariableDeductionViewModel> model, AppUser user)
        {
            ExecutionStatus executionStatus = null;
            try
            {

                StringBuilder duplicate = new StringBuilder();
                foreach (var item in model)
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("EmployeeId", item.EmployeeId);
                    parameters.Add("EmployeeCode", item.EmployeeCode);
                    parameters.Add("NotMonthlyVariableDeductionId", item.MonthlyVariableDeductionId);
                    parameters.Add("DeductionNameId", item.DeductionNameId);
                    parameters.Add("StateStatusList", "Approved,Pending");
                    parameters.Add("SalaryMonth", item.SalaryMonth);
                    parameters.Add("SalaryYear", item.SalaryMonth);
                    parameters.Add("CompanyId", user.CompanyId);
                    parameters.Add("Organizationid", user.OrganizationId);
                    parameters.Add("ExecutionFlag", Data.Read);

                    var data = await _dapper.SqlQueryFirstAsync<MonthlyVariableDeductionViewModel>(user.Database, "sp_Payroll_MonthlyVariableDeduction", parameters, commandType: CommandType.StoredProcedure);
                    if (data != null && data.MonthlyVariableDeductionId > 0)
                    {
                        executionStatus = new ExecutionStatus();
                        executionStatus.Status = false;
                        executionStatus.Msg = "Validation Error";
                        executionStatus.Errors = new Dictionary<string, string>();

                        duplicate.Append(data.EmployeeName)
                          .Append(" has got this ")
                          .Append(data.DeductionName)
                          .Append(" For the month ")
                          .Append(Utility.GetMonthName(item.SalaryMonth ?? 0))
                          .Append(" of ")
                          .Append(item.SalaryYear)
                          .Append("<br/>");
                    }
                }
                if (executionStatus != null)
                {
                    if (executionStatus.Errors != null)
                    {
                        executionStatus.Errors.Add("duplicateAllowance", duplicate.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                executionStatus = ResponseMessage.Invalid();
                await _sysLogger.SaveHRMSException(ex, user.Database, "MonthlyVariableDeductionBusiness", "MonthlyVariableDeductionValidatorAsync", user);
            }
            return executionStatus;
        }
        public async Task<ExecutionStatus> SaveMonthlyVariableDeductionStatusAsync(MonthlyVariableDeductionStatusDTO model, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                string sqlQuery = "sp_Payroll_MonthlyVariableDeduction";
                var parameters = new DynamicParameters();
                parameters.Add("MonthlyVariableDeductionId", model.MonthlyVariableDeductionId);
                parameters.Add("EmployeeId", model.EmployeeId);
                parameters.Add("StateStatus", model.StateStatus);
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("Organizationid", user.OrganizationId);
                parameters.Add("ExecutionFlag", Data.Checking);
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sqlQuery, parameters, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                executionStatus = ResponseMessage.Invalid();
                await _sysLogger.SaveHRMSException(ex, user.Database, "MonthlyVariableDeductionBusiness", "SaveMonthlyVariableDeductionStatusAsync", user);
            }
            return executionStatus;
        }
        public async Task<ExecutionStatus> UploadMonthlyVariableDeductionsAsync(List<MonthlyVariableDeductionViewModel> model, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                string sqlQuery = "sp_Payroll_MonthlyVariableDeduction";
                var parameters = new DynamicParameters();
                var jsonData = Utility.JsonData(model);
                parameters.Add("MonthlyVariableDeductionId", 0);
                parameters.Add("JsonData", jsonData);
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("UserId", user.ActionUserId);
                parameters.Add("Organizationid", user.OrganizationId);
                parameters.Add("ExecutionFlag", "Upload");
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sqlQuery, parameters, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                executionStatus = ResponseMessage.Invalid();
                await _sysLogger.SaveHRMSException(ex, user.Database, "MonthlyVariableDeductionBusiness", "UploadMonthlyVariableDeductionsAsync", user);
            }
            return executionStatus;
        }
    }
}
