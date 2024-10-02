using Dapper;
using System.Data;
using Shared.Services;
using BLL.Base.Interface;
using Shared.OtherModels.User;
using Shared.OtherModels.Response;
using AutoMapper;
using BLL.Administration.Interface;
using BLL.Salary.Allowance.Interface;
using BLL.Salary.Salary.Interface;
using BLL.Salary.Payment.Interface;
using BLL.Salary.Setup.Interface;
using BLL.Salary.Variable.Interface;
using DAL.DapperObject.Interface;
using BLL.Employee.Interface.Stage;
using BLL.PF.Interface;
using Shared.Payroll.ViewModel.Salary;
using Shared.Payroll.Filter.Salary;
using Shared.Payroll.Process.Salary;
using Shared.Payroll.Filter.Allowance;
using Shared.Payroll.ViewModel.Allowance;
using Shared.Payroll.Domain.Salary;
using Shared.Payroll.Domain.Payment;
using Shared.Payroll.Filter.Payment;
using BLL.Salary.Payment.Implementation;
using DAL.Context.Payroll;
using Shared.Payroll.Domain.Setup;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.EntityFrameworkCore.Storage;

namespace BLL.Salary.Salary.Implementation
{
    public class ExecuteSalaryProcess : IExecuteSalaryProcess
    {
        private readonly IDapperData _dapper;
        private readonly IMapper _mapper;
        private readonly ISysLogger _sysLogger;
        private readonly IModuleConfig _moduleConfig;
        private readonly IFiscalYearBusiness _fiscalYearBusiness;
        private readonly ISalaryHoldBusiness _salaryHoldBusiness;
        private readonly ISalaryReviewBusiness _salaryReviewBusiness;
        private readonly ISalaryProcessBusiness _salaryProcessBusiness;
        private readonly IAllowanceNameBusiness _allowanceNameBusiness;
        private readonly IEmployeePFActivationBusiness _employeePFActivationBusiness;
        private readonly IMonthlyVariableAllowanceBusiness _monthlyVariableAllowanceBusiness;
        private readonly IMonthlyVariableDeductionBusiness _monthlyVariableDeductionBusiness;
        private readonly IServiceAnniversaryAllowanceBusiness _serviceAnniversaryAllowanceBusiness;
        private readonly IDepositAllowancePaymentHistoryBusiness _depositAllowancePaymentHistoryBusiness;
        private readonly IConditionalDepositAllowanceConfigBusiness _conditionalDepositAllowanceConfigBusiness;
        private readonly IMonthlyAllowanceConfigBusiness _monthlyAllowanceConfigBusiness;
        private readonly IWundermanPFServiceBusiness _wundermanPFServiceBusiness;
        private readonly IBranchInfoBusiness _branchInfoBusiness;
        private readonly ISalaryAllowanceArrearAdjustmentBusiness _salaryAllowanceArrearAdjustmentBusiness;
        private readonly ISalaryComponentHistoriesBusiness _salaryComponentHistoriesBusiness;
        private readonly PayrollDbContext _payrollDbContext;
        public ExecuteSalaryProcess(IFiscalYearBusiness fiscalYearBusiness, IAllowanceNameBusiness allowanceNameBusiness, ISalaryReviewBusiness salaryReviewBusiness, IDapperData dapper, ISysLogger sysLogger, IModuleConfig moduleConfig, IEmployeePFActivationBusiness employeePFActivationBusiness, ISalaryHoldBusiness salaryHoldBusiness, ISalaryProcessBusiness salaryProcessBusiness, IMonthlyVariableAllowanceBusiness monthlyVariableAllowanceBusiness, IMonthlyVariableDeductionBusiness monthlyVariableDeductionBusiness, IConditionalDepositAllowanceConfigBusiness conditionalDepositAllowanceConfigBusiness, IDepositAllowancePaymentHistoryBusiness depositAllowancePaymentHistoryBusiness, IServiceAnniversaryAllowanceBusiness serviceAnniversaryAllowanceBusiness, IMonthlyAllowanceConfigBusiness monthlyAllowanceConfigBusiness, IMapper mapper, IWundermanPFServiceBusiness wundermanPFServiceBusiness,
            IBranchInfoBusiness branchInfoBusiness, ISalaryAllowanceArrearAdjustmentBusiness salaryAllowanceArrearAdjustmentBusiness, ISalaryComponentHistoriesBusiness salaryComponentHistoriesBusiness, PayrollDbContext payrollDbContext)
        {
            _dapper = dapper;
            _sysLogger = sysLogger;
            _moduleConfig = moduleConfig;
            _fiscalYearBusiness = fiscalYearBusiness;
            _salaryHoldBusiness = salaryHoldBusiness;
            _salaryReviewBusiness = salaryReviewBusiness;
            _allowanceNameBusiness = allowanceNameBusiness;
            _salaryProcessBusiness = salaryProcessBusiness;
            _employeePFActivationBusiness = employeePFActivationBusiness;
            _monthlyVariableAllowanceBusiness = monthlyVariableAllowanceBusiness;
            _monthlyVariableDeductionBusiness = monthlyVariableDeductionBusiness;
            _conditionalDepositAllowanceConfigBusiness = conditionalDepositAllowanceConfigBusiness;
            _depositAllowancePaymentHistoryBusiness = depositAllowancePaymentHistoryBusiness;
            _serviceAnniversaryAllowanceBusiness = serviceAnniversaryAllowanceBusiness;
            _monthlyAllowanceConfigBusiness = monthlyAllowanceConfigBusiness;
            _wundermanPFServiceBusiness = wundermanPFServiceBusiness;
            _mapper = mapper;
            _payrollDbContext = payrollDbContext;
            _branchInfoBusiness = branchInfoBusiness;
            _salaryAllowanceArrearAdjustmentBusiness = salaryAllowanceArrearAdjustmentBusiness;
            _salaryComponentHistoriesBusiness = salaryComponentHistoriesBusiness;
        }
        public async Task<ExecutionStatus> ExecuteProcess(SalaryProcessExecution filter, AppUser user)
        {
            ExecutionStatus executionStatus = null;
            try
            {
                if (filter.ProcessBy == "Systematically")
                {
                    EligibleEmployeeForSalary_Filter query = new EligibleEmployeeForSalary_Filter()
                    {
                        SalaryDate = DateTimeExtension.LastDateOfAMonth(filter.Year, filter.Month).ToString("yyyy-MM-dd"),
                        SelectedEmployees = filter.SelectedEmployees,
                        SelectedDepartmentId = filter.ProcessDepartmentId.ToString(),
                        SelectedBranchId = filter.ProcessBranchId.ToString(),
                        SalaryStartDate = DateTimeExtension.FirstDateOfAMonth(filter.Year, filter.Month).ToString("yyyy-MM-dd"),
                        SalaryMonth = filter.Month,
                        SalaryYear = filter.Year
                    };

                    filter.ProcessDate = filter.SalaryDate;
                    filter.SalaryDate = DateTimeExtension.LastDateOfAMonth(filter.Year, filter.Month);
                    var eligibleEmployees = await GetEligibleEmployees("Systematically", query, user);

                    if (eligibleEmployees.Any())
                    {
                        var list = await RunSystematically(filter, eligibleEmployees.ToList(), user);
                        if (list.Any())
                        {
                            executionStatus = await SaveSalaryAsync("Systematically", filter.Month, filter.Year, filter.IsMargeProcess ?? false, list, user);
                        }
                        else
                        {
                            executionStatus = new ExecutionStatus();
                            executionStatus.Status = true;
                            executionStatus.Msg = "No data found to process";
                        }
                    }
                    else
                    {
                        executionStatus = new ExecutionStatus();
                        executionStatus.Status = true;
                        executionStatus.Msg = "No eligible found to process";
                    }
                }
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ExecuteSalaryProcess", "ExecuteProcess", user);
            }
            return executionStatus;
        }
        public async Task<IEnumerable<EligibleEmployeeForSalaryType>> GetEligibleEmployees(string process, EligibleEmployeeForSalary_Filter filter, AppUser user)
        {
            IEnumerable<EligibleEmployeeForSalaryType> employees = new List<EligibleEmployeeForSalaryType>();
            try
            {

                if (process == "Systematically")
                {
                    var query = $@"Select ROW_NUMBER() Over(Order By emp.EmployeeId) SL,
		        emp.EmployeeId,
		        emp.EmployeeCode,
		        emp.FullName,
		        emp.GradeId,
		        GRD.GradeName,
		        emp.DesignationId,
		        DESIG.DesignationName,
		        emp.DepartmentId,
		        DEPT.DepartmentName,
		        emp.SectionId,
		        SEC.SectionName,
		        emp.SubSectionId,
		        SSEC.SubSectionName,
		        emp.CostCenterId,
		        CST.CostCenterName,
		        CST.CostCenterCode,
		        emp.JobType,
                emp.EmployeeTypeId,
                [EmployeeType]=ETYPE.EmployeeTypeName,
		        emp.BranchId,
		        '',
		        emp.DateOfJoining, 
		        emp.DateOfConfirmation,
                AccountInfoId=ISNULL(EmpAcc.AccountInfoId,0),
		        BankId=ISNULL(EmpAcc.BankId,0),
		        BankBranchId=ISNULL(EmpAcc.BankBranchId,0),
		        BankAccount=ISNULL(EmpAcc.BankAccount,''),
		        WalletAgent=ISNULL(EmpAcc.WalletAgent,''),
		        WalletNumber=ISNULL(EmpAcc.WalletNumber,''),
		        emp.TerminationDate,
		        emp.TerminationStatus, 
		        Religion=dtl.Religion,
		        IsDiscontinued=(CASE 
		        WHEN TerminationDate IS NULL THEN 0
		        WHEN MONTH(TerminationDate)  = MONTH(@SalaryDate) AND YEAR(TerminationDate) = YEAR(@SalaryDate) THEN 1 ELSE 0 END),
		        DaysWorked=0,
		        PFActiovationDate= pfa.ActiveDate,
		        PFEffectiveDate= pfa.PFEffectiveDate,
		        IsActive= emp.IsActive,
		        IsApproved=emp.IsApproved,
		        IsConfirmed = emp.IsConfirmed,
		        IsPFMember= emp.IsPFMember,
		        IsResidential=dtl.IsResidential,
		        IsMobility=dtl.IsMobility,
		        dtl.Gender,
		        dtl.MaritalStatus
		        From HR_EmployeeInformation emp
		        LEFT JOIN HR_EmployeeDetail dtl on emp.EmployeeId= dtl.EmployeeId
		        LEFT JOIN HR_Designations DESIG ON EMP.DesignationId= DESIG.DesignationId
		        LEFT JOIN HR_Grades GRD ON DESIG.GradeId = GRD.GradeId
		        LEFT JOIN HR_Departments DEPT ON EMP.DepartmentId = DEPT.DepartmentId
		        LEFT JOIN HR_Sections SEC ON EMP.SectionId= SEC.SectionId
		        LEFT JOIN HR_SubSections SSEC ON EMP.SubSectionId= SSEC.SubSectionId
                LEFT JOIN HR_EmployeeType ETYPE ON EMP.EmployeeTypeId= ETYPE.EmployeeTypeId
		        LEFT JOIN HR_Costcenter CST ON EMP.CostCenterId = CST.CostCenterId
		        LEFT JOIN HR_EmployeePFActivation pfa on emp.EmployeeId = pfa.EmployeeId AND pfa.IsApproved=1 AND pfa.StateStatus='Approved'
                LEFT JOIN vw_HR_EmployeeAllAccountInfo EmpAcc On emp.EmployeeId = EmpAcc.EmployeeId
		        Where 1=1
		        AND CAST(emp.DateOfJoining as Date) <= @SalaryDate
		        ---- take selected employee
		        AND ((@SelectedEmployees IS NULL OR @SelectedEmployees ='' OR emp.EmployeeId IN (Select Convert( bigint,[Value]) from fn_split_string_to_column(@SelectedEmployees,','))))
		        ---- not in salary process detail
		        AND (emp.EmployeeId NOT IN (Select EmployeeId FROM Payroll_SalaryProcessDetail Where SalaryMonth=@SalaryMonth AND SalaryYear=@SalaryYear AND CompanyId=@CompanyId AND OrganizationId=@OrganizationId))
		        ---- not in salary hold
		        --AND (emp.EmployeeId NOT IN (Select EmployeeId From Payroll_SalaryHold Where [MONTH]=@SalaryMonth AND [YEAR]=@SalaryYear AND CompanyId=@CompanyId AND OrganizationId=@OrganizationId))
		        ----
		        AND (@SelectedDepartmentId IS NULL OR @SelectedDepartmentId=0 OR emp.DepartmentId=@SelectedDepartmentId)
		        AND (@SelectedBranchId  IS NULL OR @SelectedBranchId =0 OR emp.BranchId=@SelectedBranchId)
		        AND (emp.CompanyId=@CompanyId)
		        AND (emp.OrganizationId=@OrganizationId)
		        AND emp.IsActive=1
		        AND 
		        (
			        (emp.TerminationDate IS NULL)
			        OR
			        (emp.TerminationDate IS NOT NULL AND emp.TerminationDate > @SalaryStartDate))";
                    var parameters = DapperParam.AddParams(filter, user, addUserId: false);

                    employees = await _dapper.SqlQueryListAsync<EligibleEmployeeForSalaryType>(user.Database, query, parameters, CommandType.Text);
                }
                else if (process == "Reporcess")
                {
                    var query = $@"Select ROW_NUMBER() Over(Order By emp.EmployeeId) SL,
		        emp.EmployeeId,
		        emp.EmployeeCode,
		        emp.FullName,
		        emp.GradeId,
		        GRD.GradeName,
		        emp.DesignationId,
		        DESIG.DesignationName,
		        emp.DepartmentId,
		        DEPT.DepartmentName,
		        emp.SectionId,
		        SEC.SectionName,
		        emp.SubSectionId,
		        SSEC.SubSectionName,
		        CostCenterId=0,
		        CST.CostCenterName,
		        CST.CostCenterCode,
		        emp.JobType,
                emp.EmployeeTypeId,
                [EmployeeType]=ETYPE.EmployeeTypeName,
		        emp.BranchId,
		        '',
		        emp.DateOfJoining, 
		        emp.DateOfConfirmation,
                AccountInfoId=ISNULL(EmpAcc.AccountInfoId,0),
		        BankId=ISNULL(EmpAcc.BankId,0),
		        BankBranchId=ISNULL(EmpAcc.BankBranchId,0),
		        BankAccount=ISNULL(EmpAcc.BankAccount,''),
		        WalletAgent=ISNULL(EmpAcc.WalletAgent,''),
		        WalletNumber=ISNULL(EmpAcc.WalletNumber,''),
		        emp.TerminationDate,
		        emp.TerminationStatus, 
		        Religion=dtl.Religion,
		        IsDiscontinued=(CASE 
		        WHEN TerminationDate IS NULL THEN 0
		        WHEN MONTH(TerminationDate)  = MONTH(@SalaryDate) AND YEAR(TerminationDate) = YEAR(@SalaryDate) THEN 1 ELSE 0 END),
		        DaysWorked=0,
		        PFActiovationDate= pfa.ActiveDate,
		        PFEffectiveDate= pfa.PFEffectiveDate,
		        IsActive= emp.IsActive,
		        IsApproved=emp.IsApproved,
		        IsConfirmed = emp.IsConfirmed,
		        IsPFMember= emp.IsPFMember,
		        IsResidential=dtl.IsResidential,
		        IsMobility=dtl.IsMobility,
		        dtl.Gender,
		        dtl.MaritalStatus
		        From HR_EmployeeInformation emp
		        LEFT JOIN HR_EmployeeDetail dtl on emp.EmployeeId= dtl.EmployeeId
		        LEFT JOIN HR_Designations DESIG ON EMP.DesignationId= DESIG.DesignationId
		        LEFT JOIN HR_Grades GRD ON DESIG.GradeId = GRD.GradeId
		        LEFT JOIN HR_Departments DEPT ON EMP.DepartmentId = DEPT.DepartmentId
		        LEFT JOIN HR_Sections SEC ON EMP.SectionId= SEC.SectionId
		        LEFT JOIN HR_SubSections SSEC ON EMP.SubSectionId= SSEC.SubSectionId
                LEFT JOIN HR_EmployeeType ETYPE ON EMP.EmployeeTypeId= ETYPE.EmployeeTypeId
		        LEFT JOIN HR_Costcenter CST ON EMP.CostCenterId = CST.CostCenterId
		        LEFT JOIN HR_EmployeePFActivation pfa on emp.EmployeeId = pfa.EmployeeId AND pfa.IsApproved=1 AND pfa.StateStatus='Approved'
                LEFT JOIN vw_HR_EmployeeAllAccountInfo EmpAcc On emp.EmployeeId = EmpAcc.EmployeeId
		        Where 1=1
		        AND CAST(emp.DateOfJoining as Date) <= @SalaryDate
		        ---- take selected employee
		        AND ((@SelectedEmployees ='' OR emp.EmployeeId IN (Select Convert( bigint,[Value]) from fn_split_string_to_column(@SelectedEmployees,','))))
		        ---- not in salary hold
		        --AND (emp.EmployeeId NOT IN (Select EmployeeId From Payroll_SalaryHold Where [MONTH]=@SalaryMonth AND [YEAR]=@SalaryYear AND CompanyId=@CompanyId AND OrganizationId=@OrganizationId))
		        ----
		        AND (@SelectedDepartmentId IS NULL OR @SelectedDepartmentId=0 OR emp.DepartmentId=@SelectedDepartmentId)
		        AND (@SelectedBranchId  IS NULL OR @SelectedBranchId =0 OR emp.BranchId=@SelectedBranchId)
		        AND (emp.CompanyId=@CompanyId)
		        AND (emp.OrganizationId=@OrganizationId)
		        AND emp.IsActive=1
		        AND 
		        (
			        (emp.TerminationDate IS NULL)
			        OR
			        (emp.TerminationDate IS NOT NULL AND emp.TerminationDate > @SalaryStartDate))";
                    var parameters = DapperParam.AddParams(filter, user, addUserId: false);

                    employees = await _dapper.SqlQueryListAsync<EligibleEmployeeForSalaryType>(user.Database, query, parameters, CommandType.Text);
                }
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ExecuteSalaryProcess", "GetEligibleEmployees", user);
            }
            return employees;
        }
        public async Task<int> GetEmployeeHoldDays(PresentCountBetweenSalaryDates_Filter parameters, AppUser user)
        {
            int holdDays = 0;
            try
            {
                bool joinedInBetweenDates = false; bool terminatedInBetweenDates = false;
                DateTime? startDate = null; DateTime? endDate = null;
                int days = 0;
                joinedInBetweenDates = parameters.JoiningDate.Value.IsDateBetweenTwoDates(parameters.FirstDate.Value, parameters.SecondDate.Value);
                terminatedInBetweenDates = parameters.TerminationDate != null ? parameters.TerminationDate.Value.IsDateBetweenTwoDates(parameters.FirstDate.Value, parameters.SecondDate.Value) : false;

                startDate = joinedInBetweenDates == false ? parameters.FirstDate : parameters.JoiningDate;
                endDate = terminatedInBetweenDates == false ? parameters.SecondDate : parameters.TerminationDate;
                days = startDate.Value.DaysBetweenDateRangeIncludingStartDate(endDate.Value);

                var parametersToFindHoldDays = new DynamicParameters();
                parametersToFindHoldDays.Add("StartDate", startDate);
                parametersToFindHoldDays.Add("EndDate", endDate);
                parametersToFindHoldDays.Add("EmployeeId", parameters.EmployeeId);

                var holdDaysQuery = $@"SELECT ISNULL((Select Count(tbl1.[Date]) From  dbo.[DateRangeTable](Cast(@StartDate AS NVARCHAR(20)),CAST(@EndDate AS NVARCHAR(20))) tbl1
					JOIN Payroll_SalaryHold tbl2 on 1=1
					Where EmployeeId=@EmployeeId AND tbl1.[Date] Between HoldFrom AND HoldTo AND tbl2.IsApproved=1 AND tbl2.IsHolded=1),0)";

