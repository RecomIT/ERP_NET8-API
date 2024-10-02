using Dapper;
using System.Data;
using Shared.Helpers;
using Shared.Services;
using DAL.DapperObject;
using BLL.Base.Interface;
using Shared.OtherModels.User;
using Shared.OtherModels.Response;
using Shared.OtherModels.DataService;
using Shared.OtherModels.Pagination;
using BLL.Salary.Salary.Interface;
using DAL.DapperObject.Interface;
using Shared.Payroll.ViewModel.Salary;
using Shared.Payroll.DTO.Salary;
using Shared.Payroll.Process.Salary;
using Shared.Payroll.Domain.Salary;
using Shared.Payroll.Filter.Salary;
using Shared.Payroll.Domain.Setup;

namespace BLL.Salary.Salary.Implementation
{
    public class SalaryProcessBusiness : ISalaryProcessBusiness
    {
        private IDapperData _dapper;
        private ISysLogger _sysLogger;
        private readonly ISalaryComponentHistoriesBusiness _salaryComponentHistoriesBusiness;
        public SalaryProcessBusiness(IDapperData dapper, ISysLogger sysLogger, ISalaryComponentHistoriesBusiness salaryComponentHistoriesBusiness)
        {
            _dapper = dapper;
            _sysLogger = sysLogger;
            _salaryComponentHistoriesBusiness = salaryComponentHistoriesBusiness;
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
                executionStatus = ResponseMessage.Invalid(ResponseMessage.SomthingWentWrong);
                await _sysLogger.SaveHRMSException(ex, user.Database, "SalaryProcessBusiness", "UploadAllowanceAsync", user);
            }
            return executionStatus;
        }
        public async Task<ExecutionStatus> UploadDeductionAsync(List<SalaryDeductionViewModel> deductions, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                var sp_name = string.Format(@"sp_Payroll_UploadSalaryComponent");
                var jsonData = JsonReverseConverter.JsonData(deductions);
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
                await _sysLogger.SaveHRMSException(ex, user.Database, "SalaryProcessBusiness", "UploadDeductionAsync", user);
            }
            return executionStatus;
        }
        public async Task<decimal> CurrentMonthSalaryAllowanceAmount(long employeeId, long allowanceNameId, string allowanceFlag, string firstDateOfthisMonth, long salaryMonth, long salaryYear, long fiscalYearId, string fiscalYearFrom, string fiscalYearTo, AppUser user)
        {
            decimal amount = 0;
            try
            {
                var query = @"SELECT dbo.fnAllowanceAmountCurrentDate(@EmployeeId,@AllowanceNameId,@AllowanceFlag,@FirstDateOfthisMonth,@SalaryMonth,@SalaryYear,@FiscalYearId,@FiscalYearFrom,@FiscalYearTo,@CompanyId,@OrganizationId)";
                var parameters = new DynamicParameters();
                parameters.Add("EmployeeId", employeeId);
                parameters.Add("AllowanceNameId", allowanceNameId);
                parameters.Add("AllowanceFlag", allowanceFlag);
                parameters.Add("FirstDateOfthisMonth", firstDateOfthisMonth);
                parameters.Add("SalaryMonth", salaryMonth);
                parameters.Add("SalaryYear", salaryYear);
                parameters.Add("FiscalYearId", fiscalYearId);
                parameters.Add("FiscalYearFrom", fiscalYearFrom);
                parameters.Add("FiscalYearTo", fiscalYearTo);
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("OrganizationId", user.OrganizationId);
                amount = await _dapper.ExecuteScalerFunction<decimal>(user.Database, query, parameters, CommandType.Text);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "SalaryProcessBusiness", "CurrentMonthSalaryAllowanceAmount", user);
            }
            return amount;
        }
        public async Task<IEnumerable<SalaryAllowanceViewModel>> GetEmplyeeSalaryAllowancesAsync(long? employeeId, long? allowanceNameId, long? salaryProcessId, long? salaryProcessDetailId, long? fiscalYearId, short? month, short? year, string batchNo, AppUser user)
        {
            IEnumerable<SalaryAllowanceViewModel> list = new List<SalaryAllowanceViewModel>();
            try
            {
                var sp_name = "sp_Payroll_EmployeeSalaryAllowance";
                var parameters = new DynamicParameters();
                parameters.Add("EmployeeId", employeeId ?? 0);
                parameters.Add("AllowanceNameId", allowanceNameId ?? 0);
                parameters.Add("SalaryProcessId", salaryProcessId ?? 0);
                parameters.Add("SalaryProcessDetailId", salaryProcessDetailId ?? 0);
                parameters.Add("Month", month ?? 0);
                parameters.Add("Year", year ?? 0);
                parameters.Add("batchNo", batchNo ?? "");
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("OrganizationId", user.OrganizationId);

                list = await _dapper.SqlQueryListAsync<SalaryAllowanceViewModel>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "SalaryProcessBusiness", "GetEmplyeeSalaryAllowancesAsync", user);
            }
            return list;
        }
        public async Task<IEnumerable<SalaryDeductionViewModel>> GetEmplyeeSalaryDeductionsAsync(long? employeeId, long? deductionNameId, long? salaryProcessId, long? salaryProcessDetailId, short? month, short? year, string batchNo, AppUser user)
        {
            IEnumerable<SalaryDeductionViewModel> list = new List<SalaryDeductionViewModel>();
            try
            {
                var sp_name = "sp_Payroll_EmployeeSalaryDeduction";
                var parameters = new DynamicParameters();
                parameters.Add("EmployeeId", employeeId ?? 0);
                parameters.Add("DeductionNameId", deductionNameId ?? 0);
                parameters.Add("SalaryProcessId", salaryProcessId ?? 0);
                parameters.Add("SalaryProcessDetailId", salaryProcessDetailId ?? 0);
                parameters.Add("Month", month ?? 0);
                parameters.Add("Year", year ?? 0);
                parameters.Add("batchNo", batchNo ?? "");
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("OrganizationId", user.OrganizationId);

                list = await _dapper.SqlQueryListAsync<SalaryDeductionViewModel>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "SalaryProcessBusiness", "GetEmplyeeSalaryDeductionsAsync", user);
            }
            return list;
        }
        public async Task<IEnumerable<SalaryProcessDetailViewModel>> GetSalaryProcessDetailsAsync(long? salaryProcessId, long? salaryProcessDetailId, long? employeeId, long? fiscalYearId, short? month, short? year, long? branchId, string batchNo, AppUser user)
        {
            IEnumerable<SalaryProcessDetailViewModel> data = new List<SalaryProcessDetailViewModel>();
            try
            {
                var sp_name = $@"Select SalaryProcessDetailId, info.BatchNo, detail.EmployeeId,emp.EmployeeCode,Email=(CASE WHEN emp.OfficeEmail IS NULL OR emp.OfficeEmail='' THEN dtl.PersonalEmailAddress ELSE emp.OfficeEmail END), (emp.FullName) 'EmployeeName', detail.DesignationId, 
		detail.CalculationForDays, CurrentBasic, ThisMonthBasic, detail.TotalAllowance, detail.TotalArrearAllowance, detail.PFAmount, detail.PFArrear,OtherDeduction=ISNULL((
		SELECT Amount=SUM(Amount) FROM Payroll_SalaryDeduction 
		Where EmployeeId=detail.EmployeeId AND SalaryMonth=detail.SalaryMonth AND SalaryYear=detail.SalaryYear 
		AND SalaryProcessId=detail.SalaryProcessId),0),
        [TotalDeduction]=ISNULL((
		SELECT Amount=SUM(Amount) FROM Payroll_SalaryDeduction 
		Where EmployeeId=detail.EmployeeId AND SalaryMonth=detail.SalaryMonth AND SalaryYear=detail.SalaryYear 
		AND SalaryProcessId=detail.SalaryProcessId),0)+ISNULL(detail.PFAmount,0)+ISNULL(detail.PFArrear,0)+ISNULL(detail.TaxDeductedAmount,0),
		detail.TotalArrearDeduction, TotalMonthlyTax=detail.TaxDeductedAmount, detail.TotalBonus, detail.GrossPay, detail.NetPay, info.SalaryProcessId, detail.CreatedDate, detail.UpdatedDate, 
		info.OrganizationId, info.CompanyId, detail.BankTransferAmount, detail.COCInWalletTransfer, detail.CashAmount, 
		detail.CurrentConveyance, detail.CurrentHouseRent, detail.CurrentMedical, detail.WalletTransferAmont, detail.AccountId, detail.BankBranchId, 
		detail.BankId, detail.SalaryMonth, detail.SalaryYear,desig.DesignationName, info.SalaryDate,info.ProcessDate,info.IsDisbursed,detail.FiscalYearId,
		detail.HoldDays,detail.HoldAmount,detail.UnholdDays,detail.UnholdAmount,detail.ProjectionTax,detail.OnceOffTax
		FROM Payroll_SalaryProcessDetail detail
		INNER JOIN Payroll_SalaryProcess info on detail.SalaryProcessId = info.SalaryProcessId
		INNER JOIN HR_EmployeeInformation emp on detail.EmployeeId = emp.EmployeeId
        INNER JOIN Payroll_FiscalYear FY ON detail.FiscalYearId = FY.FiscalYearId
		LEFT JOIN HR_EmployeeDetail dtl on emp.EmployeeId = dtl.EmployeeId
		LEFT JOIN HR_Designations desig on detail.DesignationId = desig.DesignationId
		Where 1=1
		AND (@SalaryProcessId IS NULL OR @SalaryProcessId = 0 OR info.SalaryProcessId=@SalaryProcessId)
		AND (@SalaryProcessDetail IS NULL OR @SalaryProcessDetail = 0 OR detail.SalaryProcessDetailId=@SalaryProcessDetail)
		AND (@EmployeeId IS NULL OR @EmployeeId = 0 OR detail.EmployeeId=@EmployeeId)
		AND (@FiscalYearId IS NULL OR @FiscalYearId =0 OR detail.FiscalYearId=@FiscalYearId)
		AND (@BatchNo IS NULL OR @BatchNo = '' OR info.BatchNo LIKE '%'+@BatchNo+'%')
		AND (@Month IS NULL OR @Month = 0  OR info.SalaryMonth =@Month)
        AND (EMP.EmployeeCode IN('101523399')) 
		AND (@Year IS NULL OR @Year = 0  OR  info.SalaryYear =@Year)
     
		AND (detail.CompanyId=@CompanyId)
		AND (detail.OrganizationId =@OrganizationId)";
                var parameters = new DynamicParameters();
                parameters.Add("SalaryProcessId", salaryProcessId ?? 0);
                parameters.Add("SalaryProcessDetail", salaryProcessDetailId ?? 0);
                parameters.Add("EmployeeId", employeeId);
                parameters.Add("FiscalYearId", fiscalYearId ?? 0);
                parameters.Add("BatchNo", batchNo ?? "");
                parameters.Add("Month", month ?? 0);
                parameters.Add("Year", year ?? 0);
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("OrganizationId", user.OrganizationId);
                data = await _dapper.SqlQueryListAsync<SalaryProcessDetailViewModel>(user.Database, sp_name, parameters, CommandType.Text);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "SalaryProcessBusiness", "GetSalaryProcessDetailsAsync", user);
            }
            return data;
        }
        public async Task<IEnumerable<SalaryProcessViewModel>> GetSalaryProcessInfosAsync(long? salaryProcessId, long? fiscalYearId, short? month, short? year, DateTime? salaryDate, long? branchId, string batchNo, AppUser user)
        {
            IEnumerable<SalaryProcessViewModel> data = new List<SalaryProcessViewModel>();
            try
            {
                var sp_name = $@"Select SalaryProcessId,BatchNo,SalaryDate,SalaryMonth,SalaryYear,ProcessDate,PaymentDate,IsDisbursed,TotalEmployees, 
                TotalAllowance,TotalArrearAllowance,TotalPFAmount,TotalPFArrear,
                OtherDeduction=ISNULL((SELECT SUM(Amount) FROM Payroll_SalaryDeduction Where SalaryProcessId=info.SalaryProcessId),0),
                TotalDeduction=ISNULL((SELECT SUM(Amount) FROM Payroll_SalaryDeduction Where SalaryProcessId=info.SalaryProcessId),0)+ISNULL(TotalPFAmount,0)+ISNULL(TotalPFArrear,0)+ISNULL(TotalTaxDeductedAmount,0),TotalArrearDeduction,
                TotalMonthlyTax=ISNULL(TotalProjectionTax,0)+ISNULL(TotalOnceOffTax,0),
                TotalTaxDeductedAmount, TotalBonus, TotalGrossPay, TotalNetPay,CreatedDate,UpdatedDate, 
                OrganizationId,CompanyId,TotalHoldDays,TotalHoldAmount,TotalUnholdDays,TotalUnholdAmount

                FROM Payroll_SalaryProcess info
                Where 1=1
                AND(@SalaryProcessId = 0 OR SalaryProcessId=@SalaryProcessId)
                AND(@Month = 0  OR SalaryMonth =@Month)
                AND(@Year = 0  OR  SalaryYear =@Year)
                AND(@BatchNo = '' OR @BatchNo IS NULL OR BatchNo LIKE '%'+@BatchNo+'%')
                AND(@FiscalYearId =0 OR @FiscalYearId IS NULL OR FiscalYearId=@FiscalYearId)
                AND(CompanyId=@CompanyId)
                AND(OrganizationId =@OrganizationId)
                Order By SalaryProcessId desc";

                var parameters = new DynamicParameters();
                parameters.Add("SalaryProcessId", salaryProcessId ?? 0);
                parameters.Add("SalaryProcessDetail", 0);
                parameters.Add("FiscalYearId", fiscalYearId ?? 0);
                parameters.Add("Month", month ?? 0);
                parameters.Add("Year", year ?? 0);
                parameters.Add("SalaryData", salaryDate ?? null);
                parameters.Add("BatchNo", batchNo ?? "");
                parameters.Add("UserId", "");
                parameters.Add("BranchId", branchId ?? 0);
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("OrganizationId", user.OrganizationId);

                data = await _dapper.SqlQueryListAsync<SalaryProcessViewModel>(user.Database, sp_name, parameters, CommandType.Text);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "SalaryProcessBusiness", "GetSalaryProcessInfosAsync", user);
            }
            return data;
        }
        public Task<ExecutionStatus> SalaryProcessAsync(SalaryProcessViewModel model, AppUser user)
        {
            throw new NotImplementedException();
        }
        public async Task<ExecutionStatus> SalaryProcessAsync(SalaryProcessExecution data, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                var processDate = new DateTime(data.Year, data.Month, DateTime.DaysInMonth(data.Year, data.Month));
                if (data.ProcessBy == "Uploaded Component")
                {
                    var sp_name = "sp_Payroll_SalaryProcessByUploadedComponent";
                    var parameters = new DynamicParameters();
                    parameters.Add("ProcessBy", data.ProcessBy);
                    parameters.Add("Month", data.Month);
                    parameters.Add("Year", data.Year);
                    parameters.Add("ProcessDate", data.SalaryDate);
                    parameters.Add("UserId", user.ActionUserId);
                    parameters.Add("BranchId", data.ProcessBranchId);
                    parameters.Add("CompanyId", user.CompanyId);
                    parameters.Add("OrganizationId", user.OrganizationId);
                    parameters.Add("ExecutionFlag", "Process");
                    executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
                }
                else if (data.ProcessBy == "Systematically")
                {
                    var sp_name = "sp_Payroll_SalaryProcess";
                    var parameters = new DynamicParameters();
                    parameters.Add("ProcessType", data.ProcessBy);
                    parameters.Add("SelectedEmployees", data.SelectedEmployees ?? "");
                    parameters.Add("SalaryMonth", data.Month);
                    parameters.Add("SalaryYear", data.Year);
                    parameters.Add("PaymentDate", data.SalaryDate);
                    parameters.Add("UserId", user.ActionUserId);
                    parameters.Add("CompanyId", user.CompanyId);
                    parameters.Add("OrganizationId", user.OrganizationId);
                    executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                executionStatus = ResponseMessage.Invalid(ResponseMessage.ServerResponsedWithError);
                await _sysLogger.SaveHRMSException(ex, user.Database, "SalaryProcessBusiness", "SalaryProcessAsync", user);
            }
            return executionStatus;
        }
        public async Task<ExecutionStatus> SalaryProcessDisbursedOrUndoAsync(SalaryProcessDisbursedOrUndoDTO model, AppUser user)
        {
            ExecutionStatus executionStatus = null;
            try
            {
                var sp_name = "sp_Payroll_SalaryProcessedData";
                var parameters = new DynamicParameters();
                parameters.Add("SalaryProcessId", model.SalaryProcessId);
                parameters.Add("ActionName", model.ActionName);
                parameters.Add("UserId", user.UserId);
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("OrganizationId", user.OrganizationId);
                parameters.Add("ExecutionFlag", "D_U");
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                executionStatus = ResponseMessage.Invalid(ResponseMessage.ServerResponsedWithError);
                await _sysLogger.SaveHRMSException(ex, user.Database, "SalaryProcessBusiness", "SalaryProcessDisbursedOrUndoAsync", user);
            }
            return executionStatus;
        }
        public async Task<decimal> TillMonthSalaryAllowanceAmount(long employeeId, long allowanceNameId, string allowanceFlag, string firstDateOfthisMonth, long salaryMonth, long salaryYear, long fiscalYearId, string fiscalYearFrom, string fiscalYearTo, AppUser user)
        {
            decimal amount = 0;
            try
            {
                var sp_name = @"SELECT dbo.fnAllowanceAmountTillDate(@EmployeeId,@AllowanceNameId,@AllowanceFlag,@FirstDateOfthisMonth,@SalaryMonth,@SalaryYear,@FiscalYearId,@FiscalYearFrom,@FiscalYearTo,@CompanyId,@OrganizationId)";
                var parameters = new DynamicParameters();
                parameters.Add("EmployeeId", employeeId);
                parameters.Add("AllowanceNameId", allowanceNameId);
                parameters.Add("AllowanceFlag", allowanceFlag);
                parameters.Add("FirstDateOfthisMonth", firstDateOfthisMonth);
                parameters.Add("SalaryMonth", salaryMonth);
                parameters.Add("SalaryYear", salaryYear);
                parameters.Add("FiscalYearId", fiscalYearId);
                parameters.Add("FiscalYearFrom", fiscalYearFrom);
                parameters.Add("FiscalYearTo", fiscalYearTo);
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("OrganizationId", user.OrganizationId);
                amount = await _dapper.ExecuteScalerFunction<decimal>(user.Database, sp_name, parameters, CommandType.Text);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "SalaryProcessBusiness", "TillMonthSalaryAllowanceAmount", user);
            }
            return amount;
        }
        public async Task<IEnumerable<SalaryBatchNoViewModel>> GetSalaryProcessBatchNoAsync(string isDisbursed, AppUser user)
        {
            IEnumerable<SalaryBatchNoViewModel> list = new List<SalaryBatchNoViewModel>();
            try
            {
                var query = $@"SELECT SalaryProcessId, BatchNo, SalaryYear, SalaryMonth FROM Payroll_SalaryProcess 
                    Where 1 = 1 
                AND(@IsDisbursed IS NULL OR @IsDisbursed = '' OR IsDisbursed = @IsDisbursed) 
                AND (BatchNo IS NOT NULL AND BatchNo <> '') AND CompanyId = @CompanyId AND OrganizationId = @OrganizationId Order By SalaryMonth, SalaryYear DESC";

                var parameters = new DynamicParameters();
                parameters.Add("IsDisbursed", isDisbursed);
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("OrganizationId", user.OrganizationId);
                list = await _dapper.SqlQueryListAsync<SalaryBatchNoViewModel>(user.Database, query, parameters, CommandType.Text);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "SalaryProcessBusiness", "GetSalaryProcessBatchNoAsync", user);
            }
            return list;
        }
        public async Task<IEnumerable<Dropdown>> GetSalaryProcessBatchNoDropdownAsync(string isDisbursed, AppUser user)
        {
            IEnumerable<Dropdown> list = new List<Dropdown>();
            try
            {
                var items = await GetSalaryProcessBatchNoAsync(isDisbursed, user);
                foreach (var item in items)
                {
                    Dropdown dropdown = new Dropdown();
                    dropdown.Value = item.SalaryProcessId.ToString();
                    dropdown.Id = item.SalaryProcessId;
                    var text = DateTimeExtension.GetMonthName(item.SalaryMonth);
                    dropdown.Text = item.BatchNo.ToString() + " [" + text + "-" + item.SalaryYear.ToString() + "]";
                    list.AsList().Add(dropdown);
                }
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "SalaryProcessBusiness", "GetSalaryProcessBatchNoDropdownAsync", user);
            }
            return list;
        }
        public async Task<List<long>> GetEmployeeLastSalaryProcessedIdsAsync(long employeeId, long? fiscalYearId, AppUser user)
        {
            List<long> items = new List<long>();
            try
            {
                var query = $@"SELECT TOP 1 SalaryReviewInfoIds FROM Payroll_SalaryProcessDetail
            Where EmployeeId=@EmployeeId 
            AND (@FiscalYearId=0 OR FiscalYearId=@FiscalYearId)
            AND CompanyId=@CompanyId AND OrganizationId=@OrganizationId
            Order By SalaryDate desc";
                var parameters = new DynamicParameters();
                parameters.Add("EmployeeId", employeeId);
                parameters.Add("FiscalYearId", fiscalYearId ?? 0);
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("OrganizationId", user.OrganizationId);
                string salaryReviewIds = await _dapper.SqlQueryFirstAsync<string>(user.Database, query, parameters, CommandType.Text);

                if (!Utility.IsNullEmptyOrWhiteSpace(salaryReviewIds))
                {
                    var ids = salaryReviewIds.Split(',');
                    foreach (var id in ids)
                    {
                        if (id.IsStringNumber())
                        {
                            items.Add(Convert.ToInt64(id));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "", "GetEmployeeLastSalaryProcessedIds", user);
            }
            return items;
        }
        public async Task<List<long>> GetEmployeeLastSalaryProcessedReviewIdsAsync(long employeeId, long? fiscalYearId, AppUser user)
        {
            List<long> items = new List<long>();
            try
            {
                var query = $@"SELECT TOP 1 SalaryReviewInfoIds FROM Payroll_SalaryProcessDetail
            Where EmployeeId=@EmployeeId 
            AND (@FiscalYearId=0 OR FiscalYearId=@FiscalYearId)
            AND CompanyId=@CompanyId AND OrganizationId=@OrganizationId
            Order By SalaryDate desc";
                var parameters = new DynamicParameters();
                parameters.Add("EmployeeId", employeeId);
                parameters.Add("FiscalYearId", fiscalYearId ?? 0);
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("OrganizationId", user.OrganizationId);
                string salaryReviewIds = await _dapper.SqlQueryFirstAsync<string>(user.Database, query, parameters, CommandType.Text);

                if (!Utility.IsNullEmptyOrWhiteSpace(salaryReviewIds))
                {
                    var ids = salaryReviewIds.Split(',');
                    foreach (var id in ids)
                    {
                        if (id.IsStringNumber())
                        {
                            items.Add(Convert.ToInt64(id));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "", "GetEmployeeLastSalaryProcessedIds", user);
            }
            return items;
        }
        public async Task<List<long>> GetEmployeeSalaryProcessedReviewIdsOfaMonthAsync(long employeeId, int month, int year, long? fiscalYearId, AppUser user)
        {
            List<long> items = new List<long>();
            try
            {
                var query = $@"SELECT SalaryReviewInfoIds FROM Payroll_SalaryProcessDetail
            Where EmployeeId=@EmployeeId 
            AND (@Month=0 OR SalaryMonth=@Month)
            AND (@Year=0 OR SalaryYear=@Year)
            AND (@FiscalYearId=0 OR FiscalYearId=@FiscalYearId)
            AND CompanyId=@CompanyId AND OrganizationId=@OrganizationId
            Order By SalaryDate desc";
                var parameters = new DynamicParameters();
                parameters.Add("EmployeeId", employeeId);
                parameters.Add("Month", month);
                parameters.Add("Year", year);
                parameters.Add("FiscalYearId", fiscalYearId ?? 0);
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("OrganizationId", user.OrganizationId);
                string salaryReviewIds = await _dapper.SqlQueryFirstAsync<string>(user.Database, query, parameters, CommandType.Text);

                if (!Utility.IsNullEmptyOrWhiteSpace(salaryReviewIds))
                {
                    var ids = salaryReviewIds.Split(',');
                    foreach (var id in ids)
                    {
                        if (id.IsStringNumber())
                        {
                            items.Add(Convert.ToInt64(id));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "", "GetEmployeeSalaryProcessedReviewIdsOfaMonthAsync", user);
            }
            return items;
        }
        public async Task<LastSalaryProcessInfoViewModel> GetLastPendingSalaryProcessInfoAsysnc(string processType, int salaryMonth, int salaryYear, AppUser user)
        {
            LastSalaryProcessInfoViewModel lastSalaryProcessInfoViewModel = null;
            try
            {
                var query = $@"SELECT SalaryProcessId,BatchNo,ProcessType,SalaryProcessUniqId FROM Payroll_SalaryProcess Where ProcessType=@ProcessType AND CompanyId= @CompanyId AND OrganizationId=@OrganizationId AND IsDisbursed=0 AND SalaryMonth=@SalaryMonth AND SalaryYear=@SalaryYear";
                var parameters = new DynamicParameters();
                parameters.Add("@ProcessType", processType);
                parameters.Add("@SalaryMonth", salaryMonth);
                parameters.Add("@SalaryYear", salaryYear);
                parameters.Add("@CompanyId", user.CompanyId);
                parameters.Add("@OrganizationId", user.OrganizationId);

                lastSalaryProcessInfoViewModel = await _dapper.SqlQueryFirstAsync<LastSalaryProcessInfoViewModel>(user.Database, query, parameters, CommandType.Text);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "", "GetLastPendingSalaryProcessInfoAsysnc", user);
            }
            return lastSalaryProcessInfoViewModel;
        }
        public async Task<string> GenerateBatchNoAysnc(int salaryMonth, int salaryYear, AppUser user)
        {
            string batchNo = null;
            try
            {
                var query = $@"DECLARE @BatchNo NVARCHAR(50)='SAL-',@BatchMonth NVARCHAR(100), @ProcessCount int = 0;
                SET @BatchMonth=(CASE WHEN LEN(CAST(@SalaryMonth AS NVARCHAR(10))) = 1 THEN '0'+CAST(@SalaryMonth AS NVARCHAR(10)) 
		        ELSE CAST(@SalaryMonth AS NVARCHAR(10)) END);
                SET @ProcessCount= (SELECT COUNT(*) FROM Payroll_SalaryProcess Where SalaryMonth=@SalaryMonth AND SalaryYear=@SalaryYear AND CompanyId=@CompanyId AND OrganizationId=@OrganizationId)+1
                SET @BatchNo = @BatchNo + @BatchMonth + CAST(@SalaryYear AS NVARCHAR(10))+'_'+CAST(@ProcessCount AS NVARCHAR(50));
                SELECT @BatchNo;";

                var parameters = new DynamicParameters();
                parameters.Add("@SalaryMonth", salaryMonth);
                parameters.Add("@SalaryYear", salaryYear);
                parameters.Add("@CompanyId", user.CompanyId);
                parameters.Add("@OrganizationId", user.OrganizationId);
                batchNo = await _dapper.SqlQueryFirstAsync<string>(user.Database, query, parameters, CommandType.Text);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "SalaryProcessBusiness", "GenerateBatchNoAysnc", user);
            }
            return batchNo;
        }
        public async Task<ExecutionStatus> DisbursedSalaryProcessAsync(long salaryProcessId, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                var salaryProcessInDb = await GetSalaryProcessByIdAsync(salaryProcessId, user);
                using (var connection = await _dapper.SqlConnectionForTransactionalAsync(user.Database))
                {
                    connection.Open();
                    {
                        using (var transaction = connection.BeginTransaction())
                        {
                            try
                            {
                                var query = $@"Select COUNT(*) From Payroll_SalaryProcess Where SalaryProcessId = @SalaryProcessId AND IsDisbursed=0 AND CompanyId=@CompanyId AND OrganizationId=@OrganizationId";
                                var count = await _dapper.SqlQueryFirstAsync<int>(user.Database, query, new
                                {
                                    SalaryProcessId = salaryProcessId,
                                    user.CompanyId,
                                    user.OrganizationId
                                }, CommandType.Text);
                                if (count > 0)
                                {
                                    query = $@"Update Payroll_SalaryProcess
				                               Set IsDisbursed = 1,PaymentDate=CAST(GETDATE() AS DATE)
				                               Where SalaryProcessId = @SalaryProcessId AND IsDisbursed=0 AND CompanyId=@CompanyId AND OrganizationId=@OrganizationId";
                                    count = await connection.ExecuteAsync(query, new
                                    {
                                        SalaryProcessId = salaryProcessId,
                                        user.CompanyId,
                                        user.OrganizationId
                                    }, transaction);
                                    if (count == 0)
                                    {
                                        executionStatus.Status = false;
                                        executionStatus.Msg = "Disbursed is failed";
                                    }
                                    else
                                    {
                                        bool isTrue = true;

                                        var list = await _salaryComponentHistoriesBusiness.GetComponentHistoryIds(salaryProcessInDb.SalaryProcessId, user);

                                        if (list.Any())
                                        {
                                            var variableAllowances = list.Where(i => i.Flag == "Variable Allowance").Select(i => i.ComponentId).ToArray();
                                            var variableAllowancesIds = string.Join(",", variableAllowances);
                                            var variableDeductions = list.Where(i => i.Flag == "Variable Deduction").Select(i => i.ComponentId).ToArray();
                                            var variableDeductionsIds = string.Join(",", variableDeductions);

                                            list = null;

                                            if (variableAllowances.Any())
                                            {
                                                query = $@"Update Payroll_MonthlyVariableAllowance
                                                SET StateStatus='Disbursed'
                                                Where MonthlyVariableAllowanceId IN (Select Convert( bigint,[Value]) from fn_split_string_to_column(@Ids,','))";

                                                count = await connection.ExecuteAsync(query, new { Ids = variableAllowancesIds }, transaction);
                                            }

                                            if (variableDeductions.Any())
                                            {
                                                query = $@"Update Payroll_MonthlyVariableDeduction
                                                SET StateStatus='Disbursed'
                                                Where MonthlyVariableDeductionId IN (Select Convert( bigint,[Value]) from fn_split_string_to_column(@Ids,','))";

                                                count = await connection.ExecuteAsync(query, new { Ids = variableDeductionsIds }, transaction);
                                            }
                                        }

                                        #region Payroll_DepositAllowancePaymentHistory
                                        query = $@"Select COUNT(*) From Payroll_DepositAllowancePaymentHistory Where SalaryProcessId = @SalaryProcessId AND IsDisbursed=0 AND CompanyId=@CompanyId AND OrganizationId=@OrganizationId";

                                        count = await _dapper.SqlQueryFirstAsync<int>(user.Database, query, new { SalaryProcessId = salaryProcessId, user.CompanyId, user.OrganizationId }, CommandType.Text);

                                        if (count > 0)
                                        {
                                            query = $@"Update Payroll_DepositAllowancePaymentHistory Set IsDisbursed = 1,StateStatus='Disbursed' Where SalaryProcessId = @SalaryProcessId AND CompanyId=@CompanyId AND OrganizationId=@OrganizationId";

                                            count = await connection.ExecuteAsync(query, new { SalaryProcessId = salaryProcessId, user.CompanyId, user.OrganizationId }, transaction);
                                            if (count == 0)
                                            {
                                                isTrue = false;
                                                executionStatus.Status = false;
                                                executionStatus.Msg = "Disbursed is failed";
                                            }
                                        }
                                        #endregion

                                        #region Payroll_RecipientsofServiceAnniversaryAllowance
                                        query = $@"Select COUNT(*) From Payroll_RecipientsofServiceAnniversaryAllowance Where SalaryProcessId = @SalaryProcessId AND IsDisbursed=0 AND CompanyId=@CompanyId AND OrganizationId=@OrganizationId";

                                        count = await _dapper.SqlQueryFirstAsync<int>(user.Database, query, new { SalaryProcessId = salaryProcessId, user.CompanyId, user.OrganizationId }, CommandType.Text);

                                        if (count > 0)
                                        {
                                            query = $@"Update Payroll_RecipientsofServiceAnniversaryAllowance Set IsDisbursed = 1 Where SalaryProcessId = @SalaryProcessId AND CompanyId=@CompanyId AND OrganizationId=@OrganizationId";

                                            count = await connection.ExecuteAsync(query, new { SalaryProcessId = salaryProcessId, user.CompanyId, user.OrganizationId }, transaction);
                                            if (count == 0)
                                            {
                                                isTrue = false;
                                                executionStatus.Status = false;
                                                executionStatus.Msg = "Disbursed is failed";
                                            }
                                        }
                                        #endregion

                                        #region Wunderman Thompson PF Loan Update
                                        if (user.CompanyId == 19 && user.OrganizationId == 11 && AppSettings.PF_Software_Connection == true)
                                        {
                                            query = $@"SELECT EMP.EmployeeCode,SD.Amount FROM Payroll_SalaryDeduction SD
                                            INNER JOIN HR_EmployeeInformation EMP ON SD.EmployeeId=EMP.EmployeeId
                                            Where DeductionNameId=2 AND SD.SalaryMonth=@SalaryMonth AND SD.SalaryYear=@SalaryYear AND SD.CompanyId=@CompanyId AND SD.OrganizationId=@OrganizationId AND SD.Amount > 0 AND SD.SalaryProcessId = @SalaryProcessId";

                                            var employeeAmountContainer = await _dapper.SqlQueryListAsync<EmployeeAmountContainer>(user.Database, query, new
                                            {
                                                salaryProcessInDb.SalaryMonth,
                                                salaryProcessInDb.SalaryYear,
                                                user.CompanyId,
                                                user.OrganizationId,
                                                salaryProcessInDb.SalaryProcessId
                                            });

                                            if (employeeAmountContainer.Any())
                                            {
                                                query = "sp_ProcessEmpLoanCollection";
                                                foreach (var item in employeeAmountContainer)
                                                {
                                                    var status = await _dapper.SqlExecuteNonQuery(Database.Wunderman_PF, query, new
                                                    {
                                                        Month = salaryProcessInDb.SalaryMonth,
                                                        Year = salaryProcessInDb.SalaryYear,
                                                        SalaryDate = salaryProcessInDb.SalaryDate.Value.ToString("yyyy-MM-dd"),
                                                        ID = item.EmployeeCode
                                                    }, CommandType.StoredProcedure);
                                                }
                                            }
                                        }
                                        #endregion

                                        if (isTrue == true)
                                        {
                                            transaction.Commit();
                                            executionStatus.Status = true;
                                            executionStatus.Msg = "Process haa been disbursed";
                                        }
                                    }
                                }
                                else
                                {
                                    executionStatus.Status = false;
                                    executionStatus.Msg = "Process has already been disbursed";
                                }
                            }
                            catch (Exception ex)
                            {
                                await _sysLogger.SaveHRMSException(ex, user.Database, "SalaryProcessBusiness", "DisbursedSalaryProcessAsync", user);
                                executionStatus = ResponseMessage.Invalid(ResponseMessage.SomthingWentWrong);
                            }
                            finally { connection.Close(); }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                executionStatus = ResponseMessage.Invalid(ResponseMessage.SomthingWentWrong);
                await _sysLogger.SaveHRMSException(ex, user.Database, "SalaryProcessBusiness", "DisbursedSalaryProcessAsync", user);

            }
            return executionStatus;
        }
        public async Task<ExecutionStatus> UndoSalaryProcessAsync(long salaryProcessId, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                using (var connection = await _dapper.SqlConnectionForTransactionalAsync(user.Database))
                {
                    connection.Open();
                    {
                        using (var transaction = connection.BeginTransaction())
                        {
                            try
                            {
                                var filter = new { SalaryProcessId = salaryProcessId, user.CompanyId, user.OrganizationId };
                                var query = $@"Select COUNT(*) From Payroll_SalaryProcess Where SalaryProcessId = @SalaryProcessId AND IsDisbursed=0 AND CompanyId=@CompanyId AND OrganizationId=@OrganizationId";
                                var count = await _dapper.SqlQueryFirstAsync<int>(user.Database, query, filter, CommandType.Text);
                                if (count > 0)
                                {

                                    query = $@"SELECT Count(*) FROM Payroll_SalaryAllowance Where SalaryProcessId =@SalaryProcessId AND CompanyId=@CompanyId AND OrganizationId=@OrganizationId";
                                    count = await _dapper.SqlQueryFirstAsync<int>(user.Database, query, filter, CommandType.Text);

                                    if (count > 0)
                                    {

                                        #region Delete TAX Information

                                        query = $@"DELETE Slab FROM  Payroll_EmployeeTaxProcessSlab Slab
                                        INNER JOIN Payroll_EmployeeTaxProcess PRO ON Slab.TaxProcessId = PRO.TaxProcessId
                                         Where PRO.SalaryProcessId=@SalaryProcessId AND PRO.CompanyId=@CompanyId AND PRO.OrganizationId=@OrganizationId";
                                        count = await connection.ExecuteAsync(query, filter, transaction);

                                        query = $@"DELETE FROM Payroll_EmployeeTaxProcess Where SalaryProcessId=@SalaryProcessId AND CompanyId=@CompanyId AND OrganizationId=@OrganizationId";
                                        count = await connection.ExecuteAsync(query, filter, transaction);

                                        query = $@"DELETE FROM Payroll_EmployeeTaxProcessDetail Where SalaryProcessId=@SalaryProcessId AND CompanyId=@CompanyId AND OrganizationId=@OrganizationId";
                                        count = await connection.ExecuteAsync(query, filter, transaction);

                                        #endregion

                                        query = $@"DELETE FROM Payroll_SalaryAllowance Where SalaryProcessId =@SalaryProcessId AND CompanyId=@CompanyId AND OrganizationId=@OrganizationId";
                                        count = await connection.ExecuteAsync(query, filter, transaction);

                                        query = $@"DELETE FROM Payroll_SalaryAllowanceArrear Where SalaryProcessId =@SalaryProcessId AND CompanyId=@CompanyId AND OrganizationId=@OrganizationId";
                                        count = await connection.ExecuteAsync(query, filter, transaction);

                                        query = $@"DELETE FROM Payroll_SalaryAllowanceAdjustment Where SalaryProcessId =@SalaryProcessId AND CompanyId=@CompanyId AND OrganizationId=@OrganizationId";
                                        count = await connection.ExecuteAsync(query, filter, transaction);

                                        query = $@"DELETE FROM Payroll_SalaryDeduction Where SalaryProcessId =@SalaryProcessId AND CompanyId=@CompanyId AND OrganizationId=@OrganizationId";
                                        count = await connection.ExecuteAsync(query, filter, transaction);

                                        query = $@"DELETE FROM Payroll_SalaryDeductionAdjustment Where SalaryProcessId =@SalaryProcessId AND CompanyId=@CompanyId AND OrganizationId=@OrganizationId";
                                        count = await connection.ExecuteAsync(query, filter, transaction);

                                        query = $@"DELETE FROM Payroll_DepositAllowanceHistory Where SalaryProcessId =@SalaryProcessId AND CompanyId=@CompanyId AND OrganizationId=@OrganizationId";
                                        count = await connection.ExecuteAsync(query, filter, transaction);

                                        query = $@"DELETE FROM Payroll_RecipientsofServiceAnniversaryAllowance Where SalaryProcessId =@SalaryProcessId AND CompanyId=@CompanyId AND OrganizationId=@OrganizationId";
                                        count = await connection.ExecuteAsync(query, filter, transaction);

                                        query = $@"DELETE FROM Payroll_SalaryComponentHistory Where SalaryProcessId =@SalaryProcessId AND CompanyId=@CompanyId AND OrganizationId=@OrganizationId";
                                        count = await connection.ExecuteAsync(query, filter, transaction);

                                        query = $@"DELETE FROM Payroll_MonthlyAllowanceHistory Where SalaryProcessId =@SalaryProcessId AND CompanyId=@CompanyId AND OrganizationId=@OrganizationId";
                                        count = await connection.ExecuteAsync(query, filter, transaction);

                                        query = $@"DELETE FROM Payroll_SalaryProcess Where SalaryProcessId =@SalaryProcessId AND CompanyId=@CompanyId AND OrganizationId=@OrganizationId";
                                        count = await connection.ExecuteAsync(query, filter, transaction);

                                        if (count > 0)
                                        {
                                            transaction.Commit();
                                            executionStatus.Status = true;
                                            executionStatus.Msg = "Process has been undo";
                                        }
                                        else
                                        {
                                            executionStatus.Status = false;
                                            executionStatus.Msg = "Process is failed to undo";
                                        }
                                    }
                                }
                                else
                                {
                                    executionStatus.Status = false;
                                    executionStatus.Msg = "Process has already disbursed/undo";
                                }
                            }
                            catch (Exception ex)
                            {
                                await _sysLogger.SaveHRMSException(ex, user.Database, "SalaryProcessBusiness", "DisbursedSalaryProcessAsync", user);
                                executionStatus = ResponseMessage.Invalid(ResponseMessage.SomthingWentWrong);
                            }
                            finally { connection.Close(); }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                executionStatus = ResponseMessage.Invalid(ResponseMessage.SomthingWentWrong);
                await _sysLogger.SaveHRMSException(ex, user.Database, "SalaryProcessBusiness", "DisbursedSalaryProcessAsync", user);

            }
            return executionStatus;
        }
        public async Task<int> GetSalaryReceiptInThisFiscalYearAsync(long employeeId, long fiscalYearId, int month, AppUser user)
        {
            int count = 0;
            try
            {
                var query = $@"SELECT COUNT(*) FROM Payroll_SalaryProcessDetail WHERE EmployeeId =@EmployeeId AND FiscalYearId = @FiscalYearId AND CompanyId=@CompanyId AND OrganizationId=@OrganizationId AND SalaryMonth<>@Month";
                count = await _dapper.SqlQueryFirstAsync<int>(user.Database, query, new { EmployeeId = employeeId, FiscalYearId = fiscalYearId, Month = month, user.CompanyId, user.OrganizationId });
            }
            catch (Exception ex)
            {
                count = 0;
                await _sysLogger.SaveHRMSException(ex, user.Database, "SalaryProcessBusiness", "DisbursedSalaryProcessAsync", user);
            }
            return count;
        }

        public async Task<int> GetSalaryReceiptInTotalAsync(long employeeId, AppUser user)
        {
            int count = 0;
            try
            {
                var query = $@"SELECT COUNT(*) FROM Payroll_SalaryProcessDetail WHERE EmployeeId =@EmployeeId AND CompanyId=@CompanyId AND OrganizationId=@OrganizationId ";
                count = await _dapper.SqlQueryFirstAsync<int>(user.Database, query, new { EmployeeId = employeeId, user.CompanyId, user.OrganizationId });
            }
            catch (Exception ex)
            {
                count = 0;
                await _sysLogger.SaveHRMSException(ex, user.Database, "SalaryProcessBusiness", "DisbursedSalaryProcessAsync", user);
            }
            return count;
        }
        public async Task<SalaryProcess> GetPendingSalaryProcessInfoAsync(string processType, int salaryMonth, int salaryYear, AppUser user)
        {
            SalaryProcess salaryProcess = null;
            try
            {
                var query = $@"SELECT * FROM Payroll_SalaryProcess Where ProcessType=@ProcessType AND CompanyId= @CompanyId AND OrganizationId=@OrganizationId AND IsDisbursed=0 AND SalaryMonth=@SalaryMonth AND SalaryYear=@SalaryYear";
                salaryProcess = await _dapper.SqlQueryFirstAsync<SalaryProcess>(user.Database, query, new { ProcessType = processType, SalaryMonth = salaryMonth, SalaryYear = salaryYear, user.CompanyId, user.OrganizationId });
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "", "GetPendingSalaryProcessInfoAsysnc", user);
            }
            return salaryProcess;
        }
        public async Task<SalaryProcess> GetSalaryProcessByIdAsync(long salaryProcessId, AppUser user)
        {
            SalaryProcess salaryProcess = new SalaryProcess();
            try
            {
                var query = $@"Select * From Payroll_SalaryProcess Where SalaryProcessId=@SalaryProcessId AND CompanyId=@CompanyId AND OrganizationId=@OrganizationId";
                salaryProcess = await _dapper.SqlQueryFirstAsync<SalaryProcess>(user.Database, query, new { SalaryProcessId = salaryProcessId, user.CompanyId, user.OrganizationId });
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "SalaryProcessBusiness", "GetSalaryProcessByIdAsync", user);
            }
            return salaryProcess;
        }
        public async Task<SalaryProcessDetail> GetSalaryProcessDetailByIdAsync(long salaryProcessDetailId, AppUser user)
        {
            SalaryProcessDetail salaryProcessDetail = new SalaryProcessDetail();
            try
            {
                var query = $@"SELECT * FROM Payroll_SalaryProcessDetail Where SalaryProcessDetailId=@SalaryProcessDetailId AND CompanyId=@CompanyId AND OrganizationId=@OrganizationId";
                salaryProcessDetail = await _dapper.SqlQueryFirstAsync<SalaryProcessDetail>(user.Database, query, new { SalaryProcessDetailId = salaryProcessDetailId, user.CompanyId, user.OrganizationId });
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "SalaryProcessBusiness", "GetSalaryProcessDetailByIdAsync", user);
            }
            return salaryProcessDetail;
        }
        public async Task<DBResponse<SalaryProcessDetailViewModel>> GetSalaryProcessDetailsAsync(SalaryProcessDetail_Filter filter, AppUser user)
        {
            DBResponse<SalaryProcessDetailViewModel> data = new DBResponse<SalaryProcessDetailViewModel>();
            DBResponse response = new DBResponse();
            try
            {
                var query = $@"WITH Data_CTE AS(Select SalaryProcessDetailId, info.BatchNo, detail.EmployeeId,emp.EmployeeCode,Email=(CASE WHEN emp.OfficeEmail IS NULL OR emp.OfficeEmail='' THEN dtl.PersonalEmailAddress ELSE emp.OfficeEmail END), (emp.FullName) 'EmployeeName', detail.DesignationId, 
PersonalEmail=dtl.PersonalEmailAddress,
CalculationForDays=ISNULL(detail.CalculationForDays,0), CurrentBasic, ThisMonthBasic, detail.TotalAllowance, detail.TotalArrearAllowance, detail.PFAmount, detail.PFArrear,OtherDeduction=ISNULL((
		SELECT Amount=SUM(Amount) FROM Payroll_SalaryDeduction 
		Where EmployeeId=detail.EmployeeId AND SalaryMonth=detail.SalaryMonth AND SalaryYear=detail.SalaryYear 
		AND SalaryProcessId=detail.SalaryProcessId),0), [TotalDeduction]=ISNULL((
		SELECT Amount=SUM(Amount) FROM Payroll_SalaryDeduction 
		Where EmployeeId=detail.EmployeeId AND SalaryMonth=detail.SalaryMonth AND SalaryYear=detail.SalaryYear 
		AND SalaryProcessId=detail.SalaryProcessId),0)+ISNULL(detail.PFAmount,0)+ISNULL(detail.PFArrear,0)+ISNULL(detail.TaxDeductedAmount,0),
detail.TotalAllowanceAdjustment,
detail.TotalArrearDeduction, detail.TotalMonthlyTax,detail.TaxDeductedAmount, detail.TotalBonus, detail.GrossPay, detail.NetPay, info.SalaryProcessId, detail.CreatedDate, detail.UpdatedDate, 
info.OrganizationId, info.CompanyId, detail.BankTransferAmount, detail.COCInWalletTransfer, detail.CashAmount, 
detail.CurrentConveyance, detail.CurrentHouseRent, detail.CurrentMedical, detail.WalletTransferAmont, detail.AccountId, detail.BankBranchId, 
detail.BankId, detail.SalaryMonth, detail.SalaryYear,desig.DesignationName, info.SalaryDate,info.ProcessDate,info.IsDisbursed,detail.FiscalYearId,
detail.HoldDays,detail.HoldAmount,detail.UnholdDays,detail.UnholdAmount,detail.ProjectionTax,detail.OnceOffTax
FROM Payroll_SalaryProcessDetail detail
INNER JOIN Payroll_SalaryProcess info on detail.SalaryProcessId = info.SalaryProcessId
INNER JOIN HR_EmployeeInformation emp on detail.EmployeeId = emp.EmployeeId
LEFT JOIN HR_EmployeeDetail dtl on emp.EmployeeId = dtl.EmployeeId
LEFT JOIN HR_Designations desig on detail.DesignationId = desig.DesignationId
Where 1=1
AND (@SalaryProcessId IS NULL OR @SalaryProcessId='' OR @SalaryProcessId = '0' OR info.SalaryProcessId=@SalaryProcessId)
AND (@SalaryProcessDetailId IS NULL OR @SalaryProcessDetailId='' OR @SalaryProcessDetailId = '0' OR detail.SalaryProcessDetailId=@SalaryProcessDetailId)
AND (@EmployeeId IS NULL OR @EmployeeId ='' OR @EmployeeId = '0' OR detail.EmployeeId=@EmployeeId)
AND (@FiscalYearId IS NULL OR @FiscalYearId='' OR @FiscalYearId ='0' OR detail.FiscalYearId=@FiscalYearId)
AND (@BatchNo IS NULL OR @BatchNo = '' OR info.BatchNo LIKE '%'+@BatchNo+'%')
AND (@BranchId IS NULL OR @BranchId = '0'  OR detail.BranchId =@BranchId)
AND (@Month IS NULL OR @Month='' OR @Month = '0'  OR info.SalaryMonth =@Month)
AND (@Year IS NULL OR @Year ='' OR @Year = '0'  OR  info.SalaryYear =@Year)
AND (detail.CompanyId=@CompanyId)
AND (detail.OrganizationId =@OrganizationId)),
	Count_CTE AS (
	SELECT COUNT(*) AS [TotalRows]
	FROM Data_CTE)

	SELECT JSONData=(Select * From (SELECT *
	FROM Data_CTE 
	ORDER BY SalaryProcessDetailId desc
	OFFSET (@PageNumber-1)*@PageSize ROWS
	FETCH NEXT CAST(@PageSize AS INT) ROWS ONLY) tbl FOR JSON AUTO),TotalRows=(Select TotalRows From Count_CTE),PageSize=@PageSize,PageNumber=@PageNumber";
                var parameters = Utility.DappperParams(filter, user, addBaseProperty: true);
                response = await _dapper.SqlQueryFirstAsync<DBResponse>(user.Database, query, parameters, CommandType.Text);
                data.ListOfObject = JsonReverseConverter.JsonToObject<IEnumerable<SalaryProcessDetailViewModel>>(response.JSONData) ?? new List<SalaryProcessDetailViewModel>();
                data.Pageparam = new Pageparam() { PageNumber = response.PageNumber, PageSize = response.PageSize, TotalPages = response.TotalPages, TotalRows = response.TotalRows };
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "SalaryProcessBusiness", "GetSalaryProcessDetailsAsync", user);
            }
            return data;
        }

        public async Task<SalaryProcessDetail> GetSalaryProcessDetailByEmployeeIdAsync(long employeeId, int month, int year, AppUser user)
        {
            SalaryProcessDetail salaryProcessDetail = new SalaryProcessDetail();
            try
            {
                var query = $@"SELECT * FROM Payroll_SalaryProcessDetail Where EmployeeId=@EmployeeId AND SalaryMonth=@Month AND SalaryYear=@Year AND CompanyId=@CompanyId AND OrganizationId=@OrganizationId";
                salaryProcessDetail = await _dapper.SqlQueryFirstAsync<SalaryProcessDetail>(user.Database, query, new { employeeId = employeeId, month = month, year = month, user.CompanyId, user.OrganizationId });
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "SalaryProcessBusiness", "GetSalaryProcessDetailByIdAsync", user);
            }
            return salaryProcessDetail;
        }

        public async Task<DateTime?> GetEmployeeLastSalaryMonthInAIncomeYear(long employeeId, long fiscalYearId, AppUser user)
        {
            DateTime? dateTime = null;
            try
            {
                string query = $@"SELECT 
                MAX(dbo.fnGetLastDateOfAMonth(SPD.SalaryYear,SPD.SalaryMonth)) 
                FROM Payroll_SalaryProcessDetail SPD
                INNER JOIN Payroll_SalaryProcess SP ON SPD.SalaryProcessId = SP.SalaryProcessId
                Where SPD.EmployeeId=@EmployeeId AND SPD.FiscalYearId=@FiscalYearId  AND SP.IsDisbursed=1";

                dateTime = await _dapper.SqlQueryFirstAsync<DateTime>(user.Database, query, new
                {
                    EmployeeId= employeeId,
                    FiscalYearId = fiscalYearId
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dateTime;
        }
    }
}
