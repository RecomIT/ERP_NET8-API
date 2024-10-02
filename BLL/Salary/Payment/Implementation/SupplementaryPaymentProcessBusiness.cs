using Dapper;
using AutoMapper;
using System.Data;
using Shared.Services;
using BLL.Base.Interface;
using Shared.OtherModels.User;
using BLL.Salary.Setup.Interface;
using Shared.OtherModels.Response;
using BLL.Salary.Salary.Interface;
using BLL.Salary.Payment.Interface;
using BLL.Salary.Allowance.Interface;
using DAL.DapperObject.Interface;
using Shared.Payroll.DTO.Payment;
using Shared.Payroll.Filter.Allowance;
using Shared.Payroll.Domain.Setup;
using Shared.Control_Panel.Domain;
using Shared.Payroll.Process.Allowance;
using Shared.Payroll.Process.Tax;
using Shared.Payroll.Domain.Payment;
using Shared.Payroll.ViewModel.Payment;
using DAL.Context.Payroll;
using BLL.Tax.Interface;
using Microsoft.Identity.Client;


namespace BLL.Salary.Payment.Implementation
{
    public class SupplementaryPaymentProcessBusiness : ISupplementaryPaymentProcessBusiness
    {
        private IDapperData _dapper;
        private ISysLogger _sysLogger;
        private readonly IFiscalYearBusiness _fiscalYearBusiness;
        private readonly PayrollDbContext _payrollDbContext;
        private readonly IModuleConfig _moduleConfig;
        private readonly ISalaryReviewBusiness _salaryReviewBusiness;
        private readonly IAllowanceNameBusiness _allowanceNameBusiness;
        private readonly ISalaryProcessBusiness _salaryProcessBusiness;
        private readonly IAllowanceConfigBusiness _allowanceConfigBusiness;
        private readonly IMonthlyAllowanceConfigBusiness _monthlyAllowanceConfigBusiness;
        private readonly ITaxService _taxService;
        private readonly ITaxRulesBusiness _taxRulesBusiness;
        private readonly IMapper _mapper;
        public SupplementaryPaymentProcessBusiness(
            IFiscalYearBusiness fiscalYearBusiness,
            IModuleConfig moduleConfig,
            ISysLogger sysLogger,
            IDapperData dapper,
            ITaxService taxService,
            ISalaryReviewBusiness salaryReviewBusiness,
            IMonthlyAllowanceConfigBusiness monthlyAllowanceConfigBusiness,
            IAllowanceNameBusiness allowanceNameBusiness,
            ISalaryProcessBusiness salaryProcessBusiness,
            IAllowanceConfigBusiness allowanceConfigBusiness,
            ITaxRulesBusiness taxRulesBusiness,
            IMapper mapper,
            PayrollDbContext payrollDbContext
            )
        {
            _sysLogger = sysLogger;
            _dapper = dapper;
            _moduleConfig = moduleConfig;
            _sysLogger = sysLogger;
            _fiscalYearBusiness = fiscalYearBusiness;
            _salaryReviewBusiness = salaryReviewBusiness;
            _allowanceNameBusiness = allowanceNameBusiness;
            _salaryProcessBusiness = salaryProcessBusiness;
            _allowanceConfigBusiness = allowanceConfigBusiness;
            _taxRulesBusiness = taxRulesBusiness;
            _taxService = taxService;
            _mapper = mapper;
            _payrollDbContext = payrollDbContext;
            _monthlyAllowanceConfigBusiness = monthlyAllowanceConfigBusiness;
        }
        public async Task<ExecutionStatus> ProcessAsync(SupplementaryProcessDTO model, AppUser user)
        {
            ExecutionStatus executionStatus = null;
            try
            {
                List<SupplementaryPaymentProcessSaveDTO> list = new List<SupplementaryPaymentProcessSaveDTO>();

                var fiscalYearInfo = await _fiscalYearBusiness.GetFiscalYearInfoWithinADate(DateTimeExtension.FirstDateOfAMonth((int)model.PaymentYear, (int)model.PaymentMonth).ToString("yyyy-MM-dd"), user);

                list = (await ProcessData(model, fiscalYearInfo, user));

                if (list.Any())
                {
                    executionStatus = await SaveAsync(model, list, fiscalYearInfo, user);
                }
                else
                {
                    executionStatus = ResponseMessage.Invalid("No item(s) has been processed to save");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                executionStatus = ResponseMessage.Invalid(ResponseMessage.ServerResponsedWithError);
            }
            return executionStatus;
        }
        public async Task<SupplementaryPaymentProcessSaveDTO> TaxProcessAsync(FiscalYear fiscalYearInfo, PayrollModuleConfig payrollModuleConfig, AllowanceInfo allowanceInfo, SupplementaryAmountDTO model, AppUser user)
        {
            SupplementaryPaymentProcessSaveDTO data = new SupplementaryPaymentProcessSaveDTO();
            try
            {
                var employee = await GetEmployeeInfoAsync(model.EmployeeId, model.PaymentMonth, model.PaymentYear, user);
                short year = model.PaymentYear;
                short month = model.PaymentMonth;
                DateTime firstDateOfThisMonth = DateTimeExtension.FirstDateOfAMonth(year, month);
                employee.IsThisEmployeeDiscontinuedWithinThisFiscalYear = _taxService.IsThisEmployeeDiscontinuedWithinThisFiscalYear(employee, fiscalYearInfo);
                employee.RemainFiscalYearMonth = _taxService.RemainProjectionMonth(firstDateOfThisMonth, fiscalYearInfo.FiscalYearTo.Value);
                employee.RemainProjectionMonthForThisEmployee = _taxService.RemainProjectionMonthForThisEmployee(employee, fiscalYearInfo, firstDateOfThisMonth, employee.RemainFiscalYearMonth);
                int countOfSalaryReceiptInThisFiscalYear = await _taxService.CountOfSalaryReceipt(employee.EmployeeId, fiscalYearInfo.FiscalYearId, month, user);

                long lastSalaryReviewId = await _taxService.LastSalaryReviewId(employee, year, month, user);
                var salaryReviewDetails = await _taxService.GetSalaryReviewDetailsAsync(employee.EmployeeId, lastSalaryReviewId, user);

                bool isThisEmployeeHasGotSalaryInThisMonth = await _salaryProcessBusiness.GetSalaryProcessDetailByEmployeeIdAsync(employee.EmployeeId, month, year, user) != null;

                var currentGross = salaryReviewDetails.Sum(i => i.CurrentAmount);
                var currentBasic = salaryReviewDetails.Where(i => i.AllowanceNameId == allowanceInfo.BasicAllowance).Sum(i => i.CurrentAmount);

                var earnedAllowances = (await _taxService.AllowancesEarnedByThisEmployeeByThisFiscalYearAsync(employee, fiscalYearInfo, month, year, lastSalaryReviewId, salaryReviewDetails, employee.RemainProjectionMonthForThisEmployee, user)).ToList();
                if (!earnedAllowances.Any())
                {
                    var earnedAllowanceslist = earnedAllowances;
                    var daysInMonth = DateTimeExtension.FirstDateOfAMonth(year, month).DaysInAMonth();

                    int serial = 0;
                    foreach (var item in salaryReviewDetails)
                    {
                        serial = serial + 1;
                        var thisMonthAmount = Math.Round(item.CurrentAmount / daysInMonth, 0) * employee.DateOfJoining.Value.DaysBetweenDateRangeIncludingStartDate(DateTimeExtension.LastDateOfAMonth(employee.DateOfJoining.Value));

                        var allowance = await _allowanceConfigBusiness.GetAllownaceConfigurationByAllowanceIdAsync(new AllowanceConfig_Filter()
                        {
                            AllowanceNameId = item.AllowanceNameId.ToString()
                        }, user);
                        earnedAllowanceslist.Add(new TaxDetailInTaxProcess()
                        {
                            SL = serial,
                            EmployeeId = employee.EmployeeId,
                            IsTaxable = true,
                            ProjectRestYear = allowance.ProjectRestYear ?? false,
                            OnceOffDeduction = allowance.IsOnceOffTax ?? false,
                            ExemptionAmount = 0,
                            ExemptionPercentage = 0,
                            IsItAllowance = true,
                            TotalTaxableIncome = 0,
                            AllowanceNameId = allowance.AllowanceNameId,
                            AllowanceName = allowance.AllowanceName,
                            AllowanceConfigId = allowance.ConfigId,
                            Amount = thisMonthAmount,
                            CurrentAmount = thisMonthAmount,
                            ReviewAmount = item.CurrentAmount,
                            Arrear = 0,
                            Adjustment = 0,
                            TillAmount = 0,
                            Flag = allowance.AllowanceFlag,
                            RemainFiscalYearMonth = (short)employee.RemainFiscalYearMonth,
                            LessExemptedAmount = 0,
                            GrossAnnualIncome = 0,
                            ProjectedAmount = 0
                        });
                    }
                    earnedAllowances = earnedAllowanceslist;
                }
                else
                {
                    foreach (var item in earnedAllowances)
                    {
                        var index = salaryReviewDetails.ToList().FindIndex(i => i.AllowanceNameId == item.AllowanceNameId);
                        if (index >= 0)
                        {
                            if (item.CurrentAmount <= 0)
                            {
                                item.CurrentAmount = salaryReviewDetails.Where(i => i.AllowanceNameId == item.AllowanceNameId).Single().CurrentAmount;
                                item.ReviewAmount = item.CurrentAmount;
                            }
                        }
                    }
                }

                // Finding the supplementary allowance from the previous earings.
                var itemIndex = earnedAllowances.FindIndex(i => i.AllowanceNameId == model.AllowanceNameId);
                if (itemIndex < 0)
                {
                    var allowance = await _allowanceConfigBusiness.GetAllownaceConfigurationByAllowanceIdAsync(new AllowanceConfig_Filter()
                    {
                        AllowanceNameId = model.AllowanceNameId.ToString()
                    }, user);
                    earnedAllowances.Add(new TaxDetailInTaxProcess()
                    {
                        SL = earnedAllowances.Count + 1,
                        EmployeeId = employee.EmployeeId,
                        IsTaxable = true,
                        ProjectRestYear = allowance.ProjectRestYear ?? false,
                        OnceOffDeduction = allowance.IsOnceOffTax ?? false,
                        ExemptionAmount = 0,
                        ExemptionPercentage = 0,
                        IsItAllowance = true,
                        TotalTaxableIncome = 0,
                        AllowanceNameId = allowance.AllowanceNameId,
                        AllowanceName = allowance.AllowanceName,
                        AllowanceConfigId = allowance.ConfigId,
                        Amount = model.Amount,
                        CurrentAmount = model.Amount,
                        ReviewAmount = model.Amount,
                        Arrear = 0,
                        Adjustment = 0,
                        TillAmount = 0,
                        Flag = allowance.AllowanceFlag,
                        RemainFiscalYearMonth = (short)employee.RemainFiscalYearMonth,
                        LessExemptedAmount = 0,
                        GrossAnnualIncome = 0,
                        ProjectedAmount = 0
                    });
                }
                else
                {
                    earnedAllowances.ElementAt(itemIndex).CurrentAmount = earnedAllowances.ElementAt(itemIndex).CurrentAmount + model.Amount;
                }

                #region Thompson Medical - Yearly Bonus - Conveyance
                if (user.OrganizationId == 11 && user.CompanyId == 19)
                {
                    var medicalAllowanceIndex = earnedAllowances.FindIndex(i => i.AllowanceNameId == allowanceInfo.MedicalAllowance);
                    decimal conveyanceAmount = employee.EmployeeType == "Manager" ? 5000 : 3500;
                    var medicalAmount = Math.Round(currentGross * (decimal).04);

                    #region Medical
                    if (employee.JobType == "Permanent")
                    {
                        if (medicalAllowanceIndex >= 0)
                        {
                            earnedAllowances.ElementAt(medicalAllowanceIndex).ReviewAmount = medicalAmount;
                            if (earnedAllowances.ElementAt(medicalAllowanceIndex).CurrentAmount == 0)
                            {
                                earnedAllowances.ElementAt(medicalAllowanceIndex).CurrentAmount = medicalAmount;
                            }
                        }
                        if (medicalAllowanceIndex < 0)
                        {
                            var daysInMonth = employee.DateOfJoining.Value.DaysInAMonth();
                            var thisMonthMedicalAmount = Math.Round(medicalAmount / daysInMonth, 0) * employee.DateOfJoining.Value.DaysBetweenDateRangeIncludingStartDate(DateTimeExtension.LastDateOfAMonth(employee.DateOfJoining.Value));
                            var allowance = await _allowanceConfigBusiness.GetAllownaceConfigurationByAllowanceIdAsync(new AllowanceConfig_Filter()
                            {
                                AllowanceNameId = allowanceInfo.MedicalAllowance.ToString()
                            }, user);
                            earnedAllowances.Add(new TaxDetailInTaxProcess()
                            {
                                SL = earnedAllowances.Count + 1,
                                EmployeeId = employee.EmployeeId,
                                IsTaxable = true,
                                ProjectRestYear = allowance.ProjectRestYear ?? false,
                                OnceOffDeduction = allowance.IsOnceOffTax ?? false,
                                ExemptionAmount = 0,
                                ExemptionPercentage = 0,
                                IsItAllowance = true,
                                TotalTaxableIncome = 0,
                                AllowanceNameId = allowance.AllowanceNameId,
                                AllowanceName = allowance.AllowanceName,
                                AllowanceConfigId = allowance.ConfigId,
                                Amount = thisMonthMedicalAmount,
                                CurrentAmount = thisMonthMedicalAmount,
                                ReviewAmount = medicalAmount,
                                Arrear = 0,
                                Adjustment = 0,
                                TillAmount = 0,
                                Flag = allowance.AllowanceFlag,
                                RemainFiscalYearMonth = (short)employee.RemainFiscalYearMonth,
                                LessExemptedAmount = 0,
                                GrossAnnualIncome = 0,
                                ProjectedAmount = 0
                            });
                        }
                    }
                    if (employee.JobType == "Contractual")
                    {
                        if (medicalAllowanceIndex >= 0)
                        {
                            earnedAllowances.ElementAt(medicalAllowanceIndex).ReviewAmount = medicalAmount + conveyanceAmount;
                        }
                        else
                        {
                            var daysInMonth = employee.DateOfJoining.Value.DaysInAMonth();
                            var thisMonthMedicalAmount = Math.Round((medicalAmount + conveyanceAmount) / daysInMonth, 0) * employee.DateOfJoining.Value.DaysBetweenDateRangeIncludingStartDate(DateTimeExtension.LastDateOfAMonth(employee.DateOfJoining.Value));
                            var allowance = await _allowanceConfigBusiness.GetAllownaceConfigurationByAllowanceIdAsync(new AllowanceConfig_Filter()
                            {
                                AllowanceNameId = allowanceInfo.MedicalAllowance.ToString()
                            }, user);

                            TaxDetailInTaxProcess taxDetailInTaxProcess = new TaxDetailInTaxProcess()
                            {
                                SL = earnedAllowances.Count + 1,
                                EmployeeId = employee.EmployeeId,
                                AllowanceNameId = allowance.AllowanceNameId,
                                AllowanceName = allowance.AllowanceName,
                                IsActive = true,
                                IsMonthly = true,
                                IsTaxable = allowance.IsTaxable ?? false,
                                IsIndividual = false,
                                DepandsOnWorkingHour = null,
                                ProjectRestYear = allowance.ProjectRestYear ?? false,
                                OnceOffDeduction = allowance.IsOnceOffTax ?? false,
                                AllowanceConfigId = allowance.ConfigId,
                                Flag = "Medical",
                                TillAmount = 0,
                                CurrentAmount = thisMonthMedicalAmount,
                                Amount = thisMonthMedicalAmount,
                                Arrear = 0,
                                ReviewAmount = medicalAmount + conveyanceAmount
                            };
                            earnedAllowances.Add(taxDetailInTaxProcess);
                        }
                    }
                    #endregion

                    #region Yearly Bonus
                    if (earnedAllowances.Exists(i => i.AllowanceNameId == 7) == false && employee.IsDiscontinued == false)
                    {

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
                                ProjectedAmount = employee.JobType == Jobtype.Contractual ? currentGross + conveyanceAmount : currentGross
                            };
                            earnedAllowances.Add(taxDetailInTaxProcess);
                        }
                    }
                    #endregion

                    #region Conveyance
                    var conveyanceAllowanceIndex = earnedAllowances.FindIndex(i => i.AllowanceNameId == allowanceInfo.ConveyanceAllowance);
                    if (conveyanceAllowanceIndex == -1)
                    {
                        var daysInMonth = employee.DateOfJoining.Value.DaysInAMonth();
                        var thisConveyanceAmount = Math.Round(conveyanceAmount / daysInMonth, 0) * employee.DateOfJoining.Value.DaysBetweenDateRangeIncludingStartDate(DateTimeExtension.LastDateOfAMonth(employee.DateOfJoining.Value));
                        var allowance = await _allowanceConfigBusiness.GetAllownaceConfigurationByAllowanceIdAsync(new AllowanceConfig_Filter()
                        {
                            AllowanceNameId = allowanceInfo.ConveyanceAllowance.ToString()
                        }, user);

                        TaxDetailInTaxProcess taxDetailInTaxProcess = new TaxDetailInTaxProcess()
                        {
                            SL = earnedAllowances.Count + 1,
                            EmployeeId = employee.EmployeeId,
                            AllowanceNameId = allowance.AllowanceNameId,
                            AllowanceName = allowance.AllowanceName,
                            IsActive = true,
                            IsMonthly = true,
                            IsTaxable = allowance.IsTaxable ?? false,
                            IsIndividual = false,
                            DepandsOnWorkingHour = null,
                            ProjectRestYear = allowance.ProjectRestYear ?? false,
                            OnceOffDeduction = allowance.IsOnceOffTax ?? false,
                            AllowanceConfigId = 0,
                            Flag = allowance.AllowanceFlag,
                            TillAmount = 0,
                            CurrentAmount = thisConveyanceAmount,
                            Amount = thisConveyanceAmount,
                            Arrear = 0,
                            ReviewAmount = thisConveyanceAmount
                        };
                        earnedAllowances.Add(taxDetailInTaxProcess);
                    }
                    else
                    {
                        earnedAllowances.ElementAt(conveyanceAllowanceIndex).ReviewAmount = conveyanceAmount;
                        if (earnedAllowances.ElementAt(conveyanceAllowanceIndex).CurrentAmount == 0)
                        {
                            earnedAllowances.ElementAt(conveyanceAllowanceIndex).CurrentAmount = conveyanceAmount;
                        }
                    }
                    #endregion

                }
                #endregion

                #region Monthly Allowance // When payment before Salary
                if (isThisEmployeeHasGotSalaryInThisMonth == false)
                {
                    var monthlyAllowanceConfigs = await _monthlyAllowanceConfigBusiness.GetMonthlyAllowanceConfigsAsync(employee.EmployeeId, DateTimeExtension.FirstDateOfAMonth(year, month).ToString("yyyy-MM-dd"), user);
                    if (monthlyAllowanceConfigs != null)
                    {
                        foreach (var item in monthlyAllowanceConfigs)
                        {
                            var allowanceConfig = await _allowanceConfigBusiness.GetAllownaceConfigurationByAllowanceIdAsync(new AllowanceConfig_Filter()
                            {
                                AllowanceNameId = item.AllowanceNameId.ToString(),
                            }, user);

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
                                            gainedAmount = baseAmount / 100 * item.Percentage.Value;
                                        }
                                        else
                                        {
                                            gainedAmount = ((baseAmount / 100) * item.Percentage.Value);
                                        }

                                        gainedAmount = Math.Round(gainedAmount, MidpointRounding.AwayFromZero);
                                        earnedAllowances.Add(new TaxDetailInTaxProcess()
                                        {
                                            CurrentAmount = gainedAmount,
                                            Amount = gainedAmount,
                                            EmployeeId = employee.EmployeeId,
                                            Month = month,
                                            Year = year,
                                            AllowanceNameId = item.AllowanceNameId,
                                            AllowanceName = allowanceConfig.AllowanceName,
                                            Flag = allowanceConfig.AllowanceFlag,
                                            AllowanceConfigId = allowanceConfig.ConfigId,
                                            IsActive = allowanceConfig.IsActive,
                                            IsTaxable = allowanceConfig.IsTaxable,
                                            ProjectRestYear = allowanceConfig.ProjectRestYear,
                                            OnceOffDeduction = allowanceConfig.IsOnceOffTax
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
                                        gainedAmount = Math.Round(item.Amount.Value, 0);
                                    }
                                    else
                                    {
                                        gainedAmount = item.Amount.Value;
                                    }

                                    earnedAllowances.Add(new TaxDetailInTaxProcess()
                                    {
                                        CurrentAmount = gainedAmount,
                                        Amount = gainedAmount,
                                        EmployeeId = employee.EmployeeId,
                                        Month = month,
                                        Year = year,
                                        AllowanceNameId = item.AllowanceNameId,
                                        AllowanceName = allowanceConfig.AllowanceName,
                                        Flag = allowanceConfig.AllowanceFlag,
                                        AllowanceConfigId = allowanceConfig.ConfigId,
                                        IsActive = allowanceConfig.IsActive,
                                        IsTaxable = allowanceConfig.IsTaxable,
                                        ProjectRestYear = allowanceConfig.ProjectRestYear,
                                        OnceOffDeduction = allowanceConfig.IsOnceOffTax
                                    });
                                }
                            }
                        }
                    }
                }
                #endregion

