using AutoMapper;
using BLL.Tax.Interface;
using BLL.Base.Interface;
using Shared.Services;
using DAL.Context.Payroll;
using DAL.Context.Employee;
using Shared.Payroll.DTO.Tax;
using Shared.OtherModels.User;
using Shared.OtherModels.Response;
using BLL.Employee.Interface.Info;
using BLL.Salary.Setup.Interface;
using Shared.Payroll.Process.Tax;
using Shared.Payroll.Process.Allowance;
using Shared.Payroll.Filter.Allowance;
using Shared.Payroll.ViewModel.Allowance;
using BLL.Salary.Allowance.Interface;
using DAL.DapperObject.Interface;
using BLL.Administration.Interface;
using Shared.Payroll.Domain.Setup;
using BLL.Salary.Salary.Interface;
using Microsoft.EntityFrameworkCore;
using Shared.Payroll.ViewModel.Tax;
using System.Data;
using DAL.Payroll.Repository.Interface;
using Shared.Payroll.Domain.Tax;
using System.Collections.Generic;
using TaxDetailInTaxProcess = Shared.Payroll.Process.Tax.TaxDetailInTaxProcess;
using BLL.Salary.Payment.Interface;
using BLL.Salary.Payment.Implementation;

namespace BLL.Tax.Implementation
{
    public class FinalTaxProcessBusiness : IFinalTaxProcessBusiness
    {
        private readonly PayrollDbContext _payrollDbContext;
        private readonly EmployeeModuleDbContext _employeeModuleDbContext;
        private readonly IFiscalYearBusiness _fiscalYearBusiness;
        private readonly IInfoBusiness _infoBusiness;
        private readonly IBranchInfoBusiness _branchInfoBusiness;
        private readonly IMapper _mapper;
        private readonly IExecuteTaxProcess _executeTaxProcess;
        private readonly IModuleConfig _moduleConfig;
        private readonly IAllowanceNameBusiness _allowanceNameBusiness;
        private readonly ITaxRulesBusiness _taxRulesBusiness;
        private readonly ISalaryProcessBusiness _salaryProcessBusiness;
        private readonly IProjectedPaymentBusiness _projectedPaymentBusiness;
        private readonly IDapperData _dapper;
        private readonly IConditionalDepositAllowanceConfigRepository _conditionalDepositAllowanceConfigRepository;
        private readonly IConditionalProjectedPaymentBusiness _conditionalProjectedPaymentBusiness;


