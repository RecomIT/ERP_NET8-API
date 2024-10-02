using BLL.Base.Interface;
using BLL.Salary.Allowance.Interface;
using BLL.Salary.Payment.Interface;
using BLL.Salary.Salary.Interface;
using BLL.Tax.Interface;
using DAL.DapperObject.Interface;
using DAL.Payroll.Repository.Interface;
using Shared.Control_Panel.Domain;
using Shared.OtherModels.User;
using Shared.Payroll.Domain.Payment;
using Shared.Payroll.Domain.Setup;
using Shared.Payroll.Filter.Salary;
using Shared.Payroll.Process.Allowance;
using Shared.Payroll.Process.Tax;
using Shared.Payroll.ViewModel.Payment;
using Shared.Payroll.ViewModel.Salary;
using Shared.Services;

namespace BLL.Tax.Implementation
{
    public class TaxService : ITaxService
    {
        private readonly ISalaryProcessBusiness _salaryProcessBusiness;
        private readonly ISalaryReviewBusiness _salaryReviewBusiness;
        private readonly IConditionalDepositAllowanceConfigRepository _conditionalDepositAllowanceConfigRepository;
        private readonly IConditionalProjectedPaymentBusiness _conditionalProjectedPaymentBusiness;
        private readonly IDapperData _dapper;
        private readonly ISysLogger _sysLogger;
        private readonly string serviceName;
        private readonly IAllowanceNameBusiness _allowanceNameBusiness;
        private readonly IProjectedPaymentBusiness _projectedPaymentBusiness;

        public TaxService(
            IDapperData dapper,
            ISysLogger sysLogger,
            ISalaryProcessBusiness salaryProcessBusiness,
            ISalaryReviewBusiness salaryReviewBusiness,
            IConditionalDepositAllowanceConfigRepository conditionalDepositAllowanceConfigRepository,
            IConditionalProjectedPaymentBusiness conditionalProjectedPaymentBusiness,
            IAllowanceNameBusiness allowanceNameBusiness,
            IProjectedPaymentBusiness projectedPaymentBusiness)
        {
            _dapper = dapper;
            serviceName = "TaxService";
            _sysLogger = sysLogger;
            _salaryProcessBusiness = salaryProcessBusiness;
            _salaryReviewBusiness = salaryReviewBusiness;
            _conditionalDepositAllowanceConfigRepository = conditionalDepositAllowanceConfigRepository;
            _conditionalProjectedPaymentBusiness = conditionalProjectedPaymentBusiness;
            _allowanceNameBusiness = allowanceNameBusiness;
            _projectedPaymentBusiness = projectedPaymentBusiness;
        }
        public bool IsThisEmployeeDiscontinuedWithinThisFiscalYear(EligibleEmployeeForTaxType employee, FiscalYear fiscalYearInfo)
        {
            bool isThisEmployeeDiscontinuedWithinThisFiscalYear = false;
            if (employee.TerminationDate.HasValue)
            {
                isThisEmployeeDiscontinuedWithinThisFiscalYear = employee.TerminationDate == null ? false : employee.TerminationDate.Value.IsDateBetweenTwoDates(fiscalYearInfo.FiscalYearFrom.Value.Date, fiscalYearInfo.FiscalYearTo.Value.Date);
            }
            return isThisEmployeeDiscontinuedWithinThisFiscalYear;
        }
        public int RemainProjectionMonth(DateTime firstDateOfThisMonth, DateTime fiscalYearTo)
        {
            int remainFiscalYearMonth = firstDateOfThisMonth.GetMonthDiffExcludingThisMonth(fiscalYearTo);
            return remainFiscalYearMonth;
        }
        public int RemainProjectionMonthForThisEmployee(EligibleEmployeeForTaxType employee, FiscalYear fiscalYearInfo, DateTime firstDateOfThisMonth, int remainFiscalYearMonth)
        {
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
            return remainProjectionMonthForThisEmployee;
        }
        public async Task<int> CountOfSalaryReceipt(long employeeId, long fiscalYearId, short month, AppUser user)
        {
            var countOfSalaryReceiptInThisFiscalYear =
                await _salaryProcessBusiness.GetSalaryReceiptInThisFiscalYearAsync(employeeId, fiscalYearId, month, user);
            return countOfSalaryReceiptInThisFiscalYear;
        }
        public async Task<long> LastSalaryReviewId(EligibleEmployeeForTaxType employee, int paymentYear, int paymentMonth, AppUser user)
        {
            long lastSalaryReviewId = 0;
            var salaryReviewIds = employee.SalaryReviewInfoIds;
            if (salaryReviewIds != null)
            {
                var ids = salaryReviewIds.Split(',');
                var intIds = ids.Select(id => Utility.TryParseInt(id.Trim()));
                lastSalaryReviewId = intIds.Max();
            }

            if (lastSalaryReviewId == 0)
            {
                // Find Salary Review Info
                var lastSalaryReviewInfo = await _salaryReviewBusiness.GetLastSalaryReviewAccordingToCutOffDate(employee.EmployeeId, DateTimeExtension.LastDateOfAMonth(paymentYear, paymentMonth).ToString("yyyy-MM-dd"), user);
                lastSalaryReviewId = lastSalaryReviewInfo.SalaryReviewInfoId;
            }
            return lastSalaryReviewId;
        }
        public async Task<IEnumerable<SalaryReviewDetailViewModel>> GetSalaryReviewDetailsAsync(long employeeId, long salaryReviewId, AppUser user)
        {
            IEnumerable<SalaryReviewDetailViewModel> list = new List<SalaryReviewDetailViewModel>();
            try
            {
                list = await _salaryReviewBusiness.GetSalaryReviewDetailsAsync(new SalaryReview_Filter()
                {
                    EmployeeId = employeeId.ToString(),
                    SalaryReviewInfoId = salaryReviewId.ToString()
                }, user);
            }
            catch (Exception ex)
            {

            }
            return list;
        }
        public async Task<decimal> GetAllowanceTillAmountAsync(long employeeId, long allowanceNameId, FiscalYear fiscalYear, int month, int year, AppUser user)
        {
            decimal tillAmount = 0;
            try
            {
                var query = $@"SELECT ISNULL((Select SUM((Amount+AdjustmentAmount+ArrearAmount)) 
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

                // Find in Deposit History
                query = $@"SELECT ISNULL((SELECT SUM(ISNULL(Amount,0)) FROM Payroll_DepositAllowanceHistory 
                Where dbo.fnGetLastDateOfAMonth(DepositYear,DepositMonth) < dbo.fnGetLastDateOfAMonth(@Year,@Month) 
                AND FiscalYearId=@FiscalYearId AND AllowanceNameId=@AllowanceNameId AND EmployeeId=@EmployeeId AND CompanyId=@CompanyId AND OrganizationId=@OrganizationId),0)";

