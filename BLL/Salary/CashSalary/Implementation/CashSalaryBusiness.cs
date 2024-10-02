using Dapper;
using AutoMapper;
using Shared.Services;
using System.Data;
using Shared.OtherModels.User;
using Shared.OtherModels.Response;
using BLL.Base.Interface;
using BLL.Salary.CashSalary.Interface;
using DAL.DapperObject.Interface;
using Shared.Payroll.DTO.CashSalary;
using Shared.OtherModels.DataService;
using Shared.Payroll.Filter.CashSalary;

namespace BLL.Salary.CashSalary.Implementation
{
    public class CashSalaryBusiness : ICashSalaryBusiness
    {
        private string sqlQuery;
        private readonly IDapperData _dapper;
        private readonly ISysLogger _sysLogger;
        private readonly IMapper _mapper;

        public CashSalaryBusiness(IDapperData dapper, IMapper mapper, ISysLogger sysLogger)
        {
            _dapper = dapper;
            _mapper = mapper;
            _sysLogger = sysLogger;
        }

        public async Task<ExecutionStatus> UploadCashSalaryHeadExcelAsync(List<CashSalaryHeadDTO> salaryHeadReadDTOs, AppUser user)
        {
            ExecutionStatus executionStatus = null;
            try
            {
                sqlQuery = string.Format(@"sp_Payroll_CashSalaryHead");
                var jsonData = Utility.JsonData(salaryHeadReadDTOs);
                var paramaters = new DynamicParameters();
                paramaters.Add("JsonData", jsonData);
                paramaters.Add("UserId", user.UserId);
                paramaters.Add("CompanyId", user.CompanyId);
                paramaters.Add("OrganizationId", user.OrganizationId);
                paramaters.Add("BranchId", user.BranchId);
                paramaters.Add("Flag", "CashSalaryHead_Upload");
                if (!Utility.IsNullEmptyOrWhiteSpace(sqlQuery))
                {
                    executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sqlQuery, paramaters, commandType: CommandType.StoredProcedure);
                    return executionStatus;
                }
                executionStatus = Utility.Invalid(ResponseMessage.InvalidExecution);
            }
            catch (Exception ex)
            {
                executionStatus = Utility.Invalid(ResponseMessage.SomthingWentWrong);
            }
            return executionStatus;
        }