                holdDays = await _dapper.SqlQueryFirstAsync<int>(user.Database, holdDaysQuery, parametersToFindHoldDays, CommandType.Text);
            }
            catch (Exception ex)
            {
                holdDays = 0;
                await _sysLogger.SaveHRMSException(ex, user.Database, "ExecuteSalaryProcess", "GetPresentCountBetweenSalaryDates", user);
            }
            return holdDays;
        }
        public async Task<int> GetPresentCountBetweenSalaryDates(PresentCountBetweenSalaryDates_Filter parameters, AppUser user)
        {
            int daysWorked = 0;
            try
            {
                bool joinedInBetweenDates = false; bool terminatedInBetweenDates = false;
                DateTime? startDate = null; DateTime? endDate = null;
                int days = 0; int holdDays = 0; int absentQty = 0;
                joinedInBetweenDates = parameters.JoiningDate.Value.IsDateBetweenTwoDates(parameters.FirstDate.Value, parameters.SecondDate.Value);
                terminatedInBetweenDates = parameters.TerminationDate != null ? parameters.TerminationDate.Value.IsDateBetweenTwoDates(parameters.FirstDate.Value, parameters.SecondDate.Value) : false;

                startDate = joinedInBetweenDates == false ? parameters.FirstDate : parameters.JoiningDate;
                endDate = terminatedInBetweenDates == false ? parameters.SecondDate : parameters.TerminationDate;
                days = startDate.Value.DaysBetweenDateRangeIncludingStartDate(endDate.Value);

                var parametersToFindHoldDays = new DynamicParameters();
                parametersToFindHoldDays.Add("StartDate", startDate);
                parametersToFindHoldDays.Add("EndDate", endDate);
                parametersToFindHoldDays.Add("EmployeeId", parameters.EmployeeId);

                var holdDaysQuery = $@"SELECT ISNULL((Select Count(tbl1.[Date]) From  dbo.[DateRangeTable](Cast(@StartDate AS NVARCHAR(20)),CAST(@EndDate AS NVARCHAR(20))) tbl1
					JOIN Payroll_SalaryHold tbl2 on 1=1
					Where EmployeeId=@EmployeeId AND tbl1.[Date] Between HoldFrom AND HoldTo AND tbl2.IsApproved=1 AND tbl2.IsHolded=1),0)";

                holdDays = await _dapper.SqlQueryFirstAsync<int>(user.Database, holdDaysQuery, parametersToFindHoldDays, CommandType.Text);

                daysWorked = days - (absentQty + holdDays);

            }
            catch (Exception ex)
            {
                daysWorked = 0;
                await _sysLogger.SaveHRMSException(ex, user.Database, "ExecuteSalaryProcess", "GetPresentCountBetweenSalaryDates", user);
            }
            return daysWorked;
        }
        public async Task<int> GetPresentCountBetweenSalaryDatesWhenSalaryWasHold(PresentCountBetweenSalaryDates_Filter parameters, AppUser user)
        {
            int daysWorked = 0;
            try
            {
                bool joinedInBetweenDates = false; bool terminatedInBetweenDates = false;
                DateTime? startDate = null; DateTime? endDate = null;
                int days = 0; int holdDays = 0; int absentQty = 0;
                joinedInBetweenDates = parameters.JoiningDate.Value.IsDateBetweenTwoDates(parameters.FirstDate.Value, parameters.SecondDate.Value);
                terminatedInBetweenDates = parameters.TerminationDate.Value.IsDateBetweenTwoDates(parameters.FirstDate.Value, parameters.SecondDate.Value);

                startDate = joinedInBetweenDates == false ? parameters.FirstDate : parameters.JoiningDate;
                endDate = terminatedInBetweenDates == false ? parameters.SecondDate : parameters.TerminationDate;
                days = startDate.Value.DaysBetweenDateRangeIncludingStartDate(endDate.Value);

                daysWorked = days - (absentQty + holdDays);
            }
            catch (Exception ex)
            {
                daysWorked = 0;
                await _sysLogger.SaveHRMSException(ex, user.Database, "ExecuteSalaryProcess", "GetPresentCountBetweenSalaryDates", user);
            }
            return daysWorked;
        }
        public async Task<ExecutionStatus> ReProcess(SalaryReprocess reprocess, AppUser user)
        {
            ExecutionStatus executionStatus = null;
            try
            {
                var salaryProcessInfo = await _salaryProcessBusiness.GetSalaryProcessByIdAsync(reprocess.SalaryProcessId, user);
                if (salaryProcessInfo != null && salaryProcessInfo.ProcessType == "Systematically" && salaryProcessInfo.IsDisbursed == false)
                {
                    if (salaryProcessInfo.IsDisbursed == false)
                    {
                        var employee = await GetEligibleEmployees("Reporcess", new EligibleEmployeeForSalary_Filter()
                        {
                            SelectedEmployees = reprocess.EmployeeId.ToString(),
                            SalaryMonth = reprocess.Month,
                            SalaryYear = reprocess.Year,
                            SalaryDate = DateTimeExtension.LastDateOfAMonth(reprocess.Year, reprocess.Month).ToString("yyyy-MM-dd"),
                            SalaryStartDate = DateTimeExtension.FirstDateOfAMonth(reprocess.Year, reprocess.Month).ToString("yyyy-MM-dd")
                        }, user); ;

                        if (employee != null && employee.Any())
                        {
                            var deleteEmployeeSalary = await DeleteSingleEmployeeSalaryAsync(reprocess, user);


                            if (deleteEmployeeSalary != null && deleteEmployeeSalary.Status)
                            {
                                var list = await this.RunSystematically(new SalaryProcessExecution()
                                {
                                    Month = salaryProcessInfo.SalaryMonth,
                                    Year = salaryProcessInfo.SalaryYear,
                                }, employee.ToList(), user);
                                if (list.Any())
                                {
                                    executionStatus = await UpdateSalaryAsync("Systematically", salaryProcessInfo.SalaryProcessId, 0, salaryProcessInfo.SalaryMonth, salaryProcessInfo.SalaryYear, list.First(), user);
                                    if (executionStatus.Status)
                                    {
                                        executionStatus.Ids = employee.First().EmployeeId.ToString();
                                    }
                                }
                                else
                                {
                                    executionStatus = new ExecutionStatus();
                                    executionStatus.Status = true;
                                    executionStatus.Msg = "No data found to process";
                                }
                            }
                            else
                            {
                                executionStatus.Status = false;
                                executionStatus.Msg = "Cannot erase the previous salary data";
                            }
                        }
                        else
                        {
                            executionStatus.Status = false;
                            executionStatus.Msg = "Employee not found";
                        }
                    }
                }
                else if (salaryProcessInfo != null && salaryProcessInfo.ProcessType != "Systematically")
                {

                }
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ExecuteSalaryProcess", "ReProcess", user);
            }
            return executionStatus;
        }
        public async Task<List<EmployeeSalaryProcessedInfo>> RunSystematically(SalaryProcessExecution execution, List<EligibleEmployeeForSalaryType> eligibleEmployees, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            List<EmployeeSalaryProcessedInfo> listOfemployeeSalaryProcessed = new List<EmployeeSalaryProcessedInfo>();
            if (eligibleEmployees.Any())
            {
                var payrollModuleConfig = await _moduleConfig.PayrollModuleConfig(user);
                decimal pfPercentage = Utility.TryParseInt(payrollModuleConfig.PercentageOfProvidentFund);
                var fiscalYearInfo = await _fiscalYearBusiness.GetFiscalYearInfoWithinADate(DateTimeExtension.LastDateOfAMonth(execution.Year, execution.Month).ToString("yyyy-MM-dd"), user);

                var salaryStartDate = DateTimeExtension.FirstDateOfAMonth(execution.Year, execution.Month);
                var salaryEndDate = DateTimeExtension.LastDateOfAMonth(execution.Year, execution.Month);

                var branchInfos = await _branchInfoBusiness.GetBranchsAsync(null, user);

                var basicAllowance = (await _allowanceNameBusiness.GetAllowanceNamesAsync(new AllowanceName_Filter()
                {
                    AllowanceFlag = "BASIC"
                }, user)).FirstOrDefault();
                AllowanceNameViewModel basicAllowanceInfo = null;
                if (basicAllowance != null)
                {
                    basicAllowanceInfo = basicAllowance;
                }

                var houseAllowance = (await _allowanceNameBusiness.GetAllowanceNamesAsync(new AllowanceName_Filter()
                {
                    AllowanceFlag = "HR"
                }, user)).FirstOrDefault();
                AllowanceNameViewModel houseAllowanceInfo = null;
                if (houseAllowance != null)
                {
                    houseAllowanceInfo = houseAllowance;
                }

                var conveyanceAllowance = (await _allowanceNameBusiness.GetAllowanceNamesAsync(new AllowanceName_Filter()
                {
                    AllowanceFlag = "CONVEYANCE"
                }, user)).FirstOrDefault();
                AllowanceNameViewModel conveyanceAllowanceInfo = null;
                if (conveyanceAllowance != null)
                {
                    conveyanceAllowanceInfo = conveyanceAllowance;
                }

                var medicalAllowance = (await _allowanceNameBusiness.GetAllowanceNamesAsync(new AllowanceName_Filter()
                {
                    AllowanceFlag = "MEDICAL"
                }, user)).FirstOrDefault();
                AllowanceNameViewModel medicalAllowanceInfo = null;
                if (conveyanceAllowance != null)
                {
                    medicalAllowanceInfo = medicalAllowance;
                }


                try
                {
                    bool isActualDays = string.IsNullOrEmpty(payrollModuleConfig.WhatDoesConsiderationForMonth) ? true :
                        payrollModuleConfig.WhatDoesConsiderationForMonth == "Actual Days" ? true : false;
                    int actualDaysInSalaryMonth = salaryEndDate.DaysInAMonth();
                    int daysInSalaryMonth = isActualDays == true ? actualDaysInSalaryMonth : 30;
                    foreach (var employee in eligibleEmployees)
                    {
                        EmployeeSalaryProcessedInfo employeeSalaryProcessed = new EmployeeSalaryProcessedInfo();
                        List<SalaryAllowance> list_of_salary_allowance = new List<SalaryAllowance>();
                        List<SalaryAllowanceArrear> list_of_salary_allowance_arrear = new List<SalaryAllowanceArrear>();
                        List<SalaryAllowanceAdjustment> list_of_salary_allowance_adjustment = new List<SalaryAllowanceAdjustment>();
                        List<SalaryDeduction> list_of_salary_deduction = new List<SalaryDeduction>();
                        List<SalaryDeductionAdjustment> list_of_salary_deduction_adjustment = new List<SalaryDeductionAdjustment>();
                        List<DepositAllowanceHistory> list_of_deposite_Allowance_History = new List<DepositAllowanceHistory>();
                        List<SalaryComponentHistory> list_of_salary_component_History = new List<SalaryComponentHistory>();
                        List<MonthlyAllowanceHistory> list_of_monthly_allowance_History = new List<MonthlyAllowanceHistory>();


                        var employeePFInfo = await _employeePFActivationBusiness.EmployeePFActionInfoAysnc(employee.EmployeeId, user);
                        int totalDayWorked = 0;
                        int holdDays = 0;
                        decimal holdAmount = 0;
                        var salaryInfos = await _salaryReviewBusiness.GetEmployeeSalaryReviewesInSalaryProcess(new SalaryReviewInSalaryProcess_Filter()
                        {
                            EmployeeId = employee.EmployeeId.ToString(),
                            SalaryStartDate = salaryStartDate.ToString("yyyy-MM-dd"),
                            SalaryEndDate = salaryEndDate.ToString("yyyy-MM-dd"),
                        }, user);

                        decimal pfAmount = 0; decimal pfArrear = 0;
                        foreach (var salaryInfo in salaryInfos)
                        {
                            decimal salaryDiffAmount = 0;
                            var salaryReviewDetails = await _salaryReviewBusiness.GetSalaryReviewDetailsAsync(salaryInfo.SalaryReviewInfoId, user);
                            var grossAmount = salaryReviewDetails.Sum(i => i.CurrentAmount);
                            var perDayGross = Math.Round(grossAmount / daysInSalaryMonth);

                            decimal basicAmount = 0;
                            if (basicAllowanceInfo != null)
                            {
                                var basicAllowanceInSalaryReview = salaryReviewDetails.FirstOrDefault(i => i.AllowanceNameId == basicAllowanceInfo.AllowanceNameId);
                                if (basicAllowanceInSalaryReview != null)
                                {
                                    basicAmount = basicAllowanceInSalaryReview.CurrentAmount;
                                }
                            }
                            var perDayBasic = basicAmount > 0 ? Math.Round(basicAmount / daysInSalaryMonth) : 0;
                            int daysWorked = 0;

                            if (salaryInfo.DeactivationDate == null || salaryInfo.DeactivationDate.Value > salaryStartDate)
                            {
                                DateTime? startDate = null;
                                DateTime? endDate = null;
                                startDate = salaryInfo.EffectiveFrom.Value <= salaryStartDate ? salaryStartDate : salaryInfo.EffectiveFrom.Value > salaryStartDate ? salaryInfo.EffectiveFrom.Value : salaryStartDate;

                                endDate = salaryInfo.EffectiveTo == null ? salaryEndDate : salaryInfo.EffectiveTo.Value >= salaryEndDate ? salaryEndDate : salaryInfo.EffectiveTo.Value < salaryEndDate ? salaryInfo.EffectiveTo.Value : salaryEndDate;

                                var presentCountParams = new PresentCountBetweenSalaryDates_Filter()
                                {
                                    EmployeeId = employee.EmployeeId,
                                    FirstDate = startDate,
                                    SecondDate = endDate,
                                    JoiningDate = employee.DateOfJoining,
                                    IsActualDays = isActualDays,
                                    SalaryDate = execution.SalaryDate,
                                    TerminationDate = employee.TerminationDate

                                };
                                daysWorked = await GetPresentCountBetweenSalaryDates(presentCountParams, user);

                                if (actualDaysInSalaryMonth != daysInSalaryMonth)
                                {
                                    if (daysWorked == actualDaysInSalaryMonth)
                                    {
                                        daysWorked = daysInSalaryMonth;
                                    }
                                    else if (daysWorked < actualDaysInSalaryMonth && actualDaysInSalaryMonth >= daysInSalaryMonth)
                                    {
                                        daysWorked = daysInSalaryMonth - (daysInSalaryMonth - daysWorked);
                                    }
                                    else if (daysWorked < actualDaysInSalaryMonth && actualDaysInSalaryMonth == 28 && actualDaysInSalaryMonth < daysInSalaryMonth)
                                    {
                                        daysWorked = daysInSalaryMonth - (daysInSalaryMonth - (daysWorked + 2));
                                    }
                                    else if (daysWorked < actualDaysInSalaryMonth && actualDaysInSalaryMonth == 29 && actualDaysInSalaryMonth < daysInSalaryMonth)
                                    {
                                        daysWorked = daysInSalaryMonth - (daysInSalaryMonth - (daysWorked + 1));
                                    }
                                    else
                                    {
                                        daysWorked = daysInSalaryMonth;
                                    }
                                }


                                int currentHoldDays = await GetEmployeeHoldDays(presentCountParams, user);
                                if (user.CompanyId == 17 && user.OrganizationId == 10)
                                {
                                    daysWorked = daysWorked + currentHoldDays;
                                }

                                foreach (var salaryReview in salaryReviewDetails)
                                {
                                    //if (salaryReview.AllowanceNameId != 8) {

                                    //}
                                    SalaryAllowance salaryAllowance = new SalaryAllowance();
                                    salaryAllowance.EmployeeId = employee.EmployeeId;
                                    salaryAllowance.SalaryMonth = execution.Month;
                                    salaryAllowance.SalaryYear = execution.Year;
                                    salaryAllowance.SalaryDate = DateTimeExtension.LastDateOfAMonth(execution.Year, execution.Month);
                                    salaryAllowance.CalculationForDays = daysWorked;
                                    salaryAllowance.AllowanceNameId = salaryReview.AllowanceNameId;
                                    salaryAllowance.Amount = Math.Round(salaryReview.CurrentAmount / daysInSalaryMonth * daysWorked + (salaryReview.AdditionalAmount ?? 0), MidpointRounding.AwayFromZero);
                                    salaryAllowance.ArrearAmount = 0;
                                    salaryAllowance.Remarks = "";
                                    salaryAllowance.CreatedBy = user.ActionUserId;
                                    salaryAllowance.CreatedDate = DateTime.Now;
                                    salaryAllowance.BranchId = employee.BranchId;
                                    salaryAllowance.CompanyId = user.CompanyId;
                                    salaryAllowance.OrganizationId = user.OrganizationId;
                                    salaryAllowance.SalaryReviewInfoId = salaryInfo.SalaryReviewInfoId;
                                    salaryAllowance.FiscalYearId = fiscalYearInfo.FiscalYearId;
                                    list_of_salary_allowance.Add(salaryAllowance);
                                }

                                totalDayWorked = totalDayWorked + daysWorked;
                                currentHoldDays = currentHoldDays > daysInSalaryMonth ? daysInSalaryMonth : currentHoldDays;
                                holdDays = holdDays + currentHoldDays;

                                if (currentHoldDays > 0)
                                {
                                    foreach (var salaryReview in salaryReviewDetails)
                                    {
                                        //if (salaryReview.AllowanceNameId != 8) {

                                        //}
                                        var amt = salaryReview.CurrentAmount / daysInSalaryMonth * currentHoldDays;
                                        holdAmount = holdAmount + Math.Round(amt);
                                    }
                                }

                            }

                            // Arrear Calculation
                            if (salaryInfo.IsArrearCalculated == false && salaryInfo.ArrearCalculatedDate != null
                                && salaryInfo.EffectiveFrom.Value.Date < salaryInfo.ActivationDate.Value.Date
                                && salaryInfo.ArrearCalculatedDate.Value.Month == salaryStartDate.Month
                                && salaryInfo.ArrearCalculatedDate.Value.Year == salaryStartDate.Year)
                            {

                                if (salaryInfo.PreviousSalaryAmount == 0)
                                {
                                    salaryDiffAmount = salaryInfo.PreviousSalaryAmount;
                                }
                                else if (salaryInfo.CurrentSalaryAmount > salaryInfo.PreviousSalaryAmount)
                                {
                                    salaryDiffAmount = salaryInfo.CurrentSalaryAmount - salaryInfo.PreviousSalaryAmount;
                                }
                                else if (salaryInfo.CurrentSalaryAmount < salaryInfo.PreviousSalaryAmount)
                                {
                                    salaryDiffAmount = salaryInfo.PreviousSalaryAmount - salaryInfo.CurrentSalaryAmount;
                                }
                                else
                                {
                                    salaryDiffAmount = 0;
                                }
                                var salaryReviewMonthDiff = salaryInfo.EffectiveFrom.Value.GetMonthDiffExcludingThisMonth(salaryInfo.ActivationDate.Value);

                                var arrearMonthDate = salaryInfo.EffectiveFrom.Value;

                                int salaryReviewDayDiff = 0;
                                int daysInArrearMonth = 0;

                                for (int arrearMonthCount = 1; arrearMonthCount <= salaryReviewMonthDiff; arrearMonthCount++)
                                {
                                    if (arrearMonthCount == 1)
                                    {
                                        salaryReviewDayDiff = arrearMonthDate.DaysBetweenDateRangeIncludingStartDate(arrearMonthDate.LastDateOfAMonth());
                                    }
                                    else if (arrearMonthCount > 1 && arrearMonthCount < salaryReviewMonthDiff)
                                    {
                                        salaryReviewDayDiff = arrearMonthDate.DaysBetweenDateRangeIncludingStartDate(arrearMonthDate.LastDateOfAMonth());
                                    }
                                    else
                                    {
                                        salaryReviewDayDiff = arrearMonthDate.DaysBetweenDateRangeIncludingStartDate(salaryInfo.ActivationDate.Value.AddDays(-1));
                                    }

                                    daysInArrearMonth = arrearMonthDate.DaysInAMonth();
                                    foreach (var salaryReview in salaryReviewDetails)
                                    {

                                        var isTrue = this.IsItSpecialAllowanceOfPWCForTheMonthJune(employee.EmployeeCode, salaryReview.AllowanceNameId, salaryStartDate.Year, salaryStartDate.Month, user);
                                        if (isTrue == false)
                                        {
                                            SalaryAllowanceArrear salaryAllowanceArrear = new SalaryAllowanceArrear();
                                            salaryAllowanceArrear.EmployeeId = employee.EmployeeId;
                                            salaryAllowanceArrear.AllowanceNameId = salaryReview.AllowanceNameId;

                                            salaryAllowanceArrear.SalaryDate = DateTimeExtension.LastDateOfAMonth(execution.Year, execution.Month);
                                            salaryAllowanceArrear.SalaryMonth = execution.Month;
                                            salaryAllowanceArrear.SalaryYear = execution.Year;
                                            salaryAllowanceArrear.CalculationForDays = salaryReviewDayDiff;
                                            if (salaryReview.CurrentAmount >= salaryReview.PreviousAmount)
                                            {
                                                salaryAllowanceArrear.Amount = Math.Round((salaryReview.CurrentAmount - salaryReview.PreviousAmount) / daysInArrearMonth * salaryReviewDayDiff, 0);
                                            }
                                            salaryAllowanceArrear.ArrearMonth = (short)arrearMonthDate.Month;
                                            salaryAllowanceArrear.ArrearYear = (short)arrearMonthDate.Year;
                                            salaryAllowanceArrear.ArrearFrom = null;
                                            salaryAllowanceArrear.ArrearTo = null;
                                            salaryAllowanceArrear.SalaryReviewInfoId = salaryReview.SalaryReviewInfoId;
                                            salaryAllowanceArrear.CreatedBy = user.ActionUserId;
                                            salaryAllowanceArrear.CreatedDate = DateTime.Now;
                                            salaryAllowanceArrear.BranchId = employee.BranchId;
                                            salaryAllowanceArrear.CompanyId = user.CompanyId;
                                            salaryAllowanceArrear.OrganizationId = user.OrganizationId;
                                            salaryAllowanceArrear.FiscalYearId = fiscalYearInfo.FiscalYearId;
                                            list_of_salary_allowance_arrear.Add(salaryAllowanceArrear);
                                        }

                                    }
                                    arrearMonthDate = arrearMonthDate.AddMonths(1);
                                    arrearMonthDate = arrearMonthDate.FirstDateOfAMonth();
                                    salaryReviewDayDiff = 0;
                                }
                            }

                            // PF Calculation
                            int pfDays = 0; int totalPFDays = 0;
                            if (payrollModuleConfig.IsProvidentFundactivated.Value == true
                                && Utility.TryParseInt(payrollModuleConfig.PercentageOfProvidentFund) > 0
                                && daysWorked > 0)
                            {
                                decimal earnedAmount = 0; decimal arrearAmount = 0;
                                if (payrollModuleConfig.BaseOfProvidentFund.ToLower() == "basic")
                                {
                                    earnedAmount = list_of_salary_allowance.Where(i => (i.SalaryReviewInfoId ?? 0) == salaryInfo.SalaryReviewInfoId && i.AllowanceNameId == basicAllowanceInfo.AllowanceNameId).Sum(i => i.Amount);
                                    arrearAmount = list_of_salary_allowance_arrear.Where(i => (i.SalaryReviewInfoId ?? 0) == salaryInfo.SalaryReviewInfoId && i.AllowanceNameId == basicAllowanceInfo.AllowanceNameId).Sum(i => i.Amount);
                                }
                                else
                                {
                                    earnedAmount = list_of_salary_allowance.Where(i => (i.SalaryReviewInfoId ?? 0) == salaryInfo.SalaryReviewInfoId).Sum(i => i.Amount);
                                    arrearAmount = list_of_salary_allowance_arrear.Where(i => (i.SalaryReviewInfoId ?? 0) == salaryInfo.SalaryReviewInfoId).Sum(i => i.Amount);
                                }

                                if (employee.PFActiovationDate == null && employee.IsPFMember == true)
                                {
                                    if (salaryInfo.EffectiveFrom.Value.Month == execution.Month
                                        && salaryInfo.EffectiveFrom.Value.Year == execution.Year
                                        && salaryInfo.ActivationDate.Value.Month == execution.Month
                                        && salaryInfo.ActivationDate.Value.Year == execution.Year)
                                    {
                                        if (salaryInfo.ActivationDate.Value.IsDateBetweenTwoDates(salaryStartDate, salaryEndDate) && salaryInfo.DeactivationDate == null)
                                        {
                                            pfDays = salaryInfo.ActivationDate.Value.DaysBetweenDateRangeIncludingStartDate(salaryEndDate);
                                            totalPFDays = totalPFDays + pfDays;
                                            pfAmount = pfAmount + earnedAmount * (pfPercentage / 100);
                                            pfArrear = pfArrear + arrearAmount * (pfPercentage / 100);
                                        }
                                    }
                                    else if (salaryInfo.EffectiveFrom.Value.Month == execution.Month && salaryInfo.EffectiveFrom.Value.Year == execution.Year
                                        && salaryInfo.ActivationDate.Value < salaryStartDate)
                                    {
                                        pfDays = salaryInfo.ActivationDate.Value.DaysBetweenDateRangeIncludingStartDate(salaryEndDate);
                                        totalPFDays = totalPFDays + pfDays;
                                        pfAmount = pfAmount + earnedAmount * (pfPercentage / 100);
                                        pfArrear = pfArrear + arrearAmount * (pfPercentage / 100);
                                    }
                                    else if (salaryInfo.EffectiveFrom.Value < salaryStartDate && salaryInfo.DeactivationDate == null)
                                    {
                                        pfDays = salaryStartDate.DaysBetweenDateRangeIncludingStartDate(salaryEndDate);
                                        totalPFDays = totalPFDays + pfDays;
                                        pfAmount = pfAmount + earnedAmount * (pfPercentage / 100);
                                        pfArrear = pfArrear + arrearAmount * (pfPercentage / 100);
                                    }
                                    else if (salaryInfo.EffectiveFrom.Value < salaryStartDate && salaryInfo.DeactivationDate != null && salaryInfo.DeactivationDate.Value.IsDateBetweenTwoDates(salaryStartDate, salaryEndDate) == true)
                                    {
                                        pfDays = salaryStartDate.DaysBetweenDateRangeIncludingStartDate(salaryInfo.DeactivationDate.Value.Date);
                                        totalPFDays = totalPFDays + pfDays;
                                        pfAmount = pfAmount + earnedAmount * (pfPercentage / 100);
                                        pfArrear = pfArrear + arrearAmount * (pfPercentage / 100);
                                    }
                                    else if (salaryInfo.EffectiveFrom.Value < salaryStartDate && salaryInfo.DeactivationDate != null && salaryInfo.DeactivationDate.Value > salaryEndDate)
                                    {
                                        pfDays = salaryStartDate.DaysBetweenDateRangeIncludingStartDate(salaryEndDate.Date);
                                        totalPFDays = totalPFDays + pfDays;
                                        pfAmount = pfAmount + earnedAmount * (pfPercentage / 100);
                                        pfArrear = pfArrear + arrearAmount * (pfPercentage / 100);
                                    }
                                    else if (salaryInfo.EffectiveFrom.Value.IsDateBetweenTwoDates(salaryStartDate, salaryEndDate) && salaryInfo.DeactivationDate != null && salaryInfo.DeactivationDate.Value.IsDateBetweenTwoDates(salaryStartDate, salaryEndDate))
                                    {
                                        pfDays = salaryInfo.EffectiveFrom.Value.Date.DaysBetweenDateRangeIncludingStartDate(salaryInfo.DeactivationDate.Value.Date);
                                        totalPFDays = totalPFDays + pfDays;
                                        pfAmount = pfAmount + earnedAmount * (pfPercentage / 100);
                                        pfArrear = pfArrear + arrearAmount * (pfPercentage / 100);
                                    }
                                    else if (salaryInfo.EffectiveFrom.Value.IsDateBetweenTwoDates(salaryStartDate, salaryEndDate) && salaryInfo.DeactivationDate != null && salaryInfo.DeactivationDate.Value > salaryEndDate)
                                    {
                                        pfDays = salaryInfo.EffectiveFrom.Value.Date.DaysBetweenDateRangeIncludingStartDate(salaryEndDate.Date);
                                        totalPFDays = totalPFDays + pfDays;
                                        pfAmount = pfAmount + earnedAmount * (pfPercentage / 100);
                                        pfArrear = pfArrear + arrearAmount * (pfPercentage / 100);
                                    }

                                }
                                else
                                {

                                    if (employeePFInfo != null)
                                    {
                                        if (employeePFInfo.ActiveDate.HasValue)
                                        {
                                            // When PF is activated date within salary month
                                            if (employeePFInfo.ActiveDate.Value.Date.IsDateBetweenTwoDates(salaryStartDate.Date, salaryEndDate.Date))
                                            {
                                                if (employeePFInfo.PFEffectiveDate.Value.Date.IsDateBetweenTwoDates(salaryStartDate.Date, salaryEndDate.Date))
                                                {
                                                    if (employeePFInfo.PFEffectiveDate.Value.Date == employeePFInfo.ActiveDate.Value.Date)
                                                    {
                                                        if (salaryInfo.ActivationDate.Value.IsDateBetweenTwoDates(employeePFInfo.PFEffectiveDate.Value, salaryEndDate) && salaryInfo.DeactivationDate == null)
                                                        {
                                                            pfDays = salaryInfo.ActivationDate.Value.DaysBetweenDateRangeIncludingStartDate(salaryEndDate);
                                                            totalPFDays = totalPFDays + pfDays;
                                                            pfAmount = pfAmount + earnedAmount * (pfPercentage / 100);
                                                            pfArrear = pfArrear + arrearAmount * (pfPercentage / 100);
                                                        }

                                                        else if (salaryInfo.ActivationDate.Value < employeePFInfo.PFEffectiveDate.Value
                                                            && (salaryInfo.DeactivationDate == null
                                                            || salaryInfo.DeactivationDate != null && salaryInfo.DeactivationDate.Value > salaryEndDate))
                                                        {
                                                            pfDays = employeePFInfo.PFEffectiveDate.Value.DaysBetweenDateRangeIncludingStartDate(salaryEndDate);
                                                            totalPFDays = totalPFDays + pfDays;
                                                            pfAmount = pfAmount + earnedAmount * (pfPercentage / 100);
                                                            pfArrear = pfArrear + arrearAmount * (pfPercentage / 100);
                                                        }
                                                        else if (salaryInfo.ActivationDate.Value < employeePFInfo.PFEffectiveDate.Value) { }
                                                    }
                                                }
                                                else
                                                {

                                                }
                                            }
                                            // If PF is activated date before salary month
                                            else if (employeePFInfo.ActiveDate.Value.Date < salaryStartDate)
                                            {
                                                pfAmount = pfAmount + earnedAmount * (pfPercentage / 100);
                                                pfArrear = pfArrear + arrearAmount * (pfPercentage / 100);
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        // Process PF If it is Activate this month but effective previously
                        //------------------------------------------------------------------
                        if (salaryInfos.Any())
                        {
                            employeeSalaryProcessed.SalaryProcessDetail.SalaryReviewInfoIds = string.Join(',', salaryInfos.Select(i => i.SalaryReviewInfoId).ToArray());

                            // Hold Salaries
                            var holdSalaries = await _salaryHoldBusiness.GetEmployeeUnholdSalaryInfoAsync(employee.EmployeeId, salaryStartDate.Month, salaryStartDate.Year, user);
                            short unholdDays = 0;

                            foreach (var holdSalary in holdSalaries)
                            {
                                var salaryReviewIds = await _salaryProcessBusiness.GetEmployeeSalaryProcessedReviewIdsOfaMonthAsync(employee.EmployeeId, holdSalary.HoldFrom.Value.Month, holdSalary.HoldFrom.Value.Year, fiscalYearInfo.FiscalYearId, user);

                                var actualDaysInHoldMonth = holdSalary.HoldFrom.Value.DaysInAMonth();
                                var daysInHold = isActualDays == true ? actualDaysInHoldMonth : 30;

                                foreach (var item in salaryReviewIds)
                                {
                                    var salaryReviewInfo = (await _salaryReviewBusiness.GetSalaryReviewInfosAsync(new SalaryReview_Filter()
                                    {
                                        EmployeeId = employee.EmployeeId.ToString(),
                                        SalaryReviewInfoId = item.ToString()
                                    }, user)).FirstOrDefault();
                                    //
                                    if (salaryReviewInfo != null)
                                    {
                                        var secondDate = new DateTime();
                                        if (salaryReviewInfo.EffectiveTo == null)
                                        {
                                            secondDate = holdSalary.HoldTo.Value;
                                        }
                                        else if (holdSalary.HoldTo.Value.Date > salaryReviewInfo.EffectiveTo.Value.Date)
                                        {
                                            secondDate = salaryReviewInfo.EffectiveTo.Value.Date;
                                        }
                                        else if (holdSalary.HoldTo.Value.Date < salaryReviewInfo.EffectiveTo)
                                        {
                                            secondDate = holdSalary.HoldTo.Value.Date;
                                        }
                                        else
                                        {
                                            secondDate = holdSalary.HoldTo.Value.Date;
                                        }

                                        var daysWorked = await GetPresentCountBetweenSalaryDatesWhenSalaryWasHold(new PresentCountBetweenSalaryDates_Filter()
                                        {
                                            EmployeeId = employee.EmployeeId,
                                            IsActualDays = isActualDays,
                                            SalaryDate = holdSalary.HoldFrom,
                                            FirstDate = holdSalary.HoldFrom > salaryReviewInfo.EffectiveFrom.Value.Date ? holdSalary.HoldFrom : salaryReviewInfo.EffectiveFrom.Value.Date,
                                            SecondDate = secondDate,
                                            JoiningDate = employee.DateOfJoining,
                                            TerminationDate = employee.TerminationDate
                                        }, user);

                                        if (isActualDays == false)
                                        {
                                            if (daysWorked == actualDaysInHoldMonth)
                                            {
                                                daysWorked = 30;
                                            }
                                        }

                                        var salaryReviewDetails = await _salaryReviewBusiness.GetSalaryReviewDetailsAsync(salaryReviewInfo.SalaryReviewInfoId, user);
                                        foreach (var reviewItem in salaryReviewDetails)
                                        {
                                            SalaryAllowanceArrear arrear = new SalaryAllowanceArrear();
                                            arrear.EmployeeId = employee.EmployeeId;
                                            arrear.SalaryDate = DateTimeExtension.LastDateOfAMonth(execution.Year, execution.Month);
                                            arrear.ArrearMonth = (short)salaryStartDate.Month;
                                            arrear.ArrearYear = (short)salaryStartDate.Year;
                                            arrear.AllowanceNameId = reviewItem.AllowanceNameId;
                                            arrear.Remarks = $"SalaryHoldId: {holdSalary.SalaryHoldId.ToString()}";
                                            arrear.SalaryMonth = (short)salaryStartDate.Month;
                                            arrear.SalaryYear = (short)salaryStartDate.Year;
                                            arrear.Amount = reviewItem.CurrentAmount / daysInHold * daysWorked;
                                            arrear.AllowanceNameId = reviewItem.AllowanceNameId;
                                            arrear.FiscalYearId = fiscalYearInfo.FiscalYearId;
                                            arrear.SalaryDate = salaryEndDate;
                                            arrear.CalculationForDays = daysWorked;
                                            arrear.CreatedBy = user.ActionUserId;
                                            arrear.CreatedDate = DateTime.Now;
                                            arrear.BranchId = employee.BranchId;
                                            arrear.CompanyId = user.CompanyId;
                                            arrear.OrganizationId = user.OrganizationId;
                                            list_of_salary_allowance_arrear.Add(arrear);
                                            unholdDays = (short)(unholdDays + daysWorked);
                                        }
                                    }
                                }
                            }

                            //if (list_of_salary_allowance_adjustment.Count > 0)
                            //{
                            //    foreach (var adjustment in employeeSalaryProcessed.SalaryAllowanceAdjustments)
                            //    {
                            //        list_of_salary_allowance_arrear.Add(new SalaryAllowanceArrear()
                            //        {
                            //            AllowanceNameId = adjustment.AllowanceNameId,
                            //            Amount = adjustment.Amount,
                            //            ArrearMonth = adjustment.SalaryMonth,
                            //            ArrearYear = adjustment.SalaryYear,
                            //            SalaryReviewInfoId = 0,
                            //            SalaryDate = DateTimeExtension.LastDateOfAMonth(execution.Year, execution.Month),
                            //            SalaryMonth = adjustment.SalaryMonth,
                            //            SalaryYear = adjustment.SalaryYear,
                            //            CalculationForDays = adjustment.CalculationForDays,
                            //            ArrearFrom = adjustment.SalaryDate,
                            //            ArrearTo = adjustment.SalaryDate,
                            //            EmployeeId = employee.EmployeeId,
                            //            FiscalYearId = fiscalYearInfo.FiscalYearId,
                            //            CreatedBy = user.ActionUserId,
                            //            CreatedDate = DateTime.Now,
                            //            CompanyId = user.CompanyId,
                            //            OrganizationId = user.OrganizationId
                            //        });
                            //    }
                            //}

                            if (employee.EmployeeCode == "101137951" && salaryEndDate.Month == 6 && salaryEndDate.Year == 2024)
                            {
                                pfAmount = pfAmount - 1;
                            }
                            if (employee.EmployeeCode == "101536049")
                            {
                                pfAmount = 79000;
                            }
                            // End Of Hold Salary Calculation

                            // Calculated Gross & Basic On Salary Breakdown in this process

                            #region Last Salary Review in this process scope
                            var lastSalaryReviewInfoId = salaryInfos.Max(i => i.SalaryReviewInfoId);
                            var lastSalaryReviewInfo = salaryInfos.FirstOrDefault(i => i.SalaryReviewInfoId == lastSalaryReviewInfoId);

                            List<SalaryReviewDetail> listSalaryReviewDetail = new List<SalaryReviewDetail>();
                            decimal currentGross = 0;
                            decimal currentBasic = 0;
                            decimal currentHR = 0;
                            decimal currentConveyance = 0;
                            decimal currentMedical = 0;

                            if (lastSalaryReviewInfo != null)
                            {
                                listSalaryReviewDetail = (await _salaryReviewBusiness.GetSalaryReviewDetailsAsync(lastSalaryReviewInfoId, user)).AsList();
                                currentGross = listSalaryReviewDetail.Sum(i => i.CurrentAmount);
                                currentBasic = listSalaryReviewDetail.Where(i => i.AllowanceNameId == basicAllowanceInfo.AllowanceNameId).Sum(i => i.CurrentAmount);
                                currentHR = listSalaryReviewDetail.Where(i => i.AllowanceNameId == houseAllowanceInfo.AllowanceNameId).Sum(i => i.CurrentAmount);
                                currentConveyance = listSalaryReviewDetail.Where(i => i.AllowanceNameId == conveyanceAllowanceInfo.AllowanceNameId).Sum(i => i.CurrentAmount);
                                currentMedical = listSalaryReviewDetail.Where(i => i.AllowanceNameId == medicalAllowanceInfo.AllowanceNameId).Sum(i => i.CurrentAmount);
                            }
                            #endregion

                            #region allowance earned amount from the salary breakdown
                            var gross_earned = Math.Round(list_of_salary_allowance.Sum(i => i.Amount), 0);
                            var gross_earned_arrear = Math.Round(list_of_salary_allowance_arrear.Sum(i => i.Amount), 0);

                            var basic_earned = Math.Round(list_of_salary_allowance.Where(i => i.AllowanceNameId == basicAllowanceInfo.AllowanceNameId).Sum(i => i.Amount), 0);
                            var basic_earned_arrear = Math.Round(list_of_salary_allowance_arrear.Where(i => i.AllowanceNameId == basicAllowanceInfo.AllowanceNameId).Sum(i => i.Amount), 0);

                            var house_earned = Math.Round(list_of_salary_allowance.Where(i => i.AllowanceNameId == houseAllowanceInfo.AllowanceNameId).Sum(i => i.Amount), 0);
                            var house_earned_arrear = Math.Round(list_of_salary_allowance_arrear.Where(i => i.AllowanceNameId == houseAllowanceInfo.AllowanceNameId).Sum(i => i.Amount), 0);


                            var conveyance_earned = Math.Round(list_of_salary_allowance.Where(i => i.AllowanceNameId == conveyanceAllowanceInfo.AllowanceNameId).Sum(i => i.Amount), 0);
                            var conveyance_earned_arrear = Math.Round(list_of_salary_allowance_arrear.Where(i => i.AllowanceNameId == conveyanceAllowanceInfo.AllowanceNameId).Sum(i => i.Amount), 0);

                            var medical_earned = Math.Round(list_of_salary_allowance.Where(i => i.AllowanceNameId == medicalAllowanceInfo.AllowanceNameId).Sum(i => i.Amount), 0);
                            var medical_earned_arrear = Math.Round(list_of_salary_allowance_arrear.Where(i => i.AllowanceNameId == medicalAllowanceInfo.AllowanceNameId).Sum(i => i.Amount), 0);
                            #endregion

                            #region Find Deposite Amount
                            var _conditionalDeposits = await _conditionalDepositAllowanceConfigBusiness.GetEmployeeConditionalDepositAllowanceConfigsAsync(new EligibilityInConditionalDeposit_Filter()
                            {
                                JobType = employee.JobType,
                                Gender = employee.Gender,
                                MaritalStatus = employee.MaritalStatus,
                                PhysicalCondition = employee.PhysicalCondition,
                                Religion = employee.Religion,
                                SalaryDate = salaryEndDate.ToString("yyyy-MM-dd")
                            }, user);

                            foreach (var item in _conditionalDeposits)
                            {
                                DepositAllowanceHistory depositAllowanceHistory = new DepositAllowanceHistory();
                                depositAllowanceHistory.EmployeeId = employee.EmployeeId;
                                depositAllowanceHistory.AllowanceNameId = item.AllowanceNameId;
                                depositAllowanceHistory.FiscalYearId = fiscalYearInfo.FiscalYearId;
                                depositAllowanceHistory.DepositMonth = salaryEndDate.Month;
                                depositAllowanceHistory.DepositYear = salaryEndDate.Year;
                                depositAllowanceHistory.PayableDays = (short)totalDayWorked;
                                depositAllowanceHistory.ConditionalDepositAllowanceConfigId = item.Id;
                                depositAllowanceHistory.CreatedBy = user.ActionUserId;
                                depositAllowanceHistory.CreatedDate = DateTime.Now;
                                depositAllowanceHistory.CompanyId = user.CompanyId;
                                depositAllowanceHistory.OrganizationId = user.OrganizationId;
                                depositAllowanceHistory.BranchId = employee.BranchId;
                                depositAllowanceHistory.IncomingFlag = "Conditional";
                                depositAllowanceHistory.ConditionalDepositAllowanceConfigId = item.Id;
                                depositAllowanceHistory.DepositDate = salaryEndDate.Date;

                                if (item.DepositType == "Monthly")
                                {
                                    if (item.BaseOfPayment == "Flat")
                                    {
                                        depositAllowanceHistory.BaseAmount = item.Amount ?? 0;
                                        depositAllowanceHistory.Amount = item.Amount ?? 0;
                                    }
                                    else if (item.BaseOfPayment == "Basic")
                                    {
                                        if ((item.Percentage ?? 0) > 0)
                                        {
                                            depositAllowanceHistory.BaseAmount = currentBasic / 100 * item.Percentage ?? 0;
                                            depositAllowanceHistory.Amount = basic_earned / 100 * item.Percentage ?? 0;
                                            depositAllowanceHistory.Arrear = basic_earned_arrear / 100 * item.Percentage ?? 0;
                                        }
                                    }
                                    else if (item.BaseOfPayment == "Gross")
                                    {
                                        if ((item.Percentage ?? 0) > 0)
                                        {
                                            depositAllowanceHistory.BaseAmount = currentGross / 100 * item.Percentage ?? 0;
                                            depositAllowanceHistory.Amount = gross_earned / 100 * item.Percentage ?? 0;
                                            depositAllowanceHistory.Arrear = gross_earned_arrear / 100 * item.Percentage ?? 0;
                                        }
                                    }
                                    else if (item.BaseOfPayment == "Gross Without Conveyance")
                                    {
                                        if ((item.Percentage ?? 0) > 0)
                                        {
                                            var gross_without_conveyance = gross_earned - conveyance_earned;
                                            var gross_arrear_without_conveyance = gross_earned_arrear - conveyance_earned_arrear;

                                            var currentGross_without_conveyance = currentGross - currentConveyance;

                                            depositAllowanceHistory.BaseAmount = currentGross_without_conveyance / 100 * item.Percentage ?? 0;
                                            depositAllowanceHistory.Amount = gross_without_conveyance / 100 * item.Percentage ?? 0;
                                            depositAllowanceHistory.Arrear = gross_arrear_without_conveyance / 100 * item.Percentage ?? 0;
                                        }
                                    }
                                }
                                list_of_deposite_Allowance_History.Add(depositAllowanceHistory);
                            }
                            #endregion

                            #region Monthly Allowance
                            var monthlyAllowanceConfigs = await _monthlyAllowanceConfigBusiness.GetMonthlyAllowanceConfigsAsync(employee.EmployeeId, salaryStartDate.ToString("yyyy-MM-dd"), user);

                            if (monthlyAllowanceConfigs != null)
                            {
                                if (monthlyAllowanceConfigs.Count() > 0)
                                {
                                    foreach (var item in monthlyAllowanceConfigs)
                                    {
                                        if (item.BaseOfPayment == "Basic" || item.BaseOfPayment == "Gross")
                                        {
                                            var baseAmount = item.BaseOfPayment == "Basic" ? currentBasic : currentGross;
                                            if (item.Percentage != null && item.Percentage > 0)
                                            {
                                                if (baseAmount > 0)
                                                {
                                                    decimal gainedAmount = 0;
                                                    if (item.IsProrated)
                                                    {
                                                        gainedAmount = baseAmount / 100 * item.Percentage.Value / daysInSalaryMonth * totalDayWorked;
                                                    }
                                                    else
                                                    {
                                                        gainedAmount = ((baseAmount / 100) * item.Percentage.Value);
                                                    }

                                                    gainedAmount = Math.Round(gainedAmount, MidpointRounding.AwayFromZero);

                                                    if (item.IsVisibleInSalarySheet)
                                                    {
                                                        list_of_salary_allowance.Add(new SalaryAllowance()
                                                        {
                                                            EmployeeId = employee.EmployeeId,
                                                            AllowanceNameId = item.AllowanceNameId,
                                                            CalculationForDays = 0,
                                                            Amount = Math.Round(gainedAmount, 0),
                                                            AdjustmentAmount = 0,
                                                            ArrearAmount = 0,
                                                            SalaryDate = salaryEndDate,
                                                            SalaryMonth = (short)salaryEndDate.Month,
                                                            SalaryYear = (short)salaryEndDate.Year,
                                                            MonthlyAllowanceId = 0,
                                                            FiscalYearId = fiscalYearInfo.FiscalYearId,
                                                            CreatedBy = user.ActionUserId,
                                                            CreatedDate = DateTime.Now,
                                                            CompanyId = user.CompanyId,
                                                            OrganizationId = user.OrganizationId,
                                                        });
                                                    }

                                                    list_of_monthly_allowance_History.Add(new MonthlyAllowanceHistory()
                                                    {
                                                        Amount = gainedAmount,
                                                        CompanyId = user.CompanyId,
                                                        CreatedBy = user.ActionUserId,
                                                        CreatedDate = DateTime.Now,
                                                        Days = totalDayWorked,
                                                        EmployeeId = employee.EmployeeId,
                                                        FiscalYearId = fiscalYearInfo.FiscalYearId,
                                                        Month = (short)salaryStartDate.Month,
                                                        Year = (short)salaryStartDate.Year,
                                                        OrganizationId = user.CompanyId,
                                                        MonthlyAllowanceConfigId = item.Id

                                                    });
                                                }

                                            }
                                        }
                                        else if (item.BaseOfPayment == "Flat")
                                        {
                                            if (item.Amount != null && item.Amount > 0)
                                            {
                                                decimal gainedAmount = 0;
                                                if (item.IsProrated)
                                                {
                                                    gainedAmount = Math.Round(item.Amount.Value / daysInSalaryMonth * totalDayWorked, 0);
                                                }
                                                else
                                                {
                                                    gainedAmount = item.Amount.Value;
                                                }
                                                if (item.IsVisibleInSalarySheet)
                                                {
                                                    list_of_salary_allowance.Add(new SalaryAllowance()
                                                    {
                                                        EmployeeId = employee.EmployeeId,
                                                        AllowanceNameId = item.AllowanceNameId,
                                                        CalculationForDays = 0,
                                                        Amount = gainedAmount,
                                                        AdjustmentAmount = 0,
                                                        ArrearAmount = 0,
                                                        SalaryDate = salaryEndDate,
                                                        SalaryMonth = (short)salaryEndDate.Month,
                                                        SalaryYear = (short)salaryEndDate.Year,
                                                        MonthlyAllowanceId = 0,
                                                        FiscalYearId = fiscalYearInfo.FiscalYearId,
                                                        CreatedBy = user.ActionUserId,
                                                        CreatedDate = DateTime.Now,
                                                        CompanyId = user.CompanyId,
                                                        OrganizationId = user.OrganizationId,
                                                    });
                                                }

                                                list_of_monthly_allowance_History.Add(new MonthlyAllowanceHistory()
                                                {
                                                    Amount = gainedAmount,
                                                    CompanyId = user.CompanyId,
                                                    CreatedBy = user.ActionUserId,
                                                    CreatedDate = DateTime.Now,
                                                    Days = totalDayWorked,
                                                    EmployeeId = employee.EmployeeId,
                                                    FiscalYearId = fiscalYearInfo.FiscalYearId,
                                                    Month = (short)salaryStartDate.Month,
                                                    Year = (short)salaryStartDate.Year,
                                                    OrganizationId = user.OrganizationId,
                                                    MonthlyAllowanceConfigId = item.Id
                                                });
                                            }
                                        }
                                    }
                                }
                            }

                            if (list_of_monthly_allowance_History.Any())
                            {
                                employeeSalaryProcessed.MonthlyAllowanceHistories = list_of_monthly_allowance_History;
                            }
                            #endregion

                            #region Wounderman Thompson Contractual Employee Conveyance
                            if (user.CompanyId == 19 && user.OrganizationId == 11)
                            {
                                decimal conveyanceAmount = employee.EmployeeType == "Manager" ? 5000 : 3500;
                                decimal gainedAmount = Math.Round(conveyanceAmount / daysInSalaryMonth * totalDayWorked, 0);

                                list_of_salary_allowance.Add(new SalaryAllowance()
                                {
                                    EmployeeId = employee.EmployeeId,
                                    AllowanceNameId = conveyanceAllowanceInfo.AllowanceNameId,
                                    CalculationForDays = 0,
                                    Amount = gainedAmount,
                                    AdjustmentAmount = 0,
                                    ArrearAmount = 0,
                                    SalaryDate = salaryEndDate,
                                    SalaryMonth = (short)salaryEndDate.Month,
                                    SalaryYear = (short)salaryEndDate.Year,
                                    MonthlyAllowanceId = 0,
                                    FiscalYearId = fiscalYearInfo.FiscalYearId,
                                    CreatedBy = user.ActionUserId,
                                    CreatedDate = DateTime.Now,
                                    CompanyId = user.CompanyId,
                                    OrganizationId = user.OrganizationId,
                                });
                            }
                            #endregion Wounderman Thompson Contractual Employee Conveyance

                            #region Wounderman Thompson Contractual Employee Medical
                            if (user.CompanyId == 19 && user.OrganizationId == 11)
                            {
                                if (employee.JobType == "Contractual")
                                {
                                    var conveyanceAmount = employee.EmployeeType == "Manager" ? 5000 : 3500;

                                    //list_of_salary_allowance.Where(i => i.AllowanceNameId == conveyanceAllowanceInfo.AllowanceNameId).Sum(i => i.Amount);
                                    var medicalAmount = (currentGross + conveyanceAmount) / 100 * 4 / daysInSalaryMonth * totalDayWorked;
                                    list_of_salary_allowance.Add(new SalaryAllowance()
                                    {
                                        EmployeeId = employee.EmployeeId,
                                        AllowanceNameId = medicalAllowanceInfo.AllowanceNameId,
                                        CalculationForDays = 0,
                                        Amount = Math.Round(medicalAmount, 0),
                                        AdjustmentAmount = 0,
                                        ArrearAmount = 0,
                                        SalaryDate = salaryEndDate,
                                        SalaryMonth = (short)salaryEndDate.Month,
                                        SalaryYear = (short)salaryEndDate.Year,
                                        MonthlyAllowanceId = 0,
                                        FiscalYearId = fiscalYearInfo.FiscalYearId,
                                        CreatedBy = user.ActionUserId,
                                        CreatedDate = DateTime.Now,
                                        CompanyId = user.CompanyId,
                                        OrganizationId = user.OrganizationId,
                                    });
                                }
                            }
                            #endregion

                            #region Proposal of Payment Deposit Amount
                            employeeSalaryProcessed.DepositAllowancePaymentHistories = (await _depositAllowancePaymentHistoryBusiness.ThisMonthEmployeeDepositAllowanceCalculationInSalaryAsync(employee, list_of_deposite_Allowance_History, salaryEndDate.Month, salaryEndDate.Year, fiscalYearInfo.FiscalYearId, user)).AsList();
                            #endregion

                            conveyance_earned = Math.Round(list_of_salary_allowance.Where(i => i.AllowanceNameId == conveyanceAllowanceInfo.AllowanceNameId).Sum(i => i.Amount), 0);
                            conveyance_earned_arrear = Math.Round(list_of_salary_allowance_arrear.Where(i => i.AllowanceNameId == conveyanceAllowanceInfo.AllowanceNameId).Sum(i => i.Amount), 0);

                            #region Service anniversary allowance
                            if (employee.DateOfJoining != null)
                            {
                                var items = await _serviceAnniversaryAllowanceBusiness.GetEmployeeServiceAnniversaryAllowancesAsync(new EligibilityInServiceAnniversary_Filter()
                                {
                                    Gender = employee.Gender,
                                    JobType = employee.JobType,
                                    MaritalStatus = employee.MaritalStatus,
                                    PhysicalCondition = employee.PhysicalCondition,
                                    Religion = employee.Religion,
                                    PaymentDate = salaryEndDate.ToString("yyyy-MM-dd")
                                }, salaryEndDate.Month, salaryEndDate.Year, employee.DateOfJoining.Value.Date, user);
                                if (items != null && items.AsList().Count > 0)
                                {
                                    foreach (var item in items)
                                    {
                                        decimal amountWillAdd = 0;
                                        if (user.CompanyId == 19 && user.OrganizationId == 11 && employee.JobType == Jobtype.Contractual && item.BaseOfPayment == "Gross With Conveyance")
                                        {
                                            amountWillAdd = conveyance_earned + conveyance_earned_arrear;
                                            var serviceAnniversaryBaseAmount = item.BaseOfPayment == "Basic" ? basic_earned + basic_earned_arrear : gross_earned + gross_earned_arrear;
                                            if (item.Percentage != null && item.Percentage > 0)
                                            {
                                                if (serviceAnniversaryBaseAmount > 0)
                                                {
                                                    var serviceAnniversaryGainedAmount = serviceAnniversaryBaseAmount / 100 * item.Percentage.Value;
                                                    list_of_salary_allowance.Add(new SalaryAllowance()
                                                    {
                                                        EmployeeId = employee.EmployeeId,
                                                        AllowanceNameId = item.AllowanceNameId,
                                                        CalculationForDays = 0,
                                                        Amount = Math.Round(serviceAnniversaryGainedAmount + amountWillAdd, 0),
                                                        AdjustmentAmount = 0,
                                                        ArrearAmount = 0,
                                                        SalaryDate = salaryEndDate,
                                                        SalaryMonth = (short)salaryEndDate.Month,
                                                        SalaryYear = (short)salaryEndDate.Year,
                                                        MonthlyAllowanceId = 0,
                                                        FiscalYearId = fiscalYearInfo.FiscalYearId,
                                                        CreatedBy = user.ActionUserId,
                                                        CreatedDate = DateTime.Now,
                                                        CompanyId = user.CompanyId,
                                                        OrganizationId = user.OrganizationId,
                                                    });
                                                    employeeSalaryProcessed.RecipientsofServiceAnniversaryAllowances.Add(new RecipientsofServiceAnniversaryAllowance()
                                                    {
                                                        AllowanceNameId = item.AllowanceNameId,
                                                        EmployeeId = employee.EmployeeId,
                                                        ServiceAnniversaryAllowanceId = item.Id,
                                                        PayableAmount = Math.Round(serviceAnniversaryGainedAmount + amountWillAdd, 0),
                                                        DisbursedAmount = Math.Round(serviceAnniversaryGainedAmount + amountWillAdd, 0),
                                                        FiscalYearId = fiscalYearInfo.FiscalYearId,
                                                        PaymentMonth = salaryEndDate.Month,
                                                        PaymentYear = salaryEndDate.Year,
                                                        CreatedBy = user.ActionUserId,
                                                        CreatedDate = DateTime.Now,
                                                        CompanyId = user.CompanyId,
                                                        IsDisbursed = false,
                                                        IsVisibleInPayslip = item.IsVisibleInPayslip,
                                                        IsVisibleInSalarySheet = item.IsVisibleInSalarySheet,
                                                        OrganizationId = user.OrganizationId,
                                                        BranchId = employee.BranchId,
                                                        PaymentDate = salaryEndDate
                                                    });
                                                }
                                            }
                                        }
                                        else if (item.BaseOfPayment == "Basic" || item.BaseOfPayment == "Gross")
                                        {
                                            var serviceAnniversaryBaseAmount = item.BaseOfPayment == "Basic" ? basic_earned + basic_earned_arrear : gross_earned + gross_earned_arrear;
                                            if (item.Percentage != null && item.Percentage > 0)
                                            {
                                                if (serviceAnniversaryBaseAmount > 0)
                                                {
                                                    var serviceAnniversaryGainedAmount = serviceAnniversaryBaseAmount / 100 * item.Percentage.Value;
                                                    list_of_salary_allowance.Add(new SalaryAllowance()
                                                    {
                                                        EmployeeId = employee.EmployeeId,
                                                        AllowanceNameId = item.AllowanceNameId,
                                                        CalculationForDays = 0,
                                                        Amount = Math.Round(serviceAnniversaryGainedAmount),
                                                        AdjustmentAmount = 0,
                                                        ArrearAmount = 0,
                                                        SalaryDate = salaryEndDate,
                                                        SalaryMonth = (short)salaryEndDate.Month,
                                                        SalaryYear = (short)salaryEndDate.Year,
                                                        MonthlyAllowanceId = 0,
                                                        FiscalYearId = fiscalYearInfo.FiscalYearId,
                                                        CreatedBy = user.ActionUserId,
                                                        CreatedDate = DateTime.Now,
                                                        CompanyId = user.CompanyId,
                                                        OrganizationId = user.OrganizationId,
                                                    });
                                                    employeeSalaryProcessed.RecipientsofServiceAnniversaryAllowances.Add(new RecipientsofServiceAnniversaryAllowance()
                                                    {
                                                        AllowanceNameId = item.AllowanceNameId,
                                                        EmployeeId = employee.EmployeeId,
                                                        ServiceAnniversaryAllowanceId = item.Id,
                                                        PayableAmount = Math.Round(serviceAnniversaryGainedAmount),
                                                        DisbursedAmount = Math.Round(serviceAnniversaryGainedAmount),
                                                        FiscalYearId = fiscalYearInfo.FiscalYearId,
                                                        PaymentMonth = salaryEndDate.Month,
                                                        PaymentYear = salaryEndDate.Year,
                                                        CreatedBy = user.ActionUserId,
                                                        CreatedDate = DateTime.Now,
                                                        CompanyId = user.CompanyId,
                                                        IsDisbursed = false,
                                                        IsVisibleInPayslip = item.IsVisibleInPayslip,
                                                        IsVisibleInSalarySheet = item.IsVisibleInSalarySheet,
                                                        OrganizationId = user.OrganizationId,
                                                        BranchId = employee.BranchId,
                                                        PaymentDate = salaryEndDate
                                                    });
                                                }
                                            }
                                        }
                                        else if (item.BaseOfPayment == "Flat")
                                        {
                                            if (item.Amount != null && item.Amount > 0)
                                            {
                                                list_of_salary_allowance.Add(new SalaryAllowance()
                                                {
                                                    EmployeeId = employee.EmployeeId,
                                                    AllowanceNameId = item.AllowanceNameId,
                                                    CalculationForDays = 0,
                                                    Amount = item.Amount.Value,
                                                    AdjustmentAmount = 0,
                                                    ArrearAmount = 0,
                                                    SalaryDate = salaryEndDate,
                                                    SalaryMonth = (short)salaryEndDate.Month,
                                                    SalaryYear = (short)salaryEndDate.Year,
                                                    MonthlyAllowanceId = 0,
                                                    FiscalYearId = fiscalYearInfo.FiscalYearId,
                                                    CreatedBy = user.ActionUserId,
                                                    CreatedDate = DateTime.Now,
                                                    CompanyId = user.CompanyId,
                                                    OrganizationId = user.OrganizationId,
                                                });
                                                employeeSalaryProcessed.RecipientsofServiceAnniversaryAllowances.Add(new RecipientsofServiceAnniversaryAllowance()
                                                {
                                                    AllowanceNameId = item.AllowanceNameId,
                                                    EmployeeId = employee.EmployeeId,
                                                    ServiceAnniversaryAllowanceId = item.Id,
                                                    PayableAmount = item.Amount.Value,
                                                    DisbursedAmount = item.Amount.Value,
                                                    FiscalYearId = fiscalYearInfo.FiscalYearId,
                                                    PaymentMonth = salaryEndDate.Month,
                                                    PaymentYear = salaryEndDate.Year,
                                                    CreatedBy = user.ActionUserId,
                                                    CreatedDate = DateTime.Now,
                                                    CompanyId = user.CompanyId,
                                                    IsDisbursed = false,
                                                    IsVisibleInPayslip = item.IsVisibleInPayslip,
                                                    IsVisibleInSalarySheet = item.IsVisibleInSalarySheet,
                                                    OrganizationId = user.OrganizationId,
                                                    BranchId = employee.BranchId,
                                                    PaymentDate = salaryEndDate
                                                });
                                            }
                                        }


                                    }
                                }
                            }
                            #endregion

                            #region Monthly Variable Allowance
                            var variableAllowances = await _monthlyVariableAllowanceBusiness.GetMonthlyVariableAllowancesAsync(0, employee.EmployeeId, 0, (short)salaryStartDate.Month, (short)salaryStartDate.Year, "Approved", user);
                            foreach (var item in variableAllowances)
                            {
                                list_of_salary_allowance.Add(new SalaryAllowance()
                                {
                                    EmployeeId = employee.EmployeeId,
                                    AllowanceNameId = item.AllowanceNameId,
                                    CalculationForDays = totalDayWorked,
                                    Amount = item.Amount,
                                    AdjustmentAmount = 0,
                                    ArrearAmount = 0,
                                    SalaryDate = salaryEndDate,
                                    SalaryMonth = (short)salaryEndDate.Month,
                                    SalaryYear = (short)salaryEndDate.Year,
                                    MonthlyAllowanceId = item.MonthlyVariableAllowanceId,
                                    FiscalYearId = fiscalYearInfo.FiscalYearId,
                                    CreatedBy = user.ActionUserId,
                                    CreatedDate = DateTime.Now,
                                    CompanyId = user.CompanyId,
                                    OrganizationId = user.OrganizationId,
                                });

                                list_of_salary_component_History.Add(new SalaryComponentHistory()
                                {
                                    EmployeeId = employee.EmployeeId,
                                    SalaryMonth = (short)salaryStartDate.Date.Month,
                                    SalaryYear = (short)salaryStartDate.Date.Year,
                                    Flag = "Variable Allowance",
                                    ComponentId = item.MonthlyVariableAllowanceId.ToString(),
                                    Amount = item.Amount.ToString(),
                                    CreatedBy = user.ActionUserId,
                                    CompanyId = user.CompanyId,
                                    OrganizationId = user.OrganizationId
                                });
                            }
                            #endregion

                            #region Monthly Variable Deduction
                            var variableDeductions = await _monthlyVariableDeductionBusiness.GetMonthlyVariableDeductionsAsync(0, employee.EmployeeId, 0, (short)salaryStartDate.Month, (short)salaryStartDate.Year, "Approved", user);
                            foreach (var item in variableDeductions)
                            {
                                list_of_salary_deduction.Add(new SalaryDeduction()
                                {
                                    EmployeeId = item.EmployeeId,
                                    DeductionNameId = item.DeductionNameId,
                                    CalculationForDays = totalDayWorked,
                                    Amount = item.Amount,
                                    ArrearAmount = 0,
                                    SalaryDate = salaryEndDate,
                                    SalaryMonth = (short)salaryEndDate.Month,
                                    SalaryYear = (short)salaryEndDate.Year,
                                    MonthlyDeductionId = item.MonthlyVariableDeductionId,
                                    FiscalYearId = fiscalYearInfo.FiscalYearId,
                                    CreatedBy = user.ActionUserId,
                                    CreatedDate = DateTime.Now,
                                    CompanyId = user.CompanyId,
                                    OrganizationId = user.OrganizationId
                                });

                                list_of_salary_component_History.Add(new SalaryComponentHistory()
                                {
                                    EmployeeId = employee.EmployeeId,
                                    SalaryMonth = (short)salaryStartDate.Date.Month,
                                    SalaryYear = (short)salaryStartDate.Date.Year,
                                    Flag = "Variable Deduction",
                                    ComponentId = item.MonthlyVariableDeductionId.ToString(),
                                    Amount = item.Amount.ToString(),
                                    CreatedBy = user.ActionUserId,
                                    CompanyId = user.CompanyId,
                                    OrganizationId = user.OrganizationId
                                });
                            }
                            #endregion

                            #region Wunderman PF Load Deduction
                            if (user.CompanyId == 19 && user.OrganizationId == 11 && (short)salaryEndDate.Month > 3 && AppSettings.PF_Software_Connection == true)
                            {
                                decimal pfLoanAmount = await _wundermanPFServiceBusiness.GetEmployeeLoanDeductionAmountAsync(employee.EmployeeCode, salaryEndDate.Year, salaryEndDate.Month, user);
                                if (pfLoanAmount > 0)
                                {
                                    list_of_salary_deduction.Add(new SalaryDeduction()
                                    {
                                        EmployeeId = employee.EmployeeId,
                                        DeductionNameId = 2,
                                        CalculationForDays = 0,
                                        Amount = pfLoanAmount,
                                        ArrearAmount = 0,
                                        SalaryDate = salaryEndDate,
                                        SalaryMonth = (short)salaryEndDate.Month,
                                        SalaryYear = (short)salaryEndDate.Year,
                                        MonthlyDeductionId = 0,
                                        FiscalYearId = fiscalYearInfo.FiscalYearId,
                                        CreatedBy = user.ActionUserId,
                                        CreatedDate = DateTime.Now,
                                        CompanyId = user.CompanyId,
                                        OrganizationId = user.OrganizationId
                                    });
                                }
                            }
                            #endregion

                            #region Salary Alloawnce Arrear/Adjustment
                            decimal totalArreardjusted = 0;
                            decimal totalAdjustment = 0;

                            var listOfArrearOrAdjustment = await _salaryAllowanceArrearAdjustmentBusiness.GetSalaryAllowanceArrearAdjustmentByEmployeeIdInSalaryProcessAsync(employee.EmployeeId, salaryStartDate.Year, salaryStartDate.Month, user);
                            foreach (var item in listOfArrearOrAdjustment)
                            {
                                totalArreardjusted = totalArreardjusted + item.Amount;
                                if (item.Flag == "Arrear")
                                {
                                    list_of_salary_allowance_arrear.Add(new SalaryAllowanceArrear()
                                    {
                                        AllowanceNameId = item.AllowanceNameId,
                                        EmployeeId = item.EmployeeId,
                                        SalaryMonth = item.SalaryMonth ?? 0,
                                        SalaryYear = item.SalaryYear ?? 0,
                                        SalaryDate = salaryEndDate,
                                        Amount = item.Amount,
                                        ArrearMonth = (short)salaryStartDate.Month,
                                        ArrearYear = (short)salaryStartDate.Year,
                                        SalaryReviewInfoId = 0,
                                        CreatedBy = user.ActionUserId,
                                        CreatedDate = DateTime.Now,
                                        BranchId = employee.BranchId,
                                        CompanyId = user.CompanyId,
                                        OrganizationId = user.OrganizationId,
                                        FiscalYearId = fiscalYearInfo.FiscalYearId
                                    });
                                }

                                if (item.Flag == "Adjustment")
                                {
                                    totalAdjustment = totalAdjustment + item.Amount;    
                                    list_of_salary_allowance_adjustment.Add(new SalaryAllowanceAdjustment()
                                    {
                                        AllowanceNameId = item.AllowanceNameId,
                                        EmployeeId = item.EmployeeId,
                                        SalaryMonth = item.SalaryMonth ?? 0,
                                        SalaryYear = item.SalaryYear ?? 0,
                                        SalaryDate = salaryEndDate,
                                        Amount = item.Amount,
                                        AdjustmentMonth = item.ArrearAdjustmentMonth??0,
                                        AdjustmentYear = item.ArrearAdjustmentYear??0,
                                        CreatedBy = user.ActionUserId,
                                        CreatedDate = DateTime.Now,
                                        BranchId = employee.BranchId,
                                        CompanyId = user.CompanyId,
                                        OrganizationId = user.OrganizationId,
                                        FiscalYearId = fiscalYearInfo.FiscalYearId
                                    });
                                }
                            }

                            if (holdDays == totalDayWorked)
                            {
                                holdAmount = holdAmount + totalArreardjusted;
                            }

                            if (!Utility.IsNullEmptyOrWhiteSpace(payrollModuleConfig.BaseOfProvidentFund) && payrollModuleConfig.BaseOfProvidentFund.ToLower() == "basic" && employee.IsPFMember)
                            {
                                var items = listOfArrearOrAdjustment.Select(i => i.AllowanceNameId).ToList();
                                var isBasicExist = items.Exists(x => x == basicAllowanceInfo.AllowanceNameId);
                                if (isBasicExist)
                                {
                                    var basicInArrearAdjustment = listOfArrearOrAdjustment.Where(item => item.AllowanceNameId == basicAllowanceInfo.AllowanceNameId).Sum(item => item.Amount);
                                    var pfInBasicInArrearAdjustment = basicInArrearAdjustment / 100 * pfPercentage;
                                    pfInBasicInArrearAdjustment = Math.Round(pfInBasicInArrearAdjustment, MidpointRounding.AwayFromZero);

                                    pfArrear = Math.Round(pfArrear, MidpointRounding.AwayFromZero) + pfInBasicInArrearAdjustment;
                                }
                            }

                            #endregion

                            #region Marge Salary Allowance
                            List<long> allowances = new List<long>();
                            var unique_salary_allowance = list_of_salary_allowance.Select(i => i.AllowanceNameId).Distinct().ToList();
                            var unique_salary_allowance_arrea = list_of_salary_allowance_arrear.Select(i => i.AllowanceNameId).Distinct().ToList();
                            var unique_salary_allowance_adjustment = list_of_salary_allowance_adjustment.Select(i => i.AllowanceNameId).Distinct().ToList();
                            var unique_deposit_allowance_payment_proposal = employeeSalaryProcessed.DepositAllowancePaymentHistories.Where(i => (i.AllowanceNameId ?? 0) > 0).Select(i => i.AllowanceNameId ?? 0).Distinct().ToList();

                            foreach (var i in unique_salary_allowance)
                            {
                                allowances.Add(i);
                            }
                            foreach (var i in unique_salary_allowance_arrea)
                            {
                                allowances.Add(i);
                            }
                            foreach (var i in unique_salary_allowance_adjustment)
                            {
                                allowances.Add(i);
                            }
                            foreach (var i in unique_deposit_allowance_payment_proposal)
                            {
                                allowances.Add(i);
                            }
                            allowances = allowances.Distinct().ToList();

                            employeeSalaryProcessed.SalaryAllowanceArrears = list_of_salary_allowance_arrear;
                            employeeSalaryProcessed.SalaryAllowanceAdjustments = list_of_salary_allowance_adjustment;
                            employeeSalaryProcessed.DepositAllowanceHistories = list_of_deposite_Allowance_History;

                            //foreach (var adjustment in list_of_salary_allowance_adjustment)
                            //{
                            //    var salaryAllowanceArrear = new SalaryAllowanceArrear()
                            //    {
                            //        AllowanceNameId = adjustment.AllowanceNameId,
                            //        Amount = adjustment.Amount,
                            //        ArrearMonth = adjustment.SalaryMonth,
                            //        ArrearYear = adjustment.SalaryYear,
                            //        SalaryReviewInfoId = 0,
                            //        SalaryDate = DateTimeExtension.LastDateOfAMonth(execution.Year, execution.Month),
                            //        SalaryMonth = adjustment.SalaryMonth,
                            //        SalaryYear = adjustment.SalaryYear,
                            //        CalculationForDays = adjustment.CalculationForDays,
                            //        FiscalYearId = fiscalYearInfo.FiscalYearId,
                            //        ArrearFrom = adjustment.SalaryDate,
                            //        ArrearTo = adjustment.SalaryDate,
                            //        EmployeeId = employee.EmployeeId,
                            //        CreatedBy = user.ActionUserId,
                            //        CreatedDate = DateTime.Now,
                            //        CompanyId = user.CompanyId,
                            //        OrganizationId = user.OrganizationId
                            //    };
                            //    employeeSalaryProcessed.SalaryAllowanceArrears.Add(salaryAllowanceArrear);
                            //}

                            decimal breakdownWiseSalaryAdjustment = 0;
                            foreach (var id in allowances)
                            {

                                bool IsExistInBreakdown = false;

                                var amount = list_of_salary_allowance.Where(i => i.AllowanceNameId == id).Sum(i => i.Amount);
                                var arrearAmount = list_of_salary_allowance_arrear.Where(i => i.AllowanceNameId == id).Sum(i => i.Amount);
                                var monthlyAllowanceIds = string.Join(',', list_of_salary_allowance.Where(i => i.MonthlyAllowanceId != null && i.MonthlyAllowanceId.Value > 0).Select(i => i.MonthlyAllowanceId).ToList());
                                var periodicallyAllowanceIds = string.Join(',', list_of_salary_allowance.Where(i => i.PeriodicallyAllowanceId != null && i.PeriodicallyAllowanceId.Value > 0).Select(i => i.PeriodicallyAllowanceId).ToList());

                                var allowanceAdjustmentAmount = list_of_salary_allowance_adjustment.Where(i => i.AllowanceNameId == id).Sum(i => i.Amount);

                                amount = amount + employeeSalaryProcessed.DepositAllowancePaymentHistories.Where(i => i.AllowanceNameId == id).Sum(i => i.DisbursedAmount);

                                decimal adjustmentAmount = 0;
                                decimal breakDownAmount = 0;

                                if (listSalaryReviewDetail.Exists(i => i.AllowanceNameId == id))
                                {
                                    IsExistInBreakdown = true;
                                    breakDownAmount = listSalaryReviewDetail.Where(i => i.AllowanceNameId == id).Sum(i => i.CurrentAmount);
                                    if (arrearAmount <= 0)
                                    {
                                        adjustmentAmount = breakDownAmount - amount;
                                        breakdownWiseSalaryAdjustment = breakdownWiseSalaryAdjustment + adjustmentAmount;
                                    }
                                }

                                #region WundermanThompson - Find Contractual employees Medical & Conveyance Amount Diff
                                if (user.CompanyId == 19 && user.OrganizationId == 11)
                                {
                                    decimal conveyance = employee.EmployeeType == "Manager" ? 5000 : 3500;
                                    // Medical
                                    if (id == 5 && employee.JobType == Jobtype.Contractual)
                                    {
                                        IsExistInBreakdown = true;
                                        breakDownAmount = (currentGross + conveyance) / 100 * 4;
                                        if (arrearAmount <= 0)
                                        {
                                            var medicalAmount = breakDownAmount / daysInSalaryMonth * totalDayWorked;
                                            adjustmentAmount = Math.Round(breakDownAmount, 0) - Math.Round(medicalAmount, 0);
                                            breakdownWiseSalaryAdjustment = breakdownWiseSalaryAdjustment + adjustmentAmount;
                                        }
                                    }

                                    if (id == 3 && (employee.JobType == Jobtype.Contractual || employee.JobType == Jobtype.Permanent))
                                    {
                                        IsExistInBreakdown = true;
                                        breakDownAmount = conveyance;
                                        if (arrearAmount <= 0)
                                        {
                                            var conveyanceAmount = conveyance / daysInSalaryMonth * totalDayWorked;
                                            adjustmentAmount = conveyance - Math.Round(conveyanceAmount, 0);
                                            breakdownWiseSalaryAdjustment = breakdownWiseSalaryAdjustment + adjustmentAmount;
                                        }
                                    }
                                }
                                #endregion

                                SalaryAllowance salaryAllowance = new SalaryAllowance()
                                {
                                    EmployeeId = employee.EmployeeId,
                                    AllowanceNameId = id,
                                    CalculationForDays = totalDayWorked,
                                    Amount = amount,
                                    ArrearAmount = arrearAmount,
                                    AdjustmentAmount = allowanceAdjustmentAmount,
                                    SalaryDate = salaryEndDate,
                                    SalaryMonth = (short)salaryEndDate.Month,
                                    SalaryYear = (short)salaryEndDate.Year,
                                    MonthlyAllowanceIds = monthlyAllowanceIds,
                                    PeriodicallyAllowanceIds = periodicallyAllowanceIds,
                                    FiscalYearId = fiscalYearInfo.FiscalYearId,
                                    BaseAmount = breakDownAmount,
                                    CreatedBy = user.ActionUserId,
                                    CreatedDate = DateTime.Now,
                                    CompanyId = user.CompanyId,
                                    OrganizationId = user.OrganizationId,
                                    BranchId = employee.BranchId
                                };
                                employeeSalaryProcessed.SalaryAllowances.Add(salaryAllowance);
                            }
                            #endregion

                            #region Marge Salary Deduction
                            List<long> deductions = new List<long>();
                            var unique_salary_deduction = list_of_salary_deduction.Select(i => i.DeductionNameId).Distinct().ToList();
                            var unique_salary_deduction_adjustment = list_of_salary_deduction_adjustment.Select(i => i.DeductionNameId).Distinct().ToList();
                            foreach (var i in unique_salary_deduction)
                            {
                                deductions.Add(i);
                            }
                            foreach (var i in unique_salary_deduction_adjustment)
                            {
                                deductions.Add(i);
                            }

                            deductions = deductions.Distinct().ToList();

                            foreach (var id in deductions)
                            {
                                var amount = list_of_salary_deduction.Where(i => i.DeductionNameId == id).Sum(i => i.Amount);
                                var arrearAmount = list_of_salary_deduction_adjustment.Where(i => i.DeductionNameId == id).Sum(i => i.Amount);
                                var monthlyDeductionIds = string.Join(',', list_of_salary_deduction.Where(i => i.MonthlyDeductionId != null && i.MonthlyDeductionId.Value > 0).Select(i => i.MonthlyDeductionId).ToList());
                                var periodicallyDeductionIds = string.Join(',', list_of_salary_deduction.Where(i => i.PeriodicallyDeductionId != null && i.PeriodicallyDeductionId.Value > 0).Select(i => i.PeriodicallyDeductionId).ToList());

                                SalaryDeduction salaryDeduction = new SalaryDeduction()
                                {
                                    EmployeeId = employee.EmployeeId,
                                    DeductionNameId = id,
                                    CalculationForDays = totalDayWorked,
                                    Amount = amount,
                                    ArrearAmount = arrearAmount,
                                    SalaryDate = salaryEndDate,
                                    SalaryMonth = (short)salaryEndDate.Month,
                                    SalaryYear = (short)salaryEndDate.Year,
                                    //MonthlyAllowanceIds = monthlyDeductionIds,
                                    //PeriodicallyAllowanceIds = periodicallyDeductionIds,
                                    FiscalYearId = fiscalYearInfo.FiscalYearId,
                                    CreatedBy = user.ActionUserId,
                                    CreatedDate = DateTime.Now,
                                    CompanyId = user.CompanyId,
                                    OrganizationId = user.OrganizationId,
                                };
                                employeeSalaryProcessed.SalaryDeductions.Add(salaryDeduction);
                            }

                            var lastSalaryReviewIdWithThisProcess = salaryInfos.OrderByDescending(i => i.ActivationDate).FirstOrDefault();
                            if (lastSalaryReviewIdWithThisProcess != null)
                            {
                                var lastSalaryReviewDetails = await _salaryReviewBusiness.GetSalaryReviewDetailsAsync(lastSalaryReviewIdWithThisProcess.SalaryReviewInfoId, user);
                                if (lastSalaryReviewDetails != null)
                                {
                                    employeeSalaryProcessed.SalaryProcessDetail.CurrentBasic = lastSalaryReviewDetails.Sum(item => item.AllowanceNameId = basicAllowanceInfo.AllowanceNameId);
                                    employeeSalaryProcessed.SalaryProcessDetail.CurrentHouseRent = lastSalaryReviewDetails.Sum(item => item.AllowanceNameId = houseAllowanceInfo.AllowanceNameId);
                                    employeeSalaryProcessed.SalaryProcessDetail.CurrentMedical = lastSalaryReviewDetails.Sum(item => item.AllowanceNameId = medicalAllowanceInfo.AllowanceNameId);
                                    employeeSalaryProcessed.SalaryProcessDetail.CurrentConveyance = lastSalaryReviewDetails.Sum(item => item.AllowanceNameId = conveyanceAllowanceInfo.AllowanceNameId);
                                }
                            }
                            #endregion

                            #region Salary Component Hostory
                            if (list_of_salary_component_History.Any())
                            {
                                employeeSalaryProcessed.SalaryComponentHistories = list_of_salary_component_History;
                            }
                            #endregion

                            #region Salary Process Detail
                            employeeSalaryProcessed.SalaryProcessDetail.EmployeeId = employee.EmployeeId;
                            employeeSalaryProcessed.SalaryProcessDetail.GradeId = employee.GradeId;
                            employeeSalaryProcessed.SalaryProcessDetail.Grade = employee.GradeName;
                            employeeSalaryProcessed.SalaryProcessDetail.Designation = employee.DesignationName;
                            employeeSalaryProcessed.SalaryProcessDetail.DesignationId = employee.DesignationId;
                            employeeSalaryProcessed.SalaryProcessDetail.Department = employee.DepartmentName;
                            employeeSalaryProcessed.SalaryProcessDetail.DepartmentId = employee.DepartmentId;
                            employeeSalaryProcessed.SalaryProcessDetail.Section = employee.SectionName;
                            employeeSalaryProcessed.SalaryProcessDetail.SectionId = employee.SectionId;
                            employeeSalaryProcessed.SalaryProcessDetail.SubSection = employee.SubSectionName;
                            employeeSalaryProcessed.SalaryProcessDetail.SubsectionId = employee.SubSectionId;
                            employeeSalaryProcessed.SalaryProcessDetail.CostCenter = employee.CostCenterName;
                            employeeSalaryProcessed.SalaryProcessDetail.CostCenterCode = employee.CostCenterCode;
                            employeeSalaryProcessed.SalaryProcessDetail.CostCenterId = employee.CostCenterId;
                            employeeSalaryProcessed.SalaryProcessDetail.JobType = employee.JobType;
                            employeeSalaryProcessed.SalaryProcessDetail.JobCategory = employee.JobCategory;
                            employeeSalaryProcessed.SalaryProcessDetail.JobCategoryId = employee.JobCategoryId;
                            employeeSalaryProcessed.SalaryProcessDetail.InternalDesignationId = employee.InternalDesignationId;
                            employeeSalaryProcessed.SalaryProcessDetail.InternalDesignation = employee.InternalDesignationName;
                            employeeSalaryProcessed.SalaryProcessDetail.EmployeeType = employee.EmployeeType;
                            employeeSalaryProcessed.SalaryProcessDetail.EmployeeTypeId = employee.EmployeeTypeId;

                            employeeSalaryProcessed.SalaryProcessDetail.LastSalaryReviewDate =
                                salaryInfos.LastOrDefault() != null ? salaryInfos.LastOrDefault().ActivationDate : null;
                            employeeSalaryProcessed.SalaryProcessDetail.LastSalaryReviewId = salaryInfos.LastOrDefault() != null ? salaryInfos.LastOrDefault().SalaryReviewInfoId : null;
                            employeeSalaryProcessed.SalaryProcessDetail.CalculationForDays = totalDayWorked;
                            employeeSalaryProcessed.SalaryProcessDetail.HoldDays = (short)holdDays;


                            employeeSalaryProcessed.SalaryProcessDetail.UnholdDays = 0;
                            employeeSalaryProcessed.SalaryProcessDetail.UnholdAmount = 0;
                            employeeSalaryProcessed.SalaryProcessDetail.BranchId = employee.BranchId;
                            employeeSalaryProcessed.SalaryProcessDetail.SalaryDate = DateTimeExtension.LastDateOfAMonth(execution.Year, execution.Month);
                            employeeSalaryProcessed.SalaryProcessDetail.FiscalYearId = fiscalYearInfo.FiscalYearId;

                            employeeSalaryProcessed.SalaryProcessDetail.SalaryMonth = (short)salaryEndDate.Month;
                            employeeSalaryProcessed.SalaryProcessDetail.SalaryYear = (short)salaryEndDate.Year;

                            var employeeBranchInfo = branchInfos.FirstOrDefault(b => b.BranchId == employee.BranchId);
                            if (employeeBranchInfo != null)
                            {
                                employeeSalaryProcessed.SalaryProcessDetail.BranchName = employeeBranchInfo.BranchName;
                            }

                            employeeSalaryProcessed.SalaryProcessDetail.CurrentBasic = currentBasic;
                            employeeSalaryProcessed.SalaryProcessDetail.CurrentHouseRent = currentHR;
                            employeeSalaryProcessed.SalaryProcessDetail.CurrentMedical = currentMedical;
                            employeeSalaryProcessed.SalaryProcessDetail.CurrentConveyance = currentConveyance;

                            employeeSalaryProcessed.SalaryProcessDetail.ThisMonthBasic = employeeSalaryProcessed.SalaryAllowances.Where(i => i.AllowanceNameId == basicAllowanceInfo.AllowanceNameId).Sum(item => item.Amount);

                            employeeSalaryProcessed.SalaryProcessDetail.ThisMonthHouseRent = employeeSalaryProcessed.SalaryAllowances.Where(i => i.AllowanceNameId == houseAllowanceInfo.AllowanceNameId).Sum(item => item.Amount);

                            employeeSalaryProcessed.SalaryProcessDetail.ThisMonthMedical = employeeSalaryProcessed.SalaryAllowances.Where(i => i.AllowanceNameId == medicalAllowanceInfo.AllowanceNameId).Sum(item => item.Amount);

                            employeeSalaryProcessed.SalaryProcessDetail.ThisMonthConveyance = employeeSalaryProcessed.SalaryAllowances.Where(i => i.AllowanceNameId == conveyanceAllowanceInfo.AllowanceNameId).Sum(item => item.Amount);

                            employeeSalaryProcessed.SalaryProcessDetail.BreakdownWiseSalaryAdjustment = breakdownWiseSalaryAdjustment;

                            pfAmount = Math.Round(pfAmount, MidpointRounding.AwayFromZero);
                            pfArrear = Math.Round(pfArrear, MidpointRounding.AwayFromZero);

                            employeeSalaryProcessed.SalaryProcessDetail.TotalAllowance = employeeSalaryProcessed.SalaryAllowances.Sum(item => item.Amount);
                            employeeSalaryProcessed.SalaryProcessDetail.TotalAllowanceAdjustment = totalAdjustment;
                           employeeSalaryProcessed.SalaryProcessDetail.UnholdDays = unholdDays;
                            employeeSalaryProcessed.SalaryProcessDetail.TotalArrearAllowance = employeeSalaryProcessed.SalaryAllowanceArrears.Sum(item => item.Amount);
                            employeeSalaryProcessed.SalaryProcessDetail.PFAmount = pfAmount;
                            employeeSalaryProcessed.SalaryProcessDetail.PFArrear = pfArrear;
                            employeeSalaryProcessed.SalaryProcessDetail.TotalPFAmount = Math.Round(pfAmount) + Math.Round(pfArrear);
                            employeeSalaryProcessed.SalaryProcessDetail.ActualPFAmount = Math.Round(pfAmount);
                            employeeSalaryProcessed.SalaryProcessDetail.ActualPFArrear = Math.Round(pfArrear);
                            employeeSalaryProcessed.SalaryProcessDetail.TotalActualPFAmount = Math.Round(pfAmount) + Math.Round(pfArrear);

                            if (holdAmount > 0)
                            {
                                if (user.CompanyId == 17 && user.OrganizationId == 10)
                                {
                                    holdAmount = employeeSalaryProcessed.SalaryAllowances.Sum(i => i.Amount + i.ArrearAmount);
                                    var totalDeduction = employeeSalaryProcessed.SalaryDeductions.Sum(i => i.Amount);

                                    employeeSalaryProcessed.SalaryProcessDetail.HoldAmount = Math.Round(holdAmount) - (employeeSalaryProcessed.SalaryProcessDetail.PFAmount + employeeSalaryProcessed.SalaryProcessDetail.PFArrear + totalDeduction);
                                }
                                else
                                {
                                    employeeSalaryProcessed.SalaryProcessDetail.HoldAmount = Math.Round(holdAmount) - (employeeSalaryProcessed.SalaryProcessDetail.PFAmount + employeeSalaryProcessed.SalaryProcessDetail.PFArrear);
                                }
                            }
                            else
                            {
                                employeeSalaryProcessed.SalaryProcessDetail.HoldAmount = 0;
                            }

                            if (holdAmount > 0 && user.CompanyId == 17 && user.OrganizationId == 10 && salaryStartDate.Month == 3 && salaryStartDate.Year == 2024)
                            {
                                holdAmount = (employeeSalaryProcessed.SalaryProcessDetail.HoldAmount ?? 0) + employeeSalaryProcessed.SalaryAllowances.Where(item => item.AllowanceNameId == 8).Sum(item => item.Amount);
                                employeeSalaryProcessed.SalaryProcessDetail.HoldAmount = holdAmount;
                            }

                            employeeSalaryProcessed.SalaryProcessDetail.TotalDeduction = employeeSalaryProcessed.SalaryDeductions.Sum(item => item.Amount);

                            if (user.CompanyId == 17 && user.OrganizationId == 10)
                            {
                                employeeSalaryProcessed.SalaryProcessDetail.TotalDeduction = employeeSalaryProcessed.SalaryProcessDetail.TotalDeduction + (employeeSalaryProcessed.SalaryProcessDetail.TotalPFAmount ?? 0);
                            }

                            employeeSalaryProcessed.SalaryProcessDetail.GrossPay = employeeSalaryProcessed.SalaryProcessDetail.TotalAllowance + employeeSalaryProcessed.SalaryProcessDetail.TotalArrearAllowance + 0;
                            employeeSalaryProcessed.SalaryProcessDetail.TotalArrearDeduction = Math.Round(pfArrear);


                            if (user.CompanyId == 17 && user.OrganizationId == 10)
                            {
                                if ((employeeSalaryProcessed.SalaryProcessDetail.HoldAmount ?? 0) > 0)
                                {
                                    var totalDeduction = employeeSalaryProcessed.SalaryDeductions.Sum(i => i.Amount) + totalAdjustment;
                                    employeeSalaryProcessed.SalaryProcessDetail.NetPay = employeeSalaryProcessed.SalaryProcessDetail.GrossPay - ((employeeSalaryProcessed.SalaryProcessDetail.HoldAmount ?? 0) + employeeSalaryProcessed.SalaryProcessDetail.PFAmount + employeeSalaryProcessed.SalaryProcessDetail.PFArrear + totalDeduction);
                                }
                                else
                                {
                                    employeeSalaryProcessed.SalaryProcessDetail.NetPay = employeeSalaryProcessed.SalaryProcessDetail.GrossPay - (employeeSalaryProcessed.SalaryProcessDetail.TotalDeduction + totalAdjustment);
                                }
                            }
                            else
                            {
                                employeeSalaryProcessed.SalaryProcessDetail.NetPay = employeeSalaryProcessed.SalaryProcessDetail.GrossPay - (employeeSalaryProcessed.SalaryProcessDetail.TotalDeduction
                                + employeeSalaryProcessed.SalaryProcessDetail.PFAmount
                                + employeeSalaryProcessed.SalaryProcessDetail.PFArrear
                                + totalAdjustment
                                );
                            }
                            employeeSalaryProcessed.SalaryProcessDetail.SalaryProcessId = 0;
                            employeeSalaryProcessed.SalaryProcessDetail.AccountId = employee.AccountInfoId;
                            employeeSalaryProcessed.SalaryProcessDetail.BankId = employee.BankId;
                            employeeSalaryProcessed.SalaryProcessDetail.BankTransferAmount = employeeSalaryProcessed.SalaryProcessDetail.NetPay;
                            employeeSalaryProcessed.SalaryProcessDetail.BankBranchId = employee.BankBranchId;
                            employeeSalaryProcessed.SalaryProcessDetail.BankAccountNumber = employee.BankAccount;
                            employeeSalaryProcessed.SalaryProcessDetail.WalletAgent = employee.WalletAgent;
                            employeeSalaryProcessed.SalaryProcessDetail.WalletNumber = employee.WalletNumber;
                            employeeSalaryProcessed.SalaryProcessDetail.WalletTransferAmont = 0;
                            employeeSalaryProcessed.SalaryProcessDetail.COCInWalletTransfer = 0;
                            employeeSalaryProcessed.SalaryProcessDetail.CashAmount = 0;
                            employeeSalaryProcessed.SalaryProcessDetail.CreatedBy = user.ActionUserId;
                            employeeSalaryProcessed.SalaryProcessDetail.CreatedDate = DateTime.Now.Date;
                            employeeSalaryProcessed.SalaryProcessDetail.CompanyId = user.CompanyId;
                            employeeSalaryProcessed.SalaryProcessDetail.OrganizationId = user.OrganizationId;
                            #endregion

                            listOfemployeeSalaryProcessed.Add(employeeSalaryProcessed);
                        }
                    }

                    if (listOfemployeeSalaryProcessed.Any())
                    {

                    }
                    else
                    {
                        executionStatus.Status = true;
                        executionStatus.Msg = "No data found to process";
                    }
                }
                catch (Exception ex)
                {
                    await _sysLogger.SaveHRMSException(ex, user.Database, "ExecuteSalaryProcess", "RunSystematically", user);
                }
            }
            else
            {
                executionStatus.Status = true;
                executionStatus.Msg = "No eligible found to process";
            }
            return listOfemployeeSalaryProcessed;
        }
        public async Task<EmployeeSalaryProcessedInfo> ReProcessSystematically(SalaryProcessExecution execution, EligibleEmployeeForSalaryType employee, SalaryProcessDetail salaryProcessDetail, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            EmployeeSalaryProcessedInfo employeeSalaryProcessed = new EmployeeSalaryProcessedInfo();
            if (employee != null)
            {
                var payrollModuleConfig = await _moduleConfig.PayrollModuleConfig(user);
                decimal pfPercentage = Utility.TryParseInt(payrollModuleConfig.PercentageOfProvidentFund);
                var fiscalYearInfo = await _fiscalYearBusiness.GetFiscalYearInfoWithinADate(DateTimeExtension.LastDateOfAMonth(execution.Year, execution.Month).ToString("yyyy-MM-dd"), user);

                var salaryStartDate = DateTimeExtension.FirstDateOfAMonth(execution.Year, execution.Month);
                var salaryEndDate = DateTimeExtension.LastDateOfAMonth(execution.Year, execution.Month);

                var basicAllowance = (await _allowanceNameBusiness.GetAllowanceNamesAsync(new AllowanceName_Filter()
                {
                    AllowanceFlag = "BASIC"
                }, user)).FirstOrDefault();
                AllowanceNameViewModel basicAllowanceInfo = null;
                if (basicAllowance != null)
                {
                    basicAllowanceInfo = basicAllowance;
                }

                var houseAllowance = (await _allowanceNameBusiness.GetAllowanceNamesAsync(new AllowanceName_Filter()
                {
                    AllowanceFlag = "HR"
                }, user)).FirstOrDefault();
                AllowanceNameViewModel houseAllowanceInfo = null;
                if (houseAllowance != null)
                {
                    houseAllowanceInfo = houseAllowance;
                }

                var conveyanceAllowance = (await _allowanceNameBusiness.GetAllowanceNamesAsync(new AllowanceName_Filter()
                {
                    AllowanceFlag = "CONVEYANCE"
                }, user)).FirstOrDefault();
                AllowanceNameViewModel conveyanceAllowanceInfo = null;
                if (conveyanceAllowance != null)
                {
                    conveyanceAllowanceInfo = conveyanceAllowance;
                }

                var medicalAllowance = (await _allowanceNameBusiness.GetAllowanceNamesAsync(new AllowanceName_Filter()
                {
                    AllowanceFlag = "MEDICAL"
                }, user)).FirstOrDefault();
                AllowanceNameViewModel medicalAllowanceInfo = null;
                if (conveyanceAllowance != null)
                {
                    medicalAllowanceInfo = medicalAllowance;
                }


                try
                {
                    bool isActualDays = string.IsNullOrEmpty(payrollModuleConfig.WhatDoesConsiderationForMonth) ? true :
                        payrollModuleConfig.WhatDoesConsiderationForMonth == "Actual Days" ? true : false;
                    int actualDaysInSalaryMonth = salaryEndDate.DaysInAMonth();
                    int daysInSalaryMonth = isActualDays == true ? actualDaysInSalaryMonth : 30;

                    List<SalaryAllowance> list_of_salary_allowance = new List<SalaryAllowance>();
                    List<SalaryAllowanceArrear> list_of_salary_allowance_arrear = new List<SalaryAllowanceArrear>();
                    List<SalaryAllowanceAdjustment> list_of_salary_allowance_adjustment = new List<SalaryAllowanceAdjustment>();
                    List<SalaryDeduction> list_of_salary_deduction = new List<SalaryDeduction>();
                    List<SalaryDeductionAdjustment> list_of_salary_deduction_adjustment = new List<SalaryDeductionAdjustment>();
                    List<DepositAllowanceHistory> list_of_deposite_Allowance_History = new List<DepositAllowanceHistory>();
                    List<SalaryComponentHistory> list_of_salary_component_History = new List<SalaryComponentHistory>();

                    var employeePFInfo = await _employeePFActivationBusiness.EmployeePFActionInfoAysnc(employee.EmployeeId, user);
                    int totalDayWorked = 0;
                    int holdDays = 0;
                    decimal holdAmount = 0;
                    var salaryInfos = await _salaryReviewBusiness.GetEmployeeSalaryReviewesInSalaryProcess(new SalaryReviewInSalaryProcess_Filter()
                    {
                        EmployeeId = employee.EmployeeId.ToString(),
                        SalaryStartDate = salaryStartDate.ToString("yyyy-MM-dd"),
                        SalaryEndDate = salaryEndDate.ToString("yyyy-MM-dd"),
                    }, user);

                    decimal pfAmount = 0; decimal pfArrear = 0;
                    foreach (var salaryInfo in salaryInfos)
                    {
                        decimal salaryDiffAmount = 0;
                        var salaryReviewDetails = await _salaryReviewBusiness.GetSalaryReviewDetailsAsync(salaryInfo.SalaryReviewInfoId, user);
                        var grossAmount = salaryReviewDetails.Sum(i => i.CurrentAmount);
                        var perDayGross = Math.Round(grossAmount / daysInSalaryMonth);

                        decimal basicAmount = 0;
                        if (basicAllowanceInfo != null)
                        {
                            var basicAllowanceInSalaryReview = salaryReviewDetails.FirstOrDefault(i => i.AllowanceNameId == basicAllowanceInfo.AllowanceNameId);
                            if (basicAllowanceInSalaryReview != null)
                            {
                                basicAmount = basicAllowanceInSalaryReview.CurrentAmount;
                            }
                        }
                        var perDayBasic = basicAmount > 0 ? Math.Round(basicAmount / daysInSalaryMonth) : 0;
                        int daysWorked = 0;

                        if (salaryInfo.DeactivationDate == null || salaryInfo.DeactivationDate.Value > salaryStartDate)
                        {
                            DateTime? startDate = null;
                            DateTime? endDate = null;
                            startDate = salaryInfo.EffectiveFrom.Value <= salaryStartDate ? salaryStartDate : salaryInfo.EffectiveFrom.Value > salaryStartDate ? salaryInfo.EffectiveFrom.Value : salaryStartDate;

                            endDate = salaryInfo.EffectiveTo == null ? salaryEndDate : salaryInfo.EffectiveTo.Value >= salaryEndDate ? salaryEndDate : salaryInfo.EffectiveTo.Value < salaryEndDate ? salaryInfo.EffectiveTo.Value : salaryEndDate;

                            var presentCountParams = new PresentCountBetweenSalaryDates_Filter()
                            {
                                EmployeeId = employee.EmployeeId,
                                FirstDate = startDate,
                                SecondDate = endDate,
                                JoiningDate = employee.DateOfJoining,
                                IsActualDays = isActualDays,
                                SalaryDate = execution.SalaryDate,
                                TerminationDate = employee.TerminationDate

                            };
                            daysWorked = await GetPresentCountBetweenSalaryDates(presentCountParams, user);

                            if (actualDaysInSalaryMonth != daysInSalaryMonth)
                            {
                                if (daysWorked == actualDaysInSalaryMonth)
                                {
                                    daysWorked = daysInSalaryMonth;
                                }
                                else if (daysWorked < actualDaysInSalaryMonth && actualDaysInSalaryMonth >= daysInSalaryMonth)
                                {
                                    daysWorked = daysInSalaryMonth - (daysInSalaryMonth - daysWorked);
                                }
                                else if (daysWorked < actualDaysInSalaryMonth && actualDaysInSalaryMonth == 28 && actualDaysInSalaryMonth < daysInSalaryMonth)
                                {
                                    daysWorked = daysInSalaryMonth - (daysInSalaryMonth - (daysWorked + 2));
                                }
                                else if (daysWorked < actualDaysInSalaryMonth && actualDaysInSalaryMonth == 29 && actualDaysInSalaryMonth < daysInSalaryMonth)
                                {
                                    daysWorked = daysInSalaryMonth - (daysInSalaryMonth - (daysWorked + 1));
                                }
                                else
                                {
                                    daysWorked = daysInSalaryMonth;
                                }
                            }

                            totalDayWorked = totalDayWorked + daysWorked;

                            foreach (var salaryReview in salaryReviewDetails)
                            {
                                SalaryAllowance salaryAllowance = new SalaryAllowance();
                                salaryAllowance.EmployeeId = employee.EmployeeId;
                                salaryAllowance.SalaryMonth = execution.Month;
                                salaryAllowance.SalaryYear = execution.Year;
                                salaryAllowance.SalaryDate = DateTimeExtension.LastDateOfAMonth(execution.Year, execution.Month);
                                salaryAllowance.CalculationForDays = daysWorked;
                                salaryAllowance.AllowanceNameId = salaryReview.AllowanceNameId;
                                salaryAllowance.Amount = salaryReview.CurrentAmount / daysInSalaryMonth * daysWorked + (salaryReview.AdditionalAmount ?? 0);
                                salaryAllowance.ArrearAmount = 0;
                                salaryAllowance.Remarks = "";
                                salaryAllowance.CreatedBy = user.ActionUserId;
                                salaryAllowance.CreatedDate = DateTime.Now;
                                salaryAllowance.BranchId = employee.BranchId;
                                salaryAllowance.CompanyId = user.CompanyId;
                                salaryAllowance.OrganizationId = user.OrganizationId;
                                salaryAllowance.SalaryReviewInfoId = salaryInfo.SalaryReviewInfoId;
                                salaryAllowance.FiscalYearId = fiscalYearInfo.FiscalYearId;
                                list_of_salary_allowance.Add(salaryAllowance);
                            }

                            int currentHoldDays = await GetEmployeeHoldDays(presentCountParams, user);

                            currentHoldDays = currentHoldDays > daysInSalaryMonth ? daysInSalaryMonth : currentHoldDays;
                            holdDays = holdDays + currentHoldDays;

                            if (currentHoldDays > 0)
                            {
                                foreach (var salaryReview in salaryReviewDetails)
                                {
                                    holdAmount = holdAmount + salaryReview.CurrentAmount / daysInSalaryMonth * currentHoldDays;
                                }
                            }

                        }

                        // Arrear Calculation
                        if (salaryInfo.IsArrearCalculated == false && salaryInfo.ArrearCalculatedDate != null
                            && salaryInfo.EffectiveFrom.Value.Date < salaryInfo.ActivationDate.Value.Date
                            && salaryInfo.ArrearCalculatedDate.Value.Month == salaryStartDate.Month
                            && salaryInfo.ArrearCalculatedDate.Value.Year == salaryStartDate.Year)
                        {

                            if (salaryInfo.PreviousSalaryAmount == 0)
                            {
                                salaryDiffAmount = salaryInfo.PreviousSalaryAmount;
                            }
                            else if (salaryInfo.CurrentSalaryAmount > salaryInfo.PreviousSalaryAmount)
                            {
                                salaryDiffAmount = salaryInfo.CurrentSalaryAmount - salaryInfo.PreviousSalaryAmount;
                            }
                            else if (salaryInfo.CurrentSalaryAmount < salaryInfo.PreviousSalaryAmount)
                            {
                                salaryDiffAmount = salaryInfo.PreviousSalaryAmount - salaryInfo.CurrentSalaryAmount;
                            }
                            else
                            {
                                salaryDiffAmount = 0;
                            }
                            var salaryReviewMonthDiff = salaryInfo.EffectiveFrom.Value.GetMonthDiffExcludingThisMonth(salaryInfo.ActivationDate.Value);

                            var arrearMonthDate = salaryInfo.EffectiveFrom.Value;

                            int salaryReviewDayDiff = 0;
                            int daysInArrearMonth = 0;

                            for (int arrearMonthCount = 1; arrearMonthCount <= salaryReviewMonthDiff; arrearMonthCount++)
                            {
                                if (arrearMonthCount == 1)
                                {
                                    salaryReviewDayDiff = arrearMonthDate.DaysBetweenDateRangeIncludingStartDate(arrearMonthDate.LastDateOfAMonth());
                                }
                                else if (arrearMonthCount > 1 && arrearMonthCount < salaryReviewMonthDiff)
                                {
                                    salaryReviewDayDiff = arrearMonthDate.DaysBetweenDateRangeIncludingStartDate(arrearMonthDate.LastDateOfAMonth());
                                }
                                else
                                {
                                    salaryReviewDayDiff = arrearMonthDate.DaysBetweenDateRangeExcludingStartDate(salaryInfo.ActivationDate.Value.LastDateOfAMonth());
                                }

                                daysInArrearMonth = arrearMonthDate.DaysInAMonth();
                                foreach (var salaryReview in salaryReviewDetails)
                                {
                                    var isTrue = this.IsItSpecialAllowanceOfPWCForTheMonthJune(employee.EmployeeCode, salaryReview.AllowanceNameId, salaryStartDate.Year, salaryStartDate.Month, user);

                                    if (isTrue == false)
                                    {
                                        SalaryAllowanceArrear salaryAllowanceArrear = new SalaryAllowanceArrear();
                                        salaryAllowanceArrear.EmployeeId = employee.EmployeeId;
                                        salaryAllowanceArrear.AllowanceNameId = salaryReview.AllowanceNameId;

                                        salaryAllowanceArrear.SalaryDate = DateTimeExtension.LastDateOfAMonth(execution.Year, execution.Month);
                                        salaryAllowanceArrear.SalaryMonth = execution.Month;
                                        salaryAllowanceArrear.SalaryYear = execution.Year;
                                        salaryAllowanceArrear.CalculationForDays = salaryReviewDayDiff;
                                        if (salaryReview.CurrentAmount >= salaryReview.PreviousAmount)
                                        {
                                            salaryAllowanceArrear.Amount = (salaryReview.CurrentAmount - salaryReview.PreviousAmount) / daysInArrearMonth * salaryReviewDayDiff;
                                        }
                                        salaryAllowanceArrear.ArrearMonth = (short)arrearMonthDate.Month;
                                        salaryAllowanceArrear.ArrearYear = (short)arrearMonthDate.Year;
                                        salaryAllowanceArrear.ArrearFrom = null;
                                        salaryAllowanceArrear.ArrearTo = null;
                                        salaryAllowanceArrear.SalaryReviewInfoId = salaryReview.SalaryReviewInfoId;
                                        salaryAllowanceArrear.CreatedBy = user.ActionUserId;
                                        salaryAllowanceArrear.CreatedDate = DateTime.Now;
                                        salaryAllowanceArrear.BranchId = employee.BranchId;
                                        salaryAllowanceArrear.CompanyId = user.CompanyId;
                                        salaryAllowanceArrear.OrganizationId = user.OrganizationId;
                                        salaryAllowanceArrear.FiscalYearId = fiscalYearInfo.FiscalYearId;
                                        list_of_salary_allowance_arrear.Add(salaryAllowanceArrear);
                                    }
                                }
                                arrearMonthDate = arrearMonthDate.AddMonths(1);
                                arrearMonthDate = arrearMonthDate.FirstDateOfAMonth();
                                salaryReviewDayDiff = 0;
                            }

                        }

                        // PF Calculation
                        int pfDays = 0; int totalPFDays = 0;
                        if (payrollModuleConfig.IsProvidentFundactivated.Value == true
                            && Utility.TryParseInt(payrollModuleConfig.PercentageOfProvidentFund) > 0
                            && daysWorked > 0)
                        {
                            decimal earnedAmount = 0; decimal arrearAmount = 0;
                            if (payrollModuleConfig.BaseOfProvidentFund.ToLower() == "basic")
                            {
                                earnedAmount = list_of_salary_allowance.Where(i => (i.SalaryReviewInfoId ?? 0) == salaryInfo.SalaryReviewInfoId && i.AllowanceNameId == basicAllowanceInfo.AllowanceNameId).Sum(i => i.Amount);
                                arrearAmount = employeeSalaryProcessed.SalaryAllowanceArrears.Where(i => (i.SalaryReviewInfoId ?? 0) == salaryInfo.SalaryReviewInfoId && i.AllowanceNameId == basicAllowanceInfo.AllowanceNameId).Sum(i => i.Amount);
                            }
                            else
                            {
                                earnedAmount = list_of_salary_allowance.Where(i => (i.SalaryReviewInfoId ?? 0) == salaryInfo.SalaryReviewInfoId).Sum(i => i.Amount);
                                arrearAmount = employeeSalaryProcessed.SalaryAllowanceArrears.Where(i => (i.SalaryReviewInfoId ?? 0) == salaryInfo.SalaryReviewInfoId).Sum(i => i.Amount);
                            }

                            if (employee.PFActiovationDate == null && employee.IsPFMember == true)
                            {
                                if (salaryInfo.EffectiveFrom.Value.Month == execution.Month
                                    && salaryInfo.EffectiveFrom.Value.Year == execution.Year
                                    && salaryInfo.ActivationDate.Value.Month == execution.Month
                                    && salaryInfo.ActivationDate.Value.Year == execution.Year)
                                {
                                    if (salaryInfo.ActivationDate.Value.IsDateBetweenTwoDates(salaryStartDate, salaryEndDate) && salaryInfo.DeactivationDate == null)
                                    {
                                        pfDays = salaryInfo.ActivationDate.Value.DaysBetweenDateRangeIncludingStartDate(salaryEndDate);
                                        totalPFDays = totalPFDays + pfDays;
                                        pfAmount = pfAmount + earnedAmount * (pfPercentage / 100);
                                        pfArrear = pfArrear + arrearAmount * (pfPercentage / 100);
                                    }
                                }
                                else if (salaryInfo.EffectiveFrom.Value.Month == execution.Month && salaryInfo.EffectiveFrom.Value.Year == execution.Year
                                    && salaryInfo.ActivationDate.Value < salaryStartDate)
                                {
                                    pfDays = salaryInfo.ActivationDate.Value.DaysBetweenDateRangeIncludingStartDate(salaryEndDate);
                                    totalPFDays = totalPFDays + pfDays;
                                    pfAmount = pfAmount + earnedAmount * (pfPercentage / 100);
                                    pfArrear = pfArrear + arrearAmount * (pfPercentage / 100);
                                }
                                else if (salaryInfo.EffectiveFrom.Value < salaryStartDate && salaryInfo.DeactivationDate == null)
                                {
                                    pfDays = salaryStartDate.DaysBetweenDateRangeIncludingStartDate(salaryEndDate);
                                    totalPFDays = totalPFDays + pfDays;
                                    pfAmount = pfAmount + earnedAmount * (pfPercentage / 100);
                                    pfArrear = pfArrear + arrearAmount * (pfPercentage / 100);
                                }
                                else if (salaryInfo.EffectiveFrom.Value < salaryStartDate && salaryInfo.DeactivationDate != null && salaryInfo.DeactivationDate.Value.IsDateBetweenTwoDates(salaryStartDate, salaryEndDate) == true)
                                {
                                    pfDays = salaryStartDate.DaysBetweenDateRangeIncludingStartDate(salaryInfo.DeactivationDate.Value.Date);
                                    totalPFDays = totalPFDays + pfDays;
                                    pfAmount = pfAmount + earnedAmount * (pfPercentage / 100);
                                    pfArrear = pfArrear + arrearAmount * (pfPercentage / 100);
                                }
                                else if (salaryInfo.EffectiveFrom.Value < salaryStartDate && salaryInfo.DeactivationDate != null && salaryInfo.DeactivationDate.Value > salaryEndDate)
                                {
                                    pfDays = salaryStartDate.DaysBetweenDateRangeIncludingStartDate(salaryEndDate.Date);
                                    totalPFDays = totalPFDays + pfDays;
                                    pfAmount = pfAmount + earnedAmount * (pfPercentage / 100);
                                    pfArrear = pfArrear + arrearAmount * (pfPercentage / 100);
                                }
                                else if (salaryInfo.EffectiveFrom.Value.IsDateBetweenTwoDates(salaryStartDate, salaryEndDate) && salaryInfo.DeactivationDate != null && salaryInfo.DeactivationDate.Value.IsDateBetweenTwoDates(salaryStartDate, salaryEndDate))
                                {
                                    pfDays = salaryInfo.EffectiveFrom.Value.Date.DaysBetweenDateRangeIncludingStartDate(salaryInfo.DeactivationDate.Value.Date);
                                    totalPFDays = totalPFDays + pfDays;
                                    pfAmount = pfAmount + earnedAmount * (pfPercentage / 100);
                                    pfArrear = pfArrear + arrearAmount * (pfPercentage / 100);
                                }
                                else if (salaryInfo.EffectiveFrom.Value.IsDateBetweenTwoDates(salaryStartDate, salaryEndDate) && salaryInfo.DeactivationDate != null && salaryInfo.DeactivationDate.Value > salaryEndDate)
                                {
                                    pfDays = salaryInfo.EffectiveFrom.Value.Date.DaysBetweenDateRangeIncludingStartDate(salaryEndDate.Date);
                                    totalPFDays = totalPFDays + pfDays;
                                    pfAmount = pfAmount + earnedAmount * (pfPercentage / 100);
                                    pfArrear = pfArrear + arrearAmount * (pfPercentage / 100);
                                }

                            }
                            else
                            {

                                if (employeePFInfo != null)
                                {
                                    if (employeePFInfo.ActiveDate.HasValue)
                                    {
                                        if (employeePFInfo.ActiveDate.Value.Date.IsDateBetweenTwoDates(salaryStartDate.Date, salaryEndDate.Date))
                                        {
                                            if (employeePFInfo.PFEffectiveDate.Value.Date.IsDateBetweenTwoDates(salaryStartDate.Date, salaryEndDate.Date))
                                            {
                                                if (employeePFInfo.PFEffectiveDate.Value.Date == employeePFInfo.ActiveDate.Value.Date)
                                                {
                                                    if (salaryInfo.ActivationDate.Value.IsDateBetweenTwoDates(employeePFInfo.PFEffectiveDate.Value, salaryEndDate) && salaryInfo.DeactivationDate == null)
                                                    {
                                                        pfDays = salaryInfo.ActivationDate.Value.DaysBetweenDateRangeIncludingStartDate(salaryEndDate);
                                                        totalPFDays = totalPFDays + pfDays;
                                                        pfAmount = pfAmount + earnedAmount * (pfPercentage / 100);
                                                        pfArrear = pfArrear + arrearAmount * (pfPercentage / 100);
                                                    }

                                                    else if (salaryInfo.ActivationDate.Value < employeePFInfo.PFEffectiveDate.Value
                                                        && (salaryInfo.DeactivationDate == null
                                                        || salaryInfo.DeactivationDate != null && salaryInfo.DeactivationDate.Value > salaryEndDate))
                                                    {
                                                        pfDays = employeePFInfo.PFEffectiveDate.Value.DaysBetweenDateRangeIncludingStartDate(salaryEndDate);
                                                        totalPFDays = totalPFDays + pfDays;
                                                        pfAmount = pfAmount + earnedAmount * (pfPercentage / 100);
                                                        pfArrear = pfArrear + arrearAmount * (pfPercentage / 100);
                                                    }
                                                    else if (salaryInfo.ActivationDate.Value < employeePFInfo.PFEffectiveDate.Value) { }
                                                }
                                            }
                                            else
                                            {

                                            }
                                        }
                                        else if (employeePFInfo.ActiveDate.Value.Date < salaryStartDate)
                                        {
                                            pfAmount = pfAmount + earnedAmount * (pfPercentage / 100);
                                            pfArrear = pfArrear + arrearAmount * (pfPercentage / 100);
                                        }
                                    }
                                }
                            }
                        }
                    }

                    // Process PF If it is Activate this month but effective previously
                    //------------------------------------------------------------------
                    if (salaryInfos.Any())
                    {
                        employeeSalaryProcessed.SalaryProcessDetail.SalaryReviewInfoIds = string.Join(',', salaryInfos.Select(i => i.SalaryReviewInfoId).ToArray());


                        // Hold Salaries
                        short unholdDays = 0;
                        var holdSalaries = await _salaryHoldBusiness.GetEmployeeUnholdSalaryInfoAsync(employee.EmployeeId, salaryStartDate.Month, salaryStartDate.Year, user);
                        foreach (var holdSalary in holdSalaries)
                        {
                            var salaryReviewIds = await _salaryProcessBusiness.GetEmployeeSalaryProcessedReviewIdsOfaMonthAsync(employee.EmployeeId, holdSalary.HoldFrom.Value.Month, holdSalary.HoldFrom.Value.Year, fiscalYearInfo.FiscalYearId, user);

                            var actualDaysInHoldMonth = holdSalary.HoldFrom.Value.DaysInAMonth();
                            var daysInHold = isActualDays == true ? actualDaysInHoldMonth : 30;

                            foreach (var item in salaryReviewIds)
                            {
                                var salaryReviewInfo = (await _salaryReviewBusiness.GetSalaryReviewInfosAsync(new SalaryReview_Filter()
                                {
                                    EmployeeId = employee.EmployeeId.ToString(),
                                    SalaryReviewInfoId = item.ToString()
                                }, user)).FirstOrDefault();
                                //
                                if (salaryReviewInfo != null)
                                {
                                    var secondDate = new DateTime();
                                    if (salaryReviewInfo.EffectiveTo == null)
                                    {
                                        secondDate = holdSalary.HoldTo.Value;
                                    }
                                    else if (holdSalary.HoldTo.Value.Date > salaryReviewInfo.EffectiveTo.Value.Date)
                                    {
                                        secondDate = salaryReviewInfo.EffectiveTo.Value.Date;
                                    }
                                    else if (holdSalary.HoldTo.Value.Date < salaryReviewInfo.EffectiveTo)
                                    {
                                        secondDate = holdSalary.HoldTo.Value.Date;
                                    }
                                    else
                                    {
                                        secondDate = holdSalary.HoldTo.Value.Date;
                                    }

                                    var daysWorked = await GetPresentCountBetweenSalaryDatesWhenSalaryWasHold(new PresentCountBetweenSalaryDates_Filter()
                                    {
                                        EmployeeId = employee.EmployeeId,
                                        IsActualDays = isActualDays,
                                        SalaryDate = holdSalary.HoldFrom,
                                        FirstDate = holdSalary.HoldFrom > salaryReviewInfo.EffectiveFrom.Value.Date ? holdSalary.HoldFrom : salaryReviewInfo.EffectiveFrom.Value.Date,
                                        SecondDate = secondDate,
                                        JoiningDate = employee.DateOfJoining,
                                        TerminationDate = employee.TerminationDate
                                    }, user);

                                    if (isActualDays == false)
                                    {
                                        if (daysWorked == actualDaysInHoldMonth)
                                        {
                                            daysWorked = 30;
                                        }
                                    }

                                    var salaryReviewDetails = await _salaryReviewBusiness.GetSalaryReviewDetailsAsync(salaryReviewInfo.SalaryReviewInfoId, user);
                                    foreach (var reviewItem in salaryReviewDetails)
                                    {
                                        SalaryAllowanceArrear arrear = new SalaryAllowanceArrear();
                                        arrear.EmployeeId = employee.EmployeeId;
                                        arrear.SalaryDate = DateTimeExtension.LastDateOfAMonth(execution.Year, execution.Month);
                                        arrear.ArrearMonth = (short)salaryStartDate.Month;
                                        arrear.ArrearYear = (short)salaryStartDate.Year;
                                        arrear.AllowanceNameId = reviewItem.AllowanceNameId;
                                        arrear.Remarks = $"SalaryHoldId : {holdSalary.SalaryHoldId.ToString()}";
                                        arrear.SalaryMonth = (short)salaryStartDate.Month;
                                        arrear.SalaryYear = (short)salaryStartDate.Year;
                                        arrear.Amount = reviewItem.CurrentAmount / daysInHold * daysWorked;
                                        arrear.AllowanceNameId = reviewItem.AllowanceNameId;
                                        arrear.FiscalYearId = fiscalYearInfo.FiscalYearId;
                                        arrear.SalaryDate = salaryEndDate;
                                        arrear.CalculationForDays = daysWorked;
                                        arrear.CreatedBy = user.ActionUserId;
                                        arrear.CreatedDate = DateTime.Now;
                                        arrear.BranchId = employee.BranchId;
                                        arrear.CompanyId = user.CompanyId;
                                        arrear.OrganizationId = user.OrganizationId;
                                        list_of_salary_allowance_arrear.Add(arrear);
                                        unholdDays = (short)(unholdDays + daysWorked);
                                    }


                                }
                            }
                        }

                        if (list_of_salary_allowance_adjustment.Count > 0)
                        {
                            foreach (var adjustment in employeeSalaryProcessed.SalaryAllowanceAdjustments)
                            {
                                list_of_salary_allowance_arrear.Add(new SalaryAllowanceArrear()
                                {
                                    AllowanceNameId = adjustment.AllowanceNameId,
                                    Amount = adjustment.Amount,
                                    ArrearMonth = adjustment.SalaryMonth,
                                    ArrearYear = adjustment.SalaryYear,
                                    SalaryReviewInfoId = 0,
                                    SalaryDate = DateTimeExtension.LastDateOfAMonth(execution.Year, execution.Month),
                                    SalaryMonth = adjustment.SalaryMonth,
                                    SalaryYear = adjustment.SalaryYear,
                                    CalculationForDays = adjustment.CalculationForDays,
                                    ArrearFrom = adjustment.SalaryDate,
                                    ArrearTo = adjustment.SalaryDate,
                                    EmployeeId = employee.EmployeeId,
                                    FiscalYearId = fiscalYearInfo.FiscalYearId,
                                    CreatedBy = user.ActionUserId,
                                    CreatedDate = DateTime.Now,
                                    CompanyId = user.CompanyId,
                                    OrganizationId = user.OrganizationId
                                });
                            }


                            list_of_salary_allowance_adjustment.Clear();


                        }
                        // End Of Hold Salary Calculation

                        // Calculated Gross & Basic On Salary Breakdown in this process

                        #region Last Salary Review in this process scope
                        var lastSalaryReviewInfoId = salaryInfos.Max(i => i.SalaryReviewInfoId);
                        var lastSalaryReviewInfo = salaryInfos.FirstOrDefault(i => i.SalaryReviewInfoId == lastSalaryReviewInfoId);

                        List<SalaryReviewDetail> listSalaryReviewDetail = new List<SalaryReviewDetail>();
                        decimal currentGross = 0;
                        decimal currentBasic = 0;
                        decimal currentHR = 0;
                        decimal currentConveyance = 0;
                        decimal currentMedical = 0;

                        if (lastSalaryReviewInfo != null)
                        {
                            listSalaryReviewDetail = (await _salaryReviewBusiness.GetSalaryReviewDetailsAsync(lastSalaryReviewInfoId, user)).AsList();
                            currentGross = listSalaryReviewDetail.Sum(i => i.CurrentAmount);
                            currentBasic = listSalaryReviewDetail.Where(i => i.AllowanceNameId == basicAllowanceInfo.AllowanceNameId).Sum(i => i.CurrentAmount);
                            currentHR = listSalaryReviewDetail.Where(i => i.AllowanceNameId == houseAllowanceInfo.AllowanceNameId).Sum(i => i.CurrentAmount);
                            currentConveyance = listSalaryReviewDetail.Where(i => i.AllowanceNameId == conveyanceAllowanceInfo.AllowanceNameId).Sum(i => i.CurrentAmount);
                            currentMedical = listSalaryReviewDetail.Where(i => i.AllowanceNameId == medicalAllowanceInfo.AllowanceNameId).Sum(i => i.CurrentAmount);
                        }
                        #endregion

                        #region allowance earned amount from the salary breakdown
                        var gross_earned = Math.Round(list_of_salary_allowance.Sum(i => i.Amount), 0);
                        var gross_earned_arrear = Math.Round(list_of_salary_allowance_arrear.Sum(i => i.Amount), 0);

                        var basic_earned = Math.Round(list_of_salary_allowance.Where(i => i.AllowanceNameId == basicAllowanceInfo.AllowanceNameId).Sum(i => i.Amount), 0);
                        var basic_earned_arrear = Math.Round(list_of_salary_allowance_arrear.Where(i => i.AllowanceNameId == basicAllowanceInfo.AllowanceNameId).Sum(i => i.Amount), 0);

                        var house_earned = Math.Round(list_of_salary_allowance.Where(i => i.AllowanceNameId == houseAllowanceInfo.AllowanceNameId).Sum(i => i.Amount), 0);
                        var house_earned_arrear = Math.Round(list_of_salary_allowance_arrear.Where(i => i.AllowanceNameId == houseAllowanceInfo.AllowanceNameId).Sum(i => i.Amount), 0);


                        var conveyance_earned = Math.Round(list_of_salary_allowance.Where(i => i.AllowanceNameId == conveyanceAllowanceInfo.AllowanceNameId).Sum(i => i.Amount), 0);
                        var conveyance_earned_arrear = Math.Round(list_of_salary_allowance_arrear.Where(i => i.AllowanceNameId == conveyanceAllowanceInfo.AllowanceNameId).Sum(i => i.Amount), 0);

                        var medical_earned = Math.Round(list_of_salary_allowance.Where(i => i.AllowanceNameId == medicalAllowanceInfo.AllowanceNameId).Sum(i => i.Amount), 0);
                        var medical_earned_arrear = Math.Round(list_of_salary_allowance_arrear.Where(i => i.AllowanceNameId == medicalAllowanceInfo.AllowanceNameId).Sum(i => i.Amount), 0);

                        #endregion

                        #region Find Deposite Amount
                        var _conditionalDeposits = await _conditionalDepositAllowanceConfigBusiness.GetEmployeeConditionalDepositAllowanceConfigsAsync(new EligibilityInConditionalDeposit_Filter()
                        {
                            JobType = employee.JobType,
                            Gender = employee.Gender,
                            MaritalStatus = employee.MaritalStatus,
                            PhysicalCondition = employee.PhysicalCondition,
                            Religion = employee.Religion,
                            SalaryDate = salaryEndDate.ToString("yyyy-MM-dd")
                        }, user);

                        foreach (var item in _conditionalDeposits)
                        {
                            DepositAllowanceHistory depositAllowanceHistory = new DepositAllowanceHistory();
                            depositAllowanceHistory.EmployeeId = employee.EmployeeId;
                            depositAllowanceHistory.AllowanceNameId = item.AllowanceNameId;
                            depositAllowanceHistory.FiscalYearId = fiscalYearInfo.FiscalYearId;
                            depositAllowanceHistory.DepositMonth = salaryEndDate.Month;
                            depositAllowanceHistory.DepositYear = salaryEndDate.Year;
                            depositAllowanceHistory.PayableDays = (short)totalDayWorked;
                            depositAllowanceHistory.ConditionalDepositAllowanceConfigId = item.Id;
                            depositAllowanceHistory.CreatedBy = user.ActionUserId;
                            depositAllowanceHistory.CreatedDate = DateTime.Now;
                            depositAllowanceHistory.CompanyId = user.CompanyId;
                            depositAllowanceHistory.OrganizationId = user.OrganizationId;
                            depositAllowanceHistory.BranchId = employee.BranchId;
                            depositAllowanceHistory.IncomingFlag = "Conditional";
                            depositAllowanceHistory.ConditionalDepositAllowanceConfigId = item.Id;
                            depositAllowanceHistory.DepositDate = salaryEndDate.Date;

                            if (item.DepositType == "Monthly")
                            {
                                if (item.BaseOfPayment == "Flat")
                                {
                                    depositAllowanceHistory.BaseAmount = item.Amount ?? 0;
                                    depositAllowanceHistory.Amount = item.Amount ?? 0;
                                }
                                else if (item.BaseOfPayment == "Basic")
                                {
                                    if ((item.Percentage ?? 0) > 0)
                                    {
                                        depositAllowanceHistory.BaseAmount = currentBasic / 100 * item.Percentage ?? 0;
                                        depositAllowanceHistory.Amount = basic_earned / 100 * item.Percentage ?? 0;
                                        depositAllowanceHistory.Arrear = basic_earned_arrear / 100 * item.Percentage ?? 0;
                                    }
                                }
                                else if (item.BaseOfPayment == "Gross")
                                {
                                    if ((item.Percentage ?? 0) > 0)
                                    {
                                        depositAllowanceHistory.BaseAmount = currentGross / 100 * item.Percentage ?? 0;
                                        depositAllowanceHistory.Amount = gross_earned / 100 * item.Percentage ?? 0;
                                        depositAllowanceHistory.Arrear = gross_earned / 100 * item.Percentage ?? 0;
                                    }
                                }
                                else if (item.BaseOfPayment == "Gross Without Conveyance")
                                {
                                    if ((item.Percentage ?? 0) > 0)
                                    {
                                        var gross_without_conveyance = gross_earned - conveyance_earned;
                                        var gross_arrear_without_conveyance = gross_earned_arrear - conveyance_earned_arrear;

                                        var currentGross_without_conveyance = currentGross - currentConveyance;

                                        depositAllowanceHistory.BaseAmount = currentGross_without_conveyance / 100 * item.Percentage ?? 0;
                                        depositAllowanceHistory.Amount = gross_without_conveyance / 100 * item.Percentage ?? 0;
                                        depositAllowanceHistory.Arrear = gross_arrear_without_conveyance / 100 * item.Percentage ?? 0;
                                    }
                                }
                            }
                            list_of_deposite_Allowance_History.Add(depositAllowanceHistory);
                        }
                        #endregion

                        #region Monthly Allowance


                        #region Wounderman Thompson Contractual Employee Medical
                        if (user.CompanyId == 19)
                        {
                            if (employee.JobType == "Contractual")
                            {
                                var conveyanceAmount = list_of_salary_allowance.Where(i => i.AllowanceNameId == conveyanceAllowanceInfo.AllowanceNameId).Sum(i => i.Amount);
                                var medicalAmount = (gross_earned + gross_earned_arrear + conveyanceAmount) / 100 * 4;
                                list_of_salary_allowance.Add(new SalaryAllowance()
                                {
                                    EmployeeId = employee.EmployeeId,
                                    AllowanceNameId = medicalAllowanceInfo.AllowanceNameId,
                                    CalculationForDays = 0,
                                    Amount = medicalAmount,
                                    AdjustmentAmount = 0,
                                    ArrearAmount = 0,
                                    SalaryDate = salaryEndDate,
                                    SalaryMonth = (short)salaryEndDate.Month,
                                    SalaryYear = (short)salaryEndDate.Year,
                                    MonthlyAllowanceId = 0,
                                    FiscalYearId = fiscalYearInfo.FiscalYearId,
                                    CreatedBy = user.ActionUserId,
                                    CreatedDate = DateTime.Now,
                                    CompanyId = user.CompanyId,
                                    OrganizationId = user.OrganizationId,
                                });
                            }
                        }
                        #endregion
                        #endregion

                        conveyance_earned = Math.Round(list_of_salary_allowance.Where(i => i.AllowanceNameId == conveyanceAllowanceInfo.AllowanceNameId).Sum(i => i.Amount), 0);
                        conveyance_earned_arrear = Math.Round(list_of_salary_allowance_arrear.Where(i => i.AllowanceNameId == conveyanceAllowanceInfo.AllowanceNameId).Sum(i => i.Amount), 0);

                        #region Proposal of Payment Deposit Amount
                        employeeSalaryProcessed.DepositAllowancePaymentHistories = (await _depositAllowancePaymentHistoryBusiness.ThisMonthEmployeeDepositAllowanceCalculationInSalaryAsync(employee, list_of_deposite_Allowance_History, salaryEndDate.Month, salaryEndDate.Year, fiscalYearInfo.FiscalYearId, user)).AsList();
                        #endregion

                        #region Service anniversary allowance
                        if (employee.DateOfJoining != null)
                        {
                            var items = await _serviceAnniversaryAllowanceBusiness.GetEmployeeServiceAnniversaryAllowancesAsync(new EligibilityInServiceAnniversary_Filter()
                            {
                                Gender = employee.Gender,
                                JobType = employee.JobType,
                                MaritalStatus = employee.MaritalStatus,
                                PhysicalCondition = employee.PhysicalCondition,
                                Religion = employee.Religion,
                                PaymentDate = salaryEndDate.ToString("yyyy-MM-dd")
                            }, salaryEndDate.Month, salaryEndDate.Year, employee.DateOfJoining.Value.Date, user);
                            if (items != null && items.AsList().Count > 0)
                            {
                                foreach (var item in items)
                                {
                                    decimal amountWillAdd = 0;
                                    if (user.CompanyId == 19 && user.OrganizationId == 11 && employee.JobType == Jobtype.Contractual && item.BaseOfPayment == "Gross With Conveyance")
                                    {
                                        amountWillAdd = conveyance_earned + conveyance_earned_arrear;
                                        var serviceAnniversaryBaseAmount = item.BaseOfPayment == "Basic" ? basic_earned + basic_earned_arrear : gross_earned + gross_earned_arrear;
                                        if (item.Percentage != null && item.Percentage > 0)
                                        {
                                            if (serviceAnniversaryBaseAmount > 0)
                                            {
                                                var serviceAnniversaryGainedAmount = serviceAnniversaryBaseAmount / 100 * item.Percentage.Value;
                                                list_of_salary_allowance.Add(new SalaryAllowance()
                                                {
                                                    EmployeeId = employee.EmployeeId,
                                                    AllowanceNameId = item.AllowanceNameId,
                                                    CalculationForDays = 0,
                                                    Amount = serviceAnniversaryGainedAmount + amountWillAdd,
                                                    AdjustmentAmount = 0,
                                                    ArrearAmount = 0,
                                                    SalaryDate = salaryEndDate,
                                                    SalaryMonth = (short)salaryEndDate.Month,
                                                    SalaryYear = (short)salaryEndDate.Year,
                                                    MonthlyAllowanceId = 0,
                                                    FiscalYearId = fiscalYearInfo.FiscalYearId,
                                                    CreatedBy = user.ActionUserId,
                                                    CreatedDate = DateTime.Now,
                                                    CompanyId = user.CompanyId,
                                                    OrganizationId = user.OrganizationId,
                                                });
                                                employeeSalaryProcessed.RecipientsofServiceAnniversaryAllowances.Add(new RecipientsofServiceAnniversaryAllowance()
                                                {
                                                    AllowanceNameId = item.AllowanceNameId,
                                                    EmployeeId = employee.EmployeeId,
                                                    ServiceAnniversaryAllowanceId = item.Id,
                                                    PayableAmount = serviceAnniversaryGainedAmount + amountWillAdd,
                                                    DisbursedAmount = serviceAnniversaryGainedAmount + amountWillAdd,
                                                    FiscalYearId = fiscalYearInfo.FiscalYearId,
                                                    PaymentMonth = salaryEndDate.Month,
                                                    PaymentYear = salaryEndDate.Year,
                                                    CreatedBy = user.ActionUserId,
                                                    CreatedDate = DateTime.Now,
                                                    CompanyId = user.CompanyId,
                                                    IsDisbursed = false,
                                                    IsVisibleInPayslip = item.IsVisibleInPayslip,
                                                    IsVisibleInSalarySheet = item.IsVisibleInSalarySheet,
                                                    OrganizationId = user.OrganizationId,
                                                    BranchId = employee.BranchId,
                                                    PaymentDate = salaryEndDate
                                                });
                                            }
                                        }
                                    }
                                    else if (item.BaseOfPayment == "Basic" || item.BaseOfPayment == "Gross")
                                    {
                                        var serviceAnniversaryBaseAmount = item.BaseOfPayment == "Basic" ? basic_earned + basic_earned_arrear : gross_earned + gross_earned_arrear;
                                        if (item.Percentage != null && item.Percentage > 0)
                                        {
                                            if (serviceAnniversaryBaseAmount > 0)
                                            {
                                                var serviceAnniversaryGainedAmount = serviceAnniversaryBaseAmount / 100 * item.Percentage.Value;
                                                list_of_salary_allowance.Add(new SalaryAllowance()
                                                {
                                                    EmployeeId = employee.EmployeeId,
                                                    AllowanceNameId = item.AllowanceNameId,
                                                    CalculationForDays = 0,
                                                    Amount = serviceAnniversaryGainedAmount,
                                                    AdjustmentAmount = 0,
                                                    ArrearAmount = 0,
                                                    SalaryDate = salaryEndDate,
                                                    SalaryMonth = (short)salaryEndDate.Month,
                                                    SalaryYear = (short)salaryEndDate.Year,
                                                    MonthlyAllowanceId = 0,
                                                    FiscalYearId = fiscalYearInfo.FiscalYearId,
                                                    CreatedBy = user.ActionUserId,
                                                    CreatedDate = DateTime.Now,
                                                    CompanyId = user.CompanyId,
                                                    OrganizationId = user.OrganizationId,
                                                });
                                                employeeSalaryProcessed.RecipientsofServiceAnniversaryAllowances.Add(new RecipientsofServiceAnniversaryAllowance()
                                                {
                                                    AllowanceNameId = item.AllowanceNameId,
                                                    EmployeeId = employee.EmployeeId,
                                                    ServiceAnniversaryAllowanceId = item.Id,
                                                    PayableAmount = serviceAnniversaryGainedAmount,
                                                    DisbursedAmount = serviceAnniversaryGainedAmount,
                                                    FiscalYearId = fiscalYearInfo.FiscalYearId,
                                                    PaymentMonth = salaryEndDate.Month,
                                                    PaymentYear = salaryEndDate.Year,
                                                    CreatedBy = user.ActionUserId,
                                                    CreatedDate = DateTime.Now,
                                                    CompanyId = user.CompanyId,
                                                    IsDisbursed = false,
                                                    IsVisibleInPayslip = item.IsVisibleInPayslip,
                                                    IsVisibleInSalarySheet = item.IsVisibleInSalarySheet,
                                                    OrganizationId = user.OrganizationId,
                                                    BranchId = employee.BranchId,
                                                    PaymentDate = salaryEndDate
                                                });
                                            }
                                        }
                                    }
                                    else if (item.BaseOfPayment == "Flat")
                                    {
                                        if (item.Amount != null && item.Amount > 0)
                                        {
                                            list_of_salary_allowance.Add(new SalaryAllowance()
                                            {
                                                EmployeeId = employee.EmployeeId,
                                                AllowanceNameId = item.AllowanceNameId,
                                                CalculationForDays = 0,
                                                Amount = item.Amount.Value,
                                                AdjustmentAmount = 0,
                                                ArrearAmount = 0,
                                                SalaryDate = salaryEndDate,
                                                SalaryMonth = (short)salaryEndDate.Month,
                                                SalaryYear = (short)salaryEndDate.Year,
                                                MonthlyAllowanceId = 0,
                                                FiscalYearId = fiscalYearInfo.FiscalYearId,
                                                CreatedBy = user.ActionUserId,
                                                CreatedDate = DateTime.Now,
                                                CompanyId = user.CompanyId,
                                                OrganizationId = user.OrganizationId,
                                            });
                                            employeeSalaryProcessed.RecipientsofServiceAnniversaryAllowances.Add(new RecipientsofServiceAnniversaryAllowance()
                                            {
                                                AllowanceNameId = item.AllowanceNameId,
                                                EmployeeId = employee.EmployeeId,
                                                ServiceAnniversaryAllowanceId = item.Id,
                                                PayableAmount = item.Amount.Value,
                                                DisbursedAmount = item.Amount.Value,
                                                FiscalYearId = fiscalYearInfo.FiscalYearId,
                                                PaymentMonth = salaryEndDate.Month,
                                                PaymentYear = salaryEndDate.Year,
                                                CreatedBy = user.ActionUserId,
                                                CreatedDate = DateTime.Now,
                                                CompanyId = user.CompanyId,
                                                IsDisbursed = false,
                                                IsVisibleInPayslip = item.IsVisibleInPayslip,
                                                IsVisibleInSalarySheet = item.IsVisibleInSalarySheet,
                                                OrganizationId = user.OrganizationId,
                                                BranchId = employee.BranchId,
                                                PaymentDate = salaryEndDate
                                            });
                                        }
                                    }


                                }
                            }
                        }
                        #endregion

                        #region Monthly Variable Allowance
                        var variableAllowances = await _monthlyVariableAllowanceBusiness.GetMonthlyVariableAllowancesAsync(0, employee.EmployeeId, 0, (short)salaryStartDate.Month, (short)salaryStartDate.Year, "Approved", user);
                        foreach (var item in variableAllowances)
                        {
                            list_of_salary_allowance.Add(new SalaryAllowance()
                            {
                                EmployeeId = employee.EmployeeId,
                                AllowanceNameId = item.AllowanceNameId,
                                CalculationForDays = totalDayWorked,
                                Amount = item.Amount,
                                AdjustmentAmount = 0,
                                ArrearAmount = 0,
                                SalaryDate = salaryEndDate,
                                SalaryMonth = (short)salaryEndDate.Month,
                                SalaryYear = (short)salaryEndDate.Year,
                                MonthlyAllowanceId = item.MonthlyVariableAllowanceId,
                                FiscalYearId = fiscalYearInfo.FiscalYearId,
                                CreatedBy = user.ActionUserId,
                                CreatedDate = DateTime.Now,
                                CompanyId = user.CompanyId,
                                OrganizationId = user.OrganizationId,
                            });

                            list_of_salary_component_History.Add(new SalaryComponentHistory()
                            {
                                EmployeeId = employee.EmployeeId,
                                SalaryMonth = (short)salaryStartDate.Date.Month,
                                SalaryYear = (short)salaryStartDate.Date.Year,
                                Flag = "Variable Allowance",
                                ComponentId = item.MonthlyVariableAllowanceId.ToString(),
                                Amount = item.Amount.ToString(),
                                CreatedBy = user.ActionUserId,
                                CompanyId = user.CompanyId,
                                OrganizationId = user.OrganizationId,
                            });
                        }
                        #endregion

                        #region Monthly Variable Deduction
                        var variableDeductions = await _monthlyVariableDeductionBusiness.GetMonthlyVariableDeductionsAsync(0, employee.EmployeeId, 0, (short)salaryStartDate.Month, (short)salaryStartDate.Year, "Approved", user);
                        foreach (var item in variableDeductions)
                        {
                            list_of_salary_deduction.Add(new SalaryDeduction()
                            {
                                EmployeeId = item.EmployeeId,
                                DeductionNameId = item.DeductionNameId,
                                CalculationForDays = totalDayWorked,
                                Amount = item.Amount,
                                ArrearAmount = 0,
                                SalaryDate = salaryEndDate,
                                SalaryMonth = (short)salaryEndDate.Month,
                                SalaryYear = (short)salaryEndDate.Year,
                                MonthlyDeductionId = item.MonthlyVariableDeductionId,
                                FiscalYearId = fiscalYearInfo.FiscalYearId,
                                CreatedBy = user.ActionUserId,
                                CreatedDate = DateTime.Now,
                                CompanyId = user.CompanyId,
                                OrganizationId = user.OrganizationId
                            });

                            list_of_salary_component_History.Add(new SalaryComponentHistory()
                            {
                                EmployeeId = employee.EmployeeId,
                                SalaryMonth = (short)salaryStartDate.Date.Month,
                                SalaryYear = (short)salaryStartDate.Date.Year,
                                Flag = "Variable Deduction",
                                ComponentId = item.MonthlyVariableDeductionId.ToString(),
                                Amount = item.Amount.ToString(),
                                CreatedBy = user.ActionUserId,
                                CompanyId = user.CompanyId,
                                OrganizationId = user.OrganizationId,
                            });
                        }
                        #endregion

                        employeeSalaryProcessed.SalaryComponentHistories = list_of_salary_component_History;

                        #region Marge Salary Allowance
                        List<long> allowances = new List<long>();
                        var unique_salary_allowance = list_of_salary_allowance.Select(i => i.AllowanceNameId).Distinct().ToList();
                        var unique_salary_allowance_arrea = list_of_salary_allowance_arrear.Select(i => i.AllowanceNameId).Distinct().ToList();
                        var unique_salary_allowance_adjustment = list_of_salary_allowance_adjustment.Select(i => i.AllowanceNameId).Distinct().ToList();
                        var unique_deposit_allowance_payment_proposal = employeeSalaryProcessed.DepositAllowancePaymentHistories.Where(i => (i.AllowanceNameId ?? 0) > 0).Select(i => i.AllowanceNameId ?? 0).Distinct().ToList();

                        foreach (var i in unique_salary_allowance)
                        {
                            allowances.Add(i);
                        }
                        foreach (var i in unique_salary_allowance_arrea)
                        {
                            allowances.Add(i);
                        }
                        foreach (var i in unique_salary_allowance_adjustment)
                        {
                            allowances.Add(i);
                        }
                        foreach (var i in unique_deposit_allowance_payment_proposal)
                        {
                            allowances.Add(i);
                        }
                        allowances = allowances.Distinct().ToList();

                        employeeSalaryProcessed.SalaryAllowanceArrears = list_of_salary_allowance_arrear;
                        employeeSalaryProcessed.SalaryAllowanceAdjustments = list_of_salary_allowance_adjustment;
                        employeeSalaryProcessed.DepositAllowanceHistories = list_of_deposite_Allowance_History;

                        foreach (var adjustment in list_of_salary_allowance_adjustment)
                        {
                            var salaryAllowanceArrear = new SalaryAllowanceArrear()
                            {
                                AllowanceNameId = adjustment.AllowanceNameId,
                                Amount = adjustment.Amount,
                                ArrearMonth = adjustment.SalaryMonth,
                                ArrearYear = adjustment.SalaryYear,
                                SalaryReviewInfoId = 0,
                                SalaryDate = DateTimeExtension.LastDateOfAMonth(execution.Year, execution.Month),
                                SalaryMonth = adjustment.SalaryMonth,
                                SalaryYear = adjustment.SalaryYear,
                                CalculationForDays = adjustment.CalculationForDays,
                                FiscalYearId = fiscalYearInfo.FiscalYearId,
                                ArrearFrom = adjustment.SalaryDate,
                                ArrearTo = adjustment.SalaryDate,
                                EmployeeId = employee.EmployeeId,
                                CreatedBy = user.ActionUserId,
                                CreatedDate = DateTime.Now,
                                CompanyId = user.CompanyId,
                                OrganizationId = user.OrganizationId
                            };
                            employeeSalaryProcessed.SalaryAllowanceArrears.Add(salaryAllowanceArrear);
                        }

                        foreach (var id in allowances)
                        {
                            var amount = list_of_salary_allowance.Where(i => i.AllowanceNameId == id).Sum(i => i.Amount);
                            var arrearAmount = list_of_salary_allowance_arrear.Where(i => i.AllowanceNameId == id).Sum(i => i.Amount);
                            var monthlyAllowanceIds = string.Join(',', list_of_salary_allowance.Where(i => i.MonthlyAllowanceId != null && i.MonthlyAllowanceId.Value > 0).Select(i => i.MonthlyAllowanceId).ToList());
                            var periodicallyAllowanceIds = string.Join(',', list_of_salary_allowance.Where(i => i.PeriodicallyAllowanceId != null && i.PeriodicallyAllowanceId.Value > 0).Select(i => i.PeriodicallyAllowanceId).ToList());

                            amount = amount + employeeSalaryProcessed.DepositAllowancePaymentHistories.Where(i => i.AllowanceNameId == id).Sum(i => i.DisbursedAmount);

                            SalaryAllowance salaryAllowance = new SalaryAllowance()
                            {
                                EmployeeId = employee.EmployeeId,
                                AllowanceNameId = id,
                                CalculationForDays = totalDayWorked,
                                Amount = amount,
                                ArrearAmount = arrearAmount,
                                AdjustmentAmount = 0,
                                SalaryDate = salaryEndDate,
                                SalaryMonth = (short)salaryEndDate.Month,
                                SalaryYear = (short)salaryEndDate.Year,
                                MonthlyAllowanceIds = monthlyAllowanceIds,
                                PeriodicallyAllowanceIds = periodicallyAllowanceIds,
                                FiscalYearId = fiscalYearInfo.FiscalYearId,
                                CreatedBy = user.ActionUserId,
                                CreatedDate = DateTime.Now,
                                CompanyId = user.CompanyId,
                                OrganizationId = user.OrganizationId,
                            };
                            employeeSalaryProcessed.SalaryAllowances.Add(salaryAllowance);
                        }
                        #endregion

                        #region Marge Salary Deduction
                        List<long> deductions = new List<long>();
                        var unique_salary_deduction = list_of_salary_deduction.Select(i => i.DeductionNameId).Distinct().ToList();
                        var unique_salary_deduction_adjustment = list_of_salary_deduction_adjustment.Select(i => i.DeductionNameId).Distinct().ToList();
                        foreach (var i in unique_salary_deduction)
                        {
                            deductions.Add(i);
                        }
                        foreach (var i in unique_salary_deduction_adjustment)
                        {
                            deductions.Add(i);
                        }

                        deductions = deductions.Distinct().ToList();

                        foreach (var id in deductions)
                        {
                            var amount = list_of_salary_deduction.Where(i => i.DeductionNameId == id).Sum(i => i.Amount);
                            var arrearAmount = list_of_salary_deduction_adjustment.Where(i => i.DeductionNameId == id).Sum(i => i.Amount);
                            var monthlyDeductionIds = string.Join(',', list_of_salary_deduction.Where(i => i.MonthlyDeductionId != null && i.MonthlyDeductionId.Value > 0).Select(i => i.MonthlyDeductionId).ToList());
                            var periodicallyDeductionIds = string.Join(',', list_of_salary_deduction.Where(i => i.PeriodicallyDeductionId != null && i.PeriodicallyDeductionId.Value > 0).Select(i => i.PeriodicallyDeductionId).ToList());

                            SalaryDeduction salaryDeduction = new SalaryDeduction()
                            {
                                EmployeeId = employee.EmployeeId,
                                DeductionNameId = id,
                                CalculationForDays = totalDayWorked,
                                Amount = amount,
                                ArrearAmount = arrearAmount,
                                SalaryDate = salaryEndDate,
                                SalaryMonth = (short)salaryEndDate.Month,
                                SalaryYear = (short)salaryEndDate.Year,
                                //MonthlyAllowanceIds = monthlyDeductionIds,
                                //PeriodicallyAllowanceIds = periodicallyDeductionIds,
                                FiscalYearId = fiscalYearInfo.FiscalYearId,
                                CreatedBy = user.ActionUserId,
                                CreatedDate = DateTime.Now,
                                CompanyId = user.CompanyId,
                                OrganizationId = user.OrganizationId,
                            };
                            employeeSalaryProcessed.SalaryDeductions.Add(salaryDeduction);
                        }

                        var lastSalaryReviewIdWithThisProcess = salaryInfos.OrderByDescending(i => i.ActivationDate).FirstOrDefault();
                        if (lastSalaryReviewIdWithThisProcess != null)
                        {
                            var lastSalaryReviewDetails = await _salaryReviewBusiness.GetSalaryReviewDetailsAsync(lastSalaryReviewIdWithThisProcess.SalaryReviewInfoId, user);
                            if (lastSalaryReviewDetails != null)
                            {
                                employeeSalaryProcessed.SalaryProcessDetail.CurrentBasic = lastSalaryReviewDetails.Sum(item => item.AllowanceNameId = basicAllowanceInfo.AllowanceNameId);
                                employeeSalaryProcessed.SalaryProcessDetail.CurrentHouseRent = lastSalaryReviewDetails.Sum(item => item.AllowanceNameId = houseAllowanceInfo.AllowanceNameId);
                                employeeSalaryProcessed.SalaryProcessDetail.CurrentMedical = lastSalaryReviewDetails.Sum(item => item.AllowanceNameId = medicalAllowanceInfo.AllowanceNameId);
                                employeeSalaryProcessed.SalaryProcessDetail.CurrentConveyance = lastSalaryReviewDetails.Sum(item => item.AllowanceNameId = conveyanceAllowanceInfo.AllowanceNameId);
                            }
                        }
                        #endregion

                        #region Salary Process Detail
                        employeeSalaryProcessed.SalaryProcessDetail.EmployeeId = employee.EmployeeId;
                        employeeSalaryProcessed.SalaryProcessDetail.GradeId = salaryProcessDetail.GradeId;
                        employeeSalaryProcessed.SalaryProcessDetail.Designation = salaryProcessDetail.Designation;
                        employeeSalaryProcessed.SalaryProcessDetail.DesignationId = salaryProcessDetail.DesignationId;
                        employeeSalaryProcessed.SalaryProcessDetail.Department = salaryProcessDetail.Department;
                        employeeSalaryProcessed.SalaryProcessDetail.DepartmentId = salaryProcessDetail.DepartmentId;
                        employeeSalaryProcessed.SalaryProcessDetail.Section = salaryProcessDetail.Section;
                        employeeSalaryProcessed.SalaryProcessDetail.SectionId = salaryProcessDetail.SectionId;
                        employeeSalaryProcessed.SalaryProcessDetail.SubSection = salaryProcessDetail.SubSection;
                        employeeSalaryProcessed.SalaryProcessDetail.SubsectionId = salaryProcessDetail.SubsectionId;
                        employeeSalaryProcessed.SalaryProcessDetail.CostCenter = salaryProcessDetail.CostCenter;
                        employeeSalaryProcessed.SalaryProcessDetail.CostCenterCode = salaryProcessDetail.CostCenterCode;
                        employeeSalaryProcessed.SalaryProcessDetail.CostCenterId = salaryProcessDetail.CostCenterId;
                        employeeSalaryProcessed.SalaryProcessDetail.JobType = salaryProcessDetail.JobType;
                        employeeSalaryProcessed.SalaryProcessDetail.JobCategory = salaryProcessDetail.JobCategory;
                        employeeSalaryProcessed.SalaryProcessDetail.InternalDesignationId = salaryProcessDetail.InternalDesignationId;
                        employeeSalaryProcessed.SalaryProcessDetail.InternalDesignation = salaryProcessDetail.InternalDesignation;
                        employeeSalaryProcessed.SalaryProcessDetail.CalculationForDays = totalDayWorked;
                        employeeSalaryProcessed.SalaryProcessDetail.HoldDays = (short)holdDays;

                        employeeSalaryProcessed.SalaryProcessDetail.UnholdDays = 0;
                        employeeSalaryProcessed.SalaryProcessDetail.UnholdAmount = 0;
                        employeeSalaryProcessed.SalaryProcessDetail.BranchId = employee.BranchId;
                        employeeSalaryProcessed.SalaryProcessDetail.SalaryDate = DateTimeExtension.LastDateOfAMonth(execution.Year, execution.Month);
                        employeeSalaryProcessed.SalaryProcessDetail.FiscalYearId = fiscalYearInfo.FiscalYearId;

                        employeeSalaryProcessed.SalaryProcessDetail.SalaryMonth = (short)salaryEndDate.Month;
                        employeeSalaryProcessed.SalaryProcessDetail.SalaryYear = (short)salaryEndDate.Year;

                        employeeSalaryProcessed.SalaryProcessDetail.CurrentBasic = currentBasic;
                        employeeSalaryProcessed.SalaryProcessDetail.CurrentHouseRent = currentHR;
                        employeeSalaryProcessed.SalaryProcessDetail.CurrentMedical = currentMedical;
                        employeeSalaryProcessed.SalaryProcessDetail.CurrentConveyance = currentConveyance;



                        employeeSalaryProcessed.SalaryProcessDetail.ThisMonthBasic = employeeSalaryProcessed.SalaryAllowances.Where(i => i.AllowanceNameId == basicAllowanceInfo.AllowanceNameId).Sum(item => item.Amount);

                        employeeSalaryProcessed.SalaryProcessDetail.ThisMonthHouseRent = employeeSalaryProcessed.SalaryAllowances.Where(i => i.AllowanceNameId == houseAllowanceInfo.AllowanceNameId).Sum(item => item.Amount);

                        employeeSalaryProcessed.SalaryProcessDetail.ThisMonthMedical = employeeSalaryProcessed.SalaryAllowances.Where(i => i.AllowanceNameId == medicalAllowanceInfo.AllowanceNameId).Sum(item => item.Amount);

                        employeeSalaryProcessed.SalaryProcessDetail.ThisMonthConveyance = employeeSalaryProcessed.SalaryAllowances.Where(i => i.AllowanceNameId == conveyanceAllowanceInfo.AllowanceNameId).Sum(item => item.Amount);

                        employeeSalaryProcessed.SalaryProcessDetail.TotalAllowance = employeeSalaryProcessed.SalaryAllowances.Sum(item => item.Amount);
                        employeeSalaryProcessed.SalaryProcessDetail.UnholdDays = unholdDays;
                        employeeSalaryProcessed.SalaryProcessDetail.TotalArrearAllowance = employeeSalaryProcessed.SalaryAllowanceArrears.Sum(item => item.Amount);
                        employeeSalaryProcessed.SalaryProcessDetail.PFAmount = pfAmount;
                        employeeSalaryProcessed.SalaryProcessDetail.PFArrear = pfArrear;
                        employeeSalaryProcessed.SalaryProcessDetail.TotalPFAmount = pfAmount + pfArrear;
                        employeeSalaryProcessed.SalaryProcessDetail.ActualPFAmount = pfAmount;
                        employeeSalaryProcessed.SalaryProcessDetail.ActualPFArrear = pfArrear;
                        employeeSalaryProcessed.SalaryProcessDetail.TotalActualPFAmount = pfAmount + pfArrear;
                        employeeSalaryProcessed.SalaryProcessDetail.TotalDeduction = employeeSalaryProcessed.SalaryDeductions.Sum(item => item.Amount);
                        employeeSalaryProcessed.SalaryProcessDetail.GrossPay = employeeSalaryProcessed.SalaryProcessDetail.TotalAllowance + employeeSalaryProcessed.SalaryProcessDetail.TotalArrearAllowance + 0;
                        employeeSalaryProcessed.SalaryProcessDetail.TotalArrearDeduction = pfArrear;
                        employeeSalaryProcessed.SalaryProcessDetail.NetPay = employeeSalaryProcessed.SalaryProcessDetail.GrossPay - employeeSalaryProcessed.SalaryProcessDetail.TotalDeduction;

                        employeeSalaryProcessed.SalaryProcessDetail.HoldAmount = holdAmount;
                        employeeSalaryProcessed.SalaryProcessDetail.SalaryProcessId = 0;
                        employeeSalaryProcessed.SalaryProcessDetail.AccountId = 0;
                        employeeSalaryProcessed.SalaryProcessDetail.BankId = employee.BankId;
                        employeeSalaryProcessed.SalaryProcessDetail.BankTransferAmount = employeeSalaryProcessed.SalaryProcessDetail.NetPay;
                        employeeSalaryProcessed.SalaryProcessDetail.BankBranchId = employee.BankBranchId;
                        employeeSalaryProcessed.SalaryProcessDetail.BankAccountNumber = employee.BankAccount;
                        employeeSalaryProcessed.SalaryProcessDetail.WalletAgent = employee.WalletAgent;
                        employeeSalaryProcessed.SalaryProcessDetail.WalletNumber = employee.WalletNumber;
                        employeeSalaryProcessed.SalaryProcessDetail.WalletTransferAmont = 0;
                        employeeSalaryProcessed.SalaryProcessDetail.COCInWalletTransfer = 0;
                        employeeSalaryProcessed.SalaryProcessDetail.CashAmount = 0;
                        employeeSalaryProcessed.SalaryProcessDetail.CreatedBy = user.ActionUserId;
                        employeeSalaryProcessed.SalaryProcessDetail.CreatedDate = DateTime.Now.Date;
                        employeeSalaryProcessed.SalaryProcessDetail.CompanyId = user.CompanyId;
                        employeeSalaryProcessed.SalaryProcessDetail.OrganizationId = user.OrganizationId;

                        #endregion

                    }
                }
                catch (Exception ex)
                {
                    await _sysLogger.SaveHRMSException(ex, user.Database, "ExecuteSalaryProcess", "RunSystematically", user);
                }
            }
            else
            {
                executionStatus.Status = true;
                executionStatus.Msg = "No eligible found to process";
            }
            return employeeSalaryProcessed;
        }
        public async Task<ExecutionStatus> SaveSalaryAsync(string processType, int salaryMonth, int salaryYear, bool IsMargeProcess, List<EmployeeSalaryProcessedInfo> salaryItems, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            string ids = "";
            try
            {
                var lastSalaryProcessInfo = IsMargeProcess == true ? await _salaryProcessBusiness.GetPendingSalaryProcessInfoAsync(processType, salaryMonth, salaryYear, user) : null;
                if (lastSalaryProcessInfo != null && lastSalaryProcessInfo.SalaryProcessId > 0 && salaryItems != null)
                {
                    foreach (var item in salaryItems)
                    {
                        lastSalaryProcessInfo.TotalAllowance = lastSalaryProcessInfo.TotalAllowance + item.SalaryAllowances.Sum(i => i.Amount);
                        lastSalaryProcessInfo.TotalArrearAllowance = lastSalaryProcessInfo.TotalArrearAllowance + item.SalaryAllowanceArrears.Sum(i => i.Amount);
                        lastSalaryProcessInfo.TotalAllowanceAdjustment = lastSalaryProcessInfo.TotalAllowanceAdjustment + item.SalaryAllowanceAdjustments.Sum(i => i.Amount);
                        lastSalaryProcessInfo.TotalDeduction = lastSalaryProcessInfo.TotalDeduction + item.SalaryProcessDetail.TotalDeduction;
                        lastSalaryProcessInfo.TotalPFAmount = lastSalaryProcessInfo.TotalPFAmount + item.SalaryProcessDetail.PFAmount;
                        lastSalaryProcessInfo.TotalPFArrear = lastSalaryProcessInfo.TotalPFArrear + item.SalaryProcessDetail.PFArrear;
                        lastSalaryProcessInfo.TotalHoldAmount = lastSalaryProcessInfo.TotalHoldAmount + item.SalaryProcessDetail.HoldAmount;
                        lastSalaryProcessInfo.TotalHoldDays = lastSalaryProcessInfo.TotalHoldDays ?? 0 + item.SalaryProcessDetail.HoldDays ?? 0;
                        lastSalaryProcessInfo.TotalUnholdAmount = lastSalaryProcessInfo.TotalUnholdAmount + item.SalaryProcessDetail.UnholdAmount;
                        lastSalaryProcessInfo.TotalUnholdDays = lastSalaryProcessInfo.TotalUnholdDays ?? 0 + item.SalaryProcessDetail.UnholdDays ?? 0;
                        lastSalaryProcessInfo.TotalEmployees = lastSalaryProcessInfo.TotalEmployees + 1;
                        lastSalaryProcessInfo.TotalMonthlyTax = 0;
                        lastSalaryProcessInfo.TotalBonus = 0;

                        lastSalaryProcessInfo.TotalNetPay = lastSalaryProcessInfo.TotalNetPay + item.SalaryProcessDetail.NetPay;
                        lastSalaryProcessInfo.TotalGrossPay = lastSalaryProcessInfo.TotalGrossPay + item.SalaryProcessDetail.GrossPay;
                    }
                    lastSalaryProcessInfo.UpdatedBy = user.ActionUserId;
                    lastSalaryProcessInfo.UpdatedDate = DateTime.Now;

                    using (var transaction = await _payrollDbContext.Database.BeginTransactionAsync())
                    {
                        try
                        {
                            foreach (var item in salaryItems)
                            {
                                ids = ids + item.SalaryProcessDetail.EmployeeId.ToString() + ",";
                                item.SalaryProcessDetail.SalaryProcessId = lastSalaryProcessInfo.SalaryProcessId;
                                await _payrollDbContext.Payroll_SalaryProcessDetail.AddAsync(item.SalaryProcessDetail);
                                var salaryProcessDetailRowAffected = await _payrollDbContext.SaveChangesAsync();
                                if (salaryProcessDetailRowAffected > 0)
                                {
                                    #region Salary Allowance
                                    if (item.SalaryAllowances != null && item.SalaryAllowances.Any())
                                    {
                                        item.SalaryAllowances.ForEach(i =>
                                        {
                                            i.SalaryProcessId = lastSalaryProcessInfo.SalaryProcessId;
                                            i.SalaryProcessDetailId = item.SalaryProcessDetail.SalaryProcessDetailId;
                                        });

                                        await _payrollDbContext.Payroll_SalaryAllowance.AddRangeAsync(item.SalaryAllowances);
                                        if (await _payrollDbContext.SaveChangesAsync() == 0)
                                        {
                                            throw new Exception("Salary Allowances cannot be save");
                                        }
                                    }
                                    #endregion

                                    #region Salary Allowance Arrear
                                    if (item.SalaryAllowanceArrears != null && item.SalaryAllowanceArrears.Any())
                                    {
                                        item.SalaryAllowanceArrears.ForEach(i =>
                                        {
                                            i.SalaryProcessId = lastSalaryProcessInfo.SalaryProcessId;
                                            i.SalaryProcessDetailId = item.SalaryProcessDetail.SalaryProcessDetailId;
                                        });

                                        await _payrollDbContext.Payroll_SalaryAllowanceArrear.AddRangeAsync(item.SalaryAllowanceArrears);
                                        if (await _payrollDbContext.SaveChangesAsync() == 0)
                                        {
                                            throw new Exception("Salary Allowance arrears cannot be saved");
                                        }
                                    }
                                    #endregion

                                    #region Salary Allowance Adjustment
                                    if (item.SalaryAllowanceAdjustments != null && item.SalaryAllowanceAdjustments.Any())
                                    {
                                        item.SalaryAllowanceAdjustments.ForEach(i =>
                                        {
                                            i.SalaryProcessId = lastSalaryProcessInfo.SalaryProcessId;
                                            i.SalaryProcessDetailId = item.SalaryProcessDetail.SalaryProcessDetailId;
                                        });

                                        await _payrollDbContext.Payroll_SalaryAllowanceAdjustments.AddRangeAsync(item.SalaryAllowanceAdjustments);
                                        if (await _payrollDbContext.SaveChangesAsync() == 0)
                                        {
                                            throw new Exception("Salary Allowance adjustments cannot be saved");
                                        }
                                    }
                                    #endregion

                                    #region Salary Deduction
                                    if (item.SalaryDeductions != null && item.SalaryDeductions.Any())
                                    {
                                        item.SalaryDeductions.ForEach(i =>
                                        {
                                            i.SalaryProcessId = lastSalaryProcessInfo.SalaryProcessId;
                                            i.SalaryProcessDetailId = item.SalaryProcessDetail.SalaryProcessDetailId;
                                        });

                                        await _payrollDbContext.Payroll_SalaryDeduction.AddRangeAsync(item.SalaryDeductions);
                                        if (await _payrollDbContext.SaveChangesAsync() == 0)
                                        {
                                            throw new Exception("Salary deduction cannot be saved");
                                        }
                                    }
                                    #endregion

                                    #region Salary Deduction Adjustment
                                    if (item.SalaryDeductionAdjustments != null && item.SalaryDeductionAdjustments.Any())
                                    {
                                        item.SalaryDeductionAdjustments.ForEach(i =>
                                        {
                                            i.SalaryProcessId = lastSalaryProcessInfo.SalaryProcessId;
                                            i.SalaryProcessDetailId = item.SalaryProcessDetail.SalaryProcessDetailId;
                                        });

                                        await _payrollDbContext.Payroll_SalaryDeductionAdjustments.AddRangeAsync(item.SalaryDeductionAdjustments);
                                        if (await _payrollDbContext.SaveChangesAsync() == 0)
                                        {
                                            throw new Exception("Salary deduction adjustments cannot be saved");
                                        }
                                    }
                                    #endregion

                                    #region Deposit Allowance History
                                    if (item.DepositAllowanceHistories != null && item.DepositAllowanceHistories.Any())
                                    {
                                        item.DepositAllowanceHistories.ForEach(i =>
                                        {
                                            i.SalaryProcessId = lastSalaryProcessInfo.SalaryProcessId;
                                            i.SalaryProcessDetailId = item.SalaryProcessDetail.SalaryProcessDetailId;
                                        });

                                        await _payrollDbContext.Payroll_DepositAllowanceHistory.AddRangeAsync(item.DepositAllowanceHistories);
                                        if (await _payrollDbContext.SaveChangesAsync() == 0)
                                        {
                                            throw new Exception("Salary deposit allowance history cannot be saved");
                                        }
                                    }
                                    #endregion

                                    #region Deposite Allowance Payment History
                                    if (item.DepositAllowancePaymentHistories != null && item.DepositAllowancePaymentHistories.Any())
                                    {
                                        item.DepositAllowancePaymentHistories.ForEach(i =>
                                        {
                                            i.SalaryProcessId = lastSalaryProcessInfo.SalaryProcessId;
                                            i.SalaryProcessDetailId = item.SalaryProcessDetail.SalaryProcessDetailId;
                                        });

                                        await _payrollDbContext.Payroll_DepositAllowancePaymentHistory.AddRangeAsync(item.DepositAllowancePaymentHistories);
                                        if (await _payrollDbContext.SaveChangesAsync() == 0)
                                        {
                                            throw new Exception("Salary deposit allowance payment history cannot be saved");
                                        }
                                    }
                                    #endregion

                                    #region Service Anniversary Allowance
                                    if (item.RecipientsofServiceAnniversaryAllowances != null && item.RecipientsofServiceAnniversaryAllowances.Any())
                                    {
                                        item.RecipientsofServiceAnniversaryAllowances.ForEach(i =>
                                        {
                                            i.SalaryProcessId = lastSalaryProcessInfo.SalaryProcessId;
                                            i.SalaryProcessDetailId = item.SalaryProcessDetail.SalaryProcessDetailId;
                                        });

                                        await _payrollDbContext.Payroll_RecipientsofServiceAnniversaryAllowance.AddRangeAsync(item.RecipientsofServiceAnniversaryAllowances);
                                        if (await _payrollDbContext.SaveChangesAsync() == 0)
                                        {
                                            throw new Exception("Recipients of Service Anniversary allowances cannot be saved");
                                        }
                                    }
                                    #endregion

                                    #region Monthly fixed Allowance
                                    if (item.MonthlyAllowanceHistories != null && item.MonthlyAllowanceHistories.Any())
                                    {
                                        item.MonthlyAllowanceHistories.ForEach(i =>
                                        {
                                            i.FiscalYearId = item.SalaryProcessDetail.FiscalYearId ?? 0;
                                            i.SalaryProcessId = lastSalaryProcessInfo.SalaryProcessId;
                                            i.SalaryProcessDetailId = item.SalaryProcessDetail.SalaryProcessDetailId;
                                        });

                                        await _payrollDbContext.Payroll_MonthlyAllowanceHistory.AddRangeAsync(item.MonthlyAllowanceHistories);
                                        if (await _payrollDbContext.SaveChangesAsync() == 0)
                                        {
                                            throw new Exception("Monthly allowance histories cannot be saved");
                                        }
                                    }
                                    #endregion

                                }
                                else
                                {
                                    throw new Exception("Salary process detail cannot be saved");
                                }
                            }

                            _payrollDbContext.Payroll_SalaryProcess.Update(lastSalaryProcessInfo);
                            if (await _payrollDbContext.SaveChangesAsync() == 0)
                            {
                                executionStatus = ResponseMessage.Message(false, ResponseMessage.Unsuccessful);
                                await transaction.RollbackAsync();
                            }
                            else
                            {
                                executionStatus = ResponseMessage.Message(true, ResponseMessage.Saved);
                                executionStatus.Ids = ids;
                                await transaction.CommitAsync();
                            }
                        }
                        catch (Exception ex)
                        {
                            await transaction.RollbackAsync();
                            if (ex.InnerException == null)
                            {
                                executionStatus = ResponseMessage.Message(false, ex.Message);
                            }
                            else
                            {
                                executionStatus = ResponseMessage.Message(false, ResponseMessage.SomthingWentWrong);
                            }
                        }
                    }
                    using (var connection = await _dapper.SqlConnectionForTransactionalAsync(user.Database))
                    {
                        connection.Open();
                        {
                            bool isUpdateInsertFail = false;
                            using (var transaction = connection.BeginTransaction())
                            {
                                try
                                {
                                    var parameters = Utility.DappperParamsInKeyValuePairs(lastSalaryProcessInfo, user, addBaseProperty: true, addUserId: false, addCompany: false, addOrganization: false);
                                    parameters.Remove("SalaryProcessId");
                                    parameters.Remove("SalaryProcessDetails");
                                    var query = Utility.GenerateUpdateQuery(tableName: "Payroll_SalaryProcess", paramkeys: parameters.Select(x => x.Key).ToList());
                                    query += $"WHERE SalaryProcessId = @SalaryProcessId";
                                    parameters.Add("SalaryProcessId", lastSalaryProcessInfo.SalaryProcessId);

                                    int rawAffected = await connection.ExecuteAsync(query, parameters, transaction);

                                    if (rawAffected > 0)
                                    {
                                        foreach (var employeeSalary in salaryItems)
                                        {
                                            executionStatus.Ids = executionStatus.Ids + employeeSalary.SalaryProcessDetail.EmployeeId.ToString() + ",";
                                            query = "";
                                            parameters.Clear();

                                            employeeSalary.SalaryProcessDetail.SalaryProcessId = lastSalaryProcessInfo.SalaryProcessId;
                                            employeeSalary.SalaryProcessDetail.SalaryProcessUniqId = lastSalaryProcessInfo.SalaryProcessUniqId;
                                            parameters = Utility.DappperParamsInKeyValuePairs(employeeSalary.SalaryProcessDetail, user, addBaseProperty: true, addUserId: false, addCompany: false, addOrganization: false);
                                            parameters.Remove("SalaryProcessDetailId");
                                            parameters.Remove("SalaryProcess");

                                            query = Utility.GenerateInsertQuery(tableName: "Payroll_SalaryProcessDetail", paramkeys: parameters.Select(x => x.Key).ToList(),
                                                output: "OUTPUT INSERTED.*");
                                            var insertedSalaryProcessDetail = await connection.QueryFirstOrDefaultAsync<SalaryProcessDetail>(query, parameters, transaction);

                                            if (insertedSalaryProcessDetail != null && insertedSalaryProcessDetail.SalaryProcessDetailId > 0)
                                            {
                                                executionStatus.ItemId = insertedSalaryProcessDetail.SalaryProcessDetailId;

                                                #region Salary Allowance
                                                List<Dictionary<string, dynamic>> parameterList = new();
                                                parameters.Clear();
                                                foreach (var item in employeeSalary.SalaryAllowances)
                                                {
                                                    item.SalaryProcessId = lastSalaryProcessInfo.SalaryProcessId;
                                                    item.SalaryProcessDetailId = insertedSalaryProcessDetail.SalaryProcessDetailId;
                                                    parameters = Utility.DappperParamsInKeyValuePairs(item, appUser: null, addBaseProperty: true,
                                                        addUserId: false, addCompany: false, addOrganization: false);
                                                    parameters.Remove("SalaryAllowanceId");
                                                    parameterList.Add(parameters);
                                                }
                                                query = Utility.GenerateInsertQuery(tableName: "Payroll_SalaryAllowance", paramkeys: parameters.Select(x => x.Key).ToList(), output: "OUTPUT INSERTED.*");
                                                int rawAffectedInSalaryAllowance = await connection.ExecuteAsync(query, parameterList, transaction);
                                                if (rawAffectedInSalaryAllowance != employeeSalary.SalaryAllowances.Count)
                                                {
                                                    isUpdateInsertFail = true;
                                                    executionStatus.Status = false;
                                                    executionStatus.Msg = "Something went wrong";
                                                    executionStatus.ErrorMsg = "Failed to save in salary allowance and employee id : " + employeeSalary.SalaryProcessDetail.EmployeeId.ToString();
                                                    break;
                                                }
                                                #endregion

                                                #region Salary Allowance Arrear
                                                parameterList.Clear();
                                                parameters.Clear();
                                                foreach (var item in employeeSalary.SalaryAllowanceArrears)
                                                {
                                                    item.SalaryProcessId = lastSalaryProcessInfo.SalaryProcessId;
                                                    item.SalaryProcessDetailId = insertedSalaryProcessDetail.SalaryProcessDetailId;
                                                    parameters = Utility.DappperParamsInKeyValuePairs(item, appUser: null, addBaseProperty: true,
                                                      addUserId: false, addCompany: false, addOrganization: false);
                                                    parameters.Remove("SalaryAllowanceArrearId");
                                                    parameterList.Add(parameters);
                                                }
                                                query = Utility.GenerateInsertQuery(tableName: "Payroll_SalaryAllowanceArrear", paramkeys: parameters.Select(x => x.Key).ToList(), output: "OUTPUT INSERTED.*");
                                                int rawAffectedInSalaryAllowanceArrear = await connection.ExecuteAsync(query, parameterList, transaction);
                                                if (rawAffectedInSalaryAllowanceArrear != employeeSalary.SalaryAllowanceArrears.Count)
                                                {
                                                    isUpdateInsertFail = true;
                                                    executionStatus.Status = false;
                                                    executionStatus.Msg = "Something went wrong";
                                                    executionStatus.ErrorMsg = "Failed to save in salary allowance arrear and employee id : " + employeeSalary.SalaryProcessDetail.EmployeeId.ToString();
                                                    break;
                                                }
                                                #endregion

                                                #region Salary Allowance Adjustment
                                                parameterList.Clear();
                                                parameters.Clear();
                                                foreach (var item in employeeSalary.SalaryAllowanceAdjustments)
                                                {
                                                    item.SalaryProcessId = lastSalaryProcessInfo.SalaryProcessId;
                                                    item.SalaryProcessDetailId = insertedSalaryProcessDetail.SalaryProcessDetailId;
                                                    parameters = Utility.DappperParamsInKeyValuePairs(item, appUser: null, addBaseProperty: true,
                                                      addUserId: false, addCompany: false, addOrganization: false);
                                                    parameters.Remove("AllowanceAdjustmentId");
                                                    parameterList.Add(parameters);
                                                }
                                                query = Utility.GenerateInsertQuery(tableName: "Payroll_SalaryAllowanceAdjustment", paramkeys: parameters.Select(x => x.Key).ToList(), output: "OUTPUT INSERTED.*");
                                                int rawAffectedInSalaryAllowanceAdj = await connection.ExecuteAsync(query, parameterList, transaction);
                                                if (rawAffectedInSalaryAllowanceAdj != employeeSalary.SalaryAllowanceAdjustments.Count)
                                                {
                                                    isUpdateInsertFail = true;
                                                    executionStatus.Status = false;
                                                    executionStatus.Msg = "Something went wrong";
                                                    executionStatus.ErrorMsg = "Failed to save in salary allowance adjustment and employee id : " + employeeSalary.SalaryProcessDetail.EmployeeId.ToString();
                                                    break;
                                                }
                                                #endregion

                                                #region Salary Deduction
                                                parameterList.Clear();
                                                parameters.Clear();
                                                foreach (var item in employeeSalary.SalaryDeductions)
                                                {
                                                    item.SalaryProcessId = lastSalaryProcessInfo.SalaryProcessId;
                                                    item.SalaryProcessDetailId = insertedSalaryProcessDetail.SalaryProcessDetailId;
                                                    parameters = Utility.DappperParamsInKeyValuePairs(item, appUser: null, addBaseProperty: true,
                                                     addUserId: false, addCompany: false, addOrganization: false);
                                                    parameters.Remove("SalaryDeductionId");
                                                    parameterList.Add(parameters);
                                                }
                                                query = Utility.GenerateInsertQuery(tableName: "Payroll_SalaryDeduction", paramkeys: parameters.Select(x => x.Key).ToList(), output: "OUTPUT INSERTED.*");
                                                int rawAffectedInSalaryDeduction = await connection.ExecuteAsync(query, parameterList, transaction);
                                                if (rawAffectedInSalaryDeduction != employeeSalary.SalaryDeductions.Count)
                                                {
                                                    isUpdateInsertFail = true;
                                                    executionStatus.Status = false;
                                                    executionStatus.Msg = "Something went wrong";
                                                    executionStatus.ErrorMsg = "Failed to save in salary deduction and employee id : " + employeeSalary.SalaryProcessDetail.EmployeeId.ToString();
                                                    break;
                                                }
                                                #endregion

                                                #region Salary Deduction Adjustment
                                                parameterList.Clear();
                                                parameters.Clear();
                                                foreach (var item in employeeSalary.SalaryDeductionAdjustments)
                                                {
                                                    item.SalaryProcessId = lastSalaryProcessInfo.SalaryProcessId;
                                                    item.SalaryProcessDetailId = insertedSalaryProcessDetail.SalaryProcessDetailId;
                                                    parameters = Utility.DappperParamsInKeyValuePairs(item, appUser: null, addBaseProperty: true,
                                                    addUserId: false, addCompany: false, addOrganization: false);
                                                    parameters.Remove("DeductionAdjustmentId");
                                                    parameterList.Add(parameters);
                                                }
                                                query = Utility.GenerateInsertQuery(tableName: "Payroll_SalaryDeductionAdjustment", paramkeys: parameters.Select(x => x.Key).ToList(), output: "OUTPUT INSERTED.*");
                                                int rawAffectedInSalaryDeductionAdjustment = await connection.ExecuteAsync(query, parameterList, transaction);
                                                if (rawAffectedInSalaryDeductionAdjustment != employeeSalary.SalaryDeductionAdjustments.Count)
                                                {
                                                    isUpdateInsertFail = true;
                                                    executionStatus.Status = false;
                                                    executionStatus.Msg = "Something went wrong";
                                                    executionStatus.ErrorMsg = "Failed to save in salary deduction adjustment and employee id : " + employeeSalary.SalaryProcessDetail.EmployeeId.ToString();
                                                    break;
                                                }
                                                #endregion

                                                #region Deposit Allowance History
                                                parameterList.Clear();
                                                parameters.Clear();
                                                foreach (var item in employeeSalary.DepositAllowanceHistories)
                                                {
                                                    item.SalaryProcessId = lastSalaryProcessInfo.SalaryProcessId;
                                                    item.SalaryProcessDetailId = insertedSalaryProcessDetail.SalaryProcessDetailId;
                                                    parameters = Utility.DappperParamsInKeyValuePairs(item, appUser: null, addBaseProperty: true,
                                                    addUserId: false, addCompany: false, addOrganization: false);
                                                    parameters.Remove("Id");
                                                    parameterList.Add(parameters);
                                                }
                                                query = Utility.GenerateInsertQuery(tableName: "Payroll_DepositAllowanceHistory", paramkeys: parameters.Select(x => x.Key).ToList(), output: "OUTPUT INSERTED.*");
                                                int rawAffectedInDepositAllowanceHistory = await connection.ExecuteAsync(query, parameterList, transaction);
                                                if (rawAffectedInDepositAllowanceHistory != employeeSalary.DepositAllowanceHistories.Count)
                                                {
                                                    isUpdateInsertFail = true;
                                                    executionStatus.Status = false;
                                                    executionStatus.Msg = "Something went wrong";
                                                    executionStatus.ErrorMsg = "Failed to save in deposit allowance and employee id : " + employeeSalary.SalaryProcessDetail.EmployeeId.ToString();
                                                    break;
                                                }
                                                #endregion

                                                #region Deposite Allowance Payment History
                                                parameterList.Clear();
                                                parameters.Clear();
                                                foreach (var item in employeeSalary.DepositAllowancePaymentHistories)
                                                {
                                                    parameters.Clear();
                                                    item.SalaryProcessId = lastSalaryProcessInfo.SalaryProcessId;
                                                    item.SalaryProcessDetailId = insertedSalaryProcessDetail.SalaryProcessDetailId;
                                                    item.UpdatedBy = user.ActionUserId;
                                                    item.UpdatedDate = DateTime.Now;
                                                    parameters = Utility.DappperParamsInKeyValuePairs(item, appUser: null, addBaseProperty: true,
                                                    addUserId: false, addCompany: false, addOrganization: false);
                                                    parameters.Remove("Id");
                                                    query = Utility.GenerateUpdateQuery(tableName: "Payroll_DepositAllowancePaymentHistory", paramkeys: parameters.Select(x => x.Key).ToList());
                                                    query += $"WHERE Id = @Id";
                                                    parameters.Add("Id", item.Id);

                                                    int rawAffectedInDepositAllowancePaymentHistory = await connection.ExecuteAsync(query, parameters, transaction);
                                                    if (rawAffectedInDepositAllowancePaymentHistory < 0)
                                                    {
                                                        isUpdateInsertFail = true;
                                                        executionStatus.Status = false;
                                                        executionStatus.Msg = "Something went wrong";
                                                        executionStatus.ErrorMsg = "Failed to save in deposit allowance payment and employee id : " + employeeSalary.SalaryProcessDetail.EmployeeId.ToString();
                                                        break;
                                                    }
                                                }
                                                #endregion

                                                #region Service Anniversary Allowance  
                                                parameterList.Clear();
                                                parameters.Clear();
                                                foreach (var item in employeeSalary.RecipientsofServiceAnniversaryAllowances)
                                                {
                                                    item.SalaryProcessId = lastSalaryProcessInfo.SalaryProcessId;
                                                    item.SalaryProcessDetailId = insertedSalaryProcessDetail.SalaryProcessDetailId;
                                                    parameters = Utility.DappperParamsInKeyValuePairs(item, appUser: null, addBaseProperty: true,
                                                      addUserId: false, addCompany: false, addOrganization: false);
                                                    parameters.Remove("Id");
                                                    parameterList.Add(parameters);
                                                }
                                                query = Utility.GenerateInsertQuery(tableName: "Payroll_RecipientsofServiceAnniversaryAllowance", paramkeys: parameters.Select(x => x.Key).ToList(), output: "OUTPUT INSERTED.*");
                                                int rawRecipientsofServiceAnniversaryAllowances = await connection.ExecuteAsync(query, parameterList, transaction);
                                                if (rawRecipientsofServiceAnniversaryAllowances != employeeSalary.RecipientsofServiceAnniversaryAllowances.Count)
                                                {
                                                    isUpdateInsertFail = true;
                                                    executionStatus.Status = false;
                                                    executionStatus.Msg = "Something went wrong";
                                                    executionStatus.ErrorMsg = "Failed to save in service anniversary arrear and employee id : " + employeeSalary.SalaryProcessDetail.EmployeeId.ToString();
                                                    break;
                                                }
                                                #endregion

                                                #region Salary Components
                                                parameterList.Clear();
                                                parameters.Clear();
                                                foreach (var item in employeeSalary.SalaryComponentHistories)
                                                {
                                                    item.CreatedDate = DateTime.Now;
                                                    item.SalaryProcessId = lastSalaryProcessInfo.SalaryProcessId;
                                                    item.SalaryProcessDetailId = insertedSalaryProcessDetail.SalaryProcessDetailId;
                                                    parameters = Utility.DappperParamsInKeyValuePairs(item, appUser: null, addBaseProperty: true,
                                                        addUserId: false, addCompany: false, addOrganization: false);
                                                    parameters.Remove("Id");
                                                    parameterList.Add(parameters);
                                                }
                                                query = Utility.GenerateInsertQuery(tableName: "Payroll_SalaryComponentHistory", paramkeys: parameters.Select(x => x.Key).ToList(), output: "OUTPUT INSERTED.*");
                                                int rawAffectedInSalaryComponents = await connection.ExecuteAsync(query, parameterList, transaction);
                                                if (rawAffectedInSalaryComponents != employeeSalary.SalaryComponentHistories.Count)
                                                {
                                                    executionStatus.Status = false;
                                                    executionStatus.Msg = "Something went wrong";
                                                    executionStatus.ErrorMsg = "Failed to save in salary component histories and employee id : " + employeeSalary.SalaryProcessDetail.EmployeeId.ToString();
                                                    break;
                                                }
                                                #endregion
                                            }
                                            else
                                            {
                                                isUpdateInsertFail = true;
                                                executionStatus.Status = false;
                                                executionStatus.Msg = "Something went wrong";
                                                executionStatus.ErrorMsg = "Failed to save in salary process detail and employee id : " + employeeSalary.SalaryProcessDetail.EmployeeId.ToString();
                                                break;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        isUpdateInsertFail = true;
                                        executionStatus.Status = false;
                                        executionStatus.Msg = "Something went wrong";
                                        executionStatus.ErrorMsg = "Failed to save in salary process info";
                                    }

                                    if (isUpdateInsertFail == false)
                                    {
                                        transaction.Commit();
                                        executionStatus.Status = true;
                                        executionStatus.Msg = "Process has been done";

                                    }
                                }
                                catch (Exception ex)
                                {
                                    transaction.Rollback();
                                    executionStatus = Utility.Invalid(ex.InnerException != null ? ex.InnerException.Message : ex.Message);
                                    await _sysLogger.SaveHRMSException(ex, user.Database, "ExecuteSalaryProcess", "SaveSalaryAsync", user);
                                }
                                finally { }
                            }
                        }
                    }
                }
                else
                {
                    // Batch No
                    var batchNo = await _salaryProcessBusiness.GenerateBatchNoAysnc(salaryMonth, salaryYear, user);

                    using (var transaction = await _payrollDbContext.Database.BeginTransactionAsync())
                    {
                        try
                        {
                            SalaryProcess salaryProcess = new SalaryProcess();
                            salaryProcess.ProcessType = processType;
                            salaryProcess.IsDisbursed = false;
                            salaryProcess.SalaryMonth = (short)salaryMonth;
                            salaryProcess.SalaryYear = (short)salaryYear;
                            salaryProcess.SalaryDate = DateTimeExtension.LastDateOfAMonth(salaryYear, salaryMonth);
                            salaryProcess.ProcessDate = DateTime.Now.Date;
                            salaryProcess.CompanyAccountId = (user.CompanyId == 19 && user.OrganizationId == 11) ? 1 : 0;
                            salaryProcess.BatchNo = batchNo;
                            salaryProcess.FiscalYearId = salaryItems[0].SalaryProcessDetail.FiscalYearId;
                            salaryProcess.CreatedBy = user.ActionUserId;
                            salaryProcess.CreatedDate = DateTime.Now.Date;
                            salaryProcess.CompanyId = user.CompanyId;
                            salaryProcess.OrganizationId = user.OrganizationId;
                            await _payrollDbContext.Payroll_SalaryProcess.AddAsync(salaryProcess);

                            var salaryProcessRowAffected = await _payrollDbContext.SaveChangesAsync();
                            if (salaryProcessRowAffected > 0 && salaryItems != null && salaryItems.Any())
                            {
                                foreach (var item in salaryItems)
                                {
                                    ids = ids + item.SalaryProcessDetail.EmployeeId.ToString() + ",";
                                    salaryProcess.TotalAllowance = salaryProcess.TotalAllowance + item.SalaryProcessDetail.TotalAllowance;
                                    salaryProcess.TotalArrearAllowance = salaryProcess.TotalArrearAllowance + item.SalaryProcessDetail.TotalArrearAllowance;
                                    salaryProcess.TotalAllowanceAdjustment = salaryProcess.TotalAllowanceAdjustment + item.SalaryProcessDetail.TotalAllowanceAdjustment;
                                    salaryProcess.TotalDeduction = salaryProcess.TotalDeduction + item.SalaryProcessDetail.TotalDeduction;
                                    salaryProcess.TotalPFAmount = salaryProcess.TotalPFAmount + item.SalaryProcessDetail.PFAmount;
                                    salaryProcess.TotalPFArrear = salaryProcess.TotalPFArrear + item.SalaryProcessDetail.PFArrear;
                                    salaryProcess.TotalHoldAmount = salaryProcess.TotalHoldAmount + item.SalaryProcessDetail.HoldAmount;
                                    salaryProcess.TotalHoldDays = salaryProcess.TotalHoldDays ?? 0 + item.SalaryProcessDetail.HoldDays ?? 0;
                                    salaryProcess.TotalUnholdAmount = salaryProcess.TotalUnholdAmount ?? 0 + item.SalaryProcessDetail.UnholdAmount ?? 0;
                                    salaryProcess.TotalUnholdDays = salaryProcess.TotalUnholdDays ?? 0 + item.SalaryProcessDetail.UnholdDays ?? 0;
                                    salaryProcess.TotalEmployees = salaryProcess.TotalEmployees + 1;
                                    salaryProcess.TotalMonthlyTax = 0;
                                    salaryProcess.TotalBonus = 0;
                                    salaryProcess.TotalGrossPay = salaryProcess.TotalGrossPay + item.SalaryProcessDetail.GrossPay;
                                    salaryProcess.TotalNetPay = salaryProcess.TotalNetPay + item.SalaryProcessDetail.NetPay;


                                    item.SalaryProcessDetail.SalaryProcessId = salaryProcess.SalaryProcessId;
                                    await _payrollDbContext.Payroll_SalaryProcessDetail.AddAsync(item.SalaryProcessDetail);
                                    var salaryProcessDetailRowAffected = await _payrollDbContext.SaveChangesAsync();
                                    if (salaryProcessDetailRowAffected > 0)
                                    {
                                        #region Salary Allowance
                                        if (item.SalaryAllowances != null && item.SalaryAllowances.Any())
                                        {
                                            item.SalaryAllowances.ForEach(i =>
                                            {
                                                i.SalaryProcessId = salaryProcess.SalaryProcessId;
                                                i.SalaryProcessDetailId = item.SalaryProcessDetail.SalaryProcessDetailId;
                                            });

                                            await _payrollDbContext.Payroll_SalaryAllowance.AddRangeAsync(item.SalaryAllowances);
                                            if (await _payrollDbContext.SaveChangesAsync() == 0)
                                            {
                                                throw new Exception("Salary Allowances cannot be save");
                                            }
                                        }
                                        #endregion

                                        #region Salary Allowance Arrear
                                        if (item.SalaryAllowanceArrears != null && item.SalaryAllowanceArrears.Any())
                                        {
                                            item.SalaryAllowanceArrears.ForEach(i =>
                                            {
                                                i.SalaryProcessId = salaryProcess.SalaryProcessId;
                                                i.SalaryProcessDetailId = item.SalaryProcessDetail.SalaryProcessDetailId;
                                            });

                                            await _payrollDbContext.Payroll_SalaryAllowanceArrear.AddRangeAsync(item.SalaryAllowanceArrears);
                                            if (await _payrollDbContext.SaveChangesAsync() == 0)
                                            {
                                                throw new Exception("Salary Allowance arrears cannot be saved");
                                            }
                                        }
                                        #endregion

                                        #region Salary Allowance Adjustment
                                        if (item.SalaryAllowanceAdjustments != null && item.SalaryAllowanceAdjustments.Any())
                                        {
                                            item.SalaryAllowanceAdjustments.ForEach(i =>
                                            {
                                                i.SalaryProcessId = salaryProcess.SalaryProcessId;
                                                i.SalaryProcessDetailId = item.SalaryProcessDetail.SalaryProcessDetailId;
                                            });

                                            await _payrollDbContext.Payroll_SalaryAllowanceAdjustments.AddRangeAsync(item.SalaryAllowanceAdjustments);
                                            if (await _payrollDbContext.SaveChangesAsync() == 0)
                                            {
                                                throw new Exception("Salary Allowance adjustments cannot be saved");
                                            }
                                        }
                                        #endregion

                                        #region Salary Deduction
                                        if (item.SalaryDeductions != null && item.SalaryDeductions.Any())
                                        {
                                            item.SalaryDeductions.ForEach(i =>
                                            {
                                                i.SalaryProcessId = salaryProcess.SalaryProcessId;
                                                i.SalaryProcessDetailId = item.SalaryProcessDetail.SalaryProcessDetailId;
                                            });

                                            await _payrollDbContext.Payroll_SalaryDeduction.AddRangeAsync(item.SalaryDeductions);
                                            if (await _payrollDbContext.SaveChangesAsync() == 0)
                                            {
                                                throw new Exception("Salary deduction cannot be saved");
                                            }
                                        }
                                        #endregion

                                        #region Salary Deduction Adjustment
                                        if (item.SalaryDeductionAdjustments != null && item.SalaryDeductionAdjustments.Any())
                                        {
                                            item.SalaryDeductionAdjustments.ForEach(i =>
                                            {
                                                i.SalaryProcessId = salaryProcess.SalaryProcessId;
                                                i.SalaryProcessDetailId = item.SalaryProcessDetail.SalaryProcessDetailId;
                                            });

                                            await _payrollDbContext.Payroll_SalaryDeductionAdjustments.AddRangeAsync(item.SalaryDeductionAdjustments);
                                            if (await _payrollDbContext.SaveChangesAsync() == 0)
                                            {
                                                throw new Exception("Salary deduction adjustments cannot be saved");
                                            }
                                        }
                                        #endregion

                                        #region Deposit Allowance History
                                        if (item.DepositAllowanceHistories != null && item.DepositAllowanceHistories.Any())
                                        {
                                            item.DepositAllowanceHistories.ForEach(i =>
                                            {
                                                i.SalaryProcessId = salaryProcess.SalaryProcessId;
                                                i.SalaryProcessDetailId = item.SalaryProcessDetail.SalaryProcessDetailId;
                                            });

                                            await _payrollDbContext.Payroll_DepositAllowanceHistory.AddRangeAsync(item.DepositAllowanceHistories);
                                            if (await _payrollDbContext.SaveChangesAsync() == 0)
                                            {
                                                throw new Exception("Salary deposit allowance history cannot be saved");
                                            }
                                        }
                                        #endregion

                                        #region Deposite Allowance Payment History
                                        if (item.DepositAllowancePaymentHistories != null && item.DepositAllowancePaymentHistories.Any())
                                        {
                                            item.DepositAllowancePaymentHistories.ForEach(i =>
                                            {
                                                i.SalaryProcessId = salaryProcess.SalaryProcessId;
                                                i.SalaryProcessDetailId = item.SalaryProcessDetail.SalaryProcessDetailId;
                                            });

                                            await _payrollDbContext.Payroll_DepositAllowancePaymentHistory.AddRangeAsync(item.DepositAllowancePaymentHistories);
                                            if (await _payrollDbContext.SaveChangesAsync() == 0)
                                            {
                                                throw new Exception("Salary deposit allowance payment history cannot be saved");
                                            }
                                        }
                                        #endregion

                                        #region Service Anniversary Allowance
                                        if (item.RecipientsofServiceAnniversaryAllowances != null && item.RecipientsofServiceAnniversaryAllowances.Any())
                                        {
                                            item.RecipientsofServiceAnniversaryAllowances.ForEach(i =>
                                            {
                                                i.SalaryProcessId = salaryProcess.SalaryProcessId;
                                                i.SalaryProcessDetailId = item.SalaryProcessDetail.SalaryProcessDetailId;
                                            });

                                            await _payrollDbContext.Payroll_RecipientsofServiceAnniversaryAllowance.AddRangeAsync(item.RecipientsofServiceAnniversaryAllowances);
                                            if (await _payrollDbContext.SaveChangesAsync() == 0)
                                            {
                                                throw new Exception("Recipients of Service Anniversary allowances cannot be saved");
                                            }
                                        }
                                        #endregion

                                        #region Monthly fixed Allowance
                                        if (item.MonthlyAllowanceHistories != null && item.MonthlyAllowanceHistories.Any())
                                        {
                                            item.MonthlyAllowanceHistories.ForEach(i =>
                                            {
                                                i.FiscalYearId = item.SalaryProcessDetail.FiscalYearId ?? 0;
                                                i.SalaryProcessId = salaryProcess.SalaryProcessId;
                                                i.SalaryProcessDetailId = item.SalaryProcessDetail.SalaryProcessDetailId;
                                            });

                                            await _payrollDbContext.Payroll_MonthlyAllowanceHistory.AddRangeAsync(item.MonthlyAllowanceHistories);
                                            if (await _payrollDbContext.SaveChangesAsync() == 0)
                                            {
                                                throw new Exception("Monthly allowance histories cannot be saved");
                                            }
                                        }
                                        #endregion

                                    }
                                    else
                                    {
                                        throw new Exception("Salary process detail cannot be saved");
                                    }
                                }

                                _payrollDbContext.Payroll_SalaryProcess.Update(salaryProcess);
                                if (await _payrollDbContext.SaveChangesAsync() == 0)
                                {
                                    executionStatus = ResponseMessage.Message(false, ResponseMessage.Unsuccessful);
                                    await transaction.RollbackAsync();
                                }
                                else
                                {
                                    executionStatus = ResponseMessage.Message(true, ResponseMessage.Saved);
                                    executionStatus.Ids = ids;
                                    await transaction.CommitAsync();
                                }
                            }
                            else
                            {
                                throw new Exception("Salary process information cannot be saved");
                            }
                        }
                        catch (Exception ex)
                        {
                            await transaction.RollbackAsync();
                            if (ex.InnerException == null)
                            {
                                executionStatus = ResponseMessage.Message(false, ex.Message);
                            }
                            else
                            {
                                executionStatus = ResponseMessage.Message(false, ResponseMessage.SomthingWentWrong);
                            }
                        }
                    }

                    #region Previous Save Code
                    // New Process
                    //using (var connection = await _dapper.SqlConnectionForTransactionalAsync(user.Database))
                    //{
                    //    bool isItemInsertFail = false;
                    //    #region Salary Process
                    //    SalaryProcess salaryProcess = new SalaryProcess();
                    //    salaryProcess.ProcessType = processType;
                    //    salaryProcess.IsDisbursed = false;
                    //    salaryProcess.SalaryMonth = (short)salaryMonth;
                    //    salaryProcess.SalaryYear = (short)salaryYear;
                    //    salaryProcess.SalaryDate = DateTimeExtension.LastDateOfAMonth(salaryYear, salaryMonth);
                    //    salaryProcess.ProcessDate = DateTime.Now.Date;
                    //    if (user.CompanyId == 19 && user.OrganizationId == 11)
                    //    {
                    //        salaryProcess.CompanyAccountId = 1;
                    //    }
                    //    long fiscalYearId = 0;

                    //    foreach (var item in salaryItems)
                    //    {
                    //        salaryProcess.TotalAllowance = salaryProcess.TotalAllowance + item.SalaryAllowances.Sum(i => i.Amount);
                    //        salaryProcess.TotalArrearAllowance = salaryProcess.TotalArrearAllowance + item.SalaryAllowanceArrears.Sum(i => i.Amount);
                    //        salaryProcess.TotalAllowanceAdjustment = salaryProcess.TotalAllowanceAdjustment + item.SalaryAllowanceAdjustments.Sum(i => i.Amount);
                    //        salaryProcess.TotalDeduction = salaryProcess.TotalDeduction + item.SalaryProcessDetail.TotalDeduction;
                    //        salaryProcess.TotalPFAmount = salaryProcess.TotalPFAmount + item.SalaryProcessDetail.PFAmount;
                    //        salaryProcess.TotalPFArrear = salaryProcess.TotalPFArrear + item.SalaryProcessDetail.PFArrear;
                    //        salaryProcess.TotalHoldAmount = (salaryProcess.TotalHoldAmount ?? 0) + item.SalaryProcessDetail.HoldAmount;
                    //        salaryProcess.TotalHoldDays = salaryProcess.TotalHoldDays ?? 0 + item.SalaryProcessDetail.HoldDays ?? 0;
                    //        salaryProcess.TotalUnholdAmount = (salaryProcess.TotalUnholdAmount ?? 0) + item.SalaryProcessDetail.UnholdAmount;
                    //        salaryProcess.TotalUnholdDays = salaryProcess.TotalUnholdDays ?? 0 + item.SalaryProcessDetail.UnholdDays ?? 0;
                    //        salaryProcess.TotalEmployees = salaryProcess.TotalEmployees + 1;
                    //        salaryProcess.TotalMonthlyTax = 0;
                    //        salaryProcess.TotalBonus = 0;
                    //        salaryProcess.TotalGrossPay = salaryProcess.TotalGrossPay + item.SalaryProcessDetail.GrossPay;
                    //        if (fiscalYearId <= 0)
                    //        {
                    //            fiscalYearId = item.SalaryProcessDetail.FiscalYearId ?? 0;
                    //        }
                    //        salaryProcess.TotalNetPay = salaryProcess.TotalNetPay + item.SalaryProcessDetail.NetPay;
                    //    }

                    //    salaryProcess.BatchNo = batchNo;
                    //    salaryProcess.FiscalYearId = fiscalYearId;


                    //    //salaryProcess.TotalNetPay=
                    //    //salaryProcess.TotalGrossPay - (salaryProcess.TotalDeduction + salaryProcess.TotalPFAmount + salaryProcess.TotalPFArrear + (salaryProcess.TotalHoldAmount ?? 0));
                    //    salaryProcess.CreatedBy = user.ActionUserId;
                    //    salaryProcess.CreatedDate = DateTime.Now.Date;
                    //    salaryProcess.CompanyId = user.CompanyId;
                    //    salaryProcess.OrganizationId = user.OrganizationId;
                    //    #endregion
                    //    connection.Open();
                    //    {
                    //        using (var transaction = connection.BeginTransaction())
                    //        {
                    //            try
                    //            {
                    //                // Salary Process Info Insert 
                    //                var parameters = Utility.DappperParamsInKeyValuePairs(salaryProcess, user, addBaseProperty: true, addUserId: false, addCompany: false, addOrganization: false);
                    //                parameters.Remove("SalaryProcessId");
                    //                parameters.Remove("SalaryProcessDetails");
                    //                string query = Utility.GenerateInsertQuery(tableName: "Payroll_SalaryProcess", paramkeys: parameters.Select(x => x.Key).ToList(), output: "OUTPUT INSERTED.*");
                    //                var insertedSalaryProcess = await connection.QueryFirstOrDefaultAsync<SalaryProcess>(query, parameters, transaction);

                    //                if (insertedSalaryProcess != null && insertedSalaryProcess.SalaryProcessId > 0)
                    //                {
                    //                    foreach (var employeeSalary in salaryItems)
                    //                    {
                    //                        executionStatus.Ids = executionStatus.Ids + employeeSalary.SalaryProcessDetail.EmployeeId.ToString() + ",";
                    //                        query = "";
                    //                        parameters.Clear();

                    //                        employeeSalary.SalaryProcessDetail.SalaryProcessId = insertedSalaryProcess.SalaryProcessId;
                    //                        employeeSalary.SalaryProcessDetail.SalaryProcessUniqId = insertedSalaryProcess.SalaryProcessUniqId;
                    //                        parameters = Utility.DappperParamsInKeyValuePairs(employeeSalary.SalaryProcessDetail, user, addBaseProperty: true, addUserId: false, addCompany: false, addOrganization: false);
                    //                        parameters.Remove("SalaryProcessDetailId");
                    //                        parameters.Remove("SalaryProcess");

                    //                        query = Utility.GenerateInsertQuery(tableName: "Payroll_SalaryProcessDetail", paramkeys: parameters.Select(x => x.Key).ToList(),
                    //                            output: "OUTPUT INSERTED.*");
                    //                        var insertedSalaryProcessDetail = await connection.QueryFirstOrDefaultAsync<SalaryProcessDetail>(query, parameters, transaction);

                    //                        if (insertedSalaryProcessDetail != null && insertedSalaryProcessDetail.SalaryProcessDetailId > 0)
                    //                        {

                    //                            #region Salary Allowance
                    //                            List<Dictionary<string, dynamic>> parameterList = new();
                    //                            parameters.Clear();
                    //                            foreach (var item in employeeSalary.SalaryAllowances)
                    //                            {
                    //                                item.SalaryProcessId = insertedSalaryProcess.SalaryProcessId;
                    //                                item.SalaryProcessDetailId = insertedSalaryProcessDetail.SalaryProcessDetailId;
                    //                                parameters = Utility.DappperParamsInKeyValuePairs(item, appUser: null, addBaseProperty: true,
                    //                                    addUserId: false, addCompany: false, addOrganization: false);
                    //                                parameters.Remove("SalaryAllowanceId");
                    //                                parameterList.Add(parameters);
                    //                            }
                    //                            query = Utility.GenerateInsertQuery(tableName: "Payroll_SalaryAllowance", paramkeys: parameters.Select(x => x.Key).ToList(), output: "OUTPUT INSERTED.*");
                    //                            int rawAffectedInSalaryAllowance = await connection.ExecuteAsync(query, parameterList, transaction);
                    //                            if (rawAffectedInSalaryAllowance != employeeSalary.SalaryAllowances.Count)
                    //                            {
                    //                                isItemInsertFail = true;
                    //                                executionStatus.Status = false;
                    //                                executionStatus.Msg = "Something went wrong";
                    //                                executionStatus.ErrorMsg = "Failed to save in salary allowance and employee id : " + employeeSalary.SalaryProcessDetail.EmployeeId.ToString();
                    //                                break;
                    //                            }
                    //                            #endregion

                    //                            #region Salary Allowance Arrear
                    //                            parameterList.Clear();
                    //                            parameters.Clear();
                    //                            foreach (var item in employeeSalary.SalaryAllowanceArrears)
                    //                            {
                    //                                item.SalaryProcessId = insertedSalaryProcess.SalaryProcessId;
                    //                                item.SalaryProcessDetailId = insertedSalaryProcessDetail.SalaryProcessDetailId;
                    //                                parameters = Utility.DappperParamsInKeyValuePairs(item, appUser: null, addBaseProperty: true,
                    //                                  addUserId: false, addCompany: false, addOrganization: false);
                    //                                parameters.Remove("SalaryAllowanceArrearId");
                    //                                parameterList.Add(parameters);
                    //                            }
                    //                            query = Utility.GenerateInsertQuery(tableName: "Payroll_SalaryAllowanceArrear", paramkeys: parameters.Select(x => x.Key).ToList(), output: "OUTPUT INSERTED.*");
                    //                            int rawAffectedInSalaryAllowanceArrear = await connection.ExecuteAsync(query, parameterList, transaction);
                    //                            if (rawAffectedInSalaryAllowanceArrear != employeeSalary.SalaryAllowanceArrears.Count)
                    //                            {
                    //                                isItemInsertFail = true;
                    //                                executionStatus.Status = false;
                    //                                executionStatus.Msg = "Something went wrong";
                    //                                executionStatus.ErrorMsg = "Failed to save in salary allowance arrear and employee id : " + employeeSalary.SalaryProcessDetail.EmployeeId.ToString();
                    //                                break;
                    //                            }
                    //                            #endregion

                    //                            #region Salary Allowance Adjustment
                    //                            parameterList.Clear();
                    //                            parameters.Clear();
                    //                            foreach (var item in employeeSalary.SalaryAllowanceAdjustments)
                    //                            {
                    //                                item.SalaryProcessId = insertedSalaryProcess.SalaryProcessId;
                    //                                item.SalaryProcessDetailId = insertedSalaryProcessDetail.SalaryProcessDetailId;
                    //                                parameters = Utility.DappperParamsInKeyValuePairs(item, appUser: null, addBaseProperty: true,
                    //                                  addUserId: false, addCompany: false, addOrganization: false);
                    //                                parameters.Remove("AllowanceAdjustmentId");
                    //                                parameterList.Add(parameters);
                    //                            }
                    //                            query = Utility.GenerateInsertQuery(tableName: "Payroll_SalaryAllowanceAdjustment", paramkeys: parameters.Select(x => x.Key).ToList(), output: "OUTPUT INSERTED.*");
                    //                            int rawAffectedInSalaryAllowanceAdj = await connection.ExecuteAsync(query, parameterList, transaction);
                    //                            if (rawAffectedInSalaryAllowanceAdj != employeeSalary.SalaryAllowanceAdjustments.Count)
                    //                            {
                    //                                isItemInsertFail = true;
                    //                                executionStatus.Status = false;
                    //                                executionStatus.Msg = "Something went wrong";
                    //                                executionStatus.ErrorMsg = "Failed to save in salary allowance adjustment and employee id : " + employeeSalary.SalaryProcessDetail.EmployeeId.ToString();
                    //                                break;
                    //                            }
                    //                            #endregion

                    //                            #region Salary Deduction
                    //                            parameterList.Clear();
                    //                            parameters.Clear();
                    //                            foreach (var item in employeeSalary.SalaryDeductions)
                    //                            {
                    //                                item.SalaryProcessId = insertedSalaryProcess.SalaryProcessId;
                    //                                item.SalaryProcessDetailId = insertedSalaryProcessDetail.SalaryProcessDetailId;
                    //                                parameters = Utility.DappperParamsInKeyValuePairs(item, appUser: null, addBaseProperty: true,
                    //                                 addUserId: false, addCompany: false, addOrganization: false);
                    //                                parameters.Remove("SalaryDeductionId");
                    //                                parameterList.Add(parameters);
                    //                            }
                    //                            query = Utility.GenerateInsertQuery(tableName: "Payroll_SalaryDeduction", paramkeys: parameters.Select(x => x.Key).ToList(), output: "OUTPUT INSERTED.*");
                    //                            int rawAffectedInSalaryDeduction = await connection.ExecuteAsync(query, parameterList, transaction);
                    //                            if (rawAffectedInSalaryDeduction != employeeSalary.SalaryDeductions.Count)
                    //                            {
                    //                                isItemInsertFail = true;
                    //                                executionStatus.Status = false;
                    //                                executionStatus.Msg = "Something went wrong";
                    //                                executionStatus.ErrorMsg = "Failed to save in salary deduction and employee id : " + employeeSalary.SalaryProcessDetail.EmployeeId.ToString();
                    //                                break;
                    //                            }
                    //                            #endregion

                    //                            #region Salary Deduction Adjustment
                    //                            parameterList.Clear();
                    //                            parameters.Clear();
                    //                            foreach (var item in employeeSalary.SalaryDeductionAdjustments)
                    //                            {
                    //                                item.SalaryProcessId = insertedSalaryProcess.SalaryProcessId;
                    //                                item.SalaryProcessDetailId = insertedSalaryProcessDetail.SalaryProcessDetailId;
                    //                                parameters = Utility.DappperParamsInKeyValuePairs(item, appUser: null, addBaseProperty: true,
                    //                                addUserId: false, addCompany: false, addOrganization: false);
                    //                                parameters.Remove("DeductionAdjustmentId");
                    //                                parameterList.Add(parameters);
                    //                            }
                    //                            query = Utility.GenerateInsertQuery(tableName: "Payroll_SalaryDeductionAdjustment", paramkeys: parameters.Select(x => x.Key).ToList(), output: "OUTPUT INSERTED.*");
                    //                            int rawAffectedInSalaryDeductionAdjustment = await connection.ExecuteAsync(query, parameterList, transaction);
                    //                            if (rawAffectedInSalaryDeductionAdjustment != employeeSalary.SalaryDeductionAdjustments.Count)
                    //                            {
                    //                                isItemInsertFail = true;
                    //                                executionStatus.Status = false;
                    //                                executionStatus.Msg = "Something went wrong";
                    //                                executionStatus.ErrorMsg = "Failed to save in salary deduction adjustment and employee id : " + employeeSalary.SalaryProcessDetail.EmployeeId.ToString();
                    //                                break;
                    //                            }
                    //                            #endregion

                    //                            #region Deposit Allowance History
                    //                            parameterList.Clear();
                    //                            parameters.Clear();
                    //                            foreach (var item in employeeSalary.DepositAllowanceHistories)
                    //                            {
                    //                                item.SalaryProcessId = insertedSalaryProcess.SalaryProcessId;
                    //                                item.SalaryProcessDetailId = insertedSalaryProcessDetail.SalaryProcessDetailId;
                    //                                parameters = Utility.DappperParamsInKeyValuePairs(item, appUser: null, addBaseProperty: true,
                    //                                addUserId: false, addCompany: false, addOrganization: false);
                    //                                parameters.Remove("Id");
                    //                                parameterList.Add(parameters);
                    //                            }
                    //                            query = Utility.GenerateInsertQuery(tableName: "Payroll_DepositAllowanceHistory", paramkeys: parameters.Select(x => x.Key).ToList(), output: "OUTPUT INSERTED.*");
                    //                            int rawAffectedInDepositAllowanceHistory = await connection.ExecuteAsync(query, parameterList, transaction);
                    //                            if (rawAffectedInDepositAllowanceHistory != employeeSalary.DepositAllowanceHistories.Count)
                    //                            {
                    //                                isItemInsertFail = true;
                    //                                executionStatus.Status = false;
                    //                                executionStatus.Msg = "Something went wrong";
                    //                                executionStatus.ErrorMsg = "Failed to save in deposit allowance and employee id : " + employeeSalary.SalaryProcessDetail.EmployeeId.ToString();
                    //                                break;
                    //                            }
                    //                            #endregion

                    //                            #region Deposite Allowance Payment History
                    //                            parameterList.Clear();
                    //                            parameters.Clear();
                    //                            foreach (var item in employeeSalary.DepositAllowancePaymentHistories)
                    //                            {
                    //                                parameters.Clear();
                    //                                item.SalaryProcessId = insertedSalaryProcess.SalaryProcessId;
                    //                                item.SalaryProcessDetailId = insertedSalaryProcessDetail.SalaryProcessDetailId;
                    //                                item.UpdatedBy = user.ActionUserId;
                    //                                item.UpdatedDate = DateTime.Now;
                    //                                parameters = Utility.DappperParamsInKeyValuePairs(item, appUser: null, addBaseProperty: true,
                    //                                addUserId: false, addCompany: false, addOrganization: false);
                    //                                parameters.Remove("Id");
                    //                                query = Utility.GenerateUpdateQuery(tableName: "Payroll_DepositAllowancePaymentHistory", paramkeys: parameters.Select(x => x.Key).ToList());
                    //                                query += $"WHERE Id = @Id";
                    //                                parameters.Add("Id", item.Id);

                    //                                int rawAffectedInDepositAllowancePaymentHistory = await connection.ExecuteAsync(query, parameters, transaction);
                    //                                if (rawAffectedInDepositAllowancePaymentHistory < 0)
                    //                                {
                    //                                    isItemInsertFail = true;
                    //                                    executionStatus.Status = false;
                    //                                    executionStatus.Msg = "Something went wrong";
                    //                                    executionStatus.ErrorMsg = "Failed to save in deposit allowance payment and employee id : " + employeeSalary.SalaryProcessDetail.EmployeeId.ToString();
                    //                                    break;
                    //                                }
                    //                            }
                    //                            #endregion

                    //                            #region Service Anniversary Allowance  
                    //                            parameterList.Clear();
                    //                            parameters.Clear();
                    //                            foreach (var item in employeeSalary.RecipientsofServiceAnniversaryAllowances)
                    //                            {
                    //                                item.SalaryProcessId = insertedSalaryProcess.SalaryProcessId;
                    //                                item.SalaryProcessDetailId = insertedSalaryProcessDetail.SalaryProcessDetailId;
                    //                                parameters = Utility.DappperParamsInKeyValuePairs(item, appUser: null, addBaseProperty: true,
                    //                                  addUserId: false, addCompany: false, addOrganization: false);
                    //                                parameters.Remove("Id");
                    //                                parameterList.Add(parameters);
                    //                            }
                    //                            query = Utility.GenerateInsertQuery(tableName: "Payroll_RecipientsofServiceAnniversaryAllowance", paramkeys: parameters.Select(x => x.Key).ToList(), output: "OUTPUT INSERTED.*");
                    //                            int rawRecipientsofServiceAnniversaryAllowances = await connection.ExecuteAsync(query, parameterList, transaction);
                    //                            if (rawRecipientsofServiceAnniversaryAllowances != employeeSalary.RecipientsofServiceAnniversaryAllowances.Count)
                    //                            {
                    //                                isItemInsertFail = true;
                    //                                executionStatus.Status = false;
                    //                                executionStatus.Msg = "Something went wrong";
                    //                                executionStatus.ErrorMsg = "Failed to save in service anniversary arrear and employee id : " + employeeSalary.SalaryProcessDetail.EmployeeId.ToString();
                    //                                break;
                    //                            }
                    //                            #endregion

                    //                            #region Salary Components
                    //                            if (isItemInsertFail == false)
                    //                            {
                    //                                parameterList.Clear();
                    //                                parameters.Clear();
                    //                                foreach (var item in employeeSalary.SalaryComponentHistories)
                    //                                {
                    //                                    item.SalaryProcessId = insertedSalaryProcess.SalaryProcessId;
                    //                                    item.SalaryProcessDetailId = insertedSalaryProcessDetail.SalaryProcessDetailId;
                    //                                    item.CreatedDate = DateTime.Now;
                    //                                    parameters = Utility.DappperParamsInKeyValuePairs(item, appUser: null, addBaseProperty: true,
                    //                                        addUserId: false, addCompany: false, addOrganization: false);
                    //                                    parameters.Remove("Id");
                    //                                    parameterList.Add(parameters);
                    //                                }
                    //                                query = Utility.GenerateInsertQuery(tableName: "Payroll_SalaryComponentHistory", paramkeys: parameters.Select(x => x.Key).ToList(), output: "OUTPUT INSERTED.*");
                    //                                int rawAffectedInSalaryComponents = await connection.ExecuteAsync(query, parameterList, transaction);
                    //                                if (rawAffectedInSalaryComponents != employeeSalary.SalaryComponentHistories.Count)
                    //                                {
                    //                                    isItemInsertFail = true;
                    //                                    executionStatus.Status = false;
                    //                                    executionStatus.Msg = "Something went wrong";
                    //                                    executionStatus.ErrorMsg = "Failed to save in salary component histories and employee id : " + employeeSalary.SalaryProcessDetail.EmployeeId.ToString();
                    //                                    break;

                    //                                }
                    //                            }
                    //                            #endregion
                    //                        }
                    //                        else
                    //                        {
                    //                            isItemInsertFail = true;
                    //                            executionStatus.Status = false;
                    //                            executionStatus.Msg = "Something went wrong";
                    //                            executionStatus.ErrorMsg = "Failed to save in salary process detail and employee id : " + employeeSalary.SalaryProcessDetail.EmployeeId.ToString();
                    //                            break;
                    //                        }
                    //                    }
                    //                }
                    //                else
                    //                {
                    //                    isItemInsertFail = true;
                    //                    executionStatus.Status = false;
                    //                    executionStatus.Msg = "Something went wrong";
                    //                    executionStatus.ErrorMsg = "Failed to save in salary process info";
                    //                }

                    //                if (isItemInsertFail == false)
                    //                {
                    //                    transaction.Commit();
                    //                    executionStatus.Status = true;
                    //                    executionStatus.Msg = "Process has been done";
                    //                }
                    //            }
                    //            catch (Exception ex)
                    //            {
                    //                transaction.Rollback();
                    //                executionStatus = Utility.Invalid(ex.InnerException != null ? ex.InnerException.Message : ex.Message);
                    //                await _sysLogger.SaveHRMSException(ex, user.Database, "ExecuteSalaryProcess", "SaveSalaryAsync", user);
                    //            }
                    //            finally { connection.Close(); }
                    //        }
                    //    }
                    //} 
                    #endregion
                }
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ExecuteSalaryProcess", "SaveSalaryAsync", user);
            }
            return executionStatus;
        }
        public async Task<ExecutionStatus> UpdateSalaryAsync(string processType, long salaryProcessId, long salaryProcessDetailId, int salaryMonth, int salaryYear, EmployeeSalaryProcessedInfo salaryItem, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                var salaryProcess = await _salaryProcessBusiness.GetSalaryProcessByIdAsync(salaryProcessId, user);
                using (var connection = await _dapper.SqlConnectionForTransactionalAsync(user.Database))
                {
                    bool isItemInsertFail = true;
                    connection.Open();
                    {
                        using (var transaction = connection.BeginTransaction())
                        {
                            try
                            {
                                salaryProcess.TotalEmployees = salaryProcess.TotalEmployees + 1;
                                salaryProcess.TotalAllowance = salaryProcess.TotalAllowance + salaryItem.SalaryAllowances.Sum(i => i.Amount);
                                salaryProcess.TotalArrearAllowance = salaryProcess.TotalArrearAllowance + salaryItem.SalaryAllowanceArrears.Sum(i => i.Amount);
                                salaryProcess.TotalAllowanceAdjustment = salaryProcess.TotalAllowanceAdjustment + salaryItem.SalaryAllowanceAdjustments.Sum(i => i.Amount);
                                salaryProcess.TotalDeduction = salaryProcess.TotalDeduction + salaryItem.SalaryDeductions.Sum(i => i.Amount);
                                salaryProcess.TotalPFAmount = salaryProcess.TotalPFAmount + salaryItem.SalaryProcessDetail.PFAmount;
                                salaryProcess.TotalPFArrear = salaryProcess.TotalPFArrear + salaryItem.SalaryProcessDetail.PFArrear;
                                salaryProcess.TotalHoldAmount = (salaryProcess.TotalHoldAmount ?? 0) + salaryItem.SalaryProcessDetail.HoldAmount;
                                salaryProcess.TotalHoldDays = (salaryProcess.TotalHoldDays ?? 0) + salaryItem.SalaryProcessDetail.HoldDays ?? 0;
                                salaryProcess.TotalUnholdAmount = salaryProcess.TotalUnholdAmount + salaryItem.SalaryProcessDetail.UnholdAmount;
                                salaryProcess.TotalUnholdDays = (salaryProcess.TotalUnholdDays ?? 0) + salaryItem.SalaryProcessDetail.UnholdDays ?? 0;
                                salaryProcess.TotalGrossPay = salaryProcess.TotalAllowance + salaryProcess.TotalArrearAllowance;

                                salaryProcess.TotalNetPay = salaryProcess.TotalNetPay + salaryItem.SalaryProcessDetail.NetPay;
                                salaryProcess.UpdatedBy = user.ActionUserId;
                                salaryProcess.UpdatedDate = DateTime.Now;

                                // Salary Process Info Insert 
                                var parameters = Utility.DappperParamsInKeyValuePairs(salaryProcess, user, addBaseProperty: true, addUserId: false, addCompany: false, addOrganization: false);
                                parameters.Remove("SalaryProcessId");
                                parameters.Remove("SalaryProcessDetails");
                                string query = Utility.GenerateUpdateQuery(tableName: "Payroll_SalaryProcess", paramkeys: parameters.Select(x => x.Key).ToList());
                                query += $"WHERE SalaryProcessId = @SalaryProcessId";
                                parameters.Add("SalaryProcessId", salaryProcess.SalaryProcessId);
                                int rawAffected = await connection.ExecuteAsync(query, parameters, transaction);

                                if (rawAffected > 0)
                                {
                                    isItemInsertFail = false;

                                    query = "";
                                    parameters.Clear();

                                    salaryItem.SalaryProcessDetail.SalaryProcessId = salaryProcess.SalaryProcessId;
                                    salaryItem.SalaryProcessDetail.SalaryProcessUniqId = salaryProcess.SalaryProcessUniqId;
                                    parameters = Utility.DappperParamsInKeyValuePairs(salaryItem.SalaryProcessDetail, user, addBaseProperty: true, addUserId: false, addCompany: false, addOrganization: false);
                                    parameters.Remove("SalaryProcessDetailId");
                                    parameters.Remove("SalaryProcess");

                                    query = Utility.GenerateInsertQuery(tableName: "Payroll_SalaryProcessDetail", paramkeys: parameters.Select(x => x.Key).ToList(),
                                        output: "OUTPUT INSERTED.*");
                                    var insertedSalaryProcessDetail = await connection.QueryFirstOrDefaultAsync<SalaryProcessDetail>(query, parameters, transaction);

                                    if (insertedSalaryProcessDetail != null && insertedSalaryProcessDetail.SalaryProcessDetailId > 0)
                                    {
                                        #region Salary Allowance
                                        List<Dictionary<string, dynamic>> parameterList = new();
                                        parameters.Clear();

                                        foreach (var item in salaryItem.SalaryAllowances)
                                        {
                                            item.SalaryProcessId = salaryProcessId;
                                            item.SalaryProcessDetailId = insertedSalaryProcessDetail.SalaryProcessDetailId;
                                            parameters = Utility.DappperParamsInKeyValuePairs(item, appUser: null, addBaseProperty: true,
                                                addUserId: false, addCompany: false, addOrganization: false);
                                            parameters.Remove("SalaryAllowanceId");
                                            parameterList.Add(parameters);
                                        }
                                        query = Utility.GenerateInsertQuery(tableName: "Payroll_SalaryAllowance", paramkeys: parameters.Select(x => x.Key).ToList(), output: "OUTPUT INSERTED.*");
                                        int rawAffectedInSalaryAllowance = await connection.ExecuteAsync(query, parameterList, transaction);
                                        if (rawAffectedInSalaryAllowance != salaryItem.SalaryAllowances.Count)
                                        {
                                            executionStatus.Status = false;
                                            executionStatus.Msg = "Something went wrong";
                                            executionStatus.ErrorMsg = "Failed to save in salary allowance and employee id : " + salaryItem.SalaryProcessDetail.EmployeeId.ToString();
                                            isItemInsertFail = true;
                                        }

                                        #endregion

                                        #region Salary Allowance Arrear
                                        if (isItemInsertFail == false)
                                        {
                                            parameterList.Clear();
                                            parameters.Clear();
                                            foreach (var item in salaryItem.SalaryAllowanceArrears)
                                            {
                                                item.SalaryProcessId = salaryProcessId;
                                                item.SalaryProcessDetailId = insertedSalaryProcessDetail.SalaryProcessDetailId;
                                                parameters = Utility.DappperParamsInKeyValuePairs(item, appUser: null, addBaseProperty: true,
                                                  addUserId: false, addCompany: false, addOrganization: false);
                                                parameters.Remove("SalaryAllowanceArrearId");
                                                parameterList.Add(parameters);
                                            }
                                            query = Utility.GenerateInsertQuery(tableName: "Payroll_SalaryAllowanceArrear", paramkeys: parameters.Select(x => x.Key).ToList(), output: "OUTPUT INSERTED.*");
                                            int rawAffectedInSalaryAllowanceArrear = await connection.ExecuteAsync(query, parameterList, transaction);
                                            if (rawAffectedInSalaryAllowanceArrear != salaryItem.SalaryAllowanceArrears.Count)
                                            {
                                                executionStatus.Status = false;
                                                executionStatus.Msg = "Something went wrong";
                                                executionStatus.ErrorMsg = "Failed to save in salary allowance arrear and employee id : " + salaryItem.SalaryProcessDetail.EmployeeId.ToString();
                                            }
                                        }
                                        #endregion

                                        #region Salary Allowance Adjustment
                                        if (isItemInsertFail == false)
                                        {
                                            parameterList.Clear();
                                            parameters.Clear();
                                            foreach (var item in salaryItem.SalaryAllowanceAdjustments)
                                            {
                                                item.SalaryProcessId = salaryProcessId;
                                                item.SalaryProcessDetailId = insertedSalaryProcessDetail.SalaryProcessDetailId;
                                                parameters = Utility.DappperParamsInKeyValuePairs(item, appUser: null, addBaseProperty: true,
                                                  addUserId: false, addCompany: false, addOrganization: false);
                                                parameters.Remove("AllowanceAdjustmentId");
                                                parameterList.Add(parameters);
                                            }
                                            query = Utility.GenerateInsertQuery(tableName: "Payroll_SalaryAllowanceAdjustment", paramkeys: parameters.Select(x => x.Key).ToList(), output: "OUTPUT INSERTED.*");
                                            int rawAffectedInSalaryAllowanceAdj = await connection.ExecuteAsync(query, parameterList, transaction);
                                            if (rawAffectedInSalaryAllowanceAdj != salaryItem.SalaryAllowanceAdjustments.Count)
                                            {
                                                executionStatus.Status = false;
                                                executionStatus.Msg = "Something went wrong";
                                                executionStatus.ErrorMsg = "Failed to save in salary allowance adjustment and employee id : " + salaryItem.SalaryProcessDetail.EmployeeId.ToString();
                                            }
                                        }
                                        #endregion

                                        #region Salary Deduction
                                        if (isItemInsertFail == false)
                                        {
                                            parameterList.Clear();
                                            parameters.Clear();
                                            foreach (var item in salaryItem.SalaryDeductions)
                                            {
                                                item.SalaryProcessId = salaryProcessId;
                                                item.SalaryProcessDetailId = insertedSalaryProcessDetail.SalaryProcessDetailId;
                                                parameters = Utility.DappperParamsInKeyValuePairs(item, appUser: null, addBaseProperty: true,
                                                 addUserId: false, addCompany: false, addOrganization: false);
                                                parameters.Remove("SalaryDeductionId");
                                                parameterList.Add(parameters);
                                            }
                                            query = Utility.GenerateInsertQuery(tableName: "Payroll_SalaryDeduction", paramkeys: parameters.Select(x => x.Key).ToList(), output: "OUTPUT INSERTED.*");
                                            int rawAffectedInSalaryDeduction = await connection.ExecuteAsync(query, parameterList, transaction);
                                            if (rawAffectedInSalaryDeduction != salaryItem.SalaryDeductions.Count)
                                            {
                                                executionStatus.Status = false;
                                                executionStatus.Msg = "Something went wrong";
                                                executionStatus.ErrorMsg = "Failed to save in salary deduction and employee id : " + salaryItem.SalaryProcessDetail.EmployeeId.ToString();
                                            }
                                        }
                                        #endregion

                                        #region Salary Deduction Adjustment
                                        if (isItemInsertFail == false)
                                        {
                                            parameterList.Clear();
                                            parameters.Clear();
                                            foreach (var item in salaryItem.SalaryDeductionAdjustments)
                                            {
                                                item.SalaryProcessId = salaryProcessId;
                                                item.SalaryProcessDetailId = insertedSalaryProcessDetail.SalaryProcessDetailId;
                                                parameters = Utility.DappperParamsInKeyValuePairs(item, appUser: null, addBaseProperty: true,
                                                addUserId: false, addCompany: false, addOrganization: false);
                                                parameters.Remove("DeductionAdjustmentId");
                                                parameterList.Add(parameters);
                                            }
                                            query = Utility.GenerateInsertQuery(tableName: "Payroll_SalaryDeductionAdjustment", paramkeys: parameters.Select(x => x.Key).ToList(), output: "OUTPUT INSERTED.*");
                                            int rawAffectedInSalaryDeductionAdjustment = await connection.ExecuteAsync(query, parameterList, transaction);
                                            if (rawAffectedInSalaryDeductionAdjustment != salaryItem.SalaryDeductionAdjustments.Count)
                                            {
                                                executionStatus.Status = false;
                                                executionStatus.Msg = "Something went wrong";
                                                executionStatus.ErrorMsg = "Failed to save in salary deduction adjustment and employee id : " + salaryItem.SalaryProcessDetail.EmployeeId.ToString();
                                            }
                                        }
                                        #endregion

                                        #region Deposit Allowance History
                                        if (isItemInsertFail == false)
                                        {
                                            parameterList.Clear();
                                            parameters.Clear();
                                            foreach (var item in salaryItem.DepositAllowanceHistories)
                                            {
                                                item.SalaryProcessId = salaryProcessId;
                                                item.SalaryProcessDetailId = insertedSalaryProcessDetail.SalaryProcessDetailId;
                                                parameters = Utility.DappperParamsInKeyValuePairs(item, appUser: null, addBaseProperty: true,
                                                addUserId: false, addCompany: false, addOrganization: false);
                                                parameters.Remove("Id");
                                                parameterList.Add(parameters);
                                            }
                                            query = Utility.GenerateInsertQuery(tableName: "Payroll_DepositAllowanceHistory", paramkeys: parameters.Select(x => x.Key).ToList(), output: "OUTPUT INSERTED.*");
                                            int rawAffectedInDepositAllowanceHistory = await connection.ExecuteAsync(query, parameterList, transaction);
                                            if (rawAffectedInDepositAllowanceHistory != salaryItem.DepositAllowanceHistories.Count)
                                            {
                                                executionStatus.Status = false;
                                                executionStatus.Msg = "Something went wrong";
                                                executionStatus.ErrorMsg = "Failed to save in deposit allowance and employee id : " + salaryItem.SalaryProcessDetail.EmployeeId.ToString();
                                            }
                                        }
                                        #endregion

                                        #region Deposite Allowance Payment History
                                        if (isItemInsertFail == false)
                                        {
                                            parameterList.Clear();
                                            parameters.Clear();
                                            foreach (var item in salaryItem.DepositAllowancePaymentHistories)
                                            {
                                                parameters.Clear();
                                                item.SalaryProcessId = salaryProcessId;
                                                item.SalaryProcessDetailId = insertedSalaryProcessDetail.SalaryProcessDetailId;
                                                item.UpdatedBy = user.ActionUserId;
                                                item.UpdatedDate = DateTime.Now;
                                                parameters = Utility.DappperParamsInKeyValuePairs(item, appUser: null, addBaseProperty: true,
                                                addUserId: false, addCompany: false, addOrganization: false);
                                                parameters.Remove("Id");
                                                query = Utility.GenerateUpdateQuery(tableName: "Payroll_DepositAllowancePaymentHistory", paramkeys: parameters.Select(x => x.Key).ToList());
                                                query += $"WHERE Id = @Id";
                                                parameters.Add("Id", item.Id);

                                                int rawAffectedInDepositAllowancePaymentHistory = await connection.ExecuteAsync(query, parameters, transaction);
                                                if (rawAffectedInDepositAllowancePaymentHistory < 0)
                                                {

                                                    executionStatus.Status = false;
                                                    executionStatus.Msg = "Something went wrong";
                                                    executionStatus.ErrorMsg = "Failed to save in deposit allowance payment and employee id : " + salaryItem.SalaryProcessDetail.EmployeeId.ToString();
                                                    break;
                                                }
                                            }
                                        }
                                        #endregion

                                        #region Service Anniversary Allowance  
                                        if (isItemInsertFail == false)
                                        {
                                            parameterList.Clear();
                                            parameters.Clear();
                                            foreach (var item in salaryItem.RecipientsofServiceAnniversaryAllowances)
                                            {
                                                item.SalaryProcessId = salaryProcessId;
                                                item.SalaryProcessDetailId = insertedSalaryProcessDetail.SalaryProcessDetailId;
                                                parameters = Utility.DappperParamsInKeyValuePairs(item, appUser: null, addBaseProperty: true,
                                                  addUserId: false, addCompany: false, addOrganization: false);
                                                parameters.Remove("Id");
                                                parameterList.Add(parameters);
                                            }
                                            query = Utility.GenerateInsertQuery(tableName: "Payroll_RecipientsofServiceAnniversaryAllowance", paramkeys: parameters.Select(x => x.Key).ToList(), output: "OUTPUT INSERTED.*");
                                            int rawRecipientsofServiceAnniversaryAllowances = await connection.ExecuteAsync(query, parameterList, transaction);
                                            if (rawRecipientsofServiceAnniversaryAllowances != salaryItem.RecipientsofServiceAnniversaryAllowances.Count)
                                            {
                                                executionStatus.Status = false;
                                                executionStatus.Msg = "Something went wrong";
                                                executionStatus.ErrorMsg = "Failed to save in service anniversary arrear and employee id : " + salaryItem.SalaryProcessDetail.EmployeeId.ToString();
                                            }
                                        }
                                        #endregion

                                        #region SalaryComponentHistory
                                        if (isItemInsertFail == false)
                                        {

                                        }
                                        #endregion
                                    }

                                    if (isItemInsertFail == false)
                                    {
                                        transaction.Commit();
                                        executionStatus.ItemId = insertedSalaryProcessDetail.SalaryProcessDetailId;
                                        executionStatus.Status = true;
                                        executionStatus.Msg = "Reprocess has been done";
                                    }

                                    else
                                    {
                                        isItemInsertFail = true;
                                        executionStatus = Utility.Invalid("Salary Process detail can not be updated");
                                    }
                                }
                                else
                                {
                                    executionStatus = Utility.Invalid("Salary Process can not be updated");
                                }
                            }
                            catch (Exception ex)
                            {
                                transaction.Rollback();
                                executionStatus = Utility.Invalid(ex.InnerException != null ? ex.InnerException.Message : ex.Message);
                                await _sysLogger.SaveHRMSException(ex, user.Database, "ExecuteSalaryProcess", "UpdateSalaryAsync", user);
                            }
                            finally
                            {
                                connection.Close();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ExecuteSalaryProcess", "UpdateSalaryAsync", user);
            }
            return executionStatus;
        }
        public bool IsItSpecialAllowanceOfPWCForTheMonthJune(string employeeCode, long allowanceId, int year, int month, AppUser user)
        {
            bool isTrue = false;

            string[] array = new string[] { "101268774", "101333987", "101431257", "101464590", "101503035", "101503038", "101503385", "101506477", "101509468", "101510280", "101512820", "101523399", "101536173", "101542330" };
            var list = array.ToList();

            if (user.CompanyId == 17 && user.OrganizationId == 10 && year == 2024 && month == 6)
            {
                if (list.Exists(i => i == employeeCode))
                {
                    isTrue = true;
                }
            }
            return isTrue;
        }
        public async Task<ExecutionStatus> DeleteSingleEmployeeSalaryAsync(SalaryReprocess reprocess, AppUser user)
        {
            ExecutionStatus executionStatus = null;
            try
            {
                var query = $@"SELECT * FROM Payroll_SalaryProcessDetail Where EmployeeId = @EmployeeId AND SalaryProcessId=@SalaryProcessId AND SalaryMonth=@SalaryMonth AND SalaryYear=@SalaryYear";
                var salaryProcessDetail = await _dapper.SqlQueryFirstAsync<SalaryProcessDetail>(user.Database, query, new
                {
                    reprocess.EmployeeId,
                    reprocess.SalaryProcessId,
                    SalaryMonth = reprocess.Month,
                    SalaryYear = reprocess.Year
                });

                query = $@"SELECT * FROM Payroll_SalaryProcess Where SalaryProcessId=@SalaryProcessId AND SalaryMonth=@SalaryMonth AND SalaryYear=@SalaryYear";
                var salaryProcess = await _dapper.SqlQueryFirstAsync<SalaryProcess>(user.Database, query, new
                {
                    reprocess.SalaryProcessId,
                    SalaryMonth = reprocess.Month,
                    SalaryYear = reprocess.Year
                });


                using (var connection = await _dapper.SqlConnectionForTransactionalAsync(user.Database))
                {
                    connection.Open();
                    using (var transaction = connection.BeginTransaction())
                    {
                        query = $@"DELETE FROM Payroll_SalaryProcessDetail Where EmployeeId = @EmployeeId AND  SalaryProcessId=@SalaryProcessId AND SalaryMonth=@SalaryMonth AND SalaryYear=@SalaryYear";
                        var rawDeleted = await connection.ExecuteAsync(query, new
                        {
                            reprocess.EmployeeId,
                            reprocess.SalaryProcessId,
                            SalaryMonth = reprocess.Month,
                            SalaryYear = reprocess.Year
                        }, transaction);

                        if (rawDeleted > 0)
                        {
                            query = $@"DELETE FROM Payroll_SalaryAllowance Where SalaryProcessId=@SalaryProcessId AND EmployeeId=@EmployeeId AND SalaryMonth=@SalaryMonth AND SalaryYear=@SalaryYear";
                            var allowanceDeleted = await connection.ExecuteAsync(query, new
                            {
                                reprocess.EmployeeId,
                                reprocess.SalaryProcessId,
                                SalaryMonth = reprocess.Month,
                                SalaryYear = reprocess.Year
                            }, transaction);

                            query = $@"DELETE FROM Payroll_SalaryAllowanceArrear Where SalaryProcessId=@SalaryProcessId AND EmployeeId=@EmployeeId AND SalaryMonth=@SalaryMonth AND SalaryYear=@SalaryYear";
                            var allowanceArrearDeleted = await connection.ExecuteAsync(query, new
                            {
                                reprocess.EmployeeId,
                                reprocess.SalaryProcessId,
                                SalaryMonth = reprocess.Month,
                                SalaryYear = reprocess.Year
                            }, transaction);

                            query = $@"DELETE FROM Payroll_SalaryAllowanceAdjustment Where SalaryProcessId=@SalaryProcessId AND EmployeeId=@EmployeeId AND SalaryMonth=@SalaryMonth AND SalaryYear=@SalaryYear";
                            var allowanceAdjustmentDeleted = await connection.ExecuteAsync(query, new
                            {
                                reprocess.EmployeeId,
                                reprocess.SalaryProcessId,
                                SalaryMonth = reprocess.Month,
                                SalaryYear = reprocess.Year
                            }, transaction);

                            query = $@"DELETE FROM Payroll_SalaryDeduction Where SalaryProcessId=@SalaryProcessId AND EmployeeId=@EmployeeId AND SalaryMonth=@SalaryMonth AND SalaryYear=@SalaryYear";
                            var deductionDeleted = await connection.ExecuteAsync(query, new
                            {
                                reprocess.EmployeeId,
                                reprocess.SalaryProcessId,
                                SalaryMonth = reprocess.Month,
                                SalaryYear = reprocess.Year
                            }, transaction);

                            query = $@"DELETE FROM Payroll_SalaryDeductionAdjustment Where SalaryProcessId=@SalaryProcessId AND EmployeeId=@EmployeeId AND SalaryMonth=@SalaryMonth AND SalaryYear=@SalaryYear";
                            var deductionAdjustmentDeleted = await connection.ExecuteAsync(query, new
                            {
                                reprocess.EmployeeId,
                                reprocess.SalaryProcessId,
                                SalaryMonth = reprocess.Month,
                                SalaryYear = reprocess.Year
                            }, transaction);


                            query = $@"DELETE FROM Payroll_DepositAllowanceHistory Where SalaryProcessId=@SalaryProcessId AND EmployeeId=@EmployeeId AND DepositMonth=@SalaryMonth AND DepositYear=@SalaryYear";
                            var depositAllowanceDeleted = await connection.ExecuteAsync(query, new
                            {
                                reprocess.EmployeeId,
                                reprocess.SalaryProcessId,
                                SalaryMonth = reprocess.Month,
                                SalaryYear = reprocess.Year
                            }, transaction);


                            query = $@"DELETE FROM Payroll_RecipientsofServiceAnniversaryAllowance Where SalaryProcessId=@SalaryProcessId AND EmployeeId=@EmployeeId AND PaymentMonth=@SalaryMonth AND PaymentYear=@SalaryYear";
                            var anniversaryAllowanceDeleted = await connection.ExecuteAsync(query, new
                            {
                                reprocess.EmployeeId,
                                reprocess.SalaryProcessId,
                                SalaryMonth = reprocess.Month,
                                SalaryYear = reprocess.Year
                            }, transaction);

                            query = $@"DELETE FROM Payroll_SalaryComponentHistory Where SalaryProcessId=@SalaryProcessId AND EmployeeId=@EmployeeId AND SalaryMonth=@SalaryMonth AND SalaryYear=@SalaryYear";
                            var salaryComponentHistoryDeleted = await connection.ExecuteAsync(query, new
                            {
                                reprocess.EmployeeId,
                                reprocess.SalaryProcessId,
                                SalaryMonth = reprocess.Month,
                                SalaryYear = reprocess.Year
                            }, transaction);

                            query = $@"DELETE FROM Payroll_MonthlyAllowanceHistory Where SalaryProcessId=@SalaryProcessId AND EmployeeId=@EmployeeId AND Month=@SalaryMonth AND Year=@SalaryYear";
                            var monthlyAllowanceHistoryDeleted = await connection.ExecuteAsync(query, new
                            {
                                reprocess.EmployeeId,
                                reprocess.SalaryProcessId,
                                SalaryMonth = reprocess.Month,
                                SalaryYear = reprocess.Year
                            }, transaction);

                            #region Delete TAX Information

                            query = $@"DELETE FROM Payroll_EmployeeTaxProcessSlab Where EmployeeId=@EmployeeId AND SalaryMonth=@SalaryMonth AND SalaryYear=@SalaryYear";
                            int count = await connection.ExecuteAsync(query, new
                            {
                                reprocess.EmployeeId,
                                SalaryMonth = reprocess.Month,
                                SalaryYear = reprocess.Year
                            }, transaction);

                            query = $@"DELETE FROM Payroll_EmployeeTaxProcessDetail Where EmployeeId=@EmployeeId AND SalaryMonth=@SalaryMonth AND SalaryYear=@SalaryYear";
                            count = await connection.ExecuteAsync(query, new
                            {
                                reprocess.EmployeeId,
                                SalaryMonth = reprocess.Month,
                                SalaryYear = reprocess.Year
                            }, transaction);

                            query = $@"DELETE FROM Payroll_EmployeeTaxProcess Where EmployeeId=@EmployeeId AND SalaryMonth=@SalaryMonth AND SalaryYear=@SalaryYear";
                            count = await connection.ExecuteAsync(query, new
                            {
                                reprocess.EmployeeId,
                                SalaryMonth = reprocess.Month,
                                SalaryYear = reprocess.Year
                            }, transaction);

                            #endregion


                            salaryProcess.TotalEmployees = salaryProcess.TotalEmployees - 1;
                            salaryProcess.TotalAllowance = salaryProcess.TotalAllowance - salaryProcessDetail.TotalAllowance;
                            salaryProcess.TotalArrearAllowance = salaryProcess.TotalArrearAllowance - salaryProcessDetail.TotalArrearAllowance;
                            salaryProcess.TotalPFAmount = salaryProcess.TotalPFAmount - salaryProcessDetail.PFAmount;
                            salaryProcess.TotalPFArrear = salaryProcess.TotalPFArrear - salaryProcessDetail.PFArrear;

                            if (user.CompanyId == 17 && user.OrganizationId == 10)
                            {
                                salaryProcess.TotalDeduction = salaryProcess.TotalDeduction - (salaryProcessDetail.TotalDeduction - salaryProcessDetail.TotalPFAmount ?? 0);
                            }
                            else
                            {
                                salaryProcess.TotalDeduction = salaryProcess.TotalDeduction - salaryProcessDetail.TotalDeduction;
                            }

                            salaryProcess.TotalMonthlyTax = salaryProcess.TotalMonthlyTax - salaryProcessDetail.TotalMonthlyTax;
                            salaryProcess.TotalProjectionTax = salaryProcess.TotalProjectionTax - salaryProcessDetail.ProjectionTax;
                            salaryProcess.TotalOnceOffTax = salaryProcess.TotalOnceOffTax - salaryProcessDetail.OnceOffTax;
                            salaryProcess.TotalTaxDeductedAmount = salaryProcess.TotalTaxDeductedAmount - salaryProcessDetail.TaxDeductedAmount;
                            salaryProcess.TotalHoldAmount = (salaryProcess.TotalHoldAmount ?? 0) - (salaryProcessDetail.HoldAmount ?? 0);
                            salaryProcess.TotalGrossPay = salaryProcess.TotalGrossPay - salaryProcessDetail.GrossPay;
                            salaryProcess.TotalNetPay = salaryProcess.TotalNetPay - salaryProcessDetail.NetPay;


                            var parameters = Utility.DappperParamsInKeyValuePairs(salaryProcess, user, addBaseProperty: true, addUserId: false, addCompany: false, addOrganization: false);
                            parameters.Remove("SalaryProcessId");
                            parameters.Remove("SalaryProcessDetails");
                            query = Utility.GenerateUpdateQuery(tableName: "Payroll_SalaryProcess", paramkeys: parameters.Select(x => x.Key).ToList());
                            query += $"WHERE SalaryProcessId = @SalaryProcessId";
                            parameters.Add("SalaryProcessId", salaryProcess.SalaryProcessId);
                            int rawAffected = await connection.ExecuteAsync(query, parameters, transaction);

                            if (rawAffected > 0)
                            {
                                transaction.Commit();
                                executionStatus = new ExecutionStatus();
                                executionStatus.Status = true;
                                executionStatus.Msg = salaryProcessDetail.TotalMonthlyTax > 0 ? "Has Tax Amount" : null;
                            }
                        }
                        else
                        {
                            executionStatus = new ExecutionStatus();
                            executionStatus.Status = false;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ExecuteSalaryProcess", "DeleteSingleEmployeeSalaryAsync", user);
            }
            return executionStatus;
        }
    }
}
