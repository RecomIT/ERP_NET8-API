using Dapper;
using BLL.Base.Interface;
using BLL.Administration.Interface;
using BLL.Tax.Interface;
using DAL.DapperObject.Interface;
using Shared.OtherModels.Response;
using Shared.OtherModels.User;
using Shared.Payroll.DTO.Tax;
using Shared.Payroll.ViewModel.Tax;
using Shared.Services;
using System.Data;

namespace BLL.Tax.Implementation
{
    public class TaxProcessBusiness : ITaxProcessBusiness
    {
        private readonly IDapperData _dapper;
        private readonly ISysLogger _sysLogger;
        private readonly IBranchInfoBusiness _branchInfoBusiness;
        public TaxProcessBusiness(IDapperData dapper, ISysLogger sysLogger, IBranchInfoBusiness branchInfoBusiness)
        {
            _dapper = dapper;
            _sysLogger = sysLogger;
            _branchInfoBusiness = branchInfoBusiness;
        }

        public async Task<ExecutionStatus> TaxProcessAsync(TaxProcessViewModel model, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                var sp_name = ReportingHelper.ReportProcess("sp_Payroll_TaxProcess_FestivalBonusCalculation_FY_23_24", user);
                var parameters = new DynamicParameters();
                parameters.Add("ExecutionOn", model.ExecutionOn);
                parameters.Add("SelectedEmployees", model.SelectedEmployees);
                parameters.Add("SalaryMonth", model.SalaryMonth);
                parameters.Add("SalaryYear", model.SalaryYear);
                parameters.Add("ProcessDepartmentId", model.ProcessDepartmentId);
                parameters.Add("ProcessBranchId", model.ProcessBranchId);
                parameters.Add("UserId", user.UserId);
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("OrganizationId", user.OrganizationId);
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, CommandType.StoredProcedure);