        public async Task<ExecutionStatus> SaveCashSalaryHeadAsync(CashSalaryHeadDTO headDTO, AppUser user)
        {
            ExecutionStatus executionStatus = null;
            try
            {

                sqlQuery = string.Format(@"sp_Payroll_CashSalaryHead");
                var parameters = new DynamicParameters();
                parameters.Add("CashSalaryHeadId", headDTO.CashSalaryHeadId ?? 0);
                parameters.Add("CashSalaryHeadName", headDTO.CashSalaryHeadName ?? "");
                parameters.Add("CashSalaryHeadCode", headDTO.CashSalaryHeadCode ?? "");
                parameters.Add("CashSalaryHeadNameInBengali", headDTO.CashSalaryHeadNameInBengali ?? "");
                parameters.Add("IsActive", headDTO.IsActive);
                parameters.Add("BranchId", user.BranchId);
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("OrganizationId", user.OrganizationId);
                parameters.Add("UserId", user.UserId);
                parameters.Add("Flag", headDTO.CashSalaryHeadId > 0 ? Data.Update : Data.Insert);

                if (!Utility.IsNullEmptyOrWhiteSpace(sqlQuery))
                {
                    executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sqlQuery, parameters, commandType: CommandType.StoredProcedure);
                    return executionStatus;
                }
                return executionStatus = Utility.Invalid();
            }
            catch (Exception ex)
            {
                return executionStatus = Utility.Invalid(ResponseMessage.SomthingWentWrong);
            }
        }
        public async Task<IEnumerable<CashSalaryHeadDTO>> GetCashSalaryHeadListAsync(long? cashSalaryHeadId, string cashSalaryHeadName, string cashSalaryHeadCode, string cashSalaryHeadNameInBengali, AppUser user)
        {
            IEnumerable<CashSalaryHeadDTO> data = new List<CashSalaryHeadDTO>();
            try
            {
                sqlQuery = string.Format(@"sp_Payroll_CashSalaryHead");
                var parameters = new DynamicParameters();
                parameters.Add("CashSalaryHeadId", cashSalaryHeadId ?? 0);
                parameters.Add("CashSalaryHeadName", cashSalaryHeadName);
                parameters.Add("CashSalaryHeadCode", cashSalaryHeadCode);
                parameters.Add("CashSalaryHeadNameInBengali", cashSalaryHeadNameInBengali);
                parameters.Add("BranchId", user.BranchId);
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("OrganizationId", user.OrganizationId);
                parameters.Add("UserId", user.UserId);
                parameters.Add("Flag", Data.Read);

                if (!Utility.IsNullEmptyOrWhiteSpace(sqlQuery))
                {
                    data = await _dapper.SqlQueryListAsync<CashSalaryHeadDTO>(user.Database, sqlQuery, parameters, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
            }
            return data;
        }

        public async Task<IEnumerable<CashSalaryHeadDTO>> GetCashSalaryHeadByIdAsync(long cashSalaryHeadId, AppUser user)
        {
            IEnumerable<CashSalaryHeadDTO> data = new List<CashSalaryHeadDTO>();
            try
            {
                sqlQuery = string.Format(@"sp_Payroll_CashSalaryHead");
                var parameters = new DynamicParameters();
                parameters.Add("CashSalaryHeadId", cashSalaryHeadId);
                parameters.Add("BranchId", user.BranchId);
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("OrganizationId", user.OrganizationId);
                parameters.Add("UserId", user.UserId);
                parameters.Add("Flag", Data.Read);

                if (!Utility.IsNullEmptyOrWhiteSpace(sqlQuery))
                {
                    data = await _dapper.SqlQueryListAsync<CashSalaryHeadDTO>(user.Database, sqlQuery, parameters, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
            }
            return data;
        }

        //Upload Cash Salary
        public async Task<IEnumerable<Select2Dropdown>> GetCashSalaryHeadExtensionAsync(long? cashSalaryHeadId, AppUser appUser)
        {
            IEnumerable<Select2Dropdown> data = new List<Select2Dropdown>();
            try
            {
                sqlQuery = string.Format(@"sp_Payroll_UploadCashSalary");
                var parameters = new DynamicParameters();
                parameters.Add("CashSalaryHeadId", cashSalaryHeadId);
                parameters.Add("CompanyId", appUser.CompanyId);
                parameters.Add("OrganizationId", appUser.OrganizationId);
                parameters.Add("BranchId", appUser.BranchId);
                parameters.Add("Flag", Data.Extension);
                if (!Utility.IsNullEmptyOrWhiteSpace(sqlQuery))
                {
                    data = await _dapper.SqlQueryListAsync<Select2Dropdown>(appUser.Database, sqlQuery, parameters, CommandType.StoredProcedure);
                }
                return data;
            }
            catch (Exception ex)
            {
                return data;
            }
        }

        public async Task<ExecutionStatus> UploadCashSalaryExcelAync(List<UploadCashSalaryDTO> cashSalaryDTOs, AppUser user)
        {
            ExecutionStatus executionStatus = null;
            try
            {
                sqlQuery = string.Format(@"sp_Payroll_UploadCashSalary");
                var jsonData = Utility.JsonData(cashSalaryDTOs);
                var parameters = new DynamicParameters();
                parameters.Add("JsonData", jsonData);
                parameters.Add("UserId", user.UserId);
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("OrganizationId", user.OrganizationId);
                parameters.Add("BranchId", user.BranchId);
                parameters.Add("Flag", "Upload_CashSalary");
                if (!Utility.IsNullEmptyOrWhiteSpace(sqlQuery))
                {
                    executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sqlQuery, parameters, commandType: CommandType.StoredProcedure);
                    return executionStatus;
                }
                executionStatus = Utility.Invalid(ResponseMessage.InvalidExecution);
            }
            catch (Exception ex)
            {
                executionStatus = Utility.Invalid(ResponseMessage.SomthingWentWrong);
            }
            return executionStatus;
        }

        public async Task<IEnumerable<UploadCashSalaryDTO>> UploadCashSalaryListAsync(long? uploadCashSalaryId, long? employeeId, long? cashSalaryHeadId, short salaryMonth, short salaryYear, string stateStatus, AppUser user)
        {
            IEnumerable<UploadCashSalaryDTO> data = new List<UploadCashSalaryDTO>();
            try
            {
                sqlQuery = string.Format(@"sp_Payroll_UploadCashSalary");
                var parameters = new DynamicParameters();
                parameters.Add("UploadCashSalaryId", uploadCashSalaryId ?? 0);
                parameters.Add("EmployeeId", employeeId ?? 0);
                parameters.Add("SalaryMonth", salaryMonth);
                parameters.Add("SalaryYear", salaryYear);
                parameters.Add("StateStatus", stateStatus);
                parameters.Add("CashSalaryHeadId", cashSalaryHeadId ?? 0);
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("OrganizationId", user.OrganizationId);
                parameters.Add("BranchId", user.BranchId);
                parameters.Add("Flag", Data.Read);

                if (!Utility.IsNullEmptyOrWhiteSpace(sqlQuery))
                {
                    data = await _dapper.SqlQueryListAsync<UploadCashSalaryDTO>(user.Database, sqlQuery, parameters, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
            }
            return data;
        }

        public async Task<ExecutionStatus> SaveCashSalariesAync(List<UploadCashSalaryDTO> cashSalaryDTOs, AppUser user)
        {
            ExecutionStatus executionStatus = null;
            try
            {
                var jsonData = Utility.JsonData(cashSalaryDTOs);
                sqlQuery = string.Format(@"sp_Payroll_UploadCashSalary");
                var parameters = new DynamicParameters();
                parameters.Add("JSONData", jsonData);
                parameters.Add("UserId", user.UserId);
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("OrganizationId", user.OrganizationId);
                parameters.Add("BranchId", user.BranchId);
                parameters.Add("Flag", Data.Insert);
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sqlQuery, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                executionStatus = Utility.Invalid(ResponseMessage.ServerResponsedWithError);
            }
            return executionStatus;
        }
        public async Task<ExecutionStatus> UpdateUploadCashSalaryAsync(UploadCashSalaryDTO dTO, AppUser user)
        {
            ExecutionStatus executionStatus = null;
            try
            {
                string sqlQuery = string.Format(@"sp_Payroll_UploadCashSalary");
                var parameters = new DynamicParameters();
                parameters.Add("UploadCashSalaryId", dTO.UploadCashSalaryId);
                parameters.Add("EmployeeId", dTO.EmployeeId);
                parameters.Add("CashSalaryHeadId", dTO.CashSalaryHeadId);
                parameters.Add("SalaryMonth", dTO.SalaryMonth);
                parameters.Add("SalaryYear", dTO.SalaryYear);
                parameters.Add("Amount", dTO.Amount);
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("OrganizationId", user.OrganizationId);
                parameters.Add("BranchId", user.BranchId);
                parameters.Add("Flag", Data.Update);
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
                executionStatus = Utility.Invalid(ResponseMessage.ServerResponsedWithError);
            }
            return executionStatus;
        }
        public async Task<ExecutionStatus> SaveUploadCashSalaryApprovalAsync(long? uploadCashSalaryId, long? employeeId, long? cashSalaryHeadId, string stateStatus, string remarks, AppUser user)
        {
            ExecutionStatus executionStatus = null;
            try
            {
                sqlQuery = string.Format(@"sp_Payroll_UploadCashSalary");
                var parameters = new DynamicParameters();
                parameters.Add("UploadCashSalaryId", uploadCashSalaryId);
                parameters.Add("CashSalaryHeadId", cashSalaryHeadId);
                parameters.Add("EmployeeId", employeeId);
                parameters.Add("StateStatus", stateStatus);
                parameters.Add("ApprovalRemarks", remarks ?? "");
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("OrganizationId", user.OrganizationId);
                parameters.Add("BranchId", user.BranchId);
                parameters.Add("UserId", user.UserId);
                parameters.Add("Flag", Data.Checking);
                if (!Utility.IsNullEmptyOrWhiteSpace(sqlQuery))
                {
                    executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sqlQuery, parameters, commandType: CommandType.StoredProcedure);
                    return executionStatus;
                }
                return executionStatus = Utility.Invalid();
            }
            catch (Exception ex)
            {
                return executionStatus = Utility.Invalid(ResponseMessage.SomthingWentWrong);
            }
        }
        //Salary Process
        public async Task<ExecutionStatus> CashSalaryProcessAsync(CashSalaryProcessExecutionDTO data, AppUser user)
        {
            ExecutionStatus executionStatus = null;
            try
            {
                if (data.ExecutionOn == "All")
                {
                    sqlQuery = string.Format(@"sp_Payroll_CashSalaryProcess");
                    var parameters = new DynamicParameters();
                    parameters.Add("CashSalaryProcessId", 0);
                    parameters.Add("SalaryMonth", data.Month);
                    parameters.Add("SalaryYear", data.Year);
                    parameters.Add("SalaryDate", data.SalaryDate);
                    parameters.Add("UserId", user.UserId);
                    parameters.Add("CompanyId", user.CompanyId);
                    parameters.Add("OrganizationId", user.OrganizationId);
                    parameters.Add("BranchId", user.BranchId);
                    parameters.Add("Flag", "All");
                    if (!Utility.IsNullEmptyOrWhiteSpace(sqlQuery))
                    {
                        executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sqlQuery, parameters, CommandType.StoredProcedure);
                        return executionStatus;
                    }
                }
                else if (data.ExecutionOn == "Branch")
                {
                    sqlQuery = string.Format(@"sp_Payroll_CashSalaryProcess");
                    var parameters = new DynamicParameters();
                    parameters.Add("CashSalaryProcessId", 0);
                    parameters.Add("BranchId", data.ProcessBranchId);
                    parameters.Add("SalaryMonth", data.Month);
                    parameters.Add("SalaryYear", data.Year);
                    parameters.Add("SalaryDate", data.SalaryDate);
                    parameters.Add("UserId", user.UserId);
                    parameters.Add("CompanyId", user.CompanyId);
                    parameters.Add("OrganizationId", user.OrganizationId);
                    parameters.Add("Flag", "Branch");
                    if (!Utility.IsNullEmptyOrWhiteSpace(sqlQuery))
                    {
                        executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sqlQuery, parameters, CommandType.StoredProcedure);
                        return executionStatus;
                    }
                }
                else if (data.ExecutionOn == "Department")
                {
                    sqlQuery = string.Format(@"sp_Payroll_CashSalaryProcess");
                    var parameters = new DynamicParameters();
                    parameters.Add("CashSalaryProcessId", 0);
                    parameters.Add("DepartmentId", data.ProcessDepartmentId);
                    parameters.Add("SalaryMonth", data.Month);
                    parameters.Add("SalaryYear", data.Year);
                    parameters.Add("SalaryDate", data.SalaryDate);
                    parameters.Add("UserId", user.UserId);
                    parameters.Add("CompanyId", user.CompanyId);
                    parameters.Add("OrganizationId", user.OrganizationId);
                    parameters.Add("BranchId", user.BranchId);
                    parameters.Add("Flag", "Department");
                    if (!Utility.IsNullEmptyOrWhiteSpace(sqlQuery))
                    {
                        executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sqlQuery, parameters, CommandType.StoredProcedure);
                        return executionStatus;
                    }
                }
                return executionStatus = Utility.Invalid(ResponseMessage.InvalidExecution);
            }
            catch (Exception ex)
            {

                executionStatus = Utility.Invalid(ResponseMessage.ServerResponsedWithError);
            }
            return executionStatus;
        }

        public async Task<IEnumerable<CashSalaryProcessExecutionDTO>> GetCashSalaryProcessInfosAsync(long? cashSalaryProcessId, short? salaryMonth, short? salaryYear, DateTime? salaryDate, string batchNo, AppUser user)
        {
            IEnumerable<CashSalaryProcessExecutionDTO> data = new List<CashSalaryProcessExecutionDTO>();
            try
            {
                sqlQuery = string.Format(@"sp_Payroll_CashSalaryProcessedData");
                var parameters = new DynamicParameters();
                parameters.Add("CashSalaryProcessId", cashSalaryProcessId ?? 0);
                parameters.Add("SalaryMonth", salaryMonth ?? 0);
                parameters.Add("SalaryYear", salaryYear ?? 0);
                parameters.Add("SalaryDate", salaryDate ?? null);
                parameters.Add("BatchNo", batchNo ?? "");
                parameters.Add("UserId", user.UserId);
                parameters.Add("BranchId", user.BranchId);
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("OrganizationId", user.OrganizationId);
                parameters.Add("Flag", "info");

                if (!Utility.IsNullEmptyOrWhiteSpace(sqlQuery))
                {
                    data = await _dapper.SqlQueryListAsync<CashSalaryProcessExecutionDTO>(user.Database, sqlQuery, parameters, CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
            }
            return data;
        }

        public async Task<IEnumerable<CashSalaryProcessExecutionDTO>> GetCashSalaryProcessDetailAsync(long? cashSalaryProcessId, long? cashSalaryDetailId, long? employeeId, short? salaryMonth, short? salaryYear, DateTime? salaryDate, string batchNo, AppUser user)
        {
            IEnumerable<CashSalaryProcessExecutionDTO> data = new List<CashSalaryProcessExecutionDTO>();
            try
            {
                sqlQuery = string.Format(@"sp_Payroll_CashSalaryProcessedData");
                var parameters = new DynamicParameters();
                parameters.Add("CashSalaryProcessId", cashSalaryProcessId ?? 0);
                parameters.Add("CashSalaryDetailId", cashSalaryDetailId ?? 0);
                parameters.Add("EmployeeId", employeeId);
                parameters.Add("BatchNo", batchNo ?? "");
                parameters.Add("SalaryMonth", salaryMonth ?? 0);
                parameters.Add("SalaryYear", salaryYear ?? 0);
                parameters.Add("UserId", user.UserId);
                parameters.Add("BranchId", user.BranchId);
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("OrganizationId", user.OrganizationId);
                parameters.Add("Flag", "detail");
                if (!Utility.IsNullEmptyOrWhiteSpace(sqlQuery))
                {
                    data = await _dapper.SqlQueryListAsync<CashSalaryProcessExecutionDTO>(user.Database, sqlQuery, parameters, CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
            }
            return data;
        }

        public async Task<ExecutionStatus> CashSalaryProcessDisbursedOrUndoAsync(long cashSalaryProcessId, string actionName, AppUser user)
        {
            ExecutionStatus executionStatus = null;
            try
            {
                sqlQuery = string.Format(@"sp_Payroll_CashSalaryProcessedData");
                var parameters = new DynamicParameters();
                parameters.Add("CashSalaryProcessId", cashSalaryProcessId);
                parameters.Add("ActionName", actionName);
                parameters.Add("UserId", user.UserId);
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("OrganizationId", user.OrganizationId);
                parameters.Add("BranchId", user.BranchId);
                parameters.Add("Flag", "D_U");
                if (!Utility.IsNullEmptyOrWhiteSpace(sqlQuery))
                {
                    executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sqlQuery, parameters, CommandType.StoredProcedure);
                    return executionStatus;
                }
                return executionStatus = Utility.Invalid(ResponseMessage.InvalidExecution);
            }
            catch (Exception ex)
            {
                executionStatus = Utility.Invalid(ResponseMessage.ServerResponsedWithError);
            }
            return executionStatus;
        }

        public async Task<DataTable> GetCashSalarySheetAsync(CashSalarySheet_Filter filter, AppUser user)
        {
            DataTable dataTable = new DataTable();
            try
            {
                sqlQuery = "sp_Payroll_CashSalarySheet";
                var keyValuePairs = DapperParam.GetKeyValuePairs(filter, new string[] { "Format" }); //For Dynamic Parameters
                keyValuePairs.Add("CompanyId", user.CompanyId.ToString());
                keyValuePairs.Add("OrganizationId", user.OrganizationId.ToString());
                keyValuePairs.Add("BranchId", user.BranchId.ToString());
                keyValuePairs.Add("ExecutionFlag", "Report");
                dataTable = await _dapper.SqlDataTable(user.Database, sqlQuery, keyValuePairs, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                await _sysLogger.SavePayrollException(ex, user.Database, "CashSalaryBusiness", "GetCashSalarySheetAsync", user);
            }
            return dataTable;
        }

        public async Task<DataTable> GetActualCashSalarySheetDetailAsync(CashSalarySheet_Filter filter, AppUser user)
        {
            DataTable dataTable = new DataTable();
            try
            {
                sqlQuery = ReportingHelper.ReportProcess("sp_Payroll_Actual_Cash_SalarySheet", user);
                var parameters = DapperParam.GetKeyValuePairs(filter, user, new string[] { "Format" });
                parameters.Add("ExecutionFlag", "Detail");
                dataTable = await _dapper.SqlDataTable(user.Database, sqlQuery, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                await _sysLogger.SavePayrollException(ex, user.Database, "CashSalaryBusiness", "GetActualCashSalarySheetDetail", user);
            }
            return dataTable;
        }
    }
}