                // Projection Calculation on Fixed head
                earnedAllowances = (await _taxService.ProjectionCalculation(employee, earnedAllowances, fiscalYearInfo, employee.RemainProjectionMonthForThisEmployee, allowanceInfo, year, month, user)).ToList();

                // All Un-processed & Processed yearly Projection amount (accept fixed salary head)
                var incomeDetails = await _taxService.IncomeDetails(employee, fiscalYearInfo, earnedAllowances, salaryReviewDetails.ToList(), allowanceInfo, payrollModuleConfig, year, month, user);

                var dt = incomeDetails.ToDataTable();

                var taxInfo = await _taxRulesBusiness.TaxRules(employee, fiscalYearInfo, incomeDetails, payrollModuleConfig, allowanceInfo, year, month, "Supplementary", user);


                _mapper.Map(taxInfo.EmployeeTaxProcess, data.TaxInfo);
                _mapper.Map(taxInfo.EmployeeTaxProcessDetails, data.TaxDetails);
                _mapper.Map(taxInfo.EmployeeTaxProcessSlabs, data.TaxSlabs);

                data.AmountInfo = new SupplementaryPaymentAmount()
                {
                    AllowanceHeadId = 0,

                    AllowanceNameId = model.AllowanceNameId,
                    EmployeeId = model.EmployeeId,

                    GradeId = employee.GradeId,
                    Grade = employee.GradeName,

                    DesignationId = employee.DesignationId,
                    Designation = employee.DesignationName,

                    DepartmentId = employee.DepartmentId,
                    Department = employee.DepartmentName,

                    SectionId = employee.SectionId,
                    Section = employee.SectionName,

                    SubSectionId = employee.SubSectionId,
                    SubSection = employee.SubSectionName,

                    UnitId = employee.UnitId,
                    Unit = employee.UnitName,

                    CostCenterId = employee.CostCenterId,
                    CostCenterName = employee.CostCenterName,

                    EmployeeTypeId = employee.EmployeeTypeId,
                    EmployeeType = employee.EmployeeType,

                    JobCategoryId = employee.JobCategoryId,
                    JobCategory = employee.JobCategory,

                    JobType = employee.JobType,

                    Adjustment = 0,
                    PaymentMonth = month,
                    PaymentYear = year,
                    Amount = model.Amount,

                    EmployeeAccountId = employee.AccountInfoId,
                    BankId = employee.BankId,
                    BankName = employee.BankName,
                    BankBranchId = employee.BankBranchId,
                    BankBranchName = employee.BankBranchName,
                    BankAccountNumber = employee.BankAccount,
                    BankTransferAmount = model.Amount - data.TaxInfo.OnceOffTax ?? 0,

                    BranchId = employee.BranchId,
                    BaseOfPayment = "Flat",
                    CashAmount = 0,
                    COCInWalletTransfer = 0,

                    CompanyId = user.CompanyId,
                    OrganizationId = user.OrganizationId,
                    CreatedBy = user.ActionUserId,
                    CreatedDate = DateTime.Now,

                    EmployeeWalletId = 0,
                    DisbursedAmount = model.Amount - data.TaxInfo.OnceOffTax ?? 0,
                    FiscalYearId = fiscalYearInfo.FiscalYearId,
                    OnceOffAmount = data.TaxInfo.OnceOffTax ?? 0,
                    TaxAmount = data.TaxInfo.OnceOffTax ?? 0,
                    PaymentMode = "",
                    StateStatus = StateStatus.Pending
                };
            }
            catch (Exception ex)
            {
                throw;
            }
            return data;
        }
        public async Task<EligibleEmployeeForTaxType> GetEmployeeInfoAsync(long employeeId, short month, short year, AppUser user)
        {
            EligibleEmployeeForTaxType eligibleEmployee = new EligibleEmployeeForTaxType();
            try
            {
                string query = $@"Select ROW_NUMBER() Over(Order By emp.EmployeeId) SL,emp.EmployeeId,emp.EmployeeCode,emp.FullName,emp.GradeId,emp.DesignationId,
emp.InternalDesignationId,emp.DepartmentId,emp.SectionId,emp.SubsectionId,emp.UnitId,emp.JobCategoryId,emp.CostCenterId,emp.JobType,
emp.EmployeeTypeId,emp.CalculateFestivalBonusTaxProratedBasis,emp.CalculateProjectionTaxProratedBasis,
emp.TerminationDate,emp.TerminationStatus,emp.BranchId,emp.DateOfJoining,emp.DateOfConfirmation,emp.PFActivationDate,Emp.IsConfirmed,
EMP.IsPFMember,Emp.MinimumTaxAmount,dtl.IsFreedomFighter,dtl.IsPhysicallyDisabled,dtl.IsResidential,dtl.Religion,dtl.MaritalStatus,
IsDiscontinued=(CASE 
WHEN emp.TerminationDate IS NULL THEN 0
WHEN MONTH(emp.TerminationDate)  = @Month AND YEAR(emp.TerminationDate) = @Year THEN 1 ELSE 0 END), 
dtl.Gender,AgeYear=0,AgeMonth=0,AgeDay=0,
emp.EmployeeTypeId,
EmployeeType=ET.EmployeeTypeName,
SalaryReviewInfoIds=ISNULL((Select SalaryReviewInfoIds From Payroll_SalaryProcessDetail Where EmployeeId=@EmployeeId AND SalaryMonth=@Month AND SalaryYear=@Year),''),
acc.*
From HR_EmployeeInformation emp
Left Join HR_EmployeeDetail dtl On  emp.EmployeeId= dtl.EmployeeId 
Left Join HR_EmployeeType ET ON EMP.EmployeeTypeId = ET.EmployeeTypeId
LEFT JOIN vw_HR_EmployeeAllAccountInfo acc ON emp.EmployeeId=acc.EmployeeId
Where 1=1 AND emp.EmployeeId=@EmployeeId AND emp.CompanyId=@CompanyId AND emp.OrganizationId=@OrganizationId ";
                eligibleEmployee = await _dapper.SqlQueryFirstAsync<EligibleEmployeeForTaxType>(user.Database, query, new
                {
                    Month = month,
                    Year = year,
                    EmployeeId = employeeId,
                    user.CompanyId,
                    user.OrganizationId
                });
            }
            catch (Exception ex)
            {
            }
            return eligibleEmployee;
        }
        public async Task<ExecutionStatus> SaveAsync(SupplementaryProcessDTO model, List<SupplementaryPaymentProcessSaveDTO> list, FiscalYear fiscalYearInfo, AppUser user)
        {
            ExecutionStatus executionStatus = null;
            bool isSuccessful = true;
            try
            {
                decimal totalAmount = 0;
                decimal totalOnceOff = 0;
                foreach (var item in list)
                {
                    totalAmount = totalAmount + item.AmountInfo.Amount ?? 0;
                    totalOnceOff = totalOnceOff + item.AmountInfo.OnceOffAmount ?? 0;
                }
                SupplementaryPaymentProcessInfo info = new SupplementaryPaymentProcessInfo()
                {
                    AllowanceNameId = model.AllowanceNameId,
                    BatchNo = "",
                    BranchId = 0,
                    CompanyId = user.CompanyId,
                    OrganizationId = user.OrganizationId,
                    CreatedBy = user.ActionUserId,
                    CreatedDate = DateTime.Now,
                    EffectOnPayslip = model.ShowInPayslip ?? false,
                    ShowInPayslip = model.ShowInPayslip ?? false,
                    ShowInSalarySheet = model.ShowInSalarySheet ?? false,
                    FiscalYearId = fiscalYearInfo.FiscalYearId,
                    IsApproved = false,
                    IsDisbursed = false,
                    PaymentMode = model.PaymentMode,
                    PaymentHeadName = model.AllowanceName,
                    PaymentMonth = model.PaymentMonth,
                    PaymentYear = model.PaymentYear,
                    ProcessType = model.ProcessType,
                    StateStatus = StateStatus.Pending,
                    TotalAmount = totalAmount,
                    TotalOnceOffAmount = totalOnceOff,
                    TotalEmployees = list.Count,
                    TotalTax = totalOnceOff
                };
                using (var transaction = await _payrollDbContext.Database.BeginTransactionAsync())
                {
                    try
                    {
                        info.CompanyId = user.CompanyId;
                        info.OrganizationId = user.OrganizationId;
                        info.CreatedBy = user.ActionUserId;
                        info.CreatedDate = DateTime.Now;
                        _payrollDbContext.Payroll_SupplementaryPaymentProcessInfo.Add(info);
                        if (await _payrollDbContext.SaveChangesAsync() > 0)
                        {
                            foreach (var i in list)
                            {
                                i.AmountInfo.PaymentMode = info.PaymentMode;
                                i.AmountInfo.TaxAmount = i.AmountInfo.OnceOffAmount;
                                i.AmountInfo.PaymentProcessInfoId = info.PaymentProcessInfoId;
                                i.AmountInfo.CompanyId = user.CompanyId;
                                i.AmountInfo.OrganizationId = user.OrganizationId;
                                i.AmountInfo.CreatedBy = user.ActionUserId;
                                i.AmountInfo.CreatedDate = DateTime.Now;

                                _payrollDbContext.Payroll_SupplementaryPaymentAmount.Add(i.AmountInfo);
                                if (await _payrollDbContext.SaveChangesAsync() > 0)
                                {
                                    i.TaxInfo.PaymentProcessInfoId = info.PaymentProcessInfoId;
                                    i.TaxInfo.PaymentAmountId = i.AmountInfo.PaymentAmountId;

                                    _payrollDbContext.Payroll_SupplementaryPaymentTaxInfo.Add(i.TaxInfo);
                                    if (await _payrollDbContext.SaveChangesAsync() > 0)
                                    {
                                        i.TaxDetails.ForEach(item =>
                                        {
                                            item.PaymentTaxInfoId = i.TaxInfo.PaymentTaxInfoId;
                                            item.PaymentProcessInfoId = info.PaymentProcessInfoId;
                                            item.PaymentAmountId = i.AmountInfo.PaymentAmountId;
                                            item.FiscalYearId = i.TaxInfo.FiscalYearId;
                                            item.CompanyId = user.CompanyId;
                                            item.OrganizationId = user.OrganizationId;
                                            item.CreatedBy = user.ActionUserId;
                                            item.CreatedDate = DateTime.Now;
                                        });

                                        _payrollDbContext.Payroll_SupplementaryPaymentTaxDetail.AddRange(i.TaxDetails);
                                        if (await _payrollDbContext.SaveChangesAsync() > 0)
                                        {
                                            i.TaxSlabs.ForEach(item =>
                                            {
                                                item.PaymentTaxInfoId = i.TaxInfo.PaymentTaxInfoId;
                                                item.PaymentProcessInfoId = info.PaymentProcessInfoId;
                                                item.PaymentAmountId = i.AmountInfo.PaymentAmountId;
                                                item.FiscalYearId = i.TaxInfo.FiscalYearId;
                                                item.CompanyId = user.CompanyId;
                                                item.OrganizationId = user.OrganizationId;
                                                item.CreatedBy = user.ActionUserId;
                                                item.CreatedDate = DateTime.Now;
                                            });

                                            _payrollDbContext.Payroll_SupplementaryPaymentTaxSlab.AddRange(i.TaxSlabs);
                                            if (await _payrollDbContext.SaveChangesAsync() == 0)
                                            {
                                                isSuccessful = false;
                                                throw new Exception($"Tax slab is failed to save at {i.TaxInfo.EmployeeId.ToString()}");
                                            }
                                        }
                                        else
                                        {
                                            isSuccessful = false;
                                            throw new Exception($"Tax detail is failed to save at {i.TaxInfo.EmployeeId.ToString()}");
                                        }
                                    }
                                    else
                                    {
                                        isSuccessful = false;
                                        throw new Exception($"Tax info is failed to save at {i.TaxInfo.EmployeeId.ToString()}");
                                    }
                                }
                                else
                                {
                                    isSuccessful = false;
                                    throw new Exception($"Amount failed to save at {i.AmountInfo.EmployeeId.ToString()}");
                                }
                            }

                            if (isSuccessful == true)
                            {
                                await transaction.CommitAsync();
                                executionStatus = ResponseMessage.Message(true, ResponseMessage.Successfull);
                            }
                        }
                        else
                        {
                            throw new Exception("Process summary failed to save");
                        }
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        if (ex.InnerException != null)
                        {
                            executionStatus = ResponseMessage.Message(false, ResponseMessage.ServerResponsedWithError);
                        }
                        else
                        {
                            executionStatus = ResponseMessage.Message(false, ex.Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    executionStatus = ResponseMessage.Message(false, ResponseMessage.ServerResponsedWithError);
                }
                else
                {
                    executionStatus = ResponseMessage.Message(false, ex.Message);
                }
            }
            return executionStatus ?? ResponseMessage.Message(false, ResponseMessage.SomthingWentWrong);
        }
        public async Task<UndisbursedSupplementaryPaymentInfoViewModel> UndisbursedSupplementaryPaymentInfoAsync(long id, AppUser user)
        {
            UndisbursedSupplementaryPaymentInfoViewModel model = new UndisbursedSupplementaryPaymentInfoViewModel();
            try
            {
                var query = $@"SELECT Info.*,IY.FiscalYearRange FROM Payroll_SupplementaryPaymentProcessInfo Info
                INNER JOIN Payroll_FiscalYear IY ON Info.FiscalYearId=IY.FiscalYearId
                Where 1=1 AND PaymentProcessInfoId=@Id
                AND Info.CompanyId=@CompanyId AND Info.OrganizationId=@OrganizationId";

                //AND Info.StateStatus='Pending'

                model.Info = await _dapper.SqlQueryFirstAsync<SupplementaryPaymentProcessInfoViewModel>(user.Database, query, new
                {
                    Id = id,
                    CompanyId = user.CompanyId,
                    OrganizationId = user.OrganizationId
                }, CommandType.Text);

                if (model.Info != null && model.Info.PaymentProcessInfoId > 0)
                {
                    query = $@"SELECT PA.PaymentAmountId, PA.EmployeeId,EMP.EmployeeCode,EmployeeName=EMP.FullName,Designation=DEG.DesignationName,PA.PaymentMonth,PA.PaymentYear,
                    PA.BaseOfPayment,PA.Amount,PA.[Percentage],OnceOffAmount=ISNULL(PA.OnceOffAmount,0),PA.AllowanceNameId,AllowanceName=ALW.[Name],
                    DisbursedAmount=PA.Amount-ISNULL(PA.OnceOffAmount,0)
                    FROM Payroll_SupplementaryPaymentAmount PA
                    INNER JOIN Payroll_SupplementaryPaymentProcessInfo PPI ON PA.PaymentProcessInfoId=PPI.PaymentProcessInfoId
                    INNER JOIN HR_EmployeeInformation EMP ON PA.EmployeeId= EMP.EmployeeId
                    LEFT JOIN HR_Designations DEG ON EMP.DesignationId=DEG.DesignationId
                    INNER JOIN Payroll_AllowanceName ALW ON ALW.AllowanceNameId=PA.AllowanceNameId
                    Where 1=1 AND PA.PaymentProcessInfoId=@Id AND PA.CompanyId=@CompanyId AND PA.OrganizationId=@OrganizationId";

                    model.Details = await _dapper.SqlQueryListAsync<SupplementaryPaymentAmountViewModel>(user.Database, query, new
                    {
                        Id = id,
                        user.CompanyId,
                        user.OrganizationId
                    }, CommandType.Text);

                    if (model.Details.Any())
                    {
                        model.Info.AllowanceNameId = model.Details.FirstOrDefault().AllowanceNameId;
                        model.Info.AllowanceName = model.Details.FirstOrDefault().AllowanceName;
                    }
                }
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(
                    ex,
                    user.Database,
                    "SupplementaryPaymentProcessBusiness",
                    "UndisbursedSupplementaryPaymentInfoAsync",
                    user);
            }
            return model;
        }
        public async Task<ExecutionStatus> DisbursedOrUndoPaymentAsync(DisbursedOrUndoPaymentDTO model, AppUser user)
        {
            ExecutionStatus executionStatus = null;
            try
            {
                if (model.Status == "Disbursed")
                {
                    try
                    {
                        using (var connection = await _dapper.SqlConnectionForTransactionalAsync(user.Database))
                        {
                            connection.Open();
                            using (var transaction = connection.BeginTransaction())
                            {
                                try
                                {
                                    var query = $@"Update Payroll_SupplementaryPaymentProcessInfo
                                    SET StateStatus='Disbursed', IsDisbursed=1, ApprovedBy=@UserId, ApprovedDate=GETDATE()
                                    Where 1=1 AND StateStatus='Pending' AND PaymentProcessInfoId=@Id AND CompanyId=@CompanyId AND OrganizationId=@OrganizationId";

                                    var updateInfo = await connection.ExecuteAsync(query, new
                                    {
                                        UserId = user.ActionUserId,
                                        model.Id,
                                        user.CompanyId,
                                        user.OrganizationId
                                    }, transaction);

                                    if (updateInfo > 0)
                                    {
                                        query = $@"Update Payroll_SupplementaryPaymentAmount
                                        SET StateStatus='Disbursed', IsApproved=1, ApprovedBy=@UserId, ApprovedDate=GETDATE()
                                        Where 1=1 AND StateStatus='Pending' AND PaymentProcessInfoId=@Id AND CompanyId=@CompanyId AND OrganizationId=@OrganizationId";

                                        var updateAmounts = await connection.ExecuteAsync(query, new
                                        {
                                            UserId = user.ActionUserId,
                                            model.Id,
                                            user.CompanyId,
                                            user.OrganizationId
                                        }, transaction);

                                        if (updateAmounts > 0)
                                        {
                                            transaction.Commit();
                                            executionStatus = ResponseMessage.Message(true, ResponseMessage.Successfull);
                                        }
                                        else
                                        {
                                            throw new Exception("Data has been failed to disbursed while updating payments status");
                                        }
                                    }
                                    else
                                    {
                                        throw new Exception("Data has been failed to disbursed while updating process info status");
                                    }
                                }
                                catch (Exception ex)
                                {
                                    transaction.Rollback();
                                    if (ex.InnerException != null)
                                    {
                                        executionStatus = ResponseMessage.Message(false, ResponseMessage.ServerResponsedWithError);
                                        await _sysLogger.SaveHRMSException(ex, user.Database, "SupplementaryPaymentProcessBusiness", "DisbursedOrUndoPaymentAsync", user);
                                    }
                                    else
                                    {
                                        executionStatus = ResponseMessage.Message(true, ex.Message);
                                    }
                                }
                                finally
                                {
                                    connection.Close();
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        executionStatus = ResponseMessage.Message(false, "Something went worng while disbursing payments");
                        await _sysLogger.SaveHRMSException(ex, user.Database, "SupplementaryPaymentProcessBusiness", "DisbursedOrUndoPaymentAsync", user);
                    }
                }
                else if (model.Status == "Undo")
                {
                    try
                    {
                        using (var connection = await _dapper.SqlConnectionForTransactionalAsync(user.Database))
                        {
                            connection.Open();
                            using (var transaction = connection.BeginTransaction())
                            {
                                try
                                {
                                    var query = "DELETE FROM Payroll_SupplementaryPaymentTaxSlab Where PaymentProcessInfoId=@Id AND CompanyId=@CompanyId AND OrganizationId=@OrganizationId";

                                    var deleteTax = await connection.ExecuteAsync(query, new
                                    {
                                        model.Id,
                                        user.CompanyId,
                                        user.OrganizationId
                                    }, transaction);

                                    query = "DELETE FROM Payroll_SupplementaryPaymentTaxDetail Where PaymentProcessInfoId=@Id AND CompanyId=@CompanyId AND OrganizationId=@OrganizationId";

                                    deleteTax = await connection.ExecuteAsync(query, new
                                    {
                                        model.Id,
                                        user.CompanyId,
                                        user.OrganizationId
                                    }, transaction);

                                    query = "DELETE FROM Payroll_SupplementaryPaymentTaxInfo Where PaymentProcessInfoId=@Id AND CompanyId=@CompanyId AND OrganizationId=@OrganizationId";

                                    deleteTax = await connection.ExecuteAsync(query, new
                                    {
                                        model.Id,
                                        user.CompanyId,
                                        user.OrganizationId
                                    }, transaction);

                                    // 
                                    query = "DELETE FROM Payroll_SupplementaryPaymentAmount Where PaymentProcessInfoId=@Id AND CompanyId=@CompanyId AND OrganizationId=@OrganizationId";

                                    var deleteAmounts = await connection.ExecuteAsync(query, new
                                    {
                                        model.Id,
                                        user.CompanyId,
                                        user.OrganizationId
                                    }, transaction);

                                    if (deleteAmounts > 0)
                                    {
                                        query = "DELETE FROM Payroll_SupplementaryPaymentProcessInfo Where PaymentProcessInfoId=@Id AND CompanyId=@CompanyId AND OrganizationId=@OrganizationId";

                                        var deleteInfo = await connection.ExecuteAsync(query, new
                                        {
                                            model.Id,
                                            user.CompanyId,
                                            user.OrganizationId
                                        }, transaction);

                                        if (deleteInfo > 0)
                                        {
                                            transaction.Commit();
                                            executionStatus = ResponseMessage.Message(true, ResponseMessage.Successfull);
                                        }
                                        else
                                        {
                                            throw new Exception("Data has been failed to undo while deleting process");
                                        }
                                    }
                                    else
                                    {
                                        throw new Exception("Data has been failed to undo while deleting payments");
                                    }
                                }
                                catch (Exception ex)
                                {
                                    transaction.Rollback();
                                    if (ex.InnerException != null)
                                    {
                                        executionStatus = ResponseMessage.Message(false, ResponseMessage.ServerResponsedWithError);
                                        await _sysLogger.SaveHRMSException(ex, user.Database, "SupplementaryPaymentProcessBusiness", "DisbursedOrUndoPaymentAsync", user);
                                    }
                                    else
                                    {
                                        executionStatus = ResponseMessage.Message(true, ex.Message);
                                    }
                                }
                                finally
                                {
                                    connection.Close();
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        executionStatus = ResponseMessage.Message(false, "Something went worng while undo payments");
                        await _sysLogger.SaveHRMSException(ex, user.Database, "SupplementaryPaymentProcessBusiness", "DisbursedOrUndoPaymentAsync", user);
                    }
                }
            }
            catch (Exception ex)
            {
                executionStatus = ResponseMessage.Invalid(ResponseMessage.SomthingWentWrong);
                await _sysLogger.SaveHRMSException(ex, user.Database, "SupplementaryPaymentProcessBusiness", "DisbursedOrUndoPaymentAsync", user);
            }
            return executionStatus;
        }
        public async Task<SupplementaryPaymentInfoAndDetailViewModel> GetSupplementaryPaymentInfoAndDetailAsync(long id, AppUser user)
        {
            SupplementaryPaymentInfoAndDetailViewModel model = new SupplementaryPaymentInfoAndDetailViewModel();
            try
            {
                var query = $@"SELECT Info.*,IY.FiscalYearRange FROM Payroll_SupplementaryPaymentProcessInfo Info
                INNER JOIN Payroll_FiscalYear IY ON Info.FiscalYearId=IY.FiscalYearId
                Where 1=1 AND PaymentProcessInfoId=@Id AND Info.StateStatus='Pending'
                AND Info.CompanyId=@CompanyId AND Info.OrganizationId=@OrganizationId";

                model.Info = await _dapper.SqlQueryFirstAsync<SupplementaryPaymentProcessInfoViewModel>(user.Database, query, new
                {
                    Id = id,
                    user.CompanyId,
                    user.OrganizationId
                }, CommandType.Text);

                if (model.Info != null && model.Info.PaymentProcessInfoId > 0)
                {
                    query = $@"SELECT PA.EmployeeId,EMP.EmployeeCode,EmployeeName=EMP.FullName,Designation=DEG.DesignationName,PA.PaymentMonth,PA.PaymentYear,
                    PA.BaseOfPayment,PA.Amount,PA.[Percentage],OnceOffAmount=ISNULl(PA.OnceOffAmount,0),PA.AllowanceNameId,AllowanceName=ALW.[Name] 
                    FROM Payroll_SupplementaryPaymentAmount PA
                    INNER JOIN Payroll_SupplementaryPaymentProcessInfo PPI ON PA.PaymentProcessInfoId=PPI.PaymentProcessInfoId
                    INNER JOIN HR_EmployeeInformation EMP ON PA.EmployeeId= EMP.EmployeeId
                    LEFT JOIN HR_Designations DEG ON EMP.DesignationId=DEG.DesignationId
                    INNER JOIN Payroll_AllowanceName ALW ON ALW.AllowanceNameId=PA.AllowanceNameId
                    Where 1=1 AND PA.PaymentProcessInfoId=@Id AND PA.CompanyId=@CompanyId AND PA.OrganizationId=@OrganizationId";

                    model.Details = await _dapper.SqlQueryListAsync<SupplementaryPaymentAmountViewModel>(user.Database, query, new
                    {
                        Id = id,
                        user.CompanyId,
                        user.OrganizationId
                    }, CommandType.Text);

                    if (model.Details.Any())
                    {
                        model.Info.AllowanceNameId = model.Details.FirstOrDefault().AllowanceNameId;
                        model.Info.AllowanceName = model.Details.FirstOrDefault().AllowanceName;
                    }
                }
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(
                    ex,
                    user.Database,
                    "SupplementaryPaymentProcessBusiness",
                    "SupplementaryPaymentInfoAndDetailAsync",
                    user);
            }
            return model;
        }
        public async Task<List<SupplementaryPaymentProcessSaveDTO>> ProcessData(SupplementaryProcessDTO model, FiscalYear fiscalYearInfo, AppUser user)
        {
            List<SupplementaryPaymentProcessSaveDTO> list = new List<SupplementaryPaymentProcessSaveDTO>();

            var allowanceConfig = await _allowanceConfigBusiness.GetAllownaceConfigurationByAllowanceIdAsync(new AllowanceConfig_Filter()
            {
                AllowanceNameId = model.AllowanceNameId.ToString()
            }, user);
            var allowanceInfo = await _allowanceNameBusiness.GetAllowanceInfos(user);
            var payrollModuleConfig = await _moduleConfig.PayrollModuleConfig(user);
            foreach (var item in model.Payments)
            {
                item.PaymentYear = model.PaymentYear;
                item.PaymentMonth = model.PaymentMonth;
                item.AllowanceNameId = model.AllowanceNameId;
                item.PaymentMode = model.PaymentMode;
                if (allowanceConfig != null && (allowanceConfig.IsTaxable ?? false) == true)
                {
                    var paymentProcessedInfo = await TaxProcessAsync(fiscalYearInfo, payrollModuleConfig, allowanceInfo, item, user);
                    var employee = await GetEmployeeInfoAsync(item.EmployeeId, model.PaymentMonth, model.PaymentYear, user);
                    paymentProcessedInfo.AmountInfo.CreatedBy = user.ActionUserId;
                    paymentProcessedInfo.AmountInfo.CreatedDate = DateTime.Now;
                    paymentProcessedInfo.AmountInfo.CompanyId = user.CompanyId;
                    paymentProcessedInfo.AmountInfo.OrganizationId = user.OrganizationId;

                    paymentProcessedInfo.AmountInfo.Designation = employee.DesignationName;
                    paymentProcessedInfo.AmountInfo.Department = employee.DepartmentName;
                    paymentProcessedInfo.AmountInfo.Section = employee.SectionName;
                    paymentProcessedInfo.AmountInfo.SubSection = employee.SubSectionName;
                    paymentProcessedInfo.AmountInfo.EmployeeType = employee.EmployeeType;
                    paymentProcessedInfo.AmountInfo.JobType = employee.JobType;
                    paymentProcessedInfo.AmountInfo.JobCategory = employee.JobCategory;
                    paymentProcessedInfo.AmountInfo.Unit = employee.UnitName;
                    paymentProcessedInfo.AmountInfo.InternalDesignation = employee.InternalDesignationName;
                    paymentProcessedInfo.AmountInfo.CostCenterName = employee.CostCenterName;

                    paymentProcessedInfo.TaxInfo.CreatedBy = user.ActionUserId;
                    paymentProcessedInfo.TaxInfo.CreatedDate = DateTime.Now;
                    paymentProcessedInfo.TaxInfo.CompanyId = user.CompanyId;
                    paymentProcessedInfo.TaxInfo.OrganizationId = user.OrganizationId;


                    paymentProcessedInfo.TaxDetails.ForEach(i =>
                    {
                        i.CreatedBy = user.ActionUserId;
                        i.TaxItem = i.AllowanceName;
                        i.CreatedDate = DateTime.Now;
                        i.CompanyId = user.CompanyId;
                        i.OrganizationId = user.OrganizationId;
                    });
                    paymentProcessedInfo.TaxSlabs.ForEach(i =>
                    {
                        i.CreatedBy = user.ActionUserId;
                        i.CreatedDate = DateTime.Now;
                        i.CompanyId = user.CompanyId;
                        i.OrganizationId = user.OrganizationId;
                    });
                    list.Add(paymentProcessedInfo);
                }
                else
                {
                    SupplementaryPaymentProcessSaveDTO dto = new SupplementaryPaymentProcessSaveDTO();
                    var employee = await GetEmployeeInfoAsync(item.EmployeeId, model.PaymentMonth, model.PaymentYear, user);
                    dto.AmountInfo = new SupplementaryPaymentAmount()
                    {
                        AllowanceHeadId = 0,
                        AllowanceNameId = model.AllowanceNameId,
                        EmployeeId = employee.EmployeeId,
                        Adjustment = 0,
                        Amount = item.Amount,
                        BankAccountNumber = employee.BankAccount,
                        BankName = employee.BankName,
                        BankBranchName = employee.BankBranchName,
                        BankTransferAmount = item.Amount,
                        BranchId = employee.BranchId,
                        BaseOfPayment = "Flat",
                        CashAmount = 0,
                        COCInWalletTransfer = 0,
                        CompanyId = user.CompanyId,
                        OrganizationId = user.OrganizationId,
                        CreatedBy = user.ActionUserId,
                        CreatedDate = DateTime.Now,
                        EmployeeAccountId = 0,
                        EmployeeWalletId = 0,
                        DisbursedAmount = item.Amount,
                        FiscalYearId = fiscalYearInfo.FiscalYearId,
                        OnceOffAmount = 0,
                        PaymentMode = model.PaymentMode,
                        StateStatus = StateStatus.Pending,
                        Designation = employee.DesignationName,
                        Department = employee.DepartmentName,
                        Section = employee.SectionName,
                        SubSection = employee.SubSectionName,
                        EmployeeType = employee.EmployeeType,
                        JobType = employee.JobType,
                        JobCategory = employee.JobCategory,
                        Unit = employee.UnitName,
                        InternalDesignation = employee.InternalDesignationName,
                        CostCenterName = employee.CostCenterName,
                    };
                    list.Add(dto);
                }
            }
            return list;
        }
        public async Task<ExecutionStatus> UpdatePaymentAmountAsync(SupplementaryPaymentAmountDTO item, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                var paymentInfoInDb = _payrollDbContext.Payroll_SupplementaryPaymentAmount.FirstOrDefault(i => i.PaymentAmountId == item.PaymentAmountId);
                if (paymentInfoInDb != null)
                {
                    var fiscalYearInfo = await _fiscalYearBusiness.GetFiscalYearInfoWithinADate(DateTimeExtension.FirstDateOfAMonth((int)paymentInfoInDb.PaymentYear, (int)paymentInfoInDb.PaymentMonth).ToString("yyyy-MM-dd"), user);

                    var processInfoInDb = _payrollDbContext.Payroll_SupplementaryPaymentProcessInfo.FirstOrDefault(i => i.PaymentProcessInfoId == paymentInfoInDb.PaymentProcessInfoId);
                    if (processInfoInDb != null && fiscalYearInfo != null)
                    {
                        SupplementaryProcessDTO model = new SupplementaryProcessDTO();
                        model.AllowanceNameId = item.AllowanceNameId;
                        model.PaymentMonth = item.PaymentMonth;
                        model.PaymentYear = item.PaymentYear;
                        model.PaymentMode = processInfoInDb.PaymentMode;
                        model.ProcessType = processInfoInDb.ProcessType;
                        model.ShowInPayslip = processInfoInDb.ShowInPayslip;
                        model.ShowInSalarySheet = processInfoInDb.ShowInSalarySheet;
                        model.Payments = new List<SupplementaryAmountDTO>();

                        model.Payments.Add(new SupplementaryAmountDTO()
                        {
                            AllowanceNameId = paymentInfoInDb.AllowanceNameId,
                            AllowanceHeadId = paymentInfoInDb.AllowanceHeadId ?? 0,
                            Amount = item.Amount ?? 0,
                            EmployeeId = paymentInfoInDb.EmployeeId,
                            PaymentMonth = paymentInfoInDb.PaymentMonth,
                            PaymentYear = paymentInfoInDb.PaymentYear,
                            PaymentMode = processInfoInDb.PaymentMode
                        });

                        var list = await ProcessData(model, fiscalYearInfo, user);
                        if (list != null && list.Any())
                        {
                            var processedItem = list[0];
                            if (processedItem != null)
                            {
                                using (var transaction = await _payrollDbContext.Database.BeginTransactionAsync())
                                {
                                    bool isSuccessful = true;
                                    try
                                    {
                                        // Delete Tax 
                                        var taxInfoInDb = _payrollDbContext.Payroll_SupplementaryPaymentTaxInfo.FirstOrDefault(i => i.PaymentAmountId == paymentInfoInDb.PaymentAmountId);

                                        var taxDetailsInDb = _payrollDbContext.Payroll_SupplementaryPaymentTaxDetail.Where(i => i.PaymentAmountId == paymentInfoInDb.PaymentAmountId).ToList();

                                        var taxSlabsInDb = _payrollDbContext.Payroll_SupplementaryPaymentTaxSlab.Where(i => i.PaymentAmountId == paymentInfoInDb.PaymentAmountId).ToList();

                                        if (taxInfoInDb != null)
                                        {
                                            _payrollDbContext.Payroll_SupplementaryPaymentTaxInfo.Remove(taxInfoInDb);
                                            _payrollDbContext.Payroll_SupplementaryPaymentTaxDetail.RemoveRange(taxDetailsInDb);
                                            _payrollDbContext.Payroll_SupplementaryPaymentTaxSlab.RemoveRange(taxSlabsInDb);
                                            await _payrollDbContext.SaveChangesAsync();
                                        }

                                        processInfoInDb.TotalAmount = (processInfoInDb.TotalAmount - paymentInfoInDb.Amount ?? 0) + processedItem.AmountInfo.Amount ?? 0;
                                        processInfoInDb.TotalTax = (processInfoInDb.TotalTax - paymentInfoInDb.TaxAmount) + processedItem.TaxInfo.OnceOffTax;
                                        processInfoInDb.TotalOnceOffAmount = (processInfoInDb.TotalOnceOffAmount - paymentInfoInDb.OnceOffAmount) + processedItem.TaxInfo.OnceOffTax;

                                        _payrollDbContext.Payroll_SupplementaryPaymentProcessInfo.Update(processInfoInDb);
                                        if (await _payrollDbContext.SaveChangesAsync() > 0)
                                        {
                                            paymentInfoInDb.Amount = processedItem.AmountInfo.Amount;
                                            paymentInfoInDb.OnceOffAmount = processedItem.TaxInfo.OnceOffTax;
                                            paymentInfoInDb.TaxAmount = processedItem.TaxInfo.OnceOffTax;
                                            paymentInfoInDb.UpdatedBy = user.ActionUserId;
                                            paymentInfoInDb.UpdatedDate = DateTime.Now;
                                            _payrollDbContext.Payroll_SupplementaryPaymentAmount.Update(paymentInfoInDb);

                                            if (await _payrollDbContext.SaveChangesAsync() > 0)
                                            {
                                                // Save Tax Info
                                                processedItem.TaxInfo.PaymentAmountId = paymentInfoInDb.PaymentAmountId;
                                                processedItem.TaxInfo.PaymentProcessInfoId = paymentInfoInDb.PaymentProcessInfoId;
                                                processedItem.TaxInfo.CreatedBy = user.ActionUserId;
                                                processedItem.TaxInfo.CreatedDate = DateTime.Now;
                                                processedItem.TaxInfo.CompanyId = user.CompanyId;
                                                processedItem.TaxInfo.OrganizationId = user.OrganizationId;
                                                _payrollDbContext.Payroll_SupplementaryPaymentTaxInfo.Add(processedItem.TaxInfo);
                                                if (await _payrollDbContext.SaveChangesAsync() > 0)
                                                {
                                                    processedItem.TaxDetails.ForEach(i =>
                                                    {
                                                        i.PaymentAmountId = paymentInfoInDb.PaymentAmountId;
                                                        i.FiscalYearId = paymentInfoInDb.FiscalYearId;
                                                        i.PaymentProcessInfoId = paymentInfoInDb.PaymentProcessInfoId;
                                                        i.PaymentTaxInfoId = processedItem.TaxInfo.PaymentTaxInfoId;
                                                    });
                                                    _payrollDbContext.Payroll_SupplementaryPaymentTaxDetail.AddRange(processedItem.TaxDetails);

                                                    if (await _payrollDbContext.SaveChangesAsync() > 0)
                                                    {
                                                        processedItem.TaxSlabs.ForEach(i =>
                                                        {
                                                            i.PaymentAmountId = paymentInfoInDb.PaymentAmountId;
                                                            i.FiscalYearId = paymentInfoInDb.FiscalYearId;
                                                            i.PaymentProcessInfoId = paymentInfoInDb.PaymentProcessInfoId;
                                                            i.PaymentTaxInfoId = processedItem.TaxInfo.PaymentTaxInfoId;
                                                        });
                                                        _payrollDbContext.Payroll_SupplementaryPaymentTaxSlab.AddRange(processedItem.TaxSlabs);

                                                        if (await _payrollDbContext.SaveChangesAsync() == 0)
                                                        {
                                                            isSuccessful = false;
                                                            throw new Exception("Failed to insert tax slab");
                                                        }
                                                    }
                                                    else
                                                    {
                                                        isSuccessful = false;
                                                        throw new Exception("Failed to insert tax detail");
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                isSuccessful = false;
                                                throw new Exception("Failed to update amount");
                                            }

                                            if (isSuccessful == true)
                                            {
                                                await transaction.CommitAsync();
                                                executionStatus = ResponseMessage.Message(true, ResponseMessage.Successfull);
                                            }
                                        }
                                        else
                                        {
                                            isSuccessful = false;
                                            throw new Exception("Failed to update process info");
                                        }


                                    }
                                    catch (Exception ex)
                                    {
                                        if (ex.InnerException != null)
                                        {
                                            executionStatus = ResponseMessage.Message(false, ResponseMessage.ServerResponsedWithError);
                                        }
                                        else
                                        {
                                            executionStatus = ResponseMessage.Message(false, ex.Message);
                                        }
                                        await transaction.RollbackAsync();
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return executionStatus;
        }
        public async Task<ExecutionStatus> UpdateSupplementaryTaxAmountAsync(long processId, List<UpdateSupplementaryTaxAmountDTO> employees, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                var paymentProcessInfo = _payrollDbContext.Payroll_SupplementaryPaymentProcessInfo.FirstOrDefault(i => i.PaymentProcessInfoId == processId);
                if (paymentProcessInfo != null)
                {
                    if (paymentProcessInfo.StateStatus == StateStatus.Pending)
                    {
                        decimal previousTaxDeducted = 0;
                        decimal newTaxDeducted = 0;

                        using (var transaction = await _payrollDbContext.Database.BeginTransactionAsync())
                        {
                            bool IsSuccess = true;
                            try
                            {
                                foreach (var employee in employees)
                                {
                                    var payment = _payrollDbContext.Payroll_SupplementaryPaymentAmount.FirstOrDefault(i => i.EmployeeId == employee.EmployeeId && i.CompanyId == user.CompanyId && i.OrganizationId == user.OrganizationId && i.PaymentProcessInfoId == processId);
                                    if (payment != null)
                                    {
                                        previousTaxDeducted = previousTaxDeducted + payment.OnceOffAmount ?? 0;
                                        payment.OnceOffAmount = employee.TaxAmount;
                                        newTaxDeducted = newTaxDeducted + employee.TaxAmount;
                                        payment.UpdatedBy = user.ActionUserId;
                                        payment.UpdatedDate = DateTime.Now;

                                        _payrollDbContext.Payroll_SupplementaryPaymentAmount.Update(payment);
                                        if (await _payrollDbContext.SaveChangesAsync() == 0)
                                        {
                                            IsSuccess = false;
                                            throw new Exception($"Failed to update amount info at employee {employee.EmployeeId.ToString()}");
                                        }
                                    }
                                }

                                if (IsSuccess)
                                {
                                    paymentProcessInfo.TotalOnceOffAmount = (paymentProcessInfo.TotalOnceOffAmount - previousTaxDeducted) + newTaxDeducted;
                                    paymentProcessInfo.UpdatedBy = user.ActionUserId;
                                    paymentProcessInfo.UpdatedDate = DateTime.Now;
                                    _payrollDbContext.Payroll_SupplementaryPaymentProcessInfo.Update(paymentProcessInfo);
                                    if (await _payrollDbContext.SaveChangesAsync() == 0)
                                    {
                                        IsSuccess = false;
                                    }
                                    if (IsSuccess)
                                    {
                                        await transaction.CommitAsync();
                                        executionStatus = ResponseMessage.Message(true, ResponseMessage.Successfull);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                await transaction.RollbackAsync();
                                if (ex.InnerException != null)
                                {
                                    executionStatus = ResponseMessage.Message(false, ex.Message);
                                }
                                else
                                {
                                    executionStatus = ResponseMessage.Message(false, ex.Message);
                                }
                            }
                        }
                    }
                    else
                    {
                        executionStatus = ResponseMessage.Message(false, "Process status already has been change. Not allowed to update");
                    }
                }
                else
                {
                    executionStatus = ResponseMessage.Message(false, "Cann't find process information");
                }
            }
            catch (Exception ex)
            {
                executionStatus = ResponseMessage.Message(false, ex.Message);
            }
            return executionStatus;
        }
    }
}