                if (model.EffectOnSalary && executionStatus.Status)
                {
                    sp_name = "sp_Payroll_AddTaxAmountInSalaryData";
                    var parameters2 = new DynamicParameters();
                    parameters2.Add("SalaryMonth", model.SalaryMonth);
                    parameters2.Add("SalaryYear", model.SalaryYear);
                    parameters2.Add("FiscalYearId", 0);
                    parameters2.Add("EmployeeId", 0);
                    parameters2.Add("UserId", user.UserId);
                    parameters2.Add("CompanyId", user.CompanyId);
                    parameters2.Add("OrganizationId", user.OrganizationId);
                    executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters2, CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                executionStatus = ResponseMessage.Invalid();
                await _sysLogger.SaveHRMSException(ex, user.Database, "TaxProcessBusiness", "TaxProcessAsync", user);
            }
            return executionStatus;
        }
        public async Task<ExecutionStatus> DeleteTaxProcessAsync(DeleteTaxInfoDTO model, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                var sp_name = "sp_Payroll_TaxProcessInfo";
                var parameters = Utility.DappperParams(model, user);
                parameters.Add("ExecutionFlag", Data.Delete);
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                executionStatus = ResponseMessage.Invalid();
                await _sysLogger.SaveHRMSException(ex, user.Database, "TaxProcessBusiness", "DeleteTaxProcessAsync", user);
            }
            return executionStatus;
        }
        public async Task<IEnumerable<TaxProcessSummeryInfoViewModel>> GetTaxProcessSummeryInfosAsync(long? fiscalYearId, short month, short year, AppUser user)
        {
            IEnumerable<TaxProcessSummeryInfoViewModel> data = new List<TaxProcessSummeryInfoViewModel>();
            try
            {
                var sp_name = "sp_Payroll_TaxProcessInfo";
                var parameters = new DynamicParameters();
                parameters.Add("FiscalYearId", fiscalYearId ?? 0);
                parameters.Add("SalaryMonth", month);
                parameters.Add("SalaryYear", year);
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("OrganizationId", user.OrganizationId);
                parameters.Add("ExecutionFlag", "TaxSummery");
                data = await _dapper.SqlQueryListAsync<TaxProcessSummeryInfoViewModel>(user.Database, sp_name, parameters, CommandType.StoredProcedure);

                if (data.Any())
                {
                    var branches = await _branchInfoBusiness.GetBranchsAsync(null, user);
                    if (branches.Any())
                    {
                        foreach (var item in data)
                        {
                            var branch = branches.FirstOrDefault(i => i.BranchId == (item.BranchId ?? 0));
                            if (branch != null && branch.BranchId > 0)
                            {
                                item.BranchName = branch.BranchName;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "TaxBusiness", "GetTaxProcessSummeryInfosAsync", user);
            }
            return data;
        }
        public async Task<IEnumerable<EmployeeTaxProcesDetailInfosViewModel>> GetEmployeeTaxProcesDetailInfosAsync(long? employeeId, long fiscalYearId, long? branchId, long? salaryProcessId, short month, short year, AppUser user)
        {
            IEnumerable<EmployeeTaxProcesDetailInfosViewModel> data = new List<EmployeeTaxProcesDetailInfosViewModel>();
            try
            {
                var sp_name = "sp_Payroll_TaxProcessInfo";
                var parameters = new DynamicParameters();
                parameters.Add("EmployeeId", employeeId ?? 0);
                parameters.Add("FiscalYearId", fiscalYearId);
                parameters.Add("BranchId", branchId ?? 0);
                parameters.Add("SalaryMonth", month);
                parameters.Add("SalaryProcessId", salaryProcessId ?? 0);
                parameters.Add("SalaryYear", year);
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("OrganizationId", user.OrganizationId);
                parameters.Add("ExecutionFlag", "EmployeeTaxInfo");
                data = await _dapper.SqlQueryListAsync<EmployeeTaxProcesDetailInfosViewModel>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "TaxBusiness", "GetEmployeeTaxProcesDetailInfosAsync", user);
            }
            return data;
        }
        public async Task<DataTable> GetTaxSheetData(long fiscalYearId, short month, short year, AppUser user)
        {
            DataTable dataTable = new DataTable();
            try
            {
                var sp_name = "sp_Payroll_TaxProcessInfo";
                Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
                keyValuePairs.Add("FiscalYearId", fiscalYearId.ToString());
                keyValuePairs.Add("SalaryMonth", month.ToString());
                keyValuePairs.Add("SalaryYear", year.ToString());
                keyValuePairs.Add("CompanyId", user.CompanyId.ToString());
                keyValuePairs.Add("OrganizationId", user.OrganizationId.ToString());
                keyValuePairs.Add("ExecutionFlag", "EmployeeTaxInfo");
                dataTable = await _dapper.SqlDataTable(user.Database, sp_name, keyValuePairs, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "TaxBusiness", "GetTaxProcessDetailsAsync", user);
            }
            return dataTable;
        }
        public async Task<IEnumerable<TaxDetailInTaxProcess>> GetTaxableAllowanceIncomeDetail(long employeeId, long fiscalYearId, string firstDateOfThisMonth, short salaryMonth, short salaryYear, string fiscalYearFrom, string fiscalYearTo, short remainProjectionMonthForThisEmployee, AppUser user)
        {
            IEnumerable<TaxDetailInTaxProcess> list = new List<TaxDetailInTaxProcess>();
            try
            {
                var sp_name = @"Select * From dbo.fnTaxableAllowanceIncomeDetailInTaxProcess(@EmployeeId,@FiscalYearId,@FirstDateOfThisMonth,@SalaryMonth,@SalaryYear,@FiscalYearFrom,@FiscalYearTo,@RemainProjectionMonthForThisEmployee,@CompanyId,@OrganizationId)";
                var parameters = new DynamicParameters();
                parameters.Add("EmployeeId", employeeId);
                parameters.Add("FiscalYearId", fiscalYearId);
                parameters.Add("FirstDateOfThisMonth", firstDateOfThisMonth);
                parameters.Add("SalaryMonth", salaryMonth);
                parameters.Add("SalaryYear", salaryYear);
                parameters.Add("FiscalYearFrom", fiscalYearFrom);
                parameters.Add("FiscalYearTo", fiscalYearTo);
                parameters.Add("RemainProjectionMonthForThisEmployee", remainProjectionMonthForThisEmployee);
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("OrganizationId", user.OrganizationId);

                list = await _dapper.SqlQueryListAsync<TaxDetailInTaxProcess>(user.Database, sp_name, parameters, CommandType.Text);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "TaxBusiness", "GetTaxableAllowanceIncomeDetail", user);
            }
            return list;
        }
        public async Task<IEnumerable<EmployeeRamainFestivalBonusViewModal>> GetEmployeeRamainFestivalBonusAsync(long fiscalYearId, string religionName, string dateOfConfirmation, string taxProcessDate, AppUser user)
        {
            IEnumerable<EmployeeRamainFestivalBonusViewModal> data = new List<EmployeeRamainFestivalBonusViewModal>();
            try
            {
                var sp_name = "fnGetEmployeeRemainFestivalBonusForTaxProcess";
                var parameters = new DynamicParameters();
                parameters.Add("FiscalYearId", fiscalYearId);
                parameters.Add("ReligionName", religionName);
                parameters.Add("DateOfConfirmation", dateOfConfirmation ?? null);
                parameters.Add("TaxProcessDate", taxProcessDate);
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("OrganizationId", user.OrganizationId);
                data = await _dapper.SqlQueryListAsync<EmployeeRamainFestivalBonusViewModal>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "TaxBusiness", "GetEmployeeRamainFestivalBonusAsync", user);
            }
            return data;
        }
        public async Task<ExecutionStatus> UploadTaxChallanAsync(List<TaxChallanDTO> taxChallanDTOs, AppUser user)
        {
            ExecutionStatus executionStatus = null;
            try
            {
                //var sp_name = "sp_Payroll_TaxChallan";
                //var jsonData = Utility.JsonData(taxChallanDTOs);
                //var parameters = new DynamicParameters();
                //parameters.Add("JSONData", jsonData);
                //parameters.Add("UserId", user.UserId);
                //parameters.Add("CompanyId", user.CompanyId);
                //parameters.Add("OrganizationId", user.OrganizationId);
                //parameters.Add("BranchId", user.BranchId);
                //parameters.Add("Flag", "Upload_TaxChallan");
                //if (!Utility.IsNullEmptyOrWhiteSpace(sp_name))
                //{
                //    executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, commandType: CommandType.StoredProcedure);
                //    return executionStatus;
                //}
                //executionStatus = Utility.Invalid(ResponseMessage.InvalidExecution);
            }
            catch (Exception ex)
            {
                executionStatus = Utility.Invalid(ResponseMessage.SomthingWentWrong);
                await _sysLogger.SaveHRMSException(ex, user.Database, "TaxBusiness", "UploadTaxChallanAsync", user);
            }
            return executionStatus;
        }
    }
}