        public FinalTaxProcessBusiness(
            PayrollDbContext payrollDbContext,
            IFiscalYearBusiness fiscalYearBusiness,
            EmployeeModuleDbContext employeeModuleDbContext,
            IMapper mapper,
            IDapperData dapper,
            IBranchInfoBusiness branchInfoBusiness,
            IModuleConfig moduleConfig,
            IExecuteTaxProcess executeTaxProcess,
            IAllowanceNameBusiness allowanceNameBusiness,
            ITaxRulesBusiness taxRulesBusiness,
            ISalaryProcessBusiness salaryProcessBusiness,
            IProjectedPaymentBusiness projectedPaymentBusiness,
            IConditionalProjectedPaymentBusiness conditionalProjectedPaymentBusiness,
            IConditionalDepositAllowanceConfigRepository conditionalDepositAllowanceConfigRepository,
            IInfoBusiness infoBusiness)
        {
            _fiscalYearBusiness = fiscalYearBusiness;
            _payrollDbContext = payrollDbContext;
            _employeeModuleDbContext = employeeModuleDbContext;
            _infoBusiness = infoBusiness;
            _mapper = mapper;
            _moduleConfig = moduleConfig;
            _executeTaxProcess = executeTaxProcess;
            _allowanceNameBusiness = allowanceNameBusiness;
            _taxRulesBusiness = taxRulesBusiness;
            _branchInfoBusiness = branchInfoBusiness;
            _salaryProcessBusiness = salaryProcessBusiness;
            _conditionalProjectedPaymentBusiness = conditionalProjectedPaymentBusiness;
            _projectedPaymentBusiness = projectedPaymentBusiness;
            _conditionalDepositAllowanceConfigRepository = conditionalDepositAllowanceConfigRepository;
            _dapper = dapper;
        }
        public async Task<ExecutionStatus> DeleteAsync(long employeeId, long fiscalYearId, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                _payrollDbContext.Payroll_FinalTaxProcess.Where(i => i.EmployeeId == employeeId && i.FiscalYearId == fiscalYearId).ExecuteDelete();
                _payrollDbContext.Payroll_FinalTaxProcessDetail.Where(i => i.EmployeeId == employeeId && i.FiscalYearId == fiscalYearId).ExecuteDelete();
                _payrollDbContext.Payroll_FinalTaxProcessSlab.Where(i => i.EmployeeId == employeeId && i.FiscalYearId == fiscalYearId).ExecuteDelete();
                await _payrollDbContext.SaveChangesAsync();
                executionStatus = ResponseMessage.Message(true, ResponseMessage.Successfull);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return executionStatus;
        }
        public async Task<ExecutionStatus> FinalTaxProcessAsync(FinalTaxProcessDTO model, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                var fiscalYear = await _fiscalYearBusiness.GetFiscalYearByIdAsync(model.FiscalYearId, user);
                List<FinalTaxProcessedInfo> taxProcessInfo = new List<FinalTaxProcessedInfo>();
                if (fiscalYear != null && fiscalYear.FiscalYearTo.HasValue)
                {
                    int month = 0;
                    int year = 0;
                    int remainMonth = 0;
                    var payrollModuleConfig = await _moduleConfig.PayrollModuleConfig(user);
                    var pfPercentage = Utility.TryParseInt(payrollModuleConfig.PercentageOfProvidentFund);

                    AllowanceInfo allowanceInfo = await _allowanceNameBusiness.GetAllowanceInfos(user);
                    foreach (var id in model.EmployeeIds)
                    {
                        var employee = await GetEligibleEmployeeForTaxProcess(id, fiscalYear, user);
                        var salaryGainedMonth = await _salaryProcessBusiness.GetEmployeeLastSalaryMonthInAIncomeYear(id, fiscalYear.FiscalYearId, user);
                        if (employee != null && salaryGainedMonth != null)
                        {
                            await DeleteAsync(id, fiscalYear.FiscalYearId, user); // Delete Exiting data...
                            year = salaryGainedMonth.Value.Year;
                            month = salaryGainedMonth.Value.Month;
                            if (employee.TerminationDate != null && employee.TerminationStatus == StateStatus.Approved)
                            {
                                if (fiscalYear.FiscalYearFrom.HasValue && fiscalYear.FiscalYearTo.HasValue)
                                {
                                    employee.IsDiscontinued = employee.TerminationDate.Value.IsDateBetweenTwoDates(fiscalYear.FiscalYearFrom.Value, fiscalYear.FiscalYearTo.Value);
                                }

                            }
                            if (employee.IsDiscontinued && employee.TerminationDate != null)
                            {
                                if (employee.DateOfJoining.HasValue && employee.TerminationDate.HasValue)
                                {
                                    employee.TotalServiceDays = DateTimeExtension.DaysBetweenDateRangeIncludingStartDate(employee.DateOfJoining.Value, employee.TerminationDate.Value);
                                }
                            }
                            else
                            {
                                if (employee.DateOfJoining.HasValue && fiscalYear.FiscalYearTo.HasValue)
                                {
                                    employee.TotalServiceDays = DateTimeExtension.DaysBetweenDateRangeIncludingStartDate(employee.DateOfJoining.Value, fiscalYear.FiscalYearTo.Value);
                                }
                            }


                            remainMonth = DateTimeExtension.FirstDateOfAMonth(year, month).GetMonthDiffExcludingThisMonth(fiscalYear.FiscalYearTo.Value);

                            var taxDetails = (await _executeTaxProcess.AllowancesEarnedByThisEmployeeByThisFiscalYearAsync(employee, fiscalYear, month, year, 0, user)).Select(i => new Shared.Payroll.Process.Tax.TaxDetailInTaxProcess()
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
                                ReviewAmount = 0,
                                ProjectedAmount = 0,
                                LessExemptedAmount = 0,
                                TotalTaxableIncome = 0,
                                GrossAnnualIncome = 0,
                                RemainFiscalYearMonth=0,
                            }).ToList();

                            if (taxDetails != null && taxDetails.Any())
                            {
                                var accuredAllowance = await _conditionalDepositAllowanceConfigRepository.GetEmployeeAllowanceAccuredAmountInTaxProcessAsync(employee.EmployeeId, allowanceInfo.MedicalAllowance, (short)year, (short)month, fiscalYear.FiscalYearId, user);

                                var medicalAllowanceIndex = taxDetails.FindIndex(i => i.AllowanceNameId == allowanceInfo.MedicalAllowance);
                                if (user.OrganizationId == 11 && user.CompanyId == 19 && employee.JobType == "Permanent")
                                {
                                    if (medicalAllowanceIndex < 0)
                                    {
                                        Shared.Payroll.Process.Tax.TaxDetailInTaxProcess taxDetailInTaxProcess = new Shared.Payroll.Process.Tax.TaxDetailInTaxProcess()
                                        {
                                            SL = taxDetails.Count + 1,
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
                                            TillAmount =  accuredAllowance.TillAccuredAmount ,
                                            CurrentAmount =  accuredAllowance.ThisMonthAccuredAmount + accuredAllowance.ThisMonthAccuredArrear,
                                            Amount = 0,
                                            Arrear = accuredAllowance.ThisMonthAccuredArrear,
                                            ReviewAmount = 0
                                        };
                                        taxDetails.Add(taxDetailInTaxProcess);
                                    }
                                    else
                                    {
                                        taxDetails.ElementAt(medicalAllowanceIndex).TillAmount = taxDetails.ElementAt(medicalAllowanceIndex).TillAmount + (remainMonth == 0 ? accuredAllowance.TillAccuredAmount : 0);

                                        taxDetails.ElementAt(medicalAllowanceIndex).CurrentAmount = taxDetails.ElementAt(medicalAllowanceIndex).CurrentAmount + (remainMonth == 0 ? accuredAllowance.ThisMonthAccuredAmount + accuredAllowance.ThisMonthAccuredArrear : 0);

                                        taxDetails.ElementAt(medicalAllowanceIndex).Arrear = taxDetails.ElementAt(medicalAllowanceIndex).Arrear + (remainMonth == 0 ? accuredAllowance.ThisMonthAccuredArrear : 0);

                                        taxDetails.ElementAt(medicalAllowanceIndex).ReviewAmount = 0;
                                    }
                                }

                                // Till Conditional Projected Amount
                                var tillConditionalProjectedAllowances = await _conditionalProjectedPaymentBusiness.GetAllowanceTillDisbursedAmountsAsync(employee.EmployeeId, fiscalYear.FiscalYearId, year, month, user);

                                if (tillConditionalProjectedAllowances != null)
                                {
                                    if (tillConditionalProjectedAllowances.Any())
                                    {
                                        foreach (var item in tillConditionalProjectedAllowances)
                                        {
                                            taxDetails.Add(new TaxDetailInTaxProcess()
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
                                                RemainFiscalYearMonth = 0,
                                                LessExemptedAmount = 0,
                                                TotalTaxableIncome = 0,
                                                GrossAnnualIncome = 0
                                            });
                                        }
                                    }
                                }

                                // This month Conditional Projected Amount
                                var thisMonthConditionalProjectedAllowances = await _conditionalProjectedPaymentBusiness.GetAllowanceThisMonthDisbursedAmountsAsync(employee.EmployeeId, fiscalYear.FiscalYearId, year, month, user);
                                if (thisMonthConditionalProjectedAllowances != null)
                                {
                                    if (thisMonthConditionalProjectedAllowances.Any())
                                    {
                                        foreach (var item in thisMonthConditionalProjectedAllowances)
                                        {
                                            taxDetails.Add(new TaxDetailInTaxProcess()
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
                                                RemainFiscalYearMonth = 0,
                                                LessExemptedAmount = 0,
                                                TotalTaxableIncome = 0,
                                                GrossAnnualIncome = 0
                                            });
                                        }
                                    }
                                }

                                // Till Individual Projected Amount 
                                var tillIndividualProjectedAllowances = await _projectedPaymentBusiness.GetTillProcessedProjectedAllowanceAsync(employee.EmployeeId, fiscalYear.FiscalYearId, year, month, user);
                                if (tillIndividualProjectedAllowances != null)
                                {
                                    if (tillIndividualProjectedAllowances.Any())
                                    {
                                        foreach (var item in tillIndividualProjectedAllowances)
                                        {
                                            taxDetails.Add(new TaxDetailInTaxProcess()
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
                                                RemainFiscalYearMonth = 0,
                                                LessExemptedAmount = 0,
                                                TotalTaxableIncome = 0,
                                                GrossAnnualIncome = 0
                                            });
                                        }
                                    }
                                }

                                // This Month Individual Projected Amount 
                                var thisMonthIndividualProjectedAllowances = await _projectedPaymentBusiness.GetThisMonthProcessedProjectedAllowanceAsync(employee.EmployeeId, fiscalYear.FiscalYearId, year, month, user);
                                if (thisMonthIndividualProjectedAllowances != null)
                                {
                                    if (thisMonthIndividualProjectedAllowances.Any())
                                    {
                                        foreach (var item in thisMonthIndividualProjectedAllowances)
                                        {
                                            taxDetails.Add(new TaxDetailInTaxProcess()
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
                                                RemainFiscalYearMonth = 0,
                                                LessExemptedAmount = 0,
                                                TotalTaxableIncome = 0,
                                                GrossAnnualIncome = 0
                                            });
                                        }
                                    }
                                }

