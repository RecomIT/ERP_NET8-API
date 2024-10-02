using Dapper;
using System.Data;
using Shared.Services;
using BLL.Base.Interface;
using Shared.OtherModels.User;
using Shared.OtherModels.Response;
using BLL.Salary.Salary.Interface;
using DAL.DapperObject.Interface;
using Shared.Payroll.DTO.SalaryHold;
using Shared.Payroll.Filter.Salary;
using Shared.Payroll.ViewModel.Salary;

namespace BLL.Salary.Salary.Implementation
{
    public class SalaryHoldBusiness : ISalaryHoldBusiness
    {
        private readonly IDapperData _dapper;
        private readonly ISysLogger _sysLogger;
        public SalaryHoldBusiness(IDapperData dapper, ISysLogger sysLogger)
        {
            _dapper = dapper;
            _sysLogger = sysLogger;
        }

        public async Task<ExecutionStatus> SaveSalaryHoldAsync(SalaryHoldDTO model, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus(); ;
            try
            {
                var sp_name = "sp_Payroll_SalaryHold_Insert_Update";
                var parameters = DapperParam.AddParams(model, user);
                parameters.Add("ExecutionFlag", model.SalaryHoldId > 0 ? Data.Update : Data.Insert);
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                executionStatus = ResponseMessage.Invalid();
                await _sysLogger.SavePayrollException(ex, user.Database, "SalaryHoldBusiness", "SaveSalaryHoldAsync", user);
            }
            return executionStatus;
        }
        public async Task<IEnumerable<SalaryHoldViewModel>> GetSalaryHoldListAsync(SalaryHold_Filter filter, AppUser user)
        {
            IEnumerable<SalaryHoldViewModel> data = new List<SalaryHoldViewModel>();
            try
            {
                var sp_name = "sp_Payroll_SalaryHold_List";
                var parameters = DapperParam.AddParams(filter, user);
                data = await _dapper.SqlQueryListAsync<SalaryHoldViewModel>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                await _sysLogger.SavePayrollException(ex, user.Database, "SalaryHoldBusiness", "GetSalaryHoldListAsync", user);
            }
            return data;
        }
        public async Task<ExecutionStatus> ValidatorSalaryHoldAsync(SalaryHoldDTO model, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                var sp_name = "sp_Payroll_SalaryHold_Insert_Update";
                var parameters = DapperParam.AddParams(model, user);
                parameters.Add("ExecutionFlag", Data.Validate);
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                executionStatus = ResponseMessage.Invalid();
                await _sysLogger.SavePayrollException(ex, user.Database, "SalaryHoldBusiness", "ValidatorSalaryHoldAsync", user);
            }
            return executionStatus;
        }
        public async Task<IEnumerable<ExecutionStatus>> SaveUploadHoldSalaryAsync(List<SalaryHoldDTO> models, AppUser user)
        {
            List<ExecutionStatus> listOfExecutionStatus = new List<ExecutionStatus>();
            try
            {
                foreach (var item in models)
                {
                    ExecutionStatus executionStatus = new ExecutionStatus();
                    executionStatus = await SaveSalaryHoldAsync(item, user);
                    listOfExecutionStatus.Add(executionStatus);
                }
            }
            catch (Exception ex)
            {
                await _sysLogger.SavePayrollException(ex, user.Database, "SalaryHoldBusiness", "SaveUploadHoldSalaryAsync", user);
            }
            return listOfExecutionStatus;
        }
        public async Task<IEnumerable<SalaryHoldViewModel>> GetEmployeeUnholdSalaryInfoAsync(long employeeId, int unholdMonth, int unholdYear, AppUser user)
        {
            IEnumerable<SalaryHoldViewModel> list = new List<SalaryHoldViewModel>();
            try
            {
                var query = $@"SELECT * FROM Payroll_SalaryHold Where EmployeeId=0 AND MONTH(UnholdDate) =@Month AND YEAR(UnholdDate)=@Year AND CompanyId=@CompanyId AND OrganizationId=@OrganizationId";
                var parameters = new DynamicParameters();
                parameters.Add("EmployeeId", employeeId);
                parameters.Add("Month", unholdMonth);
                parameters.Add("Year", unholdYear);
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("OrganizationId", user.OrganizationId);
                list = await _dapper.SqlQueryListAsync<SalaryHoldViewModel>(user.Database, query, parameters, CommandType.Text);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "SalaryHoldBusiness", "GetEmployeeUnholdSalaryInfoAsync", user);
            }
            return list;
        }
    }
}
