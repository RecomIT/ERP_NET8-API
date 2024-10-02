using Dapper;
using System.Data;
using Shared.Helpers;
using Shared.Services;
using DAL.DapperObject;
using BLL.Base.Interface;
using Shared.OtherModels.User;
using Shared.OtherModels.Report;
using Shared.OtherModels.Response;
using BLL.Salary.Salary.Interface;
using DAL.DapperObject.Interface;
using Shared.Payroll.Filter.Salary;
using Shared.Payroll.Report;

namespace BLL.Salary.Salary.Implementation
{
    public class SalaryReportBusiness : ISalaryReportBusiness
    {
        private readonly IDapperData _dapper;
        private readonly ISysLogger _sysLogger;
        public SalaryReportBusiness(IDapperData dapper, ISysLogger sysLogger)
        {
            _dapper = dapper;
            _sysLogger = sysLogger;
        }
        public async Task<ReportLayer> ReportLayerAsync(long organizationId, long companyId, long branchId, long divisionId)
        {
            ReportLayer reportLayer = new ReportLayer();
            try
            {
                var sp_name = "sp_ReportLayer";
                var parameters = new DynamicParameters();
                parameters.Add("OrganizationId", organizationId);
                parameters.Add("CompanyId", companyId);
                parameters.Add("BranchId", branchId);
                parameters.Add("DivisionId", divisionId);
                reportLayer = await _dapper.SqlQueryFirstAsync<ReportLayer>(Database.ControlPanel, sp_name, parameters, CommandType.StoredProcedure);
                if (reportLayer != null)
                {
                    reportLayer.CompanyPic = Utility.IsNullEmptyOrWhiteSpace(reportLayer.CompanyLogoPath) == false ?
                       Utility.GetFileBytes(Utility.PhysicalDriver + "/" + reportLayer.CompanyLogoPath) : reportLayer.CompanyPic;
                    reportLayer.ReportLogo = Utility.IsNullEmptyOrWhiteSpace(reportLayer.ReportLogoPath) == false ? Utility.GetFileBytes(Utility.PhysicalDriver + "/" + reportLayer.ReportLogoPath) : reportLayer.ReportLogo;
                    reportLayer.BranchLogo = Utility.IsNullEmptyOrWhiteSpace(reportLayer.BranchLogoPath) == false ? Utility.GetFileBytes(Utility.PhysicalDriver + "/" + reportLayer.BranchLogoPath) : reportLayer.BranchLogo;
                }
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveSystemException(ex, Database.ControlPanel, "ReportBusiness", "ReportLayerAsync", "", organizationId, companyId, branchId);
            }
            return reportLayer;
        }
        public async Task<ExecutionStatus> GetSalarySheetAsync(long SalaryProcessId, string SalaryBatch, short SalaryMonth, short SalaryYear, long EmployeeId, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                var sp_name = "sp_Payroll_SalarySheet";
                var parameters = new DynamicParameters();
                parameters.Add("SalaryMonth", SalaryMonth);
                parameters.Add("SalaryYear", SalaryYear);
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("OrganizationId", user.OrganizationId);
                parameters.Add("ExecutionFlag", "JSON");
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "SalaryReportBusiness", "GetSalarySheetAsync", user);
            }
            return executionStatus;
        }
        public async Task<DataTable> GetSalarySheetReport(long SalaryProcessId, string SalaryBatch, short SalaryMonth, short SalaryYear, long EmployeeId, AppUser user)
        {
            DataTable dataTable = new DataTable();
            try
            {
                var sp_name = "sp_Payroll_SalarySheet";
                Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
                keyValuePairs.Add("SalaryMonth", SalaryMonth.ToString());
                keyValuePairs.Add("SalaryYear", SalaryYear.ToString());
                keyValuePairs.Add("CompanyId", user.CompanyId.ToString());
                keyValuePairs.Add("OrganizationId", user.OrganizationId.ToString());
                keyValuePairs.Add("ExecutionFlag", "Report");

                dataTable = await _dapper.SqlDataTable(user.Database, sp_name, keyValuePairs, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {

                await _sysLogger.SaveHRMSException(ex, user.Database, "SalaryReportBusiness", "GetSalarySheetReport", user);
            }
            return dataTable;
        }
        public async Task<DataTable> PayslipReportInfoAsync(long employeeId, short month, short year, AppUser user)
        {
            DataTable dataTable = new DataTable();
            try
            {
                var sp_name = ReportingHelper.ReportProcess("sp_Payroll_RptPayslip_Extension", user);
                var parameters = new Dictionary<string, string>();
                parameters.Add("EmployeeId", employeeId.ToString());
                parameters.Add("SalaryProcessId", 0.ToString());
                parameters.Add("SalaryMonth", month.ToString());
                parameters.Add("SalaryYear", year.ToString());
                parameters.Add("CompanyId", user.CompanyId.ToString());
                parameters.Add("OrganizationId", user.OrganizationId.ToString());
                parameters.Add("ExecutionFlag", "Info");
                dataTable = await _dapper.SqlDataTable(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                await _sysLogger.SavePayrollException(ex, user.Database, "SalaryReportBusiness", "PayslipReportInfoAsync", user);
            }
            return dataTable;
        }
        public async Task<DataTable> PayslipReportInfoAsync(Payslip_Filter filter, AppUser user, string spName = null)
        {
            DataTable dataTable = new DataTable();
            try
            {
                string sp_name = null;
                if (spName == null)
                {
                    sp_name = spName;
                }
                else
                {
                    sp_name = ReportingHelper.ReportProcess("sp_Payroll_RptPayslip_Extension", user);
                }

                if (spName == null)
                {
                    sp_name = "sp_Payroll_RptPayslip_Extension";
                }

                var parameters = new Dictionary<string, string>();
                parameters.Add("EmployeeId", filter.EmployeeId);
                parameters.Add("SalaryMonth", filter.Month);
                parameters.Add("SalaryYear", filter.Year);
                parameters.Add("IsDisbursed", filter.IsDisbursed);
                parameters.Add("FromDate", filter.FromDate);
                parameters.Add("ToDate", filter.ToDate);
                parameters.Add("CompanyId", user.CompanyId.ToString());
                parameters.Add("OrganizationId", user.OrganizationId.ToString());
                parameters.Add("ExecutionFlag", "Info");
                dataTable = await _dapper.SqlDataTable(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                await _sysLogger.SavePayrollException(ex, user.Database, "SalaryReportBusiness", "PayslipReportInfoAsync", user);
            }
            return dataTable;
        }
        public async Task<DataTable> PayslipReportDetailAsync(Payslip_Filter filter, AppUser user)
        {
            DataTable dataTable = new DataTable();
            try
            {
                var sp_name = ReportingHelper.ReportProcess("sp_Payroll_RptPayslip_Extension", user);
                var parameters = new Dictionary<string, string>();
                parameters.Add("EmployeeId", filter.EmployeeId.ToString());
                parameters.Add("SalaryMonth", filter.Month.ToString());
                parameters.Add("SalaryYear", filter.Year.ToString());
                parameters.Add("CompanyId", user.CompanyId.ToString());
                parameters.Add("OrganizationId", user.OrganizationId.ToString());
                parameters.Add("ExecutionFlag", "Details");
                dataTable = await _dapper.SqlDataTable(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                await _sysLogger.SavePayrollException(ex, user.Database, "SalaryReportBusiness", "PayslipReportDetailAsync", user);
            }
            return dataTable;
        }
        public async Task<DataTable> PayslipExtensionAsync(Payslip_Filter filter, AppUser user)
        {
            PayslipMaster payslip = new PayslipMaster();
            DataTable dataTable = new DataTable();
            try
            {

                var sp_name = "sp_Payroll_RptPayslip_Extension_Nagad";
                var parameters1 = Utility.GetKeyValuePairs(filter);
                parameters1.Add("CompanyId", user.CompanyId.ToString());
                parameters1.Add("OrganizationId", user.OrganizationId.ToString());
                parameters1.Add("ExecutionFlag", "Info");

                dataTable = await _dapper.SqlDataTable(user.Database, sp_name, parameters1, CommandType.StoredProcedure);

                if (payslip != null && payslip.EmployeeId > 0)
                {
                    payslip.AmountInWord = NumberToWords.Input(payslip.NetPay);
                    payslip.MonthName = Utility.GetMonthName(payslip.SalaryMonth);
                    sp_name = "sp_Payroll_RptPayslip_Extension";
                    var parameters2 = Utility.GetKeyValuePairs(filter);
                    parameters2.Add("CompanyId", user.CompanyId.ToString());
                    parameters2.Add("OrganizationId", user.OrganizationId.ToString());
                    parameters2.Add("ExecutionFlag", "Details");
                    dataTable = await _dapper.SqlDataTable(user.Database, sp_name, parameters2, CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveSystemException(ex, user.Database, "SalaryReportBusiness", "PayslipExtensionAsync", "", user.OrganizationId, user.CompanyId, 0);
            }
            return dataTable;
        }
        public async Task<DataTable> GetDateRangeSalarySheetReportAsync(SalarySheet_Filter filter, AppUser user)
        {
            DataTable dataTable = new DataTable();
            try
            {
                var sp_name = "sp_Payroll_Employee_DateRangeSalarySheet";
                var keyValuePairs = Utility.GetKeyValuePairs(filter); //For Dynamic Parameters
                keyValuePairs.Add("CompanyId", user.CompanyId.ToString());
                keyValuePairs.Add("OrganizationId", user.OrganizationId.ToString());
                keyValuePairs.Add("ExecutionFlag", "Report");
                dataTable = await _dapper.SqlDataTable(user.Database, sp_name, keyValuePairs, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                await _sysLogger.SavePayrollException(ex, user.Database, "SalaryReportBusiness", "GetDateRangeSalarySheetReportAsync", user);
            }
            return dataTable;
        }
        public async Task<PayslipMaster> PayslipExtensionAsync(long employeeId, short month, short year, AppUser user)
        {
            PayslipMaster payslip = new PayslipMaster();
            try
            {
                var sp_name = ReportingHelper.ReportProcess("sp_Payroll_RptPayslip_Extension", user);
                var parameters1 = new DynamicParameters();
                parameters1.Add("EmployeeId", employeeId);
                parameters1.Add("SalaryProcessId", 0);
                parameters1.Add("SalaryMonth", month);
                parameters1.Add("SalaryYear", year);
                parameters1.Add("CompanyId", user.CompanyId);
                parameters1.Add("OrganizationId", user.OrganizationId);
                parameters1.Add("ExecutionFlag", "Info");
                payslip = await _dapper.SqlQueryFirstAsync<PayslipMaster>(user.Database, sp_name, parameters1, CommandType.StoredProcedure);

                if (payslip != null && payslip.EmployeeId > 0)
                {
                    payslip.AmountInWord = NumberToWords.Input(payslip.NetPay);
                    payslip.MonthName = Utility.GetMonthName(payslip.SalaryMonth);
                    var parameters2 = new DynamicParameters();
                    parameters2.Add("EmployeeId", employeeId);
                    parameters2.Add("SalaryProcessId", 0);
                    parameters2.Add("SalaryMonth", month);
                    parameters2.Add("SalaryYear", year);
                    parameters2.Add("CompanyId", user.CompanyId);
                    parameters2.Add("OrganizationId", user.OrganizationId);
                    parameters2.Add("ExecutionFlag", "Details");
                    payslip.PayslipDetails = await _dapper.SqlQueryListAsync<PayslipDetail>(user.Database, sp_name, parameters2, CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "SalaryReportBusiness", "PayslipExtensionAsync", user);
            }
            return payslip;
        }
        public async Task<DataTable> GetSalarySheetReport(SalarySheet_Filter filter, AppUser user)
        {
            DataTable dataTable = new DataTable();
            try
            {
                var sp_name = "sp_Payroll_SalarySheet";
                Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
                keyValuePairs.Add("EmployeeId", filter.EmployeeId);
                keyValuePairs.Add("FromDate", filter.FromDate);
                keyValuePairs.Add("ToDate", filter.ToDate);
                keyValuePairs.Add("SelectedEmployees", filter.SelectedEmployees);
                keyValuePairs.Add("DepartmentId", filter.DepartmentId ?? "0");
                keyValuePairs.Add("SalaryMonth", filter.SalaryMonth);
                keyValuePairs.Add("SalaryBatch", filter.SalaryBatch);
                keyValuePairs.Add("IsDisbursed", filter.IsDisbursed);
                keyValuePairs.Add("SalaryYear", filter.SalaryYear);
                keyValuePairs.Add("BranchId", filter.BranchId);
                keyValuePairs.Add("CompanyId", user.CompanyId.ToString());
                keyValuePairs.Add("OrganizationId", user.OrganizationId.ToString());
                keyValuePairs.Add("ExecutionFlag", "Report");
                dataTable = await _dapper.SqlDataTable(user.Database, sp_name, keyValuePairs, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "SalaryReportBusiness", "GetSalarySheetReport", user);
            }
            return dataTable;
        }
        public async Task<ExecutionStatus> GetSalarySheetAsync(SalarySheet_Filter filter, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                var sp_name = "sp_Payroll_SalarySheet";
                var parameters = Utility.DappperParams(filter, user);
                parameters.Add("ExecutionFlag", "JSON");
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {

                await _sysLogger.SaveHRMSException(ex, user.Database, "ISalaryBusiness", "GetSalarySheetReport", user);
            }
            return executionStatus;
        }
        /// <summary>
        /// Added by Nur Vai
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<ExecutionStatus> UpdateAggregateSumInSalaryProcessAsync(Reconciliation_Filter filter, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                var sp_name = "sp_Payroll_UpdateAggregateSumInSalaryProcess";
                var parameters = new DynamicParameters();
                parameters.Add("SalaryMonth", filter.SalaryMonth);
                parameters.Add("SalaryYear", filter.SalaryYear);
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("OrganizationId", user.OrganizationId);
                parameters.Add("ExecutionFlag", Data.Update);

                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {

                await _sysLogger.SaveHRMSException(ex, user.Database, "ISalaryBusiness", "UpdateAggregateSumInSalaryProcessAsync", user);
            }
            return executionStatus;
        }
        public async Task<DataTable> ReconciliationRptAsync(Reconciliation_Filter filter, AppUser user)
        {
            DataTable dataTable = new DataTable();
            try
            {
                var sp_name = "sp_Payroll_Reconciliation";
                var parameters = new Dictionary<string, string>();
                parameters.Add("SalaryMonth", filter.SalaryMonth);
                parameters.Add("SalaryYear", filter.SalaryYear);
                parameters.Add("BranchId", filter.BranchId);
                parameters.Add("CompanyId", user.CompanyId.ToString());
                parameters.Add("OrganizationId", user.OrganizationId.ToString());


                dataTable = await _dapper.SqlDataTable(user.Database, sp_name, parameters, CommandType.StoredProcedure);

            }
            catch (Exception ex)
            {
                await _sysLogger.SaveSystemException(ex, user.Database, "ReportBusiness", "ReconciliationRptAsync", "", user.OrganizationId, user.CompanyId, 0);
            }
            return dataTable;
        }
        // Added by NUR
        public async Task<DataTable> GetActualSalarySheetDetailAsync(ActualSalarySheetDetail_Filter filter, AppUser user)
        {
            DataTable dataTable = new DataTable();
            try
            {
                string spName = "";
                if (filter.SalaryType == "NewJoinerAndSeparation")
                {
                    spName = "sp_Payroll_NewJoinerAndSeparation_SalarySheet";
                }
                else if (filter.SalaryType == "InternNewJoinerAndSeparation")
                {
                    spName = "sp_Payroll_Intern_NewJoinerAndSeparation_SalarySheet";
                }
                else
                {
                    spName = "sp_Payroll_Actual_SalarySheet";
                }
                var sqlQuery = ReportingHelper.ReportProcess(spName, user);
                var parameters = DapperParam.GetKeyValuePairs(filter, user, new string[] { "Format" });
                parameters.Add("ExecutionFlag", "Detail");
                dataTable = await _dapper.SqlDataTable(user.Database, sqlQuery, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                await _sysLogger.SavePayrollException(ex, user.Database, "SalaryReportBusiness", "GetPWCSalarySheetDetail", user);
            }
            return dataTable;
        }
        public async Task<DataTable> GetActualSalarySheetInfoAsync(ActualSalarySheetInfo_Filter filter, AppUser user)
        {
            DataTable dataTable = new DataTable();
            try
            {
                var sqlQuery = ReportingHelper.ReportProcess(string.Format(@"sp_Payroll_ActualSalarySheet"), user);
                var parameters = DapperParam.GetKeyValuePairs(filter, user, new string[] { "Format" });
                parameters.Add("ExecutionFlag", "Info");
                dataTable = await _dapper.SqlDataTable(user.Database, sqlQuery, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                await _sysLogger.SavePayrollException(ex, user.Database, "SalaryReportBusiness", "GetActualSalarySheetInfo", user);
            }
            return dataTable;
        }

        public async Task<DataTable> SalaryBreakdownRptAsync(SalaryBreakdown_Filter filter, AppUser user)
        {
            DataTable dataTable = new DataTable();
            try
            {
                var sp_name = "sp_Payroll_Reconciliation_Salary_Breakdown";
                var parameters = new Dictionary<string, string>();
                parameters.Add("JobType", filter.JobType);
                parameters.Add("SalaryMonth", filter.SalaryMonth);
                parameters.Add("SalaryYear", filter.SalaryYear);
                parameters.Add("BranchId", filter.BranchId);
                parameters.Add("CompanyId", user.CompanyId.ToString());
                parameters.Add("OrganizationId", user.OrganizationId.ToString());
                parameters.Add("ExecutionFlag", "Info");

                dataTable = await _dapper.SqlDataTable(user.Database, sp_name, parameters, CommandType.StoredProcedure);

            }
            catch (Exception ex)
            {
                await _sysLogger.SaveSystemException(ex, user.Database, "ReportBusiness", "SalaryBreakdownRptAsync", "", user.OrganizationId, user.CompanyId, 0);
            }
            return dataTable;
        }
        public async Task<DataTable> SalaryBreakdownDtlsRptAsync(SalaryBreakdown_Filter filter, AppUser user)
        {
            DataTable dataTable = new DataTable();
            try
            {
                var sp_name = "sp_Payroll_Salary_Breakdown";
                var parameters = new Dictionary<string, string>();
                parameters.Add("JobType", filter.JobType);
                parameters.Add("SalaryMonth", filter.SalaryMonth);
                parameters.Add("SalaryYear", filter.SalaryYear);
                parameters.Add("BranchId", filter.BranchId);
                parameters.Add("CompanyId", user.CompanyId.ToString());
                parameters.Add("OrganizationId", user.OrganizationId.ToString());
                parameters.Add("ExecutionFlag", "Details");

                dataTable = await _dapper.SqlDataTable(user.Database, sp_name, parameters, CommandType.StoredProcedure);

            }
            catch (Exception ex)
            {
                await _sysLogger.SaveSystemException(ex, user.Database, "ReportBusiness", "SalaryBreakdownRptAsync", "", user.OrganizationId, user.CompanyId, 0);
            }
            return dataTable;
        }
        public async Task<DataTable> SalaryReconciliationRptAsync(Reconciliation_Filter filter, AppUser user)
        {
            DataTable dataTable = new DataTable();
            try
            {
                var sp_name = "sp_Payroll_Reconciliation";
                var parameters = new Dictionary<string, string>();
                parameters.Add("SalaryMonth", filter.SalaryMonth);
                parameters.Add("SalaryYear", filter.SalaryYear);
                parameters.Add("BranchId", filter.BranchId);
                parameters.Add("CompanyId", user.CompanyId.ToString());
                parameters.Add("OrganizationId", user.OrganizationId.ToString());
                // parameters.Add("ExecutionFlag", Data.Read);
                dataTable = await _dapper.SqlDataTable(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveSystemException(ex, user.Database, "ReportBusiness", "SalaryReconciliationRptAsync", "", user.OrganizationId, user.CompanyId, 0);
            }
            return dataTable;
        }
        public async Task<PayslipMaster> SelfPayslipExtensionAsync(long employeeId, short month, short year, AppUser user)
        {
            PayslipMaster payslip = new PayslipMaster();
            try
            {
                var sp_name = ReportingHelper.ReportProcess("sp_Payroll_RptPayslip_Extension", user);
                var parameters1 = new DynamicParameters();
                parameters1.Add("EmployeeId", employeeId);
                parameters1.Add("SalaryProcessId", 0);
                parameters1.Add("SalaryMonth", month);
                parameters1.Add("IsDisbursed", true);
                parameters1.Add("SalaryYear", year);
                parameters1.Add("CompanyId", user.CompanyId);
                parameters1.Add("OrganizationId", user.OrganizationId);
                parameters1.Add("ExecutionFlag", "Info");
                payslip = await _dapper.SqlQueryFirstAsync<PayslipMaster>(user.Database, sp_name, parameters1, CommandType.StoredProcedure);

                if (payslip != null && payslip.EmployeeId > 0)
                {
                    payslip.AmountInWord = NumberToWords.Input(payslip.NetPay);
                    payslip.MonthName = Utility.GetMonthName(payslip.SalaryMonth);
                    var parameters2 = new DynamicParameters();
                    parameters2.Add("EmployeeId", employeeId);
                    parameters2.Add("SalaryProcessId", 0);
                    parameters2.Add("SalaryMonth", month);
                    parameters2.Add("SalaryYear", year);
                    parameters2.Add("CompanyId", user.CompanyId);
                    parameters2.Add("OrganizationId", user.OrganizationId);
                    parameters2.Add("ExecutionFlag", "Details");
                    payslip.PayslipDetails = await _dapper.SqlQueryListAsync<PayslipDetail>(user.Database, sp_name, parameters2, CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "SalaryReportBusiness", "PayslipExtensionAsync", user);
            }
            return payslip;
        }

        public async Task<DataTable> GetBankStatementAsync(BankStatement_Filter filter, AppUser user)
        {
            DataTable dataTable = new DataTable();
            try
            {
                var sqlQuery = ReportingHelper.ReportProcess("sp_Payroll_BankStatementSheet", user);
                var parameters = DapperParam.GetKeyValuePairs(filter, user, new string[] { "Format" });
                dataTable = await _dapper.SqlDataTable(user.Database, sqlQuery, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                await _sysLogger.SavePayrollException(ex, user.Database, "SalaryReportBusiness", "GetBankStatementAsync", user);
            }
            return dataTable;

        }
    }
}
