using System.Data;
using Shared.Services;
using BLL.Base.Interface;
using Shared.OtherModels.User;
using DAL.DapperObject.Interface;
using Shared.OtherModels.Response;
using BLL.Salary.Setup.Interface;
using BLL.Salary.Salary.Interface;
using DAL.Payroll.Repository.Interface;
using BLL.Salary.Allowance.Interface;
using BLL.Salary.Payment.Interface;
using Shared.Payroll.Process.Tax;
using Shared.Payroll.Filter.Tax;
using Shared.Payroll.Filter.Allowance;
using Shared.Payroll.Process.Allowance;
using Shared.Payroll.Domain.Setup;
using Shared.Control_Panel.Domain;
using Shared.Payroll.ViewModel.Salary;
using Shared.Payroll.Filter.Salary;
using Shared.Payroll.Domain.Payment;
using BLL.Tax.Interface;
using DAL.Context.Payroll;

namespace BLL.Tax.Implementation
{
    public class ExecuteTaxProcess : IExecuteTaxProcess
    {
        private readonly IDapperData _dapper;
        private readonly ISysLogger _sysLogger;
        private readonly IModuleConfig _moduleConfig;
        private readonly IFiscalYearBusiness _fiscalYearBusiness;
        private readonly ISalaryProcessBusiness _salaryProcessBusiness;
        private readonly ISalaryReviewBusiness _salaryReviewBusiness;
        private readonly IAllowanceNameBusiness _allowanceNameBusiness;
        private readonly IAllowanceConfigBusiness _allowanceConfigBusiness;
        private readonly IConditionalProjectedPaymentBusiness _conditionalProjectedPaymentBusiness;
        private readonly IProjectedPaymentBusiness _projectedPaymentBusiness;
        private readonly ITaxRulesBusiness _taxRulesBusiness;
        private readonly IConditionalDepositAllowanceConfigRepository _conditionalDepositAllowanceConfigRepository;
        private readonly ISupplementaryPaymentAmountBusiness _supplementaryPaymentAmountBusiness;
        private readonly ITaxSettingBusiness _taxSettingBusiness;
        private readonly PayrollDbContext _payrollDbContext;
        public ExecuteTaxProcess(
            IFiscalYearBusiness fiscalYearBusiness,
            IDapperData dapper,
            ISysLogger sysLogger,
            IModuleConfig moduleConfig,
            ISalaryProcessBusiness salaryProcessBusiness,
            ISalaryReviewBusiness salaryReviewBusiness,
            IAllowanceNameBusiness allowanceNameBusiness,
            IAllowanceConfigBusiness allowanceConfigBusiness,
            IConditionalProjectedPaymentBusiness conditionalProjectedPaymentBusiness,
            IProjectedPaymentBusiness projectedPaymentBusiness,
            ITaxRulesBusiness taxRulesBusiness,
            ITaxSettingBusiness taxSettingBusiness,
            PayrollDbContext payrollDbContext,
            IConditionalDepositAllowanceConfigRepository conditionalDepositAllowanceConfigRepository,
            ISupplementaryPaymentAmountBusiness supplementaryPaymentAmountBusiness)
        {
            _dapper = dapper;
            _sysLogger = sysLogger;
            _moduleConfig = moduleConfig;
            _fiscalYearBusiness = fiscalYearBusiness;
            _salaryProcessBusiness = salaryProcessBusiness;
            _salaryReviewBusiness = salaryReviewBusiness;
            _allowanceNameBusiness = allowanceNameBusiness;
            _allowanceConfigBusiness = allowanceConfigBusiness;
            _conditionalProjectedPaymentBusiness = conditionalProjectedPaymentBusiness;
            _projectedPaymentBusiness = projectedPaymentBusiness;
            _taxRulesBusiness = taxRulesBusiness;
            _taxSettingBusiness = taxSettingBusiness;
            _payrollDbContext = payrollDbContext;
            _conditionalDepositAllowanceConfigRepository = conditionalDepositAllowanceConfigRepository;
            _supplementaryPaymentAmountBusiness = supplementaryPaymentAmountBusiness;
        }
        public async Task<ExecutionStatus> SalaryTaxProcessAsync(TaxProcessExecution execution, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            List<EmployeeTaxProcessedInfo> employeeTaxProcessList = new List<EmployeeTaxProcessedInfo>();
            try
            {

                IEnumerable<EligibleEmployeeForTaxType> employees = await GetEligibleEmployeesAysnc("Salary", new EligibleEmployeeForTax_Filter()
                {
                    Month = execution.Month,
                    Year = execution.Year,
                    ProcessBranchId = execution.ProcessBranchId ?? 0,
                    ProcessDepartmentId = execution.ProcessDepartmentId ?? 0,
                    SelectedEmployees = execution.SelectedEmployees,
                }, user);
                var fiscalYearInfo = await _fiscalYearBusiness.GetFiscalYearInfoWithinADate(DateTimeExtension.FirstDateOfAMonth(execution.Year, execution.Month).ToString("yyyy-MM-dd"), user);
                var payrollModuleConfig = await _moduleConfig.PayrollModuleConfig(user);

                var allowanceInfo = await _allowanceNameBusiness.GetAllowanceInfos(user);

                foreach (var employee in employees)
                {
                    var taxDetail = await SalaryTaxDetailsAsync(fiscalYearInfo, payrollModuleConfig, employee, allowanceInfo, execution.Month, execution.Year, user);
                    if (taxDetail != null)
                    {
                        employeeTaxProcessList.Add(taxDetail);
                    }
                }
                if (employeeTaxProcessList.Any())
                {
                    executionStatus = await SaveTaxAsync(employeeTaxProcessList, fiscalYearInfo, execution.Month, execution.Year, execution.EffectOnSalary, user);
                }
                else
                {
                    executionStatus.Status = false;
                    executionStatus.Msg = "No items found to save";
                }
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ExecuteTaxProcess", "SalaryTaxProcess", user);
            }
            return executionStatus;
        }
        public async Task<IEnumerable<EligibleEmployeeForTaxType>> GetEligibleEmployeesAysnc(string incomingFlag, EligibleEmployeeForTax_Filter filter, AppUser user)
        {
            IEnumerable<EligibleEmployeeForTaxType> list = new List<EligibleEmployeeForTaxType>();
            try
            {
                if (incomingFlag != null)
                {
                    if (incomingFlag == "Salary")
                    {
                        var query = $@"Select ROW_NUMBER() Over(Order By emp.EmployeeId) SL,emp.EmployeeId,emp.EmployeeCode,emp.FullName,
spd.Grade,spd.GradeId,spd.Designation,spd.DesignationId,spd.InternalDesignation,spd.InternalDesignationId,
spd.Department,spd.DepartmentId,spd.Section,spd.SectionId,spd.SubSection,spd.SubsectionId,spd.Unit,spd.UnitId,spd.UnitId,spd.JobCategory,
spd.JobCategoryId,spd.CostCenter,spd.CostCenterId,spd.JobType,spd.BankId,spd.BankBranchId,spd.SalaryProcessId,spd.SalaryProcessDetailId,
spd.EmployeeType,spd.EmployeeTypeId,spd.SalaryReviewInfoIds,emp.CalculateFestivalBonusTaxProratedBasis,emp.CalculateProjectionTaxProratedBasis,
emp.TerminationDate,emp.TerminationStatus,spd.BranchId,spd.BranchName,
emp.DateOfJoining,emp.DateOfConfirmation,emp.PFActivationDate,
Emp.IsConfirmed,EMP.IsPFMember,Emp.MinimumTaxAmount,
dtl.IsFreedomFighter,
dtl.IsPhysicallyDisabled,
dtl.IsResidential,
dtl.Religion,
dtl.MaritalStatus,
IsDiscontinued=(CASE 
WHEN emp.TerminationDate IS NULL THEN 0
WHEN MONTH(emp.TerminationDate)  = @Month AND YEAR(emp.TerminationDate) = @Year THEN 1 ELSE 0 END), 
dtl.Gender,AgeYear=0,AgeMonth=0,AgeDay=0,
emp.EmployeeTypeId,
EmployeeType=ET.EmployeeTypeName
From HR_EmployeeInformation emp
Left Join HR_EmployeeDetail dtl On  emp.EmployeeId= dtl.EmployeeId 
Left Join HR_EmployeeType ET ON EMP.EmployeeTypeId = ET.EmployeeTypeId
INNER JOIN Payroll_SalaryProcessDetail spd on emp.EmployeeId= spd.EmployeeId
Where 1=1
AND (@ProcessBranchId IS NULL OR @ProcessBranchId = 0 OR spd.BranchId=@ProcessBranchId)
AND ((@SelectedEmployees ='' OR emp.EmployeeId IN (Select Convert( bigint,[Value]) from fn_split_string_to_column(@SelectedEmployees,',')))) 
AND spd.SalaryMonth=@Month 
AND spd.SalaryYear=@Year AND emp.CompanyId=@CompanyId AND emp.OrganizationId=@OrganizationId 
AND spd.EmployeeId NOT IN(SELECT EmployeeId FROM Payroll_EmployeeTaxProcess Where SalaryMonth=@Month AND SalaryYear=@Year)";
                        list = await _dapper.SqlQueryListAsync<EligibleEmployeeForTaxType>(user.Database, query, new { filter.SelectedEmployees, filter.ProcessBranchId, filter.Month, filter.Year, user.CompanyId, user.OrganizationId });
                    }
                    if (incomingFlag == "Tax")
                    {
                    }
                    if (incomingFlag == "Supplementary")
                    {

                    }
                }
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ExecuteTaxProcess", "GetEligibleEmployees", user);
            }
            return list;
        }
        public async Task<EmployeeTaxProcessedInfo> SalaryTaxDetailsAsync(FiscalYear fiscalYearInfo, PayrollModuleConfig payrollModuleConfig, EligibleEmployeeForTaxType employee, AllowanceInfo allowanceInfo, int month, int year, AppUser user)
        {
            EmployeeTaxProcessedInfo taxProcessInfo = new EmployeeTaxProcessedInfo();
            try
            {
                if (
                    fiscalYearInfo != null
                    && fiscalYearInfo.FiscalYearFrom.HasValue
                    && fiscalYearInfo.FiscalYearTo.HasValue
                    && employee.DateOfJoining.HasValue)
                {

                    employee.TotalServiceDays = DateTimeExtension.DaysBetweenDateRangeIncludingStartDate(employee.DateOfJoining.Value, DateTimeExtension.LastDateOfAMonth(year, month));

                    bool isThisEmployeeDiscontinuedWithinThisFiscalYear = false;
                    DateTime firstDateOfThisMonth = DateTimeExtension.FirstDateOfAMonth(year, month);
                    int remainFiscalYearMonth = firstDateOfThisMonth.GetMonthDiffExcludingThisMonth(fiscalYearInfo.FiscalYearTo.Value);

                    if (employee.TerminationDate.HasValue)
                    {
                        isThisEmployeeDiscontinuedWithinThisFiscalYear = employee.TerminationDate == null ? false : employee.TerminationDate.Value.IsDateBetweenTwoDates(fiscalYearInfo.FiscalYearFrom.Value.Date, fiscalYearInfo.FiscalYearTo.Value.Date);
                    }

                    int remainProjectionMonthForThisEmployee = 0;
                    if (employee.TerminationDate == null)
                    {
                        remainProjectionMonthForThisEmployee = remainFiscalYearMonth;
                    }
                    else if (employee.TerminationDate.HasValue && employee.TerminationStatus == "Approved")
                    {
                        var toDate = employee.TerminationDate.Value.IsDateBetweenTwoDates(firstDateOfThisMonth, fiscalYearInfo.FiscalYearTo.Value) ? employee.TerminationDate.Value : fiscalYearInfo.FiscalYearTo.Value;
                        remainProjectionMonthForThisEmployee = firstDateOfThisMonth.GetMonthDiffExcludingThisMonth(toDate);
                    }

                    int countOfSalaryReceiptInThisFiscalYear = await _salaryProcessBusiness.GetSalaryReceiptInThisFiscalYearAsync(employee.EmployeeId, fiscalYearInfo.FiscalYearId, month, user);
                    int sumOfRemainProjectionAndCountOfSalaryReceipt = remainProjectionMonthForThisEmployee + countOfSalaryReceiptInThisFiscalYear + 1;

                    var salaryDetails = await _salaryProcessBusiness.GetSalaryProcessDetailsAsync(0, 0, employee.EmployeeId, fiscalYearInfo.FiscalYearId, (short)month, (short)year, 0, "", user);
                    SalaryProcessDetailViewModel? salaryDetail = new SalaryProcessDetailViewModel();
                    if (salaryDetails.Any())
                    {
                        salaryDetail = salaryDetails.FirstOrDefault();
                    }

                    int lastSalaryReviewId = 0;
                    var salaryReviewIds = employee.SalaryReviewInfoIds;
                    if (salaryReviewIds != null)
                    {
                        var ids = salaryReviewIds.Split(',');
                        var intIds = ids.Select(id => Utility.TryParseInt(id.Trim()));
                        lastSalaryReviewId = intIds.Max();
                    }

                    var salaryReviewDetails = await _salaryReviewBusiness.GetSalaryReviewDetailsAsync(new SalaryReview_Filter()
                    {
                        EmployeeId = employee.EmployeeId.ToString(),
                        SalaryReviewInfoId = lastSalaryReviewId.ToString()
                    }, user);
                    var currentGross = salaryReviewDetails.Sum(i => i.CurrentAmount);
                    var currentBasic = salaryReviewDetails.Where(i => i.AllowanceNameId == allowanceInfo.BasicAllowance).Sum(i => i.CurrentAmount);

                    var pfAllowance = await _allowanceNameBusiness.GetAllowanceNamesAsync(new AllowanceName_Filter()
                    {
                        AllowanceFlag = "PF"
                    }, user);

                    var earnedAllowances = (await AllowancesEarnedByThisEmployeeByThisFiscalYearAsync(employee, fiscalYearInfo, month, year, lastSalaryReviewId, user)).Select(i => new TaxDetailInTaxProcess()
                    {
                        AllowanceNameId = i.AllowanceNameId,
                        AllowanceName = i.AllowanceName,
                        Flag = i.AllowanceFlag == "" || i.AllowanceFlag == null ? i.AllowanceName : i.AllowanceFlag,
                        ProjectRestYear = i.IsProjected,
                        OnceOffDeduction = i.IsOnceOff,
                        TillAmount = i.TillAmount,
                        CurrentAmount = i.CurrentAmount,
                        Amount = i.CurrentAmount - i.ArrearAmount,
                        Adjustment = 0,
                        Arrear = i.ArrearAmount,
                        ReviewAmount = salaryReviewDetails.Where(j => j.AllowanceNameId == i.AllowanceNameId).Sum(j => j.CurrentAmount) > 0 ? salaryReviewDetails.Where(j => j.AllowanceNameId == i.AllowanceNameId).Sum(j => j.CurrentAmount) : i.CurrentAmount + i.ArrearAmount,
                        RemainFiscalYearMonth = (short)remainProjectionMonthForThisEmployee,
                        ProjectedAmount = 0,
                        LessExemptedAmount = 0,
                        TotalTaxableIncome = 0,
                        GrossAnnualIncome = 0,
                        TillAdjustment = i.TillAdjustment,
                        CurrentAdjustment = i.CurrentAdjustment

                    }).ToList();

                    bool isCalculateProjectionTaxProratedBasis = false;

                    var taxSetting = await _taxSettingBusiness.GetTaxSettingByFiscalYearIdAsync(fiscalYearInfo.FiscalYearId, user);

                    var medicalAllowanceIndex = earnedAllowances.FindIndex(i => i.AllowanceNameId == allowanceInfo.MedicalAllowance);

                    var accuredAllowance = await _conditionalDepositAllowanceConfigRepository.GetEmployeeAllowanceAccuredAmountInTaxProcessAsync(employee.EmployeeId, allowanceInfo.MedicalAllowance, (short)year, (short)month, fiscalYearInfo.FiscalYearId, user);

                    #region Thompson - Permanent Employee Medical
                    if (user.OrganizationId == 11 && user.CompanyId == 19 && employee.JobType == "Permanent")
                    {

                        if (medicalAllowanceIndex < 0)
                        {
                            TaxDetailInTaxProcess taxDetailInTaxProcess = new TaxDetailInTaxProcess()
                            {
                                SL = earnedAllowances.Count + 1,
                                EmployeeId = employee.EmployeeId,
                                AllowanceNameId = allowanceInfo.MedicalAllowance,
                                AllowanceName = allowanceInfo.MedicalAllowanceName,
                                IsActive = true,
                                IsMonthly = true,
                                IsTaxable = true,
                                IsIndividual = false,
                                DepandsOnWorkingHour = null,
                                ProjectRestYear = true,
                                OnceOffDeduction = false,
                                AllowanceConfigId = 0,
                                Flag = "Medical",
                                TillAmount = remainProjectionMonthForThisEmployee == 0 ? accuredAllowance.TillAccuredAmount : 0,
                                CurrentAmount = remainProjectionMonthForThisEmployee == 0 ? accuredAllowance.ThisMonthAccuredAmount + accuredAllowance.ThisMonthAccuredArrear : 0,
                                Amount = 0,
                                Arrear = accuredAllowance.ThisMonthAccuredArrear,
                                ReviewAmount = Math.Round(currentGross * (decimal).04)
                            };

                            earnedAllowances.Add(taxDetailInTaxProcess);
                        }
                        else
                        {
                            earnedAllowances.ElementAt(medicalAllowanceIndex).TillAmount = earnedAllowances.ElementAt(medicalAllowanceIndex).TillAmount + (remainProjectionMonthForThisEmployee == 0 ? accuredAllowance.TillAccuredAmount : 0);

                            earnedAllowances.ElementAt(medicalAllowanceIndex).CurrentAmount = earnedAllowances.ElementAt(medicalAllowanceIndex).CurrentAmount + (remainProjectionMonthForThisEmployee == 0 ? accuredAllowance.ThisMonthAccuredAmount + accuredAllowance.ThisMonthAccuredArrear : 0);

                            earnedAllowances.ElementAt(medicalAllowanceIndex).Arrear = earnedAllowances.ElementAt(medicalAllowanceIndex).Arrear + (remainProjectionMonthForThisEmployee == 0 ? accuredAllowance.ThisMonthAccuredArrear : 0);

                            earnedAllowances.ElementAt(medicalAllowanceIndex).ReviewAmount = Math.Round(currentGross * (decimal).04);
                        }
                    }
                    #endregion

                    #region Thompson - Contractual Employee Medical
                    if (user.OrganizationId == 11 && user.CompanyId == 19 && employee.JobType == "Contractual")
                    {
                        decimal conveyanceAmount = employee.EmployeeType == "Manager" ? 5000 : 3500;
                        if (medicalAllowanceIndex < 0)
                        {
                            TaxDetailInTaxProcess taxDetailInTaxProcess = new TaxDetailInTaxProcess()
                            {
                                SL = earnedAllowances.Count + 1,
                                EmployeeId = employee.EmployeeId,
                                AllowanceNameId = allowanceInfo.MedicalAllowance,
                                AllowanceName = allowanceInfo.MedicalAllowanceName,
                                IsActive = true,
                                IsMonthly = true,
                                IsTaxable = true,
                                IsIndividual = false,
                                DepandsOnWorkingHour = null,
                                ProjectRestYear = true,
                                OnceOffDeduction = false,
                                AllowanceConfigId = 0,
                                Flag = "Medical",
                                TillAmount = 0,
                                CurrentAmount = 0,
                                Amount = 0,
                                Arrear = 0,
                                ReviewAmount = Math.Round(currentGross * (decimal).04) + conveyanceAmount
                            };
                            earnedAllowances.Add(taxDetailInTaxProcess);
                        }
                    }
                    #endregion

                    #region Thompson - Yearly Bonus
                    if (user.OrganizationId == 11 && user.CompanyId == 19)
                    {
                        if (earnedAllowances.Exists(i => i.AllowanceNameId == 7) == false && employee.IsDiscontinued == false)
                        {
                            decimal conveyanceAmountAccordingToEmployeeType = employee.EmployeeType == "Manager" ? 5000 : 3500;
                            var nextServiceYear = DateTimeExtension.GetNextServiceYearCompleteDate(employee.DateOfJoining.Value, fiscalYearInfo.FiscalYearTo.Value.Year);
                            if (nextServiceYear.Month == 7 && nextServiceYear.Day < 16)
                            {
                                nextServiceYear = fiscalYearInfo.FiscalYearTo.Value;
                            }
                            if (nextServiceYear.IsDateBetweenTwoDates(fiscalYearInfo.FiscalYearFrom.Value, fiscalYearInfo.FiscalYearTo.Value))
                            {
                                TaxDetailInTaxProcess taxDetailInTaxProcess = new TaxDetailInTaxProcess()
                                {
                                    SL = earnedAllowances.Count + 1,
                                    EmployeeId = employee.EmployeeId,
                                    AllowanceNameId = 7,
                                    AllowanceName = "Yearly Bonus",
                                    IsActive = true,
                                    IsMonthly = true,
                                    IsTaxable = true,
                                    IsIndividual = false,
                                    DepandsOnWorkingHour = null,
                                    ProjectRestYear = true,
                                    OnceOffDeduction = false,
                                    AllowanceConfigId = 0,
                                    Flag = "Yearly Bonus",
                                    TillAmount = 0,
                                    CurrentAmount = 0,
                                    Amount = 0,
                                    Arrear = 0,
                                    ReviewAmount = 0,
                                    ProjectedAmount = employee.JobType == Jobtype.Contractual ? currentGross + conveyanceAmountAccordingToEmployeeType : currentGross
                                };
                                earnedAllowances.Add(taxDetailInTaxProcess);
                            }
                        }
                    }
                    #endregion

                    if (remainProjectionMonthForThisEmployee > 0)
                    {
                        foreach (var item in earnedAllowances)
                        {
                            var reviewAmount = item.ReviewAmount > 0 ? item.ReviewAmount : item.CurrentAmount;
                            if (user.OrganizationId == 11 && user.CompanyId == 19 && employee.JobType == "Permanent" && item.AllowanceNameId == allowanceInfo.MedicalAllowance)
                            {
                                item.TillAmount = accuredAllowance.TillAccuredAmount + item.TillAmount;
                                item.CurrentAmount = accuredAllowance.ThisMonthAccuredAmount + accuredAllowance.ThisMonthAccuredArrear;
                                item.Arrear = accuredAllowance.ThisMonthAccuredArrear;

                                if ((item.ProjectRestYear ?? false) == true && employee.IsDiscontinued == false && isThisEmployeeDiscontinuedWithinThisFiscalYear == false && (employee.CalculateProjectionTaxProratedBasis ?? false) == false)
                                {
                                    //item.ProjectedAmount = (accuredAllowance.RemainAmount) + reviewAmount * remainProjectionMonthForThisEmployee;
                                    if (employee.IsResidential)
                                    {
                                        item.ProjectedAmount = reviewAmount * remainProjectionMonthForThisEmployee;
                                    }
                                    else if (employee.IsResidential == false && employee.TotalServiceDays > taxSetting.IncomeTaxSetting.NonResidentialPeriod)
                                    {
                                        item.ProjectedAmount = reviewAmount * remainProjectionMonthForThisEmployee;
                                    }
                                    else
                                    {
                                        item.ProjectedAmount = 0;
                                    }

                                }
                                else if ((item.ProjectRestYear ?? false) == true && employee.IsDiscontinued == false && isThisEmployeeDiscontinuedWithinThisFiscalYear == true && (employee.CalculateProjectionTaxProratedBasis ?? false) == false)
                                {
                                    //item.ProjectedAmount = (accuredAllowance.RemainAmount) + reviewAmount * remainProjectionMonthForThisEmployee;
                                    //item.ProjectedAmount = reviewAmount * remainProjectionMonthForThisEmployee;
                                    if (employee.IsResidential)
                                    {
                                        item.ProjectedAmount = reviewAmount * remainProjectionMonthForThisEmployee;
                                    }
                                    else if (employee.IsResidential == false && employee.TotalServiceDays > taxSetting.IncomeTaxSetting.NonResidentialPeriod)
                                    {
                                        item.ProjectedAmount = reviewAmount * remainProjectionMonthForThisEmployee;
                                    }
                                    else
                                    {
                                        item.ProjectedAmount = 0;
                                    }
                                }
                                else if ((item.ProjectRestYear ?? false) == true && employee.IsDiscontinued == false && isThisEmployeeDiscontinuedWithinThisFiscalYear == true && (employee.CalculateProjectionTaxProratedBasis ?? false) == true)
                                {
                                    if (employee.IsResidential || ((employee.IsResidential == false && employee.TotalServiceDays > taxSetting.IncomeTaxSetting.NonResidentialPeriod)))
                                    {
                                        isCalculateProjectionTaxProratedBasis = true;
                                        item.ProjectedAmount = await GetProjectionAmountWhenDiscontinued(employee, reviewAmount, fiscalYearInfo, DateTimeExtension.LastDateOfAMonth(year, month), user);
                                    }
                                    else
                                    {
                                        item.ProjectedAmount = 0;
                                    }
                                }
                            }
                            // Woundermen Thompson Yearly Bonus && Festival Bonus
                            else if (user.OrganizationId == 11 && user.CompanyId == 19 && (item.AllowanceNameId == 7 || item.AllowanceNameId == 9))
                            {

                            }
                            else
                            {
                                if ((item.ProjectRestYear ?? false) == true && employee.IsDiscontinued == false && isThisEmployeeDiscontinuedWithinThisFiscalYear == false && (employee.CalculateProjectionTaxProratedBasis ?? false) == false && item.Flag != "FESTIVAL BONUS")
                                {
                                    if (employee.IsResidential || ((employee.IsResidential == false && employee.TotalServiceDays > taxSetting.IncomeTaxSetting.NonResidentialPeriod)))
                                    {
                                        item.ProjectedAmount = reviewAmount * remainProjectionMonthForThisEmployee;
                                    }
                                    else
                                    {
                                        item.ProjectedAmount = 0;
                                    }
                                }
                                else if ((item.ProjectRestYear ?? false) == true && employee.IsDiscontinued == false && isThisEmployeeDiscontinuedWithinThisFiscalYear == true && (employee.CalculateProjectionTaxProratedBasis ?? false) == false && item.Flag != "FESTIVAL BONUS")
                                {
                                    //item.ProjectedAmount = reviewAmount * remainProjectionMonthForThisEmployee;
                                    if (employee.IsResidential || ((employee.IsResidential == false && employee.TotalServiceDays > taxSetting.IncomeTaxSetting.NonResidentialPeriod)))
                                    {
                                        item.ProjectedAmount = reviewAmount * remainProjectionMonthForThisEmployee;
                                    }
                                    else
                                    {
                                        item.ProjectedAmount = 0;
                                    }
                                }
                                else if ((item.ProjectRestYear ?? false) == true && employee.IsDiscontinued == false && isThisEmployeeDiscontinuedWithinThisFiscalYear == true && (employee.CalculateProjectionTaxProratedBasis ?? false) == true && item.Flag != "FESTIVAL BONUS")
                                {
                                    if (employee.IsResidential || ((employee.IsResidential == false && employee.TotalServiceDays > taxSetting.IncomeTaxSetting.NonResidentialPeriod)))
                                    {
                                        isCalculateProjectionTaxProratedBasis = true;
                                        item.ProjectedAmount = await GetProjectionAmountWhenDiscontinued(employee, reviewAmount, fiscalYearInfo, DateTimeExtension.LastDateOfAMonth(year, month), user);
                                    }
                                    else
                                    {
                                        item.ProjectedAmount = 0;
                                    }
                                }
                            }
                        }
                    }
                    //// DataTable To Look at the table data for debugging
                    //DataTable taxDetailsDataTable = new DataTable();
                    //if (earnedAllowances.Any()) {a
                    //    taxDetailsDataTable = earnedAllowances.ToDataTable();
                    //}

                    // Conditional Projected Amount
                    IEnumerable<ConditionalProjectedPayment> unProcessedProjectedAllowances = new List<ConditionalProjectedPayment>();
                    if (employee.IsDiscontinued == false)
                    {
                        unProcessedProjectedAllowances = await _conditionalProjectedPaymentBusiness.GetUnProcessedConditionalProjectedPaymentsAsync(new EligibilityInConditionalProjectedPayment_Filter()
                        {
                            FiscalYearId = fiscalYearInfo.FiscalYearId.ToString(),
                            Gender = employee.Gender,
                            JobType = employee.JobType,
                            MaritalStatus = employee.MaritalStatus,
                            PhysicalCondition = employee.PhysicalCondition,
                            Religion = employee.Religion,
                            Citizen = employee.IsResidential ? "Yes" : "No"

                        }, new EligibleEmployeeForTaxType()
                        {
                            EmployeeId = salaryDetail.EmployeeId,
                            GradeId = salaryDetail.GradeId ?? 0,
                            DepartmentId = salaryDetail.DepartmentId ?? 0,
                            DesignationId = salaryDetail.DesignationId ?? 0,
                            InternalDesignationId = salaryDetail.InternalDesignationId ?? 0,
                            SectionId = salaryDetail.SectionId ?? 0,
                            SubSectionId = salaryDetail.SubsectionId ?? 0,
                            UnitId = salaryDetail.UnitId ?? 0,
                            BranchId = salaryDetail.BranchId ?? 0,
                            JobCategoryId = salaryDetail.JobCategoryId,
                            EmployeeTypeId = salaryDetail.EmployeeTypeId
                        }, user);
                        if (unProcessedProjectedAllowances != null)
                        {
                            if (unProcessedProjectedAllowances.Any())
                            {
                                foreach (var item in unProcessedProjectedAllowances)
                                {
                                    var projectedAllowanceInfo = await _allowanceNameBusiness.GetAllowanceNameByIdAsync(item.AllowanceNameId, user);
                                    if (item.BaseOfPayment == "Basic" || item.BaseOfPayment == "Gross")
                                    {
                                        var baseAmount = item.BaseOfPayment == "Basic" ? currentBasic : currentGross;
                                        if (item.Percentage != null && item.Percentage > 0)
                                        {
                                            baseAmount = baseAmount * ((item.Percentage ?? 0) / 100);
                                            if (baseAmount > 0)
                                            {
                                                earnedAllowances.Add(new TaxDetailInTaxProcess()
                                                {
                                                    AllowanceName = projectedAllowanceInfo != null ? projectedAllowanceInfo.Name : "",
                                                    AllowanceNameId = item.AllowanceNameId,
                                                    Flag = projectedAllowanceInfo != null ? projectedAllowanceInfo.Flag : "",
                                                    ProjectRestYear = true,
                                                    OnceOffDeduction = false,
                                                    TillAmount = 0,
                                                    CurrentAmount = 0,
                                                    Amount = 0,
                                                    ProjectedAmount =
                                                    //baseAmount
                                                    isCalculateProjectionTaxProratedBasis == false ? baseAmount : await GetProjectionAmountWhenDiscontinued(employee, baseAmount, fiscalYearInfo, DateTimeExtension.LastDateOfAMonth(year, month), user),
                                                    ReviewAmount = 0,
                                                    RemainFiscalYearMonth = (short)remainProjectionMonthForThisEmployee,
                                                    LessExemptedAmount = 0,
                                                    TotalTaxableIncome = 0,
                                                    GrossAnnualIncome = 0
                                                });
                                            }
                                        }
                                    }
                                    else if (item.BaseOfPayment == "Flat")
                                    {
                                        if (item.Amount != null && item.Amount > 0)
                                        {
                                            earnedAllowances.Add(new TaxDetailInTaxProcess()
                                            {
                                                AllowanceName = projectedAllowanceInfo != null ? projectedAllowanceInfo.Name : "",
                                                AllowanceNameId = item.AllowanceNameId,
                                                Flag = projectedAllowanceInfo != null ? projectedAllowanceInfo.Flag : "",
                                                ProjectRestYear = true,
                                                OnceOffDeduction = false,
                                                TillAmount = 0,
                                                CurrentAmount = 0,
                                                Amount = 0,
                                                ProjectedAmount = item.Amount ?? 0,
                                                //isCalculateProjectionTaxProratedBasis == false ? item.Amount ?? 0 : (await GetProjectionAmountWhenDiscontinued(employee, (item.Amount ?? 0), fiscalYearInfo, DateTimeExtension.LastDateOfAMonth(year, month), user)),
                                                ReviewAmount = 0,
                                                RemainFiscalYearMonth = (short)remainProjectionMonthForThisEmployee,
                                                LessExemptedAmount = 0,
                                                TotalTaxableIncome = 0,
                                                GrossAnnualIncome = 0
                                            });
                                        }
                                    }
                                    #region Wounderman Thompson
                                    else if (item.BaseOfPayment == "Gross With Conveyance" && user.CompanyId == 19 && user.OrganizationId == 11)
                                    {
                                        if (employee.JobType == Jobtype.Contractual)
                                        {
                                            decimal conveyanceAmountAccordingToEmployeeType = employee.EmployeeType == "Manager" ? 5000 : 3500;
                                            var baseAmount = currentGross + conveyanceAmountAccordingToEmployeeType;
                                            if (item.Percentage != null && item.Percentage > 0)
                                            {
                                                baseAmount = baseAmount * ((item.Percentage ?? 0) / 100);
                                                if (baseAmount > 0)
                                                {
                                                    earnedAllowances.Add(new TaxDetailInTaxProcess()
                                                    {
                                                        AllowanceName = projectedAllowanceInfo != null ? projectedAllowanceInfo.Name : "",
                                                        AllowanceNameId = item.AllowanceNameId,
                                                        Flag = projectedAllowanceInfo != null ? projectedAllowanceInfo.Flag : "",
                                                        ProjectRestYear = true,
                                                        OnceOffDeduction = false,
                                                        TillAmount = 0,
                                                        CurrentAmount = 0,
                                                        Amount = 0,
                                                        ProjectedAmount = baseAmount,
                                                        ReviewAmount = 0,
                                                        RemainFiscalYearMonth = (short)remainProjectionMonthForThisEmployee,
                                                        LessExemptedAmount = 0,
                                                        TotalTaxableIncome = 0,
                                                        GrossAnnualIncome = 0
                                                    });
                                                }
                                            }
                                        }
                                    }
                                    #endregion
                                }
                            }
                        }
                    }

                    // Till Conditional Projected Amount
                    var tillConditionalProjectedAllowances = await _conditionalProjectedPaymentBusiness.GetAllowanceTillDisbursedAmountsAsync(employee.EmployeeId, fiscalYearInfo.FiscalYearId, year, month, user);

                    if (tillConditionalProjectedAllowances != null)
                    {
                        if (tillConditionalProjectedAllowances.Any())
                        {
                            foreach (var item in tillConditionalProjectedAllowances)
                            {
                                earnedAllowances.Add(new TaxDetailInTaxProcess()
                                {
                                    AllowanceName = item.AllowanceName,
                                    AllowanceNameId = item.AllowanceNameId,
                                    Flag = item.AllowanceFlag,
                                    ProjectRestYear = true,
                                    OnceOffDeduction = false,
                                    TillAmount = item.DisbursedAmount,
                                    CurrentAmount = 0,
                                    Amount = 0,
                                    ProjectedAmount = 0,
                                    ReviewAmount = 0,
                                    RemainFiscalYearMonth = (short)remainProjectionMonthForThisEmployee,
                                    LessExemptedAmount = 0,
                                    TotalTaxableIncome = 0,
                                    GrossAnnualIncome = 0
                                });
                            }
                        }
                    }

                    // This month Conditional Projected Amount
                    var thisMonthConditionalProjectedAllowances = await _conditionalProjectedPaymentBusiness.GetAllowanceThisMonthDisbursedAmountsAsync(employee.EmployeeId, fiscalYearInfo.FiscalYearId, year, month, user);
                    if (thisMonthConditionalProjectedAllowances != null)
                    {
                        if (thisMonthConditionalProjectedAllowances.Any())
                        {
                            foreach (var item in thisMonthConditionalProjectedAllowances)
                            {
                                earnedAllowances.Add(new TaxDetailInTaxProcess()
                                {
                                    AllowanceName = item.AllowanceName,
                                    AllowanceNameId = item.AllowanceNameId,
                                    Flag = item.AllowanceFlag,
                                    ProjectRestYear = true,
                                    OnceOffDeduction = false,
                                    TillAmount = 0,
                                    CurrentAmount = item.DisbursedAmount,
                                    Amount = item.DisbursedAmount,
                                    ProjectedAmount = 0,
                                    ReviewAmount = 0,
                                    RemainFiscalYearMonth = (short)remainProjectionMonthForThisEmployee,
                                    LessExemptedAmount = 0,
                                    TotalTaxableIncome = 0,
                                    GrossAnnualIncome = 0
                                });
                            }
                        }
                    }

                    // Unprocessed Individual Projected Amount
                    IEnumerable<EmployeeProjectedPayment> individualUnProcessedProjectedAllowances = new List<EmployeeProjectedPayment>();
                    if (employee.IsDiscontinued == false)
                    {
                        individualUnProcessedProjectedAllowances = await _projectedPaymentBusiness.GetUnProcessedProjectedAllowanceAsync(employee.EmployeeId, fiscalYearInfo.FiscalYearId, year, month, user);
                        if (individualUnProcessedProjectedAllowances != null)
                        {
                            if (individualUnProcessedProjectedAllowances.Any())
                            {
                                foreach (var item in individualUnProcessedProjectedAllowances)
                                {
                                    var projectedAllowanceInfo = await _allowanceNameBusiness.GetAllowanceNameByIdAsync(item.AllowanceNameId, user);
                                    if (item.BaseOfPayment == "Basic" || item.BaseOfPayment == "Gross")
                                    {
                                        var baseAmount = item.BaseOfPayment == "Basic" ? currentBasic : currentGross;
                                        if (item.Percentage != null && item.Percentage > 0)
                                        {
                                            if (baseAmount > 0)
                                            {
                                                earnedAllowances.Add(new TaxDetailInTaxProcess()
                                                {
                                                    AllowanceName = projectedAllowanceInfo != null ? projectedAllowanceInfo.Name : "",
                                                    AllowanceNameId = item.AllowanceNameId,
                                                    Flag = projectedAllowanceInfo != null ? projectedAllowanceInfo.Flag : "",
                                                    ProjectRestYear = true,
                                                    OnceOffDeduction = false,
                                                    TillAmount = 0,
                                                    CurrentAmount = 0,
                                                    Amount = 0,
                                                    ProjectedAmount = isCalculateProjectionTaxProratedBasis == false ? baseAmount : await GetProjectionAmountWhenDiscontinued(employee, baseAmount, fiscalYearInfo, DateTimeExtension.LastDateOfAMonth(year, month), user),
                                                    ReviewAmount = 0,
                                                    RemainFiscalYearMonth = (short)remainProjectionMonthForThisEmployee,
                                                    LessExemptedAmount = 0,
                                                    TotalTaxableIncome = 0,
                                                    GrossAnnualIncome = 0
                                                });
                                            }
                                        }
                                    }
                                    else if (item.BaseOfPayment == "Flat")
                                    {
                                        if (item.Amount != null && item.Amount > 0)
                                        {
                                            earnedAllowances.Add(new TaxDetailInTaxProcess()
                                            {
                                                AllowanceName = projectedAllowanceInfo != null ? projectedAllowanceInfo.Name : "",
                                                AllowanceNameId = item.AllowanceNameId,
                                                Flag = projectedAllowanceInfo != null ? projectedAllowanceInfo.Flag : "",
                                                ProjectRestYear = true,
                                                OnceOffDeduction = false,
                                                TillAmount = 0,
                                                CurrentAmount = 0,
                                                Amount = 0,
                                                ProjectedAmount = isCalculateProjectionTaxProratedBasis == false ? item.Amount ?? 0 : await GetProjectionAmountWhenDiscontinued(employee, item.Amount ?? 0, fiscalYearInfo, DateTimeExtension.LastDateOfAMonth(year, month), user),
                                                ReviewAmount = 0,
                                                RemainFiscalYearMonth = (short)remainProjectionMonthForThisEmployee,
                                                LessExemptedAmount = 0,
                                                TotalTaxableIncome = 0,
                                                GrossAnnualIncome = 0
                                            });
                                        }
                                    }
                                }
                            }
                        }
                    }

                    // Till Individual Projected Amount 
                    var tillIndividualProjectedAllowances = await _projectedPaymentBusiness.GetTillProcessedProjectedAllowanceAsync(employee.EmployeeId, fiscalYearInfo.FiscalYearId, year, month, user);
                    if (tillIndividualProjectedAllowances != null)
                    {
                        if (tillIndividualProjectedAllowances.Any())
                        {
                            foreach (var item in tillIndividualProjectedAllowances)
                            {
                                earnedAllowances.Add(new TaxDetailInTaxProcess()
                                {
                                    AllowanceName = item.AllowanceName,
                                    AllowanceNameId = item.AllowanceNameId,
                                    Flag = item.AllowanceFlag,
                                    ProjectRestYear = true,
                                    OnceOffDeduction = false,
                                    TillAmount = 0,
                                    CurrentAmount = item.DisbursedAmount,
                                    Amount = item.DisbursedAmount,
                                    ProjectedAmount = 0,
                                    ReviewAmount = 0,
                                    RemainFiscalYearMonth = (short)remainProjectionMonthForThisEmployee,
                                    LessExemptedAmount = 0,
                                    TotalTaxableIncome = 0,
                                    GrossAnnualIncome = 0
                                });
                            }
                        }
                    }

                    // This Month Individual Projected Amount 
                    var thisMonthIndividualProjectedAllowances = await _projectedPaymentBusiness.GetThisMonthProcessedProjectedAllowanceAsync(employee.EmployeeId, fiscalYearInfo.FiscalYearId, year, month, user);
                    if (thisMonthIndividualProjectedAllowances != null)
                    {
                        if (thisMonthIndividualProjectedAllowances.Any())
                        {
                            foreach (var item in thisMonthIndividualProjectedAllowances)
                            {
                                earnedAllowances.Add(new TaxDetailInTaxProcess()
                                {
                                    AllowanceName = item.AllowanceName,
                                    AllowanceNameId = item.AllowanceNameId,
                                    Flag = item.AllowanceFlag,
                                    ProjectRestYear = true,
                                    OnceOffDeduction = false,
                                    TillAmount = 0,
                                    CurrentAmount = item.DisbursedAmount,
                                    Amount = item.DisbursedAmount,
                                    ProjectedAmount = 0,
                                    ReviewAmount = 0,
                                    RemainFiscalYearMonth = (short)remainProjectionMonthForThisEmployee,
                                    LessExemptedAmount = 0,
                                    TotalTaxableIncome = 0,
                                    GrossAnnualIncome = 0
                                });
                            }
                        }
                    }

                    var pfPercentage = Utility.TryParseInt(payrollModuleConfig.PercentageOfProvidentFund);
                    if ((payrollModuleConfig.IsProvidentFundactivated ?? false) == true)
                    {

                        if (employee.IsPFMember && pfPercentage > 0)
                        {
                            var baseAmount = (payrollModuleConfig.BaseOfProvidentFund == "Basic" ? currentBasic : currentGross) / 100 * pfPercentage;
                            baseAmount = Math.Round(baseAmount, MidpointRounding.AwayFromZero);
                            var employeePFAllowance = await GetEmployeePFAmountAsync(employee, fiscalYearInfo, year, month, user);

                            if (employeePFAllowance != null)
                            {
                                employeePFAllowance.AllowanceNameId = allowanceInfo.PFAllowance;
                                employeePFAllowance.AllowanceName = "PF";
                                employeePFAllowance.Flag = "PF";
                                employeePFAllowance.ProjectRestYear = true;
                                employeePFAllowance.OnceOffDeduction = false;
                                employeePFAllowance.ReviewAmount = baseAmount;
                                employeePFAllowance.RemainFiscalYearMonth = (short)remainProjectionMonthForThisEmployee;
                                if (user.CompanyId == 17 && user.OrganizationId == 10 && employee.EmployeeId == 792)
                                {
                                    employeePFAllowance.ProjectedAmount = employee.IsDiscontinued == false ? 79000 * remainProjectionMonthForThisEmployee : 0;
                                }
                                else
                                {
                                    employeePFAllowance.ProjectedAmount = employee.IsDiscontinued == false ? baseAmount * remainProjectionMonthForThisEmployee : 0;
                                    employeePFAllowance.ProjectedAmount = Math.Round(employeePFAllowance.ProjectedAmount, MidpointRounding.AwayFromZero);
                                }

                                earnedAllowances.Add(employeePFAllowance);
                            }

                        }
                    }

                    // Deductable amount from Incomeable Income
                    var deductableAmounts = await DeductableIncomeOfThisEmployeeAsync(employee, fiscalYearInfo, month, year, user);
                    if (deductableAmounts.Any())
                    {
                        foreach (var item in deductableAmounts)
                        {
                            TaxDetailInTaxProcess deduction = new TaxDetailInTaxProcess()
                            {
                                IsItAllowance = false,
                                AllowanceNameId = 0,
                                DeductionNameId = item.DeductionNameId,
                                DeductionName = item.DeductionName,
                                Flag = item.DeductionFlag == "" || item.DeductionFlag == null ? item.DeductionFlag : item.DeductionFlag,
                                ProjectRestYear = item.IsProjected,
                                OnceOffDeduction = item.IsOnceOff,
                                TillAmount = item.TillAmount > 0 ? item.TillAmount * -1 : 0,
                                CurrentAmount = item.CurrentAmount > 0 ? item.CurrentAmount * -1 : 0,
                                Adjustment = 0,
                                Arrear = 0,
                                ReviewAmount = 0,
                                RemainFiscalYearMonth = (short)remainProjectionMonthForThisEmployee,
                                ProjectedAmount = 0,
                                LessExemptedAmount = 0,
                                TotalTaxableIncome = 0,
                                GrossAnnualIncome = 0
                            };
                            earnedAllowances.Add(deduction);
                        }
                    }
                    // DataTable To Look at the table data for debugging
                    DataTable taxDetailsDataTable = new DataTable();
                    if (earnedAllowances.Any())
                    {
                        taxDetailsDataTable = earnedAllowances.ToDataTable();
                    }

                    var margedTaxDetails = await MargeTaxDetailsAsync(employee, earnedAllowances, user);
                    if (margedTaxDetails != null)
                    {
                        if (margedTaxDetails.Any())
                        {
                            taxDetailsDataTable = margedTaxDetails.ToDataTable();
                        }
                    }

                    if (margedTaxDetails != null && margedTaxDetails.Any())
                    {
                        employee.RemainFiscalYearMonth = remainProjectionMonthForThisEmployee;
                        taxProcessInfo =
                        await _taxRulesBusiness.TaxRulesIY2324(employee, fiscalYearInfo, margedTaxDetails.ToList(), payrollModuleConfig, allowanceInfo, sumOfRemainProjectionAndCountOfSalaryReceipt, remainProjectionMonthForThisEmployee, year, month, user);
                        ///remainFiscalYearMonth
                    }

                }
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ExecuteTaxProcess", "SalaryTaxDetailsAsync", user);
            }
            return taxProcessInfo;
        }
        public async Task<IEnumerable<AllowancesEarnedByThisEmployee>> AllowancesEarnedByThisEmployeeByThisFiscalYearAsync(EligibleEmployeeForTaxType employee, FiscalYear fiscalYear, int month, int year, long lastSalaryReviewId, AppUser user)
        {
            IEnumerable<AllowancesEarnedByThisEmployee> list = new List<AllowancesEarnedByThisEmployee>();
            try
            {
                var query = $@"SELECT Distinct ALW.AllowanceNameId,[AllowanceName]=ALW.Name,AllowanceFlag=Flag,IsProjected=ISNULL(ProjectRestYear,0),IsOnceOff=ISNULL(IsOnceOffTax,0) FROM (Select Distinct AllowanceNameId From Payroll_SalaryAllowance
                Where EmployeeId=@EmployeeId 
                AND CAST(SalaryDate AS DATE) Between @FiscalYearFrom AND @FiscalYearTo 
                AND CompanyId=@CompanyId
                AND OrganizationId=@OrganizationId

                UNION ALL

                Select Distinct AllowanceNameId From Payroll_SalaryAllowanceArrear
                Where EmployeeId=@EmployeeId AND SalaryDate Between @FiscalYearFrom AND @FiscalYearTo AND CompanyId=@CompanyId
                AND OrganizationId=@OrganizationId

                UNION ALL
                Select Distinct  AllowanceNameId From Payroll_SalaryReviewDetail
                Where SalaryReviewInfoId=@LastSalaryReviewId AND CompanyId=@CompanyId
                AND OrganizationId=@OrganizationId

                UNION ALL
                Select Distinct AllowanceNameId From Payroll_SupplementaryPaymentAmount SPA
                Where EmployeeId=@EmployeeId AND SPA.CompanyId=@CompanyId AND SPA.FiscalYearId= @FiscalYearId
                AND StateStatus='Disbursed' AND IsApproved=1
                AND SPA.OrganizationId=@OrganizationId 

                UNION ALL
                SELECT Distinct Config.AllowanceNameId FROM Payroll_MonthlyAllowanceHistory MALW
                INNER JOIN Payroll_MonthlyAllowanceConfig Config ON MALW.MonthlyAllowanceConfigId=Config.Id
                INNER JOIN Payroll_AllowanceName ALW ON Config.AllowanceNameId=ALW.AllowanceNameId
                Where ISNULL(Config.IsVisibleInSalarySheet,0)=0 
                AND Config.EmployeeId=@EmployeeId
                AND MALW.FiscalYearId=@FiscalYearId
                AND Config.CompanyId=@CompanyId
                AND Config.OrganizationId=@OrganizationId
                
                ) tbl
                INNER JOIN Payroll_AllowanceName ALW ON tbl.AllowanceNameId = ALW.AllowanceNameId
                Inner Join Payroll_AllowanceConfiguration conf on ALW.AllowanceNameId= conf.AllowanceNameId AND ISNULL(conf.IsTaxable,0)=1 AND conf.StateStatus='Approved'";
                var parameters = new { employee.EmployeeId, fiscalYear.FiscalYearId, fiscalYear.FiscalYearFrom, fiscalYear.FiscalYearTo, LastSalaryReviewId = lastSalaryReviewId, user.CompanyId, user.OrganizationId };
                list = await _dapper.SqlQueryListAsync<AllowancesEarnedByThisEmployee>(user.Database, query, parameters);

                if (list.Any())
                {
                    foreach (var item in list)
                    {
                        item.TillAmount = await GetAllowanceTillAmountAsync(employee.EmployeeId, item.AllowanceNameId, fiscalYear, month, year, user);
                        item.CurrentAmount = await GetAllowanceCurrentAmountAsync(employee.EmployeeId, item.AllowanceNameId, fiscalYear, month, year, user);
                        item.ArrearAmount = await GetAllowanceArrearAmountAsync(employee.EmployeeId, item.AllowanceNameId, fiscalYear, month, year, user);
                        item.TillAdjustment = await GetAllowanceTillAdjustmentAsync(employee.EmployeeId, item.AllowanceNameId, fiscalYear, month, year, user);
                        item.CurrentAdjustment = await GetAllowanceCurrentAdjustmentAsync(employee.EmployeeId, item.AllowanceNameId, fiscalYear, month, year, user);
                    }
                }
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ExecuteTaxProcess", "AllowancesEarnedByThisEmployeeByThisFiscalYearAsync", user);
            }
            return list;
        }
        public async Task<decimal> GetAllowanceTillAmountAsync(long employeeId, long allowanceNameId, FiscalYear fiscalYear, int month, int year, AppUser user)
        {
            decimal tillAmount = 0;
            try
            {
                var query = $@"SELECT ISNULL((Select SUM((Amount+ArrearAmount)) 
	            From Payroll_SalaryAllowance
	            Where AllowanceNameId=@AllowanceNameId 
	            AND EmployeeId=@EmployeeId 
	            AND SalaryDate < @FirstDateOfThisMonth 
	            AND (SalaryDate Between @FiscalYearFrom AND @FiscalYearTo)
	            AND CompanyId=@CompanyId
	            AND OrganizationId=@OrganizationId),0)";

