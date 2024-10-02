using Dapper;
using AutoMapper;
using System.Data;
using Shared.Services;
using BLL.Base.Interface;
using Shared.OtherModels.User;
using Shared.OtherModels.Response;
using DAL.DapperObject.Interface;
using BLL.Employee.Interface.Info;
using Shared.Employee.Filter.Info;
using Shared.Payroll.DTO.Variable;
using BLL.Salary.Variable.Interface;
using DAL.Payroll.Repository.Interface;
using Shared.Payroll.ViewModel.Variable;

namespace BLL.Salary.Variable.Implementation
{
    public class MonthlyVariableAllowanceBusiness : IMonthlyVariableAllowanceBusiness
    {
        private readonly IDapperData _dapper;
        private readonly ISysLogger _sysLogger;
        private readonly IInfoBusiness _employeeInfoBusiness;
        private readonly IMonthlyVariableAllowanceRepository _monthlyVariableAllowanceRepository;

        public MonthlyVariableAllowanceBusiness(IDapperData dapper, IMapper mapper, IInfoBusiness employeeInfoBusiness, ISysLogger sysLogger, IMonthlyVariableAllowanceRepository monthlyVariableAllowanceRepository)
        {
            _dapper = dapper;
            _sysLogger = sysLogger;
            _employeeInfoBusiness = employeeInfoBusiness;
            _monthlyVariableAllowanceRepository = monthlyVariableAllowanceRepository;
        }
        public async Task<IEnumerable<MonthlyVariableAllowanceViewModel>> GetMonthlyVariableAllowancesAsync(long? monthlyVariableAllowanceId, long? employeeId, long? allowanceNameId, short salaryMonth, short salaryYear, string stateStatus, AppUser user)
        {
            IEnumerable<MonthlyVariableAllowanceViewModel> data = new List<MonthlyVariableAllowanceViewModel>();
            try
            {
                var sp_name = $@"Select ia.*,an.[Name] 'AllowanceName',emp.FullName 'EmployeeName',desig.DesignationName 
			 From Payroll_MonthlyVariableAllowance ia
			 Inner Join Payroll_AllowanceName an on ia.AllowanceNameId= an.AllowanceNameId
			 Inner Join HR_EmployeeInformation emp on emp.EmployeeId = ia.EmployeeId
			 Left Join HR_Designations desig on desig.DesignationId = ia.DesignationId
			 Where 1=1
			 AND (@MonthlyVariableAllowanceId IS NULL OR @MonthlyVariableAllowanceId = 0 OR ia.MonthlyVariableAllowanceId=@MonthlyVariableAllowanceId)
			 AND (@EmployeeId IS NULL OR @EmployeeId =0 OR ia.EmployeeId = @EmployeeId)
			 AND (@SalaryMonth IS NULL OR @SalaryMonth = 0 OR ia.SalaryMonth = @SalaryMonth)
			 AND (@SalaryYear IS NULL OR @SalaryYear = 0 OR ia.SalaryYear = @SalaryYear)
			 AND (@StateStatus IS NULL OR  @StateStatus='' OR ia.StateStatus = @StateStatus)
			 AND (@AllowanceNameId IS NULL OR @AllowanceNameId = 0 OR ia.AllowanceNameId=@AllowanceNameId)
			 AND (ia.CompanyId = @CompanyId)
			 AND (ia.OrganizationId = @Organizationid)";
                var parameters = new DynamicParameters();
                parameters.Add("MonthlyVariableAllowanceId", monthlyVariableAllowanceId ?? 0);
                parameters.Add("EmployeeId", employeeId ?? 0);
                parameters.Add("SalaryMonth", salaryMonth);
                parameters.Add("SalaryYear", salaryYear);
                parameters.Add("StateStatus", stateStatus);
                parameters.Add("AllowanceNameId", allowanceNameId ?? 0);
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("OrganizationId", user.OrganizationId);
                data = await _dapper.SqlQueryListAsync<MonthlyVariableAllowanceViewModel>(user.Database, sp_name, parameters, commandType: CommandType.Text);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "MonthlyVariableAllowanceBusiness", "GetMonthlyVariableAllowancesAsync", user);
            }
            return data;
        }
        public async Task<MonthlyVariableAllowanceViewModel> GetMonthlyVariableAllowanceAsync(long monthlyVariableAllowanceId, long? employeeId, long? allowanceNameId, AppUser user)
        {
            MonthlyVariableAllowanceViewModel data = new MonthlyVariableAllowanceViewModel();
            try
            {
                var sp_name = "sp_Payroll_MonthlyVariableAllowance";
                var parameters = new DynamicParameters();
                parameters.Add("MonthlyVariableAllowanceId", monthlyVariableAllowanceId);
                parameters.Add("NotMonthlyVariableAllowanceId", null);
                parameters.Add("EmployeeId", employeeId ?? 0);
                parameters.Add("DesignationId", 0);
                parameters.Add("SalaryMonth", 0);
                parameters.Add("SalaryYear", 0);
                parameters.Add("AllowanceNameId", allowanceNameId ?? 0);
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("OrganizationId", user.OrganizationId);
                parameters.Add("ExecutionFlag", Data.Read);

                data = await _dapper.SqlQueryFirstAsync<MonthlyVariableAllowanceViewModel>(user.Database, sp_name, parameters, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "MonthlyVariableAllowanceBusiness", "GetMonthlyVariableAllowanceAsync", user);
            }
            return data;
        }
        public async Task<ExecutionStatus> SaveMonthlyVariableAllowancesAsync(List<MonthlyVariableAllowanceViewModel> model, AppUser user)
        {
            ExecutionStatus executionStatus = null;
            try
            {
                var employeesInAllowance = model.Select(s => s.EmployeeId.ToString()).ToList().ToArray();
                var employeeIds = string.Join(',', employeesInAllowance);

                var employees = await _employeeInfoBusiness.GetEmployeeServiceDataAsync(new EmployeeService_Filter()
                {
                    IncludedEmployeeId = employeeIds
                }, user);

                foreach (var item in model)
                {
                    var desig = employees.FirstOrDefault(i => i.EmployeeId == item.EmployeeId);
                    item.DesignationId = desig != null ? desig.DesignationId : 0;
                    item.SalaryMonth = item.SalaryMonth ?? 0;
                    item.SalaryYear = item.SalaryYear ?? 0;
                    item.SalaryMonthYear = new DateTime(item.SalaryYear ?? 0, item.SalaryMonth ?? 0, 1);
                }

                var sp_name = "sp_Payroll_MonthlyVariableAllowance";
                var parameters = new DynamicParameters();
                parameters.Add("MonthlyVariableAllowanceId", 0);
                parameters.Add("JsonData", Utility.JsonData(model));
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("UserId", user.ActionUserId);
                parameters.Add("Organizationid", user.OrganizationId);
                parameters.Add("ExecutionFlag", Data.Insert);

                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                executionStatus = ResponseMessage.Invalid();
                await _sysLogger.SaveHRMSException(ex, user.Database, "MonthlyVariableAllowanceBusiness", "SaveMonthlyVariableAllowancesAsync", user);
            }
            return executionStatus;
        }
        public async Task<ExecutionStatus> UpdateMonthlyVariableAllowanceAsync(MonthlyVariableAllowanceViewModel model, AppUser user)
        {
            ExecutionStatus executionStatus = null;
            try
            {
                model.SalaryMonthYear = new DateTime(model.SalaryYear ?? 0, model.SalaryMonth ?? 0, 1);
                string sqlQuery = "sp_Payroll_MonthlyVariableAllowance";
                var parameters = new DynamicParameters();
                parameters.Add("MonthlyVariableAllowanceId", model.MonthlyVariableAllowanceId);
                parameters.Add("AllowanceNameId", model.AllowanceNameId);
                parameters.Add("SalaryMonthYear", model.SalaryMonthYear);
                parameters.Add("SalaryMonth", model.SalaryMonth);
                parameters.Add("SalaryYear", model.SalaryYear);
                parameters.Add("Amount", model.Amount);
                parameters.Add("UserId", user.ActionUserId);
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("OrganizationId", user.OrganizationId);
                parameters.Add("ExecutionFlag", Data.Update);
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sqlQuery, parameters, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                executionStatus = ResponseMessage.Invalid(ResponseMessage.ServerResponsedWithError);
                await _sysLogger.SaveHRMSException(ex, user.Database, "MonthlyVariableAllowanceBusiness", "UpdateMonthlyVariableAllowanceAsync", user);
            }
            return executionStatus;
        }
        public async Task<ExecutionStatus> MonthlyVariableAllowanceValidatorAsync(List<MonthlyVariableAllowanceViewModel> model, AppUser user)
        {
            ExecutionStatus executionStatus = null;
            int count = 0;
            try
            {
                string duplicate = string.Empty;
                foreach (var item in model)
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("EmployeeId", item.EmployeeId);
                    parameters.Add("EmployeeCode", item.EmployeeCode);
                    parameters.Add("NotMonthlyVariableAllowanceId", item.MonthlyVariableAllowanceId);
                    parameters.Add("AllowanceNameId", item.AllowanceNameId);
                    parameters.Add("StateStatusList", "Approved,Pending");
                    parameters.Add("SalaryMonth", item.SalaryMonth);
                    parameters.Add("SalaryYear", item.SalaryYear);
                    parameters.Add("CompanyId", user.CompanyId);
                    parameters.Add("Organizationid", user.OrganizationId);
                    parameters.Add("ExecutionFlag", Data.Read);

                    var data = await _dapper.SqlQueryFirstAsync<MonthlyVariableAllowanceViewModel>(user.Database, string.Format("sp_Payroll_MonthlyVariableAllowance"), parameters, commandType: CommandType.StoredProcedure);
                    if (data != null && data.MonthlyVariableAllowanceId > 0)
                    {
                        if (executionStatus == null)
                        {
                            executionStatus = new ExecutionStatus();
                            executionStatus.Status = false;
                            executionStatus.Msg = "Validation Error";
                            executionStatus.Errors = new Dictionary<string, string>();
                        }
                        duplicate +=
                            count++.ToString() + ". " + data.EmployeeName + " has got this " + data.AllowanceName + " For the month " +
                            Utility.GetMonthName(item.SalaryMonth ?? 0) + " of " +
                            item.SalaryYear.ToString() + "<br/>";
                    }
                }
                if (executionStatus != null)
                {
                    executionStatus.Errors.Add("duplicateAllowance", duplicate);
                }
            }
            catch (Exception ex)
            {
                executionStatus = ResponseMessage.Invalid();
                await _sysLogger.SaveHRMSException(ex, user.Database, "MonthlyVariableAllowanceBusiness", "MonthlyVariableAllowanceValidatorAsync", user);
            }
            return executionStatus;
        }
        public async Task<ExecutionStatus> SaveMonthlyVariableAllowanceStatusAsync(MonthlyVariableAllowanceStatusDTO model, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                string sqlQuery = "sp_Payroll_MonthlyVariableAllowance";
                var parameters = DapperParam.AddParams(model, user);
                parameters.Add("ExecutionFlag", Data.Checking);

                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sqlQuery, parameters, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                executionStatus = Utility.Invalid(ResponseMessage.ServerResponsedWithError);
                await _sysLogger.SaveHRMSException(ex, user.Database, "MonthlyVariableAllowanceBusiness", "SaveMonthlyVariableAllowanceStatusAsync", user);
            }
            return executionStatus;
        }
        public async Task<ExecutionStatus> UploadMonthlyVariableAllowancesAsync(List<MonthlyVariableAllowanceViewModel> model, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                string sqlQuery = "sp_Payroll_MonthlyVariableAllowance";
                var parameters = new DynamicParameters();
                var jsonData = Utility.JsonData(model);
                parameters.Add("MonthlyVariableAllowanceId", 0);
                parameters.Add("JsonData", jsonData);
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("UserId", user.ActionUserId);
                parameters.Add("Organizationid", user.OrganizationId);
                parameters.Add("ExecutionFlag", "Upload");

                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sqlQuery, parameters, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                executionStatus = Utility.Invalid(ResponseMessage.ServerResponsedWithError);
                await _sysLogger.SaveHRMSException(ex, user.Database, "MonthlyVariableAllowanceBusiness", "UploadMonthlyVariableAllowancesAsync", user);
            }
            return executionStatus;
        }
        public async Task<ExecutionStatus> DeleteMonthlyVariableAsync(long id, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {

            }
            catch (Exception ex)
            {
                executionStatus = Utility.Invalid(ResponseMessage.ServerResponsedWithError);
                await _sysLogger.SaveHRMSException(ex, user.Database, "MonthlyVariableAllowanceBusiness", "DeleteMonthlyVariableAsync", user);
            }
            return executionStatus;
        }
        public async Task<MonthlyVariableAllowanceViewModel> GetByIdAsync(long id, AppUser user)
        {
            MonthlyVariableAllowanceViewModel data = null;
            try
            {
                var query = $@"SELECT * FROM Payroll_MonthlyVariableAllowance Where MonthlyVariableAllowanceId=@Id AND CompanyId=@CompanyId AND OrganizationId=@OrganizationId";
                data = await _dapper.SqlQueryFirstAsync<MonthlyVariableAllowanceViewModel>(user.Database, query, new
                {
                    Id = id,
                    user.CompanyId,
                    user.OrganizationId
                });
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "MonthlyVariableAllowanceBusiness", "GetByIdAsync", user);
            }
            return data;
        }
        public async Task<ExecutionStatus> UpdateApprovedAllowanceAysnc(MonthlyVariableAllowanceViewModel model, AppUser user)
        {
            ExecutionStatus executionStatus = null;
            try
            {
                executionStatus = await _monthlyVariableAllowanceRepository.UpdateApprovedAllowanceAysnc(model, user);
            }
            catch (Exception ex)
            {
                executionStatus = ResponseMessage.Invalid(ResponseMessage.ServerResponsedWithError);
                await _sysLogger.SaveHRMSException(ex, user.Database, "MonthlyVariableAllowanceBusiness", "UpdateApprovedAllowanceAysnc", user);
            }
            return executionStatus;
        }
    }
}