                var tillAmount3 = await _dapper.SqlQueryFirstAsync<decimal>(user.Database, query, new
                {
                    EmployeeId = employeeId,
                    AllowanceNameId = allowanceNameId,
                    fiscalYear.FiscalYearId,
                    Year = year,
                    Month = month,
                    user.CompanyId,
                    user.OrganizationId
                });


                tillAmount = tillAmount + tillAmount3;

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

                var tillAmount4 = await _dapper.SqlQueryFirstAsync<decimal>(user.Database, query, new
                {
                    EmployeeId = employeeId,
                    AllowanceNameId = allowanceNameId,
                    Month = month,
                    fiscalYear.FiscalYearId,
                    user.CompanyId,
                    user.OrganizationId
                });

                tillAmount = tillAmount + tillAmount4;
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
                currentAmount = await _dapper.SqlQueryFirstAsync<decimal>(user.Database, query.Trim(), new { AllowanceNameId = allowanceNameId, EmployeeId = employeeId, FirstDateOfThisMonth = firstDateOfThisMonth, fiscalYear.FiscalYearFrom, fiscalYear.FiscalYearTo, user.CompanyId, user.OrganizationId });

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
                    user.CompanyId,
                    user.OrganizationId
                });

                query = $@"SELECT ISNULL((SELECT SUM(ISNULL(Amount,0)) FROM Payroll_DepositAllowanceHistory 
                Where DepositYear=@Year AND DepositMonth=@Month AND FiscalYearId=@FiscalYearId 
                AND AllowanceNameId=@AllowanceNameId AND EmployeeId=@EmployeeId AND CompanyId=@CompanyId AND OrganizationId=@OrganizationId),0)";

                var currentAmount3 = await _dapper.SqlQueryFirstAsync<decimal>(user.Database, query, new
                {
                    EmployeeId = employeeId,
                    AllowanceNameId = allowanceNameId,
                    fiscalYear.FiscalYearId,
                    Year = year,
                    Month = month,
                    user.CompanyId,
                    user.OrganizationId
                });

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
        public async Task<IEnumerable<TaxDetailInTaxProcess>> AllowancesEarnedByThisEmployeeByThisFiscalYearAsync(EligibleEmployeeForTaxType employee, FiscalYear fiscalYear, int month, int year, long lastSalaryReviewId, IEnumerable<SalaryReviewDetailViewModel> salaryReviewDetails, int remainProjectionMonthForThisEmployee, AppUser user)
        {
            IEnumerable<TaxDetailInTaxProcess> taxDetails = new List<TaxDetailInTaxProcess>();
            IEnumerable<AllowancesEarnedByThisEmployee> list = new List<AllowancesEarnedByThisEmployee>();
            try
            {
                var query = $@"SELECT Distinct ALW.AllowanceHeadId,ALW.AllowanceNameId,[AllowanceName]=ALW.Name,AllowanceFlag=Flag,IsProjected=ISNULL(ProjectRestYear,0),IsOnceOff=ISNULL(IsOnceOffTax,0) FROM (Select Distinct AllowanceNameId From Payroll_SalaryAllowance
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
                SELECT Distinct AllowanceNameId FROM Payroll_DepositAllowanceHistory
                Where EmployeeId=@EmployeeId AND FiscalYearId=@FiscalYearId AND CompanyId=@CompanyId AND OrganizationId=@OrganizationId

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

                    taxDetails = list.Select(i => new TaxDetailInTaxProcess()
                    {
                        AllowanceHeadId = i.AllowanceHeadId,
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
                        TillAdjustment=i.TillAdjustment,
                        CurrentAdjustment=i.CurrentAdjustment
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ExecuteTaxProcess", "AllowancesEarnedByThisEmployeeByThisFiscalYearAsync", user);
            }
            return taxDetails;
        }
        public async Task<EmployeeDepositPaymentViewModel> GetEmployeeAllowanceAccuredAmountInTaxProcessAsync(long employeeId, long allowanceNameId, short year, short month, long fiscalYearId, AppUser user)
        {
            var accuredAllowance = await _conditionalDepositAllowanceConfigRepository.GetEmployeeAllowanceAccuredAmountInTaxProcessAsync(employeeId, allowanceNameId, year, month, fiscalYearId, user);
            return accuredAllowance;
        }
        public async Task<IEnumerable<TaxDetailInTaxProcess>> ProjectionCalculation(EligibleEmployeeForTaxType employee, IEnumerable<TaxDetailInTaxProcess> earnedByThisEmployees, FiscalYear fiscalYearInfo, int remainProjectionMonthForThisEmployee, AllowanceInfo allowanceInfo, int year, int month, AppUser user)
        {
            IEnumerable<TaxDetailInTaxProcess> totalEarned = earnedByThisEmployees;
            var isThisEmployeeDiscontinuedWithinThisFiscalYear = IsThisEmployeeDiscontinuedWithinThisFiscalYear(employee, fiscalYearInfo);
            foreach (var item in totalEarned)
            {
                var reviewAmount = item.ReviewAmount > 0 ? item.ReviewAmount : item.CurrentAmount;
                bool isCalculateProjectionTaxProratedBasis = false;
                // Wounderment Thompson Yearly Bonus && Festival Bonus
                if (user.OrganizationId == 11 && user.CompanyId == 19 && (item.AllowanceNameId == 7 || item.AllowanceNameId == 9))
                {

                }
                else
                {
                    if ((item.ProjectRestYear ?? false) == true && employee.IsDiscontinued == false && isThisEmployeeDiscontinuedWithinThisFiscalYear == false && (employee.CalculateProjectionTaxProratedBasis ?? false) == false && item.Flag != "FESTIVAL BONUS")
                    {
                        item.ProjectedAmount = reviewAmount * remainProjectionMonthForThisEmployee;
                    }
                    else if ((item.ProjectRestYear ?? false) == true && employee.IsDiscontinued == false && isThisEmployeeDiscontinuedWithinThisFiscalYear == true && (employee.CalculateProjectionTaxProratedBasis ?? false) == false && item.Flag != "FESTIVAL BONUS")
                    {
                        item.ProjectedAmount = reviewAmount * remainProjectionMonthForThisEmployee;
                    }
                    else if ((item.ProjectRestYear ?? false) == true && employee.IsDiscontinued == false && isThisEmployeeDiscontinuedWithinThisFiscalYear == true && (employee.CalculateProjectionTaxProratedBasis ?? false) == true && item.Flag != "FESTIVAL BONUS")
                    {
                        isCalculateProjectionTaxProratedBasis = true;
                        item.ProjectedAmount = await GetProjectionAmountWhenDiscontinued(employee, reviewAmount, fiscalYearInfo, DateTimeExtension.LastDateOfAMonth(year, month), user);
                    }
                }
            }

            return totalEarned;
        }
        public async Task<decimal> GetProjectionAmountWhenDiscontinued(EligibleEmployeeForTaxType employee, decimal amount, FiscalYear fiscalYear, DateTime startDate, AppUser user)
        {
            decimal projectedAmount = 0;
            try
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
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, serviceName, "GetProjectionAmountWhenDiscontinued", user);
            }
            return Math.Round(projectedAmount, 0);
        }
        public async Task<List<TaxDetailInTaxProcess>> IncomeDetails(EligibleEmployeeForTaxType employee, FiscalYear fiscalYearInfo, List<TaxDetailInTaxProcess> earnings, List<SalaryReviewDetailViewModel> salaryReviewDetails, AllowanceInfo allowanceInfo, PayrollModuleConfig payrollModuleConfig, int year, int month, AppUser user)
        {
            List<TaxDetailInTaxProcess> earnedAllowances = earnings;
            var currentGross = salaryReviewDetails.Sum(i => i.CurrentAmount);
            var currentBasic = salaryReviewDetails.Where(i => i.AllowanceNameId == allowanceInfo.BasicAllowance).Sum(i => i.CurrentAmount);
            bool isCalculateProjectionTaxProratedBasis = false;
            if (employee.IsThisEmployeeDiscontinuedWithinThisFiscalYear == true && (employee.CalculateProjectionTaxProratedBasis ?? false) == true)
            {
                isCalculateProjectionTaxProratedBasis = true;
            }
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
                    EmployeeId = employee.EmployeeId,
                    GradeId = employee.GradeId,
                    DepartmentId = employee.DepartmentId,
                    DesignationId = employee.DesignationId,
                    InternalDesignationId = employee.InternalDesignationId,
                    SectionId = employee.SectionId,
                    SubSectionId = employee.SubSectionId,
                    UnitId = employee.UnitId,
                    BranchId = employee.BranchId,
                    JobCategoryId = employee.JobCategoryId,
                    EmployeeTypeId = employee.EmployeeTypeId
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
                                            RemainFiscalYearMonth = (short)employee.RemainProjectionMonthForThisEmployee,
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
                                        RemainFiscalYearMonth = (short)employee.RemainProjectionMonthForThisEmployee,
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
                                                RemainFiscalYearMonth = (short)employee.RemainProjectionMonthForThisEmployee,
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
                            RemainFiscalYearMonth = (short)employee.RemainProjectionMonthForThisEmployee,
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
                            if (item.BaseOfPayment == "Basic" || item.BaseOfPayment == "Gross")
                            {
                                var projectedAllowanceInfo = await _allowanceNameBusiness.GetAllowanceNameByIdAsync(item.AllowanceNameId, user);
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
                                            RemainFiscalYearMonth = (short)employee.RemainProjectionMonthForThisEmployee,
                                            LessExemptedAmount = 0,
                                            TotalTaxableIncome = 0,
                                            GrossAnnualIncome = 0
                                        });
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
                                            RemainFiscalYearMonth = (short)employee.RemainProjectionMonthForThisEmployee,
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
                            RemainFiscalYearMonth = (short)employee.RemainProjectionMonthForThisEmployee,
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
                            RemainFiscalYearMonth = (short)employee.RemainProjectionMonthForThisEmployee,
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
                        employeePFAllowance.CurrentAmount = employeePFAllowance.CurrentAmount == 0 ? baseAmount : employeePFAllowance.CurrentAmount;
                        employeePFAllowance.Amount = employeePFAllowance.Amount == 0 ? baseAmount : employeePFAllowance.Amount;
                        employeePFAllowance.ReviewAmount = baseAmount;
                        employeePFAllowance.RemainFiscalYearMonth = (short)employee.RemainProjectionMonthForThisEmployee;
                        if (user.CompanyId == 17 && user.OrganizationId == 10 && employee.EmployeeId == 792)
                        {
                            employeePFAllowance.ProjectedAmount = employee.IsDiscontinued == false ? 79000 : 0;
                        }
                        else
                        {
                            employeePFAllowance.ProjectedAmount = employee.IsDiscontinued == false ? baseAmount * employee.RemainProjectionMonthForThisEmployee : 0;
                            employeePFAllowance.ProjectedAmount = Math.Round(employeePFAllowance.ProjectedAmount, MidpointRounding.AwayFromZero);
                        }

                        earnedAllowances.Add(employeePFAllowance);
                    }
                }
            }

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
                        RemainFiscalYearMonth = (short)employee.RemainProjectionMonthForThisEmployee,
                        ProjectedAmount = 0,
                        LessExemptedAmount = 0,
                        TotalTaxableIncome = 0,
                        GrossAnnualIncome = 0
                    };
                    earnedAllowances.Add(deduction);
                }
            }

            var margedTaxDetails = await MargeTaxDetailsAsync(employee, earnedAllowances, user);
            return margedTaxDetails.ToList();
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
                    var allowanceHead = taxDetails.FirstOrDefault(i => i.AllowanceNameId == item);
                    if (allowanceHead != null)
                    {
                        taxDetailInTax.AllowanceHeadId = allowanceHead.AllowanceHeadId;
                    }
                    taxDetailInTax.AllowanceName = allowanceName;
                    taxDetailInTax.TaxItem = allowanceName;
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
                        taxDetailInTax.TillAdjustment =0;
                        taxDetailInTax.CurrentAdjustment = 0;
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
        public async Task<TaxDetailInTaxProcess> GetEmployeePFAmountAsync(EligibleEmployeeForTaxType employee, FiscalYear fiscalYear, int year, int month, AppUser user)
        {
            TaxDetailInTaxProcess taxDetailInTax = new TaxDetailInTaxProcess();
            try
            {
                //var query = $@"SELECT 
                //TillAmount=(SELECT SUM(ISNULL(PFAmount,0)+ISNULL(PFArrear,0)) FROM Payroll_SalaryProcessDetail 
                //Where EmployeeId=@EmployeeId AND SalaryMonth<>@Month AND FiscalYearId=@FiscalYearId AND CompanyId=@CompanyId AND OrganizationId=@OrganizationId),
                //CurrentAmount= ISNULL(PFAmount,0)+ISNULL(PFArrear,0),
                //Amount = ISNULL(PFAmount,0),
                //Arrear=ISNULL(PFArrear,0)
                //FROM Payroll_SalaryProcessDetail
                //Where EmployeeId=@EmployeeId AND SalaryMonth=@Month AND FiscalYearId=@FiscalYearId AND CompanyId=@CompanyId AND OrganizationId=@OrganizationId";

                var query = $@"SELECT 
                TillAmount=ISNULL((SELECT SUM(ISNULL(PFAmount,0)+ISNULL(PFArrear,0)) FROM Payroll_SalaryProcessDetail 
                Where EmployeeId=@EmployeeId AND SalaryMonth<>@Month AND FiscalYearId=@FiscalYearId AND CompanyId=@CompanyId AND OrganizationId=@OrganizationId),0),
                CurrentAmount= ISNULL((SELECT ISNULL(PFAmount,0)+ISNULL(PFArrear,0) FROM Payroll_SalaryProcessDetail 
                Where EmployeeId=@EmployeeId AND SalaryMonth=@Month AND FiscalYearId=@FiscalYearId AND CompanyId=@CompanyId AND OrganizationId=@OrganizationId),0),
                Amount = ISNULL((SELECT ISNULL(PFAmount,0) FROM Payroll_SalaryProcessDetail 
                Where EmployeeId=@EmployeeId AND SalaryMonth=@Month AND FiscalYearId=@FiscalYearId AND CompanyId=@CompanyId AND OrganizationId=@OrganizationId),0),
                Arrear=ISNULL((SELECT ISNULL(PFArrear,0) FROM Payroll_SalaryProcessDetail 
                Where EmployeeId=@EmployeeId AND SalaryMonth=@Month AND FiscalYearId=@FiscalYearId AND CompanyId=@CompanyId 
                AND OrganizationId=@OrganizationId),0)";

                taxDetailInTax = await _dapper.SqlQueryFirstAsync<TaxDetailInTaxProcess>(user.Database, query, new { employee.EmployeeId, Month = month, fiscalYear.FiscalYearId, user.CompanyId, user.OrganizationId });
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ExecuteTaxProcess", "GetEmployeePFAmountAsync", user);
            }
            return taxDetailInTax;
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
                AND dbo.fnGetFirstDateOfAMonth(@Year,@Month) BETWEEN @FiscalYearFrom AND @FiscalYearTo";

                list = await _dapper.SqlQueryListAsync<DeductableIncomeOfThisEmployee>(user.Database, query, new
                {
                    employee.EmployeeId,
                    FiscalYearFrom = fiscalYear.FiscalYearFrom.Value.ToString("yyyy-MM-dd"),
                    FiscalYearTo = fiscalYear.FiscalYearTo.Value.ToString("yyyy-MM-dd"),
                    Month = month,
                    Year = year,
                    user.CompanyId,
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