                                var deductableAmounts = await _executeTaxProcess.DeductableIncomeOfThisEmployeeAsync(employee, fiscalYear, month, year, user);

                                if (deductableAmounts.Any())
                                {
                                    foreach (var item in deductableAmounts)
                                    {
                                        Shared.Payroll.Process.Tax.TaxDetailInTaxProcess deduction = new Shared.Payroll.Process.Tax.TaxDetailInTaxProcess()
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
                                            RemainFiscalYearMonth = 0,
                                            ProjectedAmount = 0,
                                            LessExemptedAmount = 0,
                                            TotalTaxableIncome = 0,
                                            GrossAnnualIncome = 0
                                        };
                                        taxDetails.Add(deduction);
                                    }
                                }

                                var salaryProcessDetail = (_payrollDbContext.Payroll_SalaryProcessDetail.FirstOrDefault(i => i.EmployeeId == employee.EmployeeId && i.SalaryMonth == month && i.SalaryYear == year && i.CompanyId == user.CompanyId && i.OrganizationId == user.OrganizationId));

                                if (employee.IsPFMember && pfPercentage > 0)
                                {
                                    var employeePFAllowance = await _executeTaxProcess.GetEmployeePFAmountAsync(employee, fiscalYear, year, month, user);
                                    if (employeePFAllowance != null && (employeePFAllowance.TillAmount > 0 || employeePFAllowance.CurrentAmount > 0))
                                    {
                                        employeePFAllowance.AllowanceNameId = allowanceInfo.PFAllowance <= 0 ? 999999999 : allowanceInfo.PFAllowance;
                                        employeePFAllowance.AllowanceName = "PF";
                                        employeePFAllowance.Flag = "PF";
                                        employeePFAllowance.ProjectRestYear = true;
                                        employeePFAllowance.OnceOffDeduction = false;
                                        employeePFAllowance.ReviewAmount = 0;
                                        employeePFAllowance.RemainFiscalYearMonth = 0;
                                        employeePFAllowance.ProjectedAmount = 0;
                                        taxDetails.Add(employeePFAllowance);
                                    }
                                }

                                taxDetails = (await _executeTaxProcess.MargeTaxDetailsAsync(employee, taxDetails, user)).ToList();

                                employee.RemainFiscalYearMonth = remainMonth;
                                var taxInfo = await _taxRulesBusiness.TaxRulesIY2324(employee, fiscalYear, taxDetails, payrollModuleConfig, allowanceInfo, 0, remainMonth, year, month, user);