                var firstDateOfThisMonth = DateTimeExtension.FirstDateOfAMonth(year, month).ToString("yyyy-MM-dd");
                tillAmount = await _dapper.SqlQueryFirstAsync<decimal>(user.Database, query, new { AllowanceNameId = allowanceNameId, EmployeeId = employeeId, FirstDateOfThisMonth = firstDateOfThisMonth, fiscalYear.FiscalYearFrom, fiscalYear.FiscalYearTo, user.CompanyId, user.OrganizationId });


                query = $@"SELECT ISNULL((SELECT SUM(ISNULL(SPA.Amount,0)) FROM Payroll_SupplementaryPaymentAmount SPA
	            INNER JOIN Payroll_SupplementaryPaymentProcessInfo SPI ON SPA.PaymentProcessInfoId = SPI.PaymentProcessInfoId
	            Where SPA.EmployeeId=@EmployeeId AND SPI.IsDisbursed=1 AND SPA.AllowanceNameId = @AllowanceNameId AND SPI.FiscalYearId = @FiscalYearId AND SPI.PaymentMonth <> @Month 
                AND SPA.CompanyId=@CompanyId AND SPA.OrganizationId=@OrganizationId),0)";

                var tillAmount2 = await _dapper.SqlQueryFirstAsync<decimal>(user.Database, query, new
                {
                    EmployeeId = employeeId,
                    AllowanceNameId = allowanceNameId,
                    fiscalYear.FiscalYearId,
                    Month = month,
                    user.CompanyId,
                    user.OrganizationId
                });

                tillAmount = tillAmount + tillAmount2;

                query = $@"SELECT ISNULL((SELECT SUM(MALW.Amount) FROM Payroll_MonthlyAllowanceHistory MALW
                INNER JOIN Payroll_MonthlyAllowanceConfig Config ON MALW.MonthlyAllowanceConfigId=Config.Id
                INNER JOIN Payroll_AllowanceName ALW ON Config.AllowanceNameId=ALW.AllowanceNameId
                Where ISNULL(Config.IsVisibleInSalarySheet,0)=0 
                AND Config.EmployeeId=@EmployeeId
                AND Config.AllowanceNameId=@AllowanceNameId
                AND MALW.[Month] <> @Month
                AND MALW.FiscalYearId=@FiscalYearId 
                AND Config.CompanyId=@CompanyId 
                AND Config.OrganizationId=@OrganizationId),0)";

                var tillAmount3 = await _dapper.SqlQueryFirstAsync<decimal>(user.Database, query, new
                {
                    EmployeeId = employeeId,
                    AllowanceNameId = allowanceNameId,
                    Month = month,
                    fiscalYear.FiscalYearId,
                    user.CompanyId,
                    user.OrganizationId
                });

                tillAmount = tillAmount + tillAmount3;

            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ExecuteTaxProcess", "GetAllowanceTillAmountAsync", user);
            }
            return tillAmount;
        }
        public async Task<decimal> GetAllowanceCurrentAmountAsync(long employeeId, long allowanceNameId, FiscalYear fiscalYear, int month, int year, AppUser user)
        {
            decimal currentAmount = 0;
            try
            {
                var query = $@"SELECT ISNULL((Select SUM((Amount+ArrearAmount)) 
	            From Payroll_SalaryAllowance
	            Where AllowanceNameId=@AllowanceNameId 
	            AND EmployeeId=@EmployeeId 
	            AND MONTH(SalaryDate) = MONTH(@FirstDateOfThisMonth)
                AND YEAR(SalaryDate) = YEAR(@FirstDateOfThisMonth)
	            AND (SalaryDate Between @FiscalYearFrom AND @FiscalYearTo)
	            AND CompanyId=@CompanyId
	            AND OrganizationId=@OrganizationId),0)";

                var firstDateOfThisMonth = DateTimeExtension.FirstDateOfAMonth(year, month).ToString("yyyy-MM-dd");
                currentAmount = await _dapper.SqlQueryFirstAsync<decimal>(user.Database, query.Trim(), new { AllowanceNameId = allowanceNameId, EmployeeId = employeeId, FirstDateOfThisMonth = firstDateOfThisMonth, FiscalYearFrom = fiscalYear.FiscalYearFrom, FiscalYearTo = fiscalYear.FiscalYearTo, CompanyId = user.CompanyId, OrganizationId = user.OrganizationId });

                query = $@"SELECT ISNULL((SELECT SUM(SPA.Amount) FROM Payroll_SupplementaryPaymentAmount SPA
	            INNER JOIN Payroll_SupplementaryPaymentProcessInfo SPI ON SPA.PaymentProcessInfoId = SPI.PaymentProcessInfoId
	            Where SPA.EmployeeId=@EmployeeId AND SPI.IsDisbursed=1 AND SPA.AllowanceNameId = @AllowanceNameId AND SPI.FiscalYearId = @FiscalYearId AND SPI.PaymentMonth = @Month 
                AND SPA.CompanyId=@CompanyId AND SPA.OrganizationId=@OrganizationId),0)";

                currentAmount = currentAmount + await _dapper.SqlQueryFirstAsync<decimal>(user.Database, query.Trim(), new
                {
                    EmployeeId = employeeId,
                    AllowanceNameId = allowanceNameId,
                    fiscalYear.FiscalYearId,
                    Month = month,
                    CompanyId = user.CompanyId,
                    OrganizationId = user.OrganizationId
                });

                query = $@"SELECT ISNULL((SELECT SUM(MALW.Amount) FROM Payroll_MonthlyAllowanceHistory MALW
                INNER JOIN Payroll_MonthlyAllowanceConfig Config ON MALW.MonthlyAllowanceConfigId=Config.Id
                INNER JOIN Payroll_AllowanceName ALW ON Config.AllowanceNameId=ALW.AllowanceNameId
                Where ISNULL(Config.IsVisibleInSalarySheet,0)=0 
                AND Config.EmployeeId=@EmployeeId
                AND Config.AllowanceNameId=@AllowanceNameId
                AND MALW.[Month] = @Month
                AND MALW.FiscalYearId=@FiscalYearId 
                AND Config.CompanyId=@CompanyId 
                AND Config.OrganizationId=@OrganizationId),0)";

                var currentAmount3 = await _dapper.SqlQueryFirstAsync<decimal>(user.Database, query.Trim(), new
                {
                    EmployeeId = employeeId,
                    AllowanceNameId = allowanceNameId,
                    Month = month,
                    FiscalYearId = fiscalYear.FiscalYearId,
                    CompanyId = user.CompanyId,
                    OrganizationId = user.OrganizationId
                });

                currentAmount = currentAmount + currentAmount3;

            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ExecuteTaxProcess", "GetAllowanceCurrentAmountAsync", user);
            }
            return currentAmount;
        }
        public async Task<decimal> GetAllowanceArrearAmountAsync(long employeeId, long allowanceNameId, FiscalYear fiscalYear, int month, int year, AppUser user)
        {
            decimal arrearAmount = 0;
            try
            {
                var query = $@"SELECT ISNULL((Select SUM(Amount) 
	            From Payroll_SalaryAllowanceArrear
	            Where AllowanceNameId=@AllowanceNameId 
	            AND EmployeeId=@EmployeeId 
	            AND MONTH(SalaryDate) = MONTH(@FirstDateOfThisMonth)
                AND YEAR(SalaryDate) = YEAR(@FirstDateOfThisMonth)
	            AND (SalaryDate Between @FiscalYearFrom AND @FiscalYearTo)
	            AND CompanyId=@CompanyId
	            AND OrganizationId=@OrganizationId),0)";

                var firstDateOfThisMonth = DateTimeExtension.FirstDateOfAMonth(year, month).ToString("yyyy-MM-dd");
                arrearAmount = await _dapper.SqlQueryFirstAsync<decimal>(user.Database, query.Trim(), new { AllowanceNameId = allowanceNameId, EmployeeId = employeeId, FirstDateOfThisMonth = firstDateOfThisMonth, fiscalYear.FiscalYearFrom, fiscalYear.FiscalYearTo, user.CompanyId, user.OrganizationId });

            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ExecuteTaxProcess", "GetAllowanceArrearAmountAsync", user);
            }
            return arrearAmount;
        }
        public async Task<decimal> GetProjectionAmountWhenDiscontinued(EligibleEmployeeForTaxType employee, decimal amount, FiscalYear fiscalYear, DateTime startDate, AppUser user)
        {
            decimal projectedAmount = 0;
            try
            {
                if (employee.TerminationDate.HasValue && fiscalYear.FiscalYearTo.HasValue)
                {
                    var terminationDate = employee.TerminationDate.Value > fiscalYear.FiscalYearTo.Value ? fiscalYear.FiscalYearTo.Value : employee.TerminationDate.Value;
                    if (terminationDate.Month != startDate.Month && terminationDate.Year != startDate.Year)
                    {
                        var monthDiffs = startDate.GetMonthDiffIncludingThisMonth(terminationDate);
                        for (int i = 1; i <= monthDiffs; i++)
                        {
                            var daysInMonth = startDate.DaysInAMonth();
                            if (i == 1)
                            {
                                projectedAmount = projectedAmount + amount;
                            }
                            else if (i > 1 && i < monthDiffs)
                            {
                                projectedAmount = projectedAmount + amount;
                            }
                            else
                            {
                                projectedAmount = projectedAmount + amount / daysInMonth * startDate.DaysBetweenDateRangeIncludingStartDate(terminationDate);
                            }
                            startDate.AddMonths(1);
                        }
                    }
                    else
                    {
                        projectedAmount = amount / startDate.DaysInAMonth() * startDate.DaysBetweenDateRangeIncludingStartDate(terminationDate);
                    }
                }
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ExecuteTaxProcess", "GetProjectionAmountWhenDiscontinued", user);
            }
            return Math.Round(projectedAmount);
        }
        public async Task<TaxDetailInTaxProcess> GetEmployeePFAmountAsync(EligibleEmployeeForTaxType employee, FiscalYear fiscalYear, int year, int month, AppUser user)
        {
            TaxDetailInTaxProcess taxDetailInTax = new TaxDetailInTaxProcess();
            try
            {
                var query = $@"SELECT 
                TillAmount=(SELECT SUM(ISNULL(PFAmount,0)+ISNULL(PFArrear,0)) FROM Payroll_SalaryProcessDetail 
                Where EmployeeId=@EmployeeId AND SalaryMonth<>@Month AND FiscalYearId=@FiscalYearId AND CompanyId=@CompanyId AND OrganizationId=@OrganizationId),
                CurrentAmount= ISNULL(PFAmount,0)+ISNULL(PFArrear,0),
                Amount = ISNULL(PFAmount,0),
                Arrear=ISNULL(PFArrear,0)
                FROM Payroll_SalaryProcessDetail
                Where EmployeeId=@EmployeeId AND SalaryMonth=@Month AND FiscalYearId=@FiscalYearId AND CompanyId=@CompanyId AND OrganizationId=@OrganizationId";

                taxDetailInTax = await _dapper.SqlQueryFirstAsync<TaxDetailInTaxProcess>(user.Database, query, new { employee.EmployeeId, Month = month, fiscalYear.FiscalYearId, user.CompanyId, user.OrganizationId });
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ExecuteTaxProcess", "GetEmployeePFAmountAsync", user);
            }
            return taxDetailInTax;
        }
        public async Task<IEnumerable<TaxDetailInTaxProcess>> MargeTaxDetailsAsync(EligibleEmployeeForTaxType employee, IEnumerable<TaxDetailInTaxProcess> taxDetails, AppUser user)
        {
            List<TaxDetailInTaxProcess> details = new List<TaxDetailInTaxProcess>();
            try
            {
                var allowances = taxDetails.Where(item => item.Flag == "PF" || item.AllowanceNameId > 0).Select(i => i.AllowanceNameId).Distinct();
                short count = 0;
                foreach (var item in allowances)
                {

                    var flag = taxDetails.First(i => i.AllowanceNameId == item).Flag;
                    var allowanceName = taxDetails.First(i => i.AllowanceNameId == item).AllowanceName;
                    TaxDetailInTaxProcess taxDetailInTax = new TaxDetailInTaxProcess();
                    taxDetailInTax.EmployeeId = employee.EmployeeId;
                    taxDetailInTax.AllowanceName = allowanceName;
                    taxDetailInTax.Flag = flag == "" || flag == null ? allowanceName : flag;
                    if (item == 0)
                    {
                        count++;
                    }
                    taxDetailInTax.AllowanceNameId = item == 0 ? 99999999 * count : item;

                    if (taxDetailInTax.Flag == "PF")
                    {
                        taxDetailInTax.AllowanceConfigId = taxDetails.First(i => i.AllowanceNameId == item).AllowanceConfigId;
                        taxDetailInTax.ProjectRestYear = taxDetails.FirstOrDefault(i => i.Flag == "PF").ProjectRestYear;
                        taxDetailInTax.OnceOffDeduction = taxDetails.FirstOrDefault(i => i.Flag == "PF").OnceOffDeduction;
                        taxDetailInTax.TillAmount = taxDetails.Where(i => i.Flag == "PF").Sum(i => i.TillAmount);
                        taxDetailInTax.CurrentAmount = taxDetails.Where(i => i.Flag == "PF").Sum(i => i.CurrentAmount);
                        taxDetailInTax.Amount = taxDetails.Where(i => i.Flag == "PF").Sum(i => i.Amount);
                        taxDetailInTax.Arrear = taxDetails.Where(i => i.Flag == "PF").Sum(i => i.Arrear);
                        taxDetailInTax.ProjectedAmount = taxDetails.Where(i => i.Flag == "PF").Sum(i => i.ProjectedAmount);
                        taxDetailInTax.ReviewAmount = taxDetails.Where(i => i.Flag == "PF").Sum(i => i.ReviewAmount);
                        taxDetailInTax.RemainFiscalYearMonth = taxDetails.FirstOrDefault(i => i.AllowanceNameId == item).RemainFiscalYearMonth;
                    }
                    else
                    {
                        taxDetailInTax.AllowanceConfigId = taxDetails.First(i => i.AllowanceNameId == item).AllowanceConfigId;
                        taxDetailInTax.ProjectRestYear = taxDetails.FirstOrDefault(i => i.AllowanceNameId == item).ProjectRestYear;
                        taxDetailInTax.OnceOffDeduction = taxDetails.FirstOrDefault(i => i.AllowanceNameId == item).OnceOffDeduction;
                        taxDetailInTax.TillAmount = taxDetails.Where(i => i.AllowanceNameId == item).Sum(i => i.TillAmount);
                        taxDetailInTax.CurrentAmount = taxDetails.Where(i => i.AllowanceNameId == item).Sum(i => i.CurrentAmount);
                        taxDetailInTax.Amount = taxDetails.Where(i => i.AllowanceNameId == item).Sum(i => i.Amount);
                        taxDetailInTax.Arrear = taxDetails.Where(i => i.AllowanceNameId == item).Sum(i => i.Arrear);
                        taxDetailInTax.ProjectedAmount = taxDetails.Where(i => i.AllowanceNameId == item).Sum(i => i.ProjectedAmount);
                        taxDetailInTax.ReviewAmount = taxDetails.Where(i => i.AllowanceNameId == item).Sum(i => i.ReviewAmount);
                        taxDetailInTax.RemainFiscalYearMonth = taxDetails.FirstOrDefault(i => i.AllowanceNameId == item).RemainFiscalYearMonth;
                        taxDetailInTax.TillAdjustment = taxDetails.Where(i => i.AllowanceNameId == item).Sum(i => i.TillAdjustment);
                        taxDetailInTax.CurrentAdjustment = taxDetails.Where(i => i.AllowanceNameId == item).Sum(i => i.CurrentAdjustment);

                        taxDetailInTax.TillAmount = taxDetailInTax.TillAmount - taxDetailInTax.TillAdjustment;
                        taxDetailInTax.CurrentAmount = taxDetailInTax.CurrentAmount - taxDetailInTax.CurrentAdjustment;
                    }

                    details.Add(taxDetailInTax);
                }

                var deductions = taxDetails.Where(item => item.DeductionNameId > 0).Select(i => i.DeductionNameId ?? 0).Distinct();
                foreach (var item in deductions)
                {
                    count++;
                    var flag = taxDetails.First(i => (i.DeductionNameId ?? 0) == item).Flag;
                    var deductionName = taxDetails.First(i => i.DeductionNameId == item).DeductionName;
                    TaxDetailInTaxProcess taxDetailInTax = new TaxDetailInTaxProcess();
                    taxDetailInTax.EmployeeId = employee.EmployeeId;
                    taxDetailInTax.DeductionNameId = item;
                    taxDetailInTax.DeductionName = deductionName;
                    taxDetailInTax.AllowanceNameId = 999999999 * count;
                    taxDetailInTax.AllowanceName = deductionName;
                    taxDetailInTax.Flag = flag == "" || flag == null ? deductionName : flag;

                    //taxDetailInTax.AllowanceConfigId = taxDetails.First(i => i.AllowanceNameId == item).AllowanceConfigId;
                    taxDetailInTax.ProjectRestYear = taxDetails.FirstOrDefault(i => i.DeductionNameId == item).ProjectRestYear;
                    taxDetailInTax.OnceOffDeduction = taxDetails.FirstOrDefault(i => i.DeductionNameId == item).OnceOffDeduction;
                    taxDetailInTax.TillAmount = taxDetails.Where(i => i.DeductionNameId == item).Sum(i => i.TillAmount);
                    taxDetailInTax.CurrentAmount = taxDetails.Where(i => i.DeductionNameId == item).Sum(i => i.CurrentAmount);
                    taxDetailInTax.Amount = taxDetails.Where(i => i.DeductionNameId == item).Sum(i => i.Amount);
                    taxDetailInTax.Arrear = taxDetails.Where(i => i.DeductionNameId == item).Sum(i => i.Arrear);
                    taxDetailInTax.ProjectedAmount = taxDetails.Where(i => i.DeductionNameId == item).Sum(i => i.ProjectedAmount);
                    taxDetailInTax.ReviewAmount = taxDetails.Where(i => i.DeductionNameId == item).Sum(i => i.ReviewAmount);
                    taxDetailInTax.RemainFiscalYearMonth = taxDetails.FirstOrDefault(i => i.DeductionNameId == item).RemainFiscalYearMonth;

                    details.Add(taxDetailInTax);
                }
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ExecuteTaxProcess", "MargeTaxDetailsAsync", user);
            }
            return details;
        }
        public async Task<ExecutionStatus> SaveTaxAsync(List<EmployeeTaxProcessedInfo> taxProcessInfos, FiscalYear fiscalYear, int month, int year, bool effectOnSalary, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                var salaryProcessInfo = (_payrollDbContext.Payroll_SalaryProcess.FirstOrDefault(i => i.SalaryProcessId == (taxProcessInfos[0].EmployeeTaxProcess.SalaryProcessId ?? 0) && i.CompanyId == user.CompanyId && i.OrganizationId == user.OrganizationId));
                using (var transaction = await _payrollDbContext.Database.BeginTransactionAsync())
                {
                    try
                    {
                        decimal totalMonthlyTax = 0;
                        decimal totalProjectionTax = 0;
                        decimal totalOnceOffTax = 0;
                        decimal totalTaxDeducted = 0;
                        //
                        if (salaryProcessInfo != null && taxProcessInfos != null && taxProcessInfos.Any())
                        {
                            foreach (var item in taxProcessInfos)
                            {

                                var salaryProcessDetail = (_payrollDbContext.Payroll_SalaryProcessDetail.FirstOrDefault(i => i.SalaryProcessDetailId == item.EmployeeTaxProcess.SalaryProcessDetailId && i.CompanyId == user.CompanyId && i.OrganizationId == user.OrganizationId));

                                if (salaryProcessDetail != null)
                                {
                                    item.EmployeeTaxProcess.SalaryProcessId = salaryProcessInfo.SalaryProcessId;
                                    item.EmployeeTaxProcess.SalaryProcessDetailId = salaryProcessDetail.SalaryProcessDetailId;

                                    await _payrollDbContext.Payroll_EmployeeTaxProcess.AddAsync(item.EmployeeTaxProcess);
                                    if (await _payrollDbContext.SaveChangesAsync() > 0)
                                    {
                                        item.EmployeeTaxProcessDetails.ForEach(i =>
                                        {
                                            i.TaxProcessId = item.EmployeeTaxProcess.TaxProcessId;
                                            i.SalaryProcessId = salaryProcessInfo.SalaryProcessId;
                                            i.SalaryProcessDetailId = salaryProcessDetail.SalaryProcessDetailId;
                                        });

                                        await _payrollDbContext.Payroll_EmployeeTaxProcessDetail.AddRangeAsync(item.EmployeeTaxProcessDetails);
                                        if (await _payrollDbContext.SaveChangesAsync() == 0)
                                        {
                                            throw new Exception($"tax detail failed to insert at employee id {salaryProcessDetail.EmployeeId.ToString()}");
                                        }

                                        item.EmployeeTaxProcessSlabs.ForEach(i =>
                                        {
                                            i.TaxProcessId = item.EmployeeTaxProcess.TaxProcessId;
                                        });
                                        await _payrollDbContext.Payroll_EmployeeTaxProcessSlab.AddRangeAsync(item.EmployeeTaxProcessSlabs);

                                        if (await _payrollDbContext.SaveChangesAsync() == 0)
                                        {
                                            throw new Exception($"tax slabs failed to insert at employee id {salaryProcessDetail.EmployeeId.ToString()}");
                                        }
                                    }
                                    else
                                    {
                                        throw new Exception($"tax info failed to insert at employee id {salaryProcessDetail.EmployeeId.ToString()}");
                                    }

                                    if (effectOnSalary)
                                    {
                                        var holdAmount = salaryProcessDetail.HoldAmount;
                                        var supplementaryOnceffTax = item.EmployeeTaxProcess.SupplementaryOnceffTax ?? 0;
                                        if (supplementaryOnceffTax > 0 && item.EmployeeTaxProcess.OnceOffTax - supplementaryOnceffTax == 1)
                                        {

                                        }
                                        var taxDeductedAmount = (item.EmployeeTaxProcess.ActualTaxDeductionAmount ?? 0) - (item.EmployeeTaxProcess.SupplementaryOnceffTax ?? 0);
                                        salaryProcessDetail.HoldAmount = salaryProcessDetail.HoldAmount > 0 ? salaryProcessDetail.HoldAmount + salaryProcessDetail.TaxDeductedAmount - taxDeductedAmount : 0;

                                        if (salaryProcessDetail.NetPay == 0 && salaryProcessDetail.HoldAmount > 0)
                                        {
                                            salaryProcessDetail.NetPay = 0;
                                            salaryProcessDetail.TaxDeductedAmount = taxDeductedAmount;
                                        }
                                        else
                                        {
                                            salaryProcessDetail.NetPay = salaryProcessDetail.NetPay + salaryProcessDetail.TaxDeductedAmount - ((item.EmployeeTaxProcess.ActualTaxDeductionAmount ?? 0) - (item.EmployeeTaxProcess.SupplementaryOnceffTax ?? 0));
                                            salaryProcessDetail.TaxDeductedAmount = taxDeductedAmount;
                                        }

                                        salaryProcessDetail.OnceOffTax = (item.EmployeeTaxProcess.OnceOffTax ?? 0) - (item.EmployeeTaxProcess.SupplementaryOnceffTax ?? 0);
                                        salaryProcessDetail.OnceOffTax = salaryProcessDetail.OnceOffTax == -1 ? 0 : salaryProcessDetail.OnceOffTax;
                                        salaryProcessDetail.ProjectionTax = item.EmployeeTaxProcess.ProjectionTax ?? 0;
                                        salaryProcessDetail.TotalMonthlyTax = item.EmployeeTaxProcess.MonthlyTax - (item.EmployeeTaxProcess.SupplementaryOnceffTax ?? 0);
                                        salaryProcessDetail.UpdatedBy = user.ActionUserId;
                                        salaryProcessDetail.UpdatedDate = DateTime.Now;

                                        totalMonthlyTax = totalMonthlyTax + salaryProcessDetail.TotalMonthlyTax;
                                        totalProjectionTax = totalProjectionTax + salaryProcessDetail.ProjectionTax;
                                        totalOnceOffTax = totalOnceOffTax + salaryProcessDetail.OnceOffTax;
                                        totalTaxDeducted = totalTaxDeducted + salaryProcessDetail.TaxDeductedAmount;

                                        _payrollDbContext.Payroll_SalaryProcessDetail.Update(salaryProcessDetail);
                                        if (await _payrollDbContext.SaveChangesAsync() == 0)
                                        {
                                            throw new Exception($"Salary detail info failed to update at employee id {salaryProcessDetail.EmployeeId.ToString()}");
                                        }
                                    }

                                }
                            }

                            if (effectOnSalary)
                            {
                                salaryProcessInfo.TotalMonthlyTax = totalMonthlyTax;
                                salaryProcessInfo.TotalProjectionTax = totalProjectionTax;
                                salaryProcessInfo.TotalOnceOffTax = totalOnceOffTax;
                                salaryProcessInfo.TotalTaxDeductedAmount = totalTaxDeducted;

                                _payrollDbContext.Payroll_SalaryProcess.Update(salaryProcessInfo);
                                if (await _payrollDbContext.SaveChangesAsync() > 0)
                                {
                                    await transaction.CommitAsync();
                                    executionStatus = ResponseMessage.Message(true, ResponseMessage.Successfull);
                                }
                            }
                            else
                            {
                                await transaction.CommitAsync();
                                executionStatus = ResponseMessage.Message(true, ResponseMessage.Successfull);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        if (ex.InnerException != null)
                        {
                            await transaction.RollbackAsync();
                            executionStatus = ResponseMessage.Message(false, ResponseMessage.SomthingWentWrong);
                        }
                        else
                        {
                            await transaction.RollbackAsync();
                            executionStatus = ResponseMessage.Message(false, ex.Message);
                        }

                    }
                }

            }
            catch (Exception ex)
            {
                executionStatus = ResponseMessage.Message(false, ex.Message);
                await _sysLogger.SaveHRMSException(ex, user.Database, "ExecuteTaxProcess", "SaveTaxAsync", user);
            }
            return executionStatus;
        }
        public async Task<IEnumerable<DeductableIncomeOfThisEmployee>> DeductableIncomeOfThisEmployeeAsync(EligibleEmployeeForTaxType employee, FiscalYear fiscalYear, int month, int year, AppUser user)
        {
            IEnumerable<DeductableIncomeOfThisEmployee> list = new List<DeductableIncomeOfThisEmployee>();
            try
            {
                var query = $@"SELECT DISTINCT DN.DeductionNameId,DeductionName=DN.[Name],
                DeductionFlag=DN.Flag,IsOnceOff=DC.IsOnceOffTax,IsProjected=DC.ProjectRestYear
                FROM Payroll_SalaryDeduction SD
                INNER JOIN Payroll_DeductionName DN ON SD.DeductionNameId = DN.DeductionNameId
                INNER JOIN Payroll_DeductionConfiguration DC ON DN.DeductionNameId=DC.DeductionNameId
                WHERE 1=1
                AND SD.EmployeeId=@EmployeeId
                AND SD.CompanyId=@CompanyId AND SD.OrganizationId=@OrganizationId
                AND dbo.fnGetFirstDateOfAMonth(SD.SalaryYear,SD.SalaryMonth) BETWEEN @FiscalYearFrom AND @FiscalYearTo";

                list = await _dapper.SqlQueryListAsync<DeductableIncomeOfThisEmployee>(user.Database, query, new
                {
                    employee.EmployeeId,
                    FiscalYearFrom = fiscalYear.FiscalYearFrom.Value.ToString("yyyy-MM-dd"),
                    FiscalYearTo = fiscalYear.FiscalYearTo.Value.ToString("yyyy-MM-dd"),
                    user.OrganizationId
                });

                if (list.Any())
                {
                    foreach (var item in list)
                    {
                        item.TillAmount = await GetDeductableTillAmountAsync(employee.EmployeeId, item.DeductionNameId, fiscalYear, month, year, user);
                        item.CurrentAmount = await GetDeductableCurrentAmountAsync(employee.EmployeeId, item.DeductionNameId, fiscalYear, month, year, user);
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return list;
        }
        public async Task<decimal> GetDeductableTillAmountAsync(long employeeId, long deductionNameId, FiscalYear fiscalYear, int month, int year, AppUser user)
        {
            decimal amount = 0;
            try
            {
                var query = $@"SELECT TillAmount=SUM(Amount) FROM Payroll_SalaryDeduction
                Where 1=1
                AND DeductionNameId=@DeductionNameId
                AND EmployeeId=@EmployeeId
                AND dbo.fnGetFirstDateOfAMonth(SalaryYear,SalaryMonth) < dbo.fnGetFirstDateOfAMonth(@SalaryYear,@SalaryMonth)
                AND dbo.fnGetFirstDateOfAMonth(SalaryYear,SalaryMonth) BETWEEN @FiscalYearFrom AND @FiscalYearTo
                AND CompanyId=@CompanyId
                AND OrganizationId=@OrganizationId
                ";

                amount = await _dapper.SqlQueryFirstAsync<decimal>(user.Database, query, new
                {
                    EmployeeId = employeeId,
                    DeductionNameId = deductionNameId,
                    SalaryYear = year,
                    SalaryMonth = month,
                    FiscalYearFrom = fiscalYear.FiscalYearFrom.Value.ToString("yyyy-MM-dd"),
                    FiscalYearTo = fiscalYear.FiscalYearTo.Value.ToString("yyyy-MM-dd"),
                    user.CompanyId,
                    user.OrganizationId
                });
            }
            catch (Exception ex)
            {

            }
            return amount;
        }
        public async Task<decimal> GetDeductableCurrentAmountAsync(long employeeId, long deductionNameId, FiscalYear fiscalYear, int month, int year, AppUser user)
        {
            decimal amount = 0;
            try
            {
                var query = $@"SELECT CurrentAmount=SUM(Amount) FROM Payroll_SalaryDeduction
                Where 1=1
                AND DeductionNameId=@DeductionNameId
                AND EmployeeId=@EmployeeId
                AND SalaryMonth=@SalaryMonth
                AND SalaryYear=@SalaryYear
                AND CompanyId=@CompanyId
                AND OrganizationId=@OrganizationId";

                amount = await _dapper.SqlQueryFirstAsync<decimal>(user.Database, query, new
                {
                    DeductionNameId = deductionNameId,
                    EmployeeId = employeeId,
                    SalaryYear = year,
                    SalaryMonth = month,
                    user.CompanyId,
                    user.OrganizationId
                });
            }
            catch (Exception ex)
            {

            }
            return amount;
        }
        public async Task<decimal> GetAllowanceTillAdjustmentAsync(long employeeId, long allowanceNameId, FiscalYear fiscalYear, int month, int year, AppUser user)
        {
            decimal tillAmount = 0;
            try
            {
                var firstDateOfThisMonth = DateTimeExtension.FirstDateOfAMonth(year, month).ToString("yyyy-MM-dd");
                var query = $@"SELECT ISNULL((Select SUM(AdjustmentAmount) 
	            From Payroll_SalaryAllowance
	            Where AllowanceNameId=@AllowanceNameId 
	            AND EmployeeId=@EmployeeId 
	            AND SalaryDate < @FirstDateOfThisMonth 
	            AND (SalaryDate Between @FiscalYearFrom AND @FiscalYearTo)
	            AND CompanyId=@CompanyId
	            AND OrganizationId=@OrganizationId),0)";

                tillAmount = await _dapper.SqlQueryFirstAsync<decimal>(user.Database, query, new { AllowanceNameId = allowanceNameId, EmployeeId = employeeId, FirstDateOfThisMonth = firstDateOfThisMonth, fiscalYear.FiscalYearFrom, fiscalYear.FiscalYearTo, user.CompanyId, user.OrganizationId });
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ExecuteTaxProcess", "GetAllowanceTillAdjustmentAsync", user);
            }
            return tillAmount;
        }
        public async Task<decimal> GetAllowanceCurrentAdjustmentAsync(long employeeId, long allowanceNameId, FiscalYear fiscalYear, int month, int year, AppUser user)
        {
            decimal currentAmount = 0;
            try
            {
                var firstDateOfThisMonth = DateTimeExtension.FirstDateOfAMonth(year, month).ToString("yyyy-MM-dd");
                var query = $@"SELECT ISNULL((Select SUM(AdjustmentAmount) 
	            From Payroll_SalaryAllowance
	            Where AllowanceNameId=@AllowanceNameId 
	            AND EmployeeId=@EmployeeId 
	            AND MONTH(SalaryDate) = MONTH(@FirstDateOfThisMonth)
                AND YEAR(SalaryDate) = YEAR(@FirstDateOfThisMonth)
	            AND (SalaryDate Between @FiscalYearFrom AND @FiscalYearTo)
	            AND CompanyId=@CompanyId
	            AND OrganizationId=@OrganizationId),0)";

                currentAmount = await _dapper.SqlQueryFirstAsync<decimal>(user.Database, query, new { AllowanceNameId = allowanceNameId, EmployeeId = employeeId, FirstDateOfThisMonth = firstDateOfThisMonth, fiscalYear.FiscalYearFrom, fiscalYear.FiscalYearTo, user.CompanyId, user.OrganizationId });
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ExecuteTaxProcess", "GetAllowanceCurrentAdjustmentAsync", user);
            }
            return currentAmount;
        }
    }
}