                                if (taxInfo != null)
                                {
                                    var tax = new FinalTaxProcessedInfo();
                                    tax.FinalTaxProcess = _mapper.Map(taxInfo.EmployeeTaxProcess, tax.FinalTaxProcess);
                                    tax.FinalTaxProcessDetails = _mapper.Map(taxInfo.EmployeeTaxProcessDetails, tax.FinalTaxProcessDetails);
                                    tax.FinalTaxProcessSlabs = _mapper.Map(taxInfo.EmployeeTaxProcessSlabs, tax.FinalTaxProcessSlabs);

                                    var tillDeductedAmount = await _taxRulesBusiness.GetEmployeeTaxDeductedTillMonthAsync(employee.EmployeeId, fiscalYear.FiscalYearId, year, month, user);

                                    if (tax != null && fiscalYear.FiscalYearTo.HasValue)
                                    {
                                        tax.FinalTaxProcess.SalaryMonth = (short)month;
                                        tax.FinalTaxProcess.SalaryYear = (short)year;
                                        tax.FinalTaxProcess.SalaryProcessId = salaryProcessDetail != null ? salaryProcessDetail.SalaryProcessId : 0;
                                        tax.FinalTaxProcess.SalaryProcessDetailId = salaryProcessDetail != null ? salaryProcessDetail.SalaryProcessDetailId : 0;

                                        //tax.FinalTaxProcess.ProjectionTax = salaryProcessDetail != null ? salaryProcessDetail.ProjectionTax : 0;
                                        //tax.FinalTaxProcess.OnceOffTax = salaryProcessDetail != null ? salaryProcessDetail.OnceOffTax : 0;
                                        //tax.FinalTaxProcess.MonthlyTax = tax.FinalTaxProcess.ProjectionTax??0 + tax.FinalTaxProcess.OnceOffTax??0;
                                        tax.FinalTaxProcess.ActualTaxDeductionAmount = salaryProcessDetail != null ? salaryProcessDetail.TaxDeductedAmount : 0;
                                        tax.FinalTaxProcess.PaidTotalTax = tillDeductedAmount.TillTaxDeducted;
                                        tax.FinalTaxProcess.TillProjectionTax = tillDeductedAmount.TillProjectionTax;
                                        tax.FinalTaxProcess.TillOnceOffTax = tillDeductedAmount.TillOnceOffTax;

                                        taxProcessInfo.Add(tax);
                                    }
                                }
                            }

                        }
                    }
                    if (taxProcessInfo != null && taxProcessInfo.Any())
                    {
                        executionStatus = await SaveAsync(taxProcessInfo, user);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                executionStatus = ResponseMessage.Message(true, ResponseMessage.Unsuccessful);
            }
            return executionStatus;
        }
        public async Task<EligibleEmployeeForTaxType> GetEligibleEmployeeForTaxProcess(long employeeId, FiscalYear fiscalYear, AppUser user)
        {
            EligibleEmployeeForTaxType employee = new EligibleEmployeeForTaxType();
            try
            {
                var employeeInDb = _employeeModuleDbContext.HR_EmployeeInformation.FirstOrDefault(i => i.EmployeeId == employeeId);
                if (employeeInDb != null && fiscalYear != null && fiscalYear.FiscalYearFrom.HasValue && fiscalYear.FiscalYearTo.HasValue)
                {
                    int year = fiscalYear.FiscalYearTo.Value.Year;
                    int month = fiscalYear.FiscalYearTo.Value.Month;
                    bool isDiscontinued = false;
                    if (employeeInDb.TerminationDate.HasValue && employeeInDb.TerminationStatus == StateStatus.Approved)
                    {
                        isDiscontinued = employeeInDb.TerminationDate.Value.IsDateBetweenTwoDates(fiscalYear.FiscalYearFrom.Value, fiscalYear.FiscalYearTo.Value.AddMonths(-1).LastDateOfAMonth());
                        if (isDiscontinued)
                        {
                            year = employeeInDb.TerminationDate.Value.Year;
                            month = employeeInDb.TerminationDate.Value.Month;
                        }
                    }

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
                    WHEN emp.TerminationStatus ='Approved' AND MONTH(emp.TerminationDate)  = @Month AND YEAR(emp.TerminationDate) = @Year THEN 1 ELSE 0 END), 
                    dtl.Gender,AgeYear=0,AgeMonth=0,AgeDay=0,
                    emp.EmployeeTypeId,
                    EmployeeType=ET.EmployeeTypeName
                    From HR_EmployeeInformation emp
                    Left Join HR_EmployeeDetail dtl On  emp.EmployeeId= dtl.EmployeeId 
                    Left Join HR_EmployeeType ET ON EMP.EmployeeTypeId = ET.EmployeeTypeId
                    INNER JOIN Payroll_SalaryProcessDetail spd on emp.EmployeeId= spd.EmployeeId
                    Where 1=1 AND spd.EmployeeId =@EmployeeId AND spd.SalaryMonth=@Month AND spd.SalaryYear=@Year";

                    employee = await _dapper.SqlQueryFirstAsync<EligibleEmployeeForTaxType>(user.Database, query, new
                    {
                        EmployeeId = employeeId,
                        Month = month,
                        year = year
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return employee;
        }
        public async Task<IEnumerable<Shared.Payroll.ViewModel.Tax.EmployeeInfoForFinalTaxProcessViewModel>> GetEmployeesAsync(long fiscalYearId, string flag, long branchId, AppUser user)
        {
            IEnumerable<Shared.Payroll.ViewModel.Tax.EmployeeInfoForFinalTaxProcessViewModel> list = new List<Shared.Payroll.ViewModel.Tax.EmployeeInfoForFinalTaxProcessViewModel>();
            try
            {
                var fiscalYear = await _fiscalYearBusiness.GetFiscalYearAsync(fiscalYearId, user);
                if (fiscalYear != null && fiscalYear.FiscalYearFrom.HasValue && fiscalYear.FiscalYearTo.HasValue)
                {
                    var fromDate = fiscalYear.FiscalYearFrom.Value;
                    var toDate = fiscalYear.FiscalYearTo.Value;
                    if (flag == "Discountinued Employees before June")
                    {
                        toDate = toDate.AddMonths(-1).LastDateOfAMonth();
                        var query = $@"SELECT EMP.EmployeeId,EMP.FullName,EMP.EmployeeCode,[Designation]=DEG.DesignationName,[Department]=DPT.DepartmentName,
                        [Section]=SEC.SectionName,[SubSection]=SUB.SubSectionName,DTL.Gender,DTL.Religion,DTL.IsResidential,
                        EMP.TerminationDate,HasTaxCard=(CASE WHEN (SELECT COUNT(*) FROM Payroll_FinalTaxProcess Where FiscalYearId=@FiscalYearId AND EmployeeId=EMP.EmployeeId) > 0 THEN 1 ELSE 0 END),[Branch]=EMP.BranchId
                        FROM HR_EmployeeInformation EMP
                        LEFT JOIN HR_EmployeeDetail DTL ON EMP.EmployeeId = DTL.EmployeeId
                        LEFT JOIN HR_Designations DEG ON EMP.DesignationId = DEG.DesignationId
                        LEFT JOIN HR_Departments DPT ON EMP.DesignationId = DPT.DepartmentId
                        LEFT JOIN HR_Sections SEC ON EMP.SectionId = SEC.SectionId
                        LEFT JOIN HR_SubSections SUB ON EMP.SubSectionId = SUB.SubSectionId
                        WHERE 1=1
                        AND EMP.TerminationDate IS NOT NULL AND EMP.TerminationStatus ='Approved'
                        AND EMP.TerminationDate BETWEEN @FromDate AND @ToDate
                        AND (@BranchId IS NULL OR @BranchId = 0 OR EMP.BranchId=@BranchId)
                        AND EMP.CompanyId =@CompanyId AND EMP.OrganizationId=@OrganizationId Order By EMP.TerminationDate";
                        list = await _dapper.SqlQueryListAsync<Shared.Payroll.ViewModel.Tax.EmployeeInfoForFinalTaxProcessViewModel>(user.Database, query, new
                        {
                            FiscalYearId = fiscalYear.FiscalYearId,
                            BranchId = branchId,
                            FromDate = fromDate,
                            ToDate = toDate,
                            CompanyId = user.CompanyId,
                            OrganizationId = user.OrganizationId
                        });
                    }
                    else if (flag == "June Employees")
                    {
                        fromDate = new DateTime(toDate.Year, toDate.Month, 1);
                        var query = $@"SELECT EMP.EmployeeId,EMP.FullName,EMP.EmployeeCode,[Designation]=DEG.DesignationName,[Department]=DPT.DepartmentName,
                        [Section]=SEC.SectionName,[SubSection]=SUB.SubSectionName,DTL.Gender,DTL.Religion,DTL.IsResidential,
                        [TerminationDate]=(CASE WHEN EMP.TerminationDate IS NOT NULL AND EMP.TerminationStatus ='Approved' THEN EMP.TerminationDate ELSE NULL END),[Branch]=SPD.BranchId,
                        HasTaxCard=(CASE WHEN (SELECT COUNT(*) FROM Payroll_FinalTaxProcess Where FiscalYearId=@FiscalYearId AND EmployeeId=EMP.EmployeeId) > 0 THEN 1 ELSE 0 END)
                        FROM HR_EmployeeInformation EMP
                        INNER JOIN Payroll_SalaryProcessDetail SPD ON EMP.EmployeeId = SPD.EmployeeId
                        LEFT JOIN HR_EmployeeDetail DTL ON EMP.EmployeeId = DTL.EmployeeId
                        LEFT JOIN HR_Designations DEG ON SPD.DesignationId = DEG.DesignationId
                        LEFT JOIN HR_Departments DPT ON SPD.DesignationId = DPT.DepartmentId
                        LEFT JOIN HR_Sections SEC ON SPD.SectionId = SEC.SectionId
                        LEFT JOIN HR_SubSections SUB ON SPD.SubSectionId = SUB.SubSectionId
                        WHERE 1=1
                        AND SPD.SalaryMonth = MONTH(@fromDate) AND SPD.SalaryYear = YEAR(@fromDate)
                        AND (@BranchId IS NULL OR @BranchId = 0 OR SPD.BranchId=@BranchId)
                        AND EMP.CompanyId =@CompanyId AND EMP.OrganizationId=@OrganizationId Order By EMP.TerminationDate";
                        list = await _dapper.SqlQueryListAsync<Shared.Payroll.ViewModel.Tax.EmployeeInfoForFinalTaxProcessViewModel>(user.Database, query, new
                        {
                            FiscalYearId = fiscalYear.FiscalYearId,
                            BranchId = branchId,
                            FromDate = fromDate,
                            ToDate = toDate,
                            CompanyId = user.CompanyId,
                            OrganizationId = user.OrganizationId
                        });
                    }
                    else
                    {
                        var query = $@"SELECT EMP.EmployeeId,EMP.FullName,EMP.EmployeeCode,[Designation]=DEG.DesignationName,[Department]=DPT.DepartmentName,
[Section]=SEC.SectionName,[SubSection]=SUB.SubSectionName,DTL.Gender,DTL.Religion,DTL.IsResidential,TAX.LastMonth,
[TerminationDate]=(CASE WHEN EMP.TerminationDate IS NOT NULL AND EMP.TerminationStatus ='Approved' THEN EMP.TerminationDate ELSE NULL END),
Branch=SPD.BranchId
FROM HR_EmployeeInformation EMP
INNER JOIN (
	SELECT T.FiscalYearId,T.EmployeeId,[LastMonth]=MAX(dbo.fnGetFirstDateOfAMonth(T.SalaryYear,T.SalaryMonth)) FROM Payroll_EmployeeTaxProcess T 
	Where T.FiscalYearId=@FiscalYearId
	Group By T.FiscalYearId,T.EmployeeId
) TAX ON TAX.EmployeeId=EMP.EmployeeId
INNER JOIN Payroll_EmployeeTaxProcess TSALARY 
ON TAX.FiscalYearId=TSALARY.FiscalYearId AND TAX.EmployeeId=TSALARY.EmployeeId AND TAX.LastMonth=dbo.fnGetFirstDateOfAMonth(TSALARY.SalaryYear,TSALARY.SalaryMonth)
INNER JOIN Payroll_SalaryProcessDetail SPD ON TSALARY.EmployeeId=SPD.EmployeeId AND TSALARY.SalaryProcessDetailId=SPD.SalaryProcessDetailId
LEFT JOIN HR_EmployeeDetail DTL ON SPD.EmployeeId = DTL.EmployeeId
LEFT JOIN HR_Designations DEG ON SPD.DesignationId = DEG.DesignationId
LEFT JOIN HR_Departments DPT ON SPD.DepartmentId = DPT.DepartmentId
LEFT JOIN HR_Sections SEC ON SPD.SectionId = SEC.SectionId
LEFT JOIN HR_SubSections SUB ON SPD.SubSectionId = SUB.SubSectionId
WHERE 1=1
AND TAX.FiscalYearId=@FiscalYearId
AND (@BranchId IS NULL OR @BranchId=0 OR SPD.BranchId=@BranchId)
AND EMP.CompanyId=@CompanyId
AND EMP.OrganizationId=@OrganizationId
ORDER BY TAX.LastMonth";
                        list = await _dapper.SqlQueryListAsync<Shared.Payroll.ViewModel.Tax.EmployeeInfoForFinalTaxProcessViewModel>(user.Database, query, new
                        {
                            FiscalYearId = fiscalYear.FiscalYearId,
                            BranchId = branchId,
                            CompanyId = user.CompanyId,
                            OrganizationId = user.OrganizationId
                        });
                    }


                    if (list != null && list.Any())
                    {
                        var branches = await _branchInfoBusiness.GetBranchsAsync("", user);
                        if (branches != null && branches.Any())
                        {
                            list.ToList().ForEach(i =>
                            {
                                var branch = branches.FirstOrDefault(item => item.BranchId == Utility.TryParseLong(i.Branch));
                                if (branch != null)
                                {
                                    i.Branch = branch.BranchName;
                                }
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return list ?? new List<Shared.Payroll.ViewModel.Tax.EmployeeInfoForFinalTaxProcessViewModel>();
        }
        public async Task<IEnumerable<EmployeeTaxProcesDetailInfosViewModel>> GetFinalTaxProcesSummariesAsync(long employeeId, long fiscalYearId, long branchId, AppUser user)
        {
            IEnumerable<EmployeeTaxProcesDetailInfosViewModel> list = new List<EmployeeTaxProcesDetailInfosViewModel>();
            try
            {
                var query = $@"SELECT IY.FiscalYearId,IY.FiscalYearRange,IY.AssesmentYear,TAX.BranchId,
                SalaryMonth= TAX.SalaryMonth,
                SalaryYear= TAX.SalaryYear,
                TAX.EmployeeId,
                EMP.EmployeeCode,
                [EmployeeName]=EMP.FullName,
                [Designation]=DEG.DesignationName,
                [Department]=DPT.DepartmentName,
                YearlyTaxableIncome=ISNULL(ROUND(TAX.YearlyTaxableIncome,0),0),
                TotalTaxPayable=ISNULL(ROUND(TAX.TotalTaxPayable,0),0),
                AITAmount=ISNULL(TAX.AITAmount,0),
                TaxReturnAmount =ISNULL(TAX.TaxReturnAmount,0),
                ExcessTaxPaidRefundAmount = ISNULL(TAX.ExcessTaxPaidRefundAmount,0),
                YearlyTax = ISNULL(ROUND(TAX.YearlyTax,0),0),
                PaidTotalTax = ISNULL(TAX.PaidTotalTax,0),
                ProjectionAmount= ISNULL(ROUND(TAX.ProjectionAmount,0),0),
                ProjectionTax= ISNULL(ROUND(TAX.ProjectionTax,0),0),
                OnceOffAmount= ISNULL(ROUND(TAX.OnceOffAmount,0),0),
                OnceOffTax= ISNULL(ROUND(TAX.OnceOffTax,0),0),
                MonthlyTax = ISNULL(ROUND(TAX.MonthlyTax,0),0),
                ActualTaxDeductionAmount = ISNULL(ROUND(TAX.ActualTaxDeductionAmount,0),0),
                PFContributionBothPart = ISNULL(ROUND(TAX.PFContributionBothPart,0),0),
                OtherInvestment = ISNULL(ROUND(TAX.OtherInvestment,0),0),
                ActualInvestmentMade = ISNULL(ROUND(TAX.ActualInvestmentMade,0),0),
                InvestmentRebateAmount = ISNULL(TAX.InvestmentRebateAmount,0),
                ExemptionAmountOnAnnualIncome= ISNULL(TAX.ExemptionAmountOnAnnualIncome,0),
                TaxableIncome= ISNULL(ROUND(TAX.TaxableIncome,0),0),
                TotalLessExemptedAmount= ISNULL(TAX.TotalLessExemptedAmount,0),
                TotalCurrentMonthAllowanceAmount= ISNULL(TAX.TotalCurrentMonthAllowanceAmount,0),
                TotalProjectedAllowanceAmount= ISNULL(TAX.TotalProjectedAllowanceAmount,0),
                TotalTillMonthAllowanceAmount= ISNULL(TAX.TotalTillMonthAllowanceAmount,0),
                TotalGrossAnnualIncome= ISNULL(ROUND(TAX.TotalGrossAnnualIncome,0),0),
                GrossTaxableIncome= ISNULL(ROUND(TAX.TotalGrossAnnualIncome,0),0)
                From Payroll_FinalTaxProcess TAX
                INNER JOIN Payroll_FiscalYear IY ON TAX.FiscalYearId = IY.FiscalYearId
                INNER JOIN Payroll_SalaryProcessDetail SPD ON TAX.SalaryProcessDetailId = SPD.SalaryProcessDetailId
                INNER JOIN HR_EmployeeInformation EMP ON EMP.EmployeeId = TAX.EmployeeId
                LEFT JOIN HR_Designations DEG ON SPD.DesignationId = DEG.DesignationId
                LEFT JOIN HR_Departments DPT ON SPD.DepartmentId = DPT.DepartmentId
                Where 1=1
                AND (@EmployeeId IS NULL OR @EmployeeId=0 OR TAX.EmployeeId =@EmployeeId)
                AND (TAX.FiscalYearId =@FiscalYearId)
                AND (TAX.BranchId =@BranchId)
                AND TAX.CompanyId = @CompanyId
                AND TAX.OrganizationId = @OrganizationId";

                list = await _dapper.SqlQueryListAsync<EmployeeTaxProcesDetailInfosViewModel>(user.Database, query, new
                {
                    EmployeeId = employeeId,
                    FiscalYearId = fiscalYearId,
                    BranchId = branchId,
                    CompanyId = user.CompanyId,
                    OrganizationId = user.OrganizationId
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return list;
        }
        public async Task<IEnumerable<Shared.Payroll.ViewModel.Tax.TaxProcessSummeryInfoViewModel>> GetFinalTaxProcessSummaryAsync(long fiscalYearId, long branchId, AppUser user)
        {
            IEnumerable<Shared.Payroll.ViewModel.Tax.TaxProcessSummeryInfoViewModel> list = new List<Shared.Payroll.ViewModel.Tax.TaxProcessSummeryInfoViewModel>();
            try
            {
                var query = $@"SELECT IY.FiscalYearId,IY.FiscalYearRange,IY.AssesmentYear,TAX.BranchId,
                SalaryMonth= MONTH(IY.FiscalYearTo),
                SalaryYear= YEAR(IY.FiscalYearTo),
                HeadCount=COUNT(*),
                YearlyTaxableIncome=ISNULL(SUM(ROUND(TAX.YearlyTaxableIncome,0)),0),
                TotalTaxPayable=ISNULL(SUM(ROUND(TAX.TotalTaxPayable,0)),0),
                AITAmount=ISNULL(SUM(TAX.AITAmount),0),
                TaxReturnAmount =ISNULL(SUM(TAX.TaxReturnAmount),0),
                ExcessTaxPaidRefundAmount = ISNULL(SUM(TAX.ExcessTaxPaidRefundAmount),0),
                YearlyTax = ISNULL(SUM(ROUND(TAX.YearlyTax,0)),0),
                PaidTotalTax = ISNULL(SUM(TAX.PaidTotalTax),0),
                ProjectionAmount= ISNULL(SUM(ROUND(TAX.ProjectionAmount,0)),0),
                ProjectionTax= ISNULL(SUM(ROUND(TAX.ProjectionTax,0)),0),
                OnceOffAmount= ISNULL(SUM(ROUND(TAX.OnceOffAmount,0)),0),
                OnceOffTax= ISNULL(SUM(ROUND(TAX.OnceOffTax,0)),0),
                MonthlyTax = ISNULL(SUM(ROUND(TAX.MonthlyTax,0)),0),
                ActualTaxDeductionAmount = ISNULL(SUM(ROUND(TAX.ActualTaxDeductionAmount,0)),0),
                PFContributionBothPart = ISNULL(SUM(ROUND(TAX.PFContributionBothPart,0)),0),
                OtherInvestment = ISNULL(SUM(ROUND(TAX.OtherInvestment,0)),0),
                ActualInvestmentMade = ISNULL(SUM(ROUND(TAX.ActualInvestmentMade,0)),0),
                InvestmentRebateAmount = ISNULL(SUM(TAX.InvestmentRebateAmount),0),
                ExemptionAmountOnAnnualIncome= ISNULL(SUM(TAX.ExemptionAmountOnAnnualIncome),0),
                TaxableIncome= ISNULL(SUM(ROUND(TAX.TaxableIncome,0)),0),
                TotalLessExemptedAmount= ISNULL(SUM(TAX.TotalLessExemptedAmount),0),
                TotalCurrentMonthAllowanceAmount= ISNULL(SUM(TAX.TotalCurrentMonthAllowanceAmount),0),
                TotalProjectedAllowanceAmount= ISNULL(SUM(TAX.TotalProjectedAllowanceAmount),0),
                TotalTillMonthAllowanceAmount= ISNULL(SUM(TAX.TotalTillMonthAllowanceAmount),0),
                TotalGrossAnnualIncome= ISNULL(SUM(ROUND(TAX.TotalGrossAnnualIncome,0)),0),
                GrossTaxableIncome= ISNULL(SUM(ROUND(TAX.TotalGrossAnnualIncome,0)),0)
                From Payroll_FinalTaxProcess TAX
                INNER JOIN Payroll_FiscalYear IY ON TAX.FiscalYearId = IY.FiscalYearId
                WHERE 1=1
                AND (@FiscalYearId IS NULL OR @FiscalYearId =0 OR IY.FiscalYearId=@FiscalYearId)
                AND (@BranchId IS NULL OR @BranchId =0 OR TAX.BranchId=@BranchId)
                AND TAX.CompanyId=@CompanyId AND TAX.OrganizationId=@OrganizationId
                Group By IY.FiscalYearId,IY.FiscalYearRange,IY.AssesmentYear,IY.FiscalYearTo,TAX.BranchId";
                list = await _dapper.SqlQueryListAsync<Shared.Payroll.ViewModel.Tax.TaxProcessSummeryInfoViewModel>(user.Database, query, new
                {
                    FiscalYearId = fiscalYearId,
                    BranchId = branchId,
                    CompanyId = user.CompanyId,
                    OrganizationId = user.OrganizationId
                });
                if (list != null && list.Any())
                {
                    var branches = await _branchInfoBusiness.GetBranchsAsync("", user);
                    if (branches != null && branches.Any())
                    {
                        list.ToList().ForEach(item =>
                        {
                            var branch = branches.FirstOrDefault(i => i.BranchId == item.BranchId);
                            if (branch != null)
                            {
                                item.BranchName = branch.BranchName;
                            }
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return list ?? new List<TaxProcessSummeryInfoViewModel>();
        }
        public async Task<ExecutionStatus> SaveAsync(List<FinalTaxProcessedInfo> infos, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                using (var transaction = await _payrollDbContext.Database.BeginTransactionAsync())
                {
                    try
                    {
                        bool isSuccessful = true;
                        foreach (var item in infos)
                        {
                            await _payrollDbContext.Payroll_FinalTaxProcess.AddAsync(item.FinalTaxProcess);
                            if (await _payrollDbContext.SaveChangesAsync() > 0)
                            {
                                item.FinalTaxProcessDetails.ForEach(i =>
                                {
                                    i.TaxProcessId = item.FinalTaxProcess.TaxProcessId;
                                    i.SalaryMonth = item.FinalTaxProcess.SalaryMonth;
                                    i.SalaryYear = item.FinalTaxProcess.SalaryYear;
                                });

                                item.FinalTaxProcessSlabs.ForEach(i =>
                                {
                                    i.TaxProcessId = item.FinalTaxProcess.TaxProcessId;
                                    i.SalaryMonth = item.FinalTaxProcess.SalaryMonth;
                                    i.SalaryYear = item.FinalTaxProcess.SalaryYear;
                                });

                                await _payrollDbContext.Payroll_FinalTaxProcessDetail.AddRangeAsync(item.FinalTaxProcessDetails);
                                if (await _payrollDbContext.SaveChangesAsync() > 0)
                                {
                                    await _payrollDbContext.Payroll_FinalTaxProcessSlab.AddRangeAsync(item.FinalTaxProcessSlabs);
                                    if (await _payrollDbContext.SaveChangesAsync() == 0)
                                    {
                                        isSuccessful = false;
                                        throw new Exception($"Failed to save income slab at employee id {item.FinalTaxProcess.EmployeeId.ToString()}");
                                    }
                                }
                                else
                                {
                                    isSuccessful = false;
                                    throw new Exception($"Failed to save taxable income detail at employee id {item.FinalTaxProcess.EmployeeId.ToString()}");
                                }
                            }
                            else
                            {
                                isSuccessful = false;
                                throw new Exception($"Failed to save tax info at employee id {item.FinalTaxProcess.EmployeeId.ToString()}");
                            }
                        }

                        if (isSuccessful)
                        {
                            await transaction.CommitAsync();
                            executionStatus = ResponseMessage.Message(true, ResponseMessage.Successfull);
                        }
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        Console.WriteLine($"Failed to save {ex.ToString()}");
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
                Console.WriteLine($"Failed to save {ex.ToString()}");
                executionStatus = ResponseMessage.Message(false, "Something went while saving tax data");
            }
            return executionStatus;
        }
        public async Task<DataTable> Download108Report(long fiscalYearId, long branchId, AppUser user)
        {
            DataTable dataTable = new DataTable();
            try
            {
                var fiscalYearInfo = await _fiscalYearBusiness.GetFiscalYearByIdAsync(fiscalYearId, user);
                if (fiscalYearInfo != null && fiscalYearInfo.FiscalYearFrom.HasValue && fiscalYearInfo.FiscalYearTo.HasValue)
                {
                    var year = fiscalYearInfo.FiscalYearTo.Value.Year;
                    var month = fiscalYearInfo.FiscalYearTo.Value.Month;

                    var parameters = new Dictionary<string, string>();
                    parameters.Add("FiscalYearId", fiscalYearInfo.FiscalYearId.ToString());
                    parameters.Add("Month", month.ToString());
                    parameters.Add("Year", year.ToString());
                    parameters.Add("BranchId", branchId.ToString());
                    parameters.Add("CompanyId", user.CompanyId.ToString());
                    parameters.Add("OrganizationId", user.OrganizationId.ToString());

                    dataTable = await _dapper.SqlDataTable(user.Database, "sp_Payroll_IncomeTax108Report", parameters, CommandType.StoredProcedure);
                    if (dataTable.Rows.Count > 0)
                    {
                        var branches = await _branchInfoBusiness.GetBranchsAsync("", user);
                        if (dataTable.Columns.Contains("BranchName") && branches.Any())
                        {
                            for (int i = 0; i < dataTable.Rows.Count; i++)
                            {
                                branchId = Utility.TryParseLong(dataTable.Rows[i]["BranchName"].ToString());
                                var branch = branches.FirstOrDefault(item => item.BranchId == branchId);
                                if (branch != null)
                                {
                                    dataTable.Rows[i]["BranchName"] = branch.BranchName;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return dataTable;
        }
        public IEnumerable<EmployeeTaxProcessDetail> GetTaxProcessDetails(long employeeId, long fiscalYearId, int year, int month, AppUser user)
        {
            IEnumerable<EmployeeTaxProcessDetail> list = new List<EmployeeTaxProcessDetail>();
            try
            {
                list = _payrollDbContext.Payroll_EmployeeTaxProcessDetail.Where(i => i.EmployeeId == employeeId && i.FiscalYearId == fiscalYearId && i.SalaryMonth == month && i.SalaryYear == year && i.CompanyId == user.CompanyId && i.OrganizationId == user.OrganizationId).ToList();
            }
            catch (Exception ex)
            {
            }
            return list;
        }
    }
}
