using Shared.Services;
using BLL.Base.Interface;
using Shared.OtherModels.User;
using System.Data;
using Shared.Payroll.Process.Tax;
using DAL.DapperObject.Interface;
using Shared.Payroll.Domain.Tax;
using Shared.Payroll.Domain.Setup;
using Shared.Payroll.Process.Allowance;
using Shared.Control_Panel.Domain;
using BLL.Tax.Interface;
using Shared.Payroll.ViewModel.Tax;

namespace BLL.Tax.Implementation
{
    public class TaxRulesBusiness : ITaxRulesBusiness
    {
        private readonly IDapperData _dapper;
        private readonly ISysLogger _sysLogger;
        private readonly ITaxAITBusiness _taxAITBusiness;
        private readonly ITaxSettingBusiness _taxSettingBusiness;
        private readonly IIncomeTaxSlabBusiness _incomeTaxSlabBusiness;
        private readonly ISpecialTaxSlabBusiness _specialTaxSlabBusiness;
        private readonly IEmployeeFreeCarBusiness _employeeFreeCarBusiness;
        private readonly IEmployeeInvestmentSubmissionBusiness _employeeInvestmentSubmissionBusiness;

        public TaxRulesBusiness(
            ISysLogger sysLogger,
            IDapperData dapper,
            IEmployeeFreeCarBusiness employeeFreeCarBusiness,
            ITaxSettingBusiness taxSettingBusiness,
            IIncomeTaxSlabBusiness incomeTaxSlabBusiness,
            ITaxAITBusiness taxAITBusiness,
            IEmployeeInvestmentSubmissionBusiness employeeInvestmentSubmissionBusiness,
            ISpecialTaxSlabBusiness specialTaxSlabBusiness)
        {
            _sysLogger = sysLogger;
            _dapper = dapper;
            _employeeFreeCarBusiness = employeeFreeCarBusiness;
            _taxSettingBusiness = taxSettingBusiness;
            _incomeTaxSlabBusiness = incomeTaxSlabBusiness;
            _taxAITBusiness = taxAITBusiness;
            _specialTaxSlabBusiness = specialTaxSlabBusiness;
            _employeeInvestmentSubmissionBusiness = employeeInvestmentSubmissionBusiness;
        }
        public async Task<TaxDeductedTillMonth> GetEmployeeTaxDeductedTillMonthAsync(long employeeId, long fiscalYearId, int year, int month, AppUser user)
        {
            TaxDeductedTillMonth taxDeductedTillMonth = new TaxDeductedTillMonth();
            try
            {
                var query = $@"SELECT 
                TillTaxDeducted=ISNULL(SUM(ActualTaxDeductionAmount),0),
                TillProjectionTax=ISNULL(SUM(ProjectionTax),0),
                TillOnceOffTax=ISNULL(SUM(OnceOffTax),0)
                FROM Payroll_EmployeeTaxProcess Where EmployeeId=@EmployeeId AND FiscalYearId=@FiscalYearId AND CompanyId=@CompanyId AND OrganizationId=@OrganizationId AND SalaryMonth<>@Month";

                taxDeductedTillMonth = await _dapper.SqlQueryFirstAsync<TaxDeductedTillMonth>(user.Database, query.Trim(), new { EmployeeId = employeeId, FiscalYearId = fiscalYearId, Month = month, user.CompanyId, user.OrganizationId });

                //taxDeductedTillMonth.TillTaxDeducted = taxDeductedTillMonth.TillTaxDeducted +
                  //  await GetBeforeThisMonthSupplementaryOnceOffTaxAsync(employeeId, fiscalYearId, year, month, user);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "TaxRulesBusiness", "GetTotalTaxDeductedAsync", user);
            }
            return taxDeductedTillMonth;
        }
        public async Task<decimal> GetThisMonthSupplementaryOnceOffTaxAsync(long employeeId, long fiscalYearId, int year, int month, AppUser user)
        {
            decimal taxAmount = 0;
            try
            {
                var query = $@"SELECT ISNULL((SELECT SUM(SPA.OnceOffAmount) FROM Payroll_SupplementaryPaymentAmount SPA
	            INNER JOIN Payroll_SupplementaryPaymentProcessInfo SPI ON SPA.PaymentProcessInfoId = SPI.PaymentProcessInfoId
	            WHERE SPA.EmployeeId=@EmployeeId AND SPI.FiscalYearId=@FiscalYearId AND SPA.StateStatus='Disbursed' AND SPA.PaymentYear=@Year AND SPA.PaymentMonth=@Month AND SPA.CompanyId=@CompanyId AND SPA.OrganizationId = @OrganizationId),0)";

                taxAmount = await _dapper.SqlQueryFirstAsync<decimal>(user.Database, query, new { EmployeeId = employeeId, FiscalYearId = fiscalYearId, Year = year, Month = month, user.CompanyId, user.OrganizationId });
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "TaxRulesBusiness", "GetThisMonthSupplementaryOnceOffTax", user);
            }
            return taxAmount;
        }
        public async Task<EmployeeTaxProcessedInfo> TaxRulesIY2324(EligibleEmployeeForTaxType employee, FiscalYear fiscalYear, List<Shared.Payroll.Process.Tax.TaxDetailInTaxProcess> taxDetails, PayrollModuleConfig payrollModuleConfig, AllowanceInfo allowanceInfo, int totalMonthReceipt, int remainMonth, int year, int month, AppUser user)
        {
            EmployeeTaxProcessedInfo employeeTaxProcessedInfo = new EmployeeTaxProcessedInfo();
            try
            {

                // Free Car //
                int cc = await _employeeFreeCarBusiness.GetEmployeeFreeCarByEmployeeIdInTaxProcessAsync(employee, fiscalYear, year, month, user);
                var taxSetting = await _taxSettingBusiness.GetTaxSettingByFiscalYearIdAsync(fiscalYear.FiscalYearId, user);
                //decimal freeCarTillAmount = await _employeeFreeCarBusiness.CCTillAmountInTaxProcessAsync(employee, fiscalYear, year, month, user);

                //if (cc > 0 || freeCarTillAmount > 0)
                //{
                //    decimal ccAmount = 0;
                //    if (cc > 0 && cc <= (taxSetting.IncomeTaxSetting.FreeCarCCMinimumLimit ?? 0))
                //    {
                //        ccAmount = taxSetting.IncomeTaxSetting.FreeCarMinTaxableAmount ?? 0;
                //    }
                //    else
                //    {
                //        ccAmount = taxSetting.IncomeTaxSetting.FreeCarMaxTaxableAmount ?? 0;
                //    }

                //    taxDetails.Add(new Shared.Payroll.Process.Tax.TaxDetailInTaxProcess()
                //    {
                //        EmployeeId = employee.EmployeeId,
                //        AllowanceNameId = 0,
                //        AllowanceName = "Free Car",
                //        ProjectRestYear = true,
                //        OnceOffDeduction = false,
                //        Flag = "FREE CAR",
                //        TillAmount = freeCarTillAmount,
                //        CurrentAmount = ccAmount,
                //        Amount = 0,
                //        Arrear = 0,
                //        ReviewAmount = 0,
                //        RemainFiscalYearMonth = (short)remainMonth,
                //        ProjectedAmount = employee.IsDiscontinued == false ? ccAmount * remainMonth : 0
                //    });
                //}
                foreach (var item in taxDetails)
                {
                    item.EmployeeId = employee.EmployeeId;
                    item.GrossAnnualIncome = item.TillAmount + item.CurrentAmount + item.ProjectedAmount;
                    item.LessExemptedAmount = 0;
                    item.ExemptionAmount = 0;
                    item.ExemptionPercentage = 0;
                    item.RemainFiscalYearMonth = (short)remainMonth;
                    item.TotalTaxableIncome = item.TillAmount + item.CurrentAmount + item.ProjectedAmount;
                }

                decimal totalPFExemptionAmount = 0;
                if ((taxSetting.IncomeTaxSetting.PFBothPartExemption ?? false) == true)
                {
                    totalPFExemptionAmount = taxDetails.Where(i => i.Flag == "PF").Sum(i => i.TotalTaxableIncome) * 2;
                }

                decimal totalTillAmount = taxDetails.Sum(i => i.TillAmount);
                decimal totalCurrentAmount = taxDetails.Sum(i => i.CurrentAmount);
                decimal totalProjectedAmount = taxDetails.Sum(i => i.ProjectedAmount);
                decimal totalGrossAnnualIncome = taxDetails.Sum(i => i.GrossAnnualIncome);
                decimal totalLessExemptedAmount = taxDetails.Sum(i => i.LessExemptedAmount);
                decimal grossTaxableIncome = taxDetails.Sum(i => i.TotalTaxableIncome);

                decimal totalIncomeAfterPFExemption = 0;
                decimal actualAnnualExemptionAmount = grossTaxableIncome / 100 * taxSetting.IncomeTaxSetting.ExemptionPercentageOfAnnualIncome ?? 0;
                decimal minimumAnnualExemptionAmount = 0;

                if (actualAnnualExemptionAmount < taxSetting.IncomeTaxSetting.ExemptionAmountOfAnnualIncome)
                {
                    minimumAnnualExemptionAmount = actualAnnualExemptionAmount;
                }
                else
                {
                    minimumAnnualExemptionAmount = taxSetting.IncomeTaxSetting.ExemptionAmountOfAnnualIncome ?? 0;
                }

                decimal totalTaxableIncome = grossTaxableIncome - minimumAnnualExemptionAmount;
                totalTaxableIncome = totalTaxableIncome > 0 ? totalTaxableIncome : 0;
                totalTaxableIncome = totalPFExemptionAmount > 0 ? totalTaxableIncome - totalPFExemptionAmount : totalTaxableIncome;
                totalIncomeAfterPFExemption = totalTaxableIncome;

                decimal totalTaxableIncomeTrace = totalTaxableIncome;

                // Slab //
                // Find Special Slab
                var slabs = await _specialTaxSlabBusiness.GetByEmployeeId(employee.EmployeeId, fiscalYear.FiscalYearId, user);
                if (!slabs.Any())
                {
                    if (!employee.IsResidential)
                    {
                        if (employee.TotalServiceDays <= taxSetting.IncomeTaxSetting.NonResidentialPeriod)
                        {
                            slabs = await _incomeTaxSlabBusiness.GetIncomeTaxSlabsByImpliedConditionAsync("Non Residential", fiscalYear.FiscalYearId, user);
                        }
                        else
                        {
                            slabs = await _incomeTaxSlabBusiness.GetIncomeTaxSlabsByImpliedConditionAsync(employee.Gender, fiscalYear.FiscalYearId, user);
                        }
                    }
                    else
                    {
                        slabs = await _incomeTaxSlabBusiness.GetIncomeTaxSlabsByImpliedConditionAsync(employee.Gender, fiscalYear.FiscalYearId, user);
                    }

                }

                if (slabs.Any())
                {
                    slabs = slabs.OrderBy(i => i.SlabPercentage);
                    int slabCount = 1;
                    foreach (var item in slabs)
                    {

                        decimal currentRate = item.SlabPercentage;
                        decimal taxableDiffAmt = item.SlabMaximumAmount - item.SlabMininumAmount;
                        long incomeTaxSlabId = item.IncomeTaxSlabId;
                        string impliedCondition = item.ImpliedCondition;
                        decimal individualTaxLiablity = 0;

                        string parameter = "";
                        if (slabCount == 1)
                        {
                            if (slabs.ToList().Count > 1)
                            {
                                parameter = "On the First BDT-" + taxableDiffAmt.ToString();
                            }
                            else if (slabs.ToList().Count == 1)
                            {
                                parameter = "On the Total Income";
                            }

                        }
                        else if (slabCount != 1 && slabCount < slabs.Count())
                        {
                            parameter = "On the Next BDT-" + taxableDiffAmt.ToString();
                        }
                        else
                        {
                            parameter = "On remaining balance";
                        }

                        decimal taxableIncome = 0;
                        if (totalTaxableIncomeTrace >= taxableDiffAmt)
                        {
                            taxableIncome = taxableDiffAmt;
                        }
                        else if (totalTaxableIncomeTrace > 0 && totalTaxableIncomeTrace < taxableDiffAmt)
                        {
                            taxableIncome = totalTaxableIncomeTrace;
                        }
                        else
                        {
                            taxableIncome = 0;
                        }
                        individualTaxLiablity = taxableIncome > 0 ? taxableIncome / 100 * item.SlabPercentage : 0;

                        employeeTaxProcessedInfo.EmployeeTaxProcessSlabs.Add(new EmployeeTaxProcessSlab()
                        {
                            EmployeeId = employee.EmployeeId,
                            FiscalYearId = fiscalYear.FiscalYearId,
                            IncomeTaxSlabId = item.IncomeTaxSlabId,
                            ImpliedCondition = item.ImpliedCondition,
                            SlabPercentage = item.SlabPercentage,
                            ParameterName = parameter,
                            TaxableIncome = Math.Round(taxableIncome, MidpointRounding.AwayFromZero),
                            TaxLiability = Math.Round(individualTaxLiablity, MidpointRounding.AwayFromZero),
                            SalaryDate = DateTimeExtension.LastDateOfAMonth(year, month),
                            SalaryMonth = (short)month,
                            SalaryYear = (short)year,
                            CreatedBy = user.ActionUserId,
                            CreatedDate = DateTime.Now,
                            CompanyId = user.CompanyId,
                            OrganizationId = user.OrganizationId,
                            BranchId = employee.BranchId
                        });

                        if (totalTaxableIncomeTrace > 0)
                        {
                            if (totalTaxableIncomeTrace >= taxableDiffAmt)
                            {
                                totalTaxableIncomeTrace = totalTaxableIncomeTrace - taxableDiffAmt;
                            }
                            else if (totalTaxableIncomeTrace < taxableDiffAmt)
                            {
                                totalTaxableIncomeTrace = totalTaxableIncomeTrace - taxableIncome;
                            }
                            else if (totalTaxableIncomeTrace > 0 && totalTaxableIncomeTrace < taxableDiffAmt)
                            {
                                totalTaxableIncomeTrace = taxableDiffAmt;
                            }
                        }
                        slabCount = slabCount + 1;
                    }
                }

                // Minimum Tax Amount
                decimal minimumTaxAmount = 0;
                if ((employee.MinimumTaxAmount ?? 0) <= 0)
                {
                    minimumTaxAmount = taxSetting.IncomeTaxSetting.MinTaxAmount ?? 0;
                }
                else
                {
                    minimumTaxAmount = employee.MinimumTaxAmount ?? 0;
                }

                // AIT
                var carAIT = await _taxAITBusiness.GetCarAITAmountInTaxProcessAsync(employee.EmployeeId, fiscalYear.FiscalYearId, user);
                // Tax Refund
                var taxRefundAmount = await _taxAITBusiness.GetTaxRefundAmountInTaxProcessAsync(employee.EmployeeId, fiscalYear.FiscalYearId, user);
                // Start of Rebate Calculation
                decimal pfAmountInTaxProcess = 0;
                decimal pfContributionBothPart = 0;
                decimal otherInvestmentRecogPF = 0;
                decimal actualInvestmentMade = 0;
                decimal taxRebateDueToInvestment = 0;
                decimal actualRebatePercentage = 0;
                decimal? employeeYearlyInvestmentAmount = null;
                decimal? employeeMayNeedToInvestment = null;

                if (taxSetting.TaxInvestmentSetting.IsFlatRebate)
                {
                    actualRebatePercentage = taxSetting.TaxInvestmentSetting.MinRebate;
                }
                pfAmountInTaxProcess = taxDetails.Where(i => i.Flag == "PF").Sum(i => i.TotalTaxableIncome);
                pfContributionBothPart = Math.Round(pfAmountInTaxProcess, MidpointRounding.AwayFromZero) * 2;

                decimal? employeeActualYearlyInvestmentAmount = await _employeeInvestmentSubmissionBusiness.GetYearlInvestmentAmountInTaxProcessAsync(employee.EmployeeId, fiscalYear.FiscalYearId, user);

                decimal totalTaxLiability = employeeTaxProcessedInfo.EmployeeTaxProcessSlabs.Sum(i => i.TaxLiability ?? 0);

                if (employeeActualYearlyInvestmentAmount == null)
                {
                    // -- REBEAT|ACTUAL INVESTMENT|OTHER INVESTMENT
                    if (employee.IsResidential && totalTaxLiability > 0)
                    {
                        taxRebateDueToInvestment = totalTaxableIncome / 100 * actualRebatePercentage;
                        taxRebateDueToInvestment = taxRebateDueToInvestment > (taxSetting.TaxInvestmentSetting.RebateAmount ?? 0) ? taxSetting.TaxInvestmentSetting.RebateAmount ?? 0 : taxRebateDueToInvestment;
                        employeeMayNeedToInvestment = taxRebateDueToInvestment / (taxSetting.TaxInvestmentSetting.MaxInvestmentPercentage / 100);
                        actualInvestmentMade = Math.Round(employeeMayNeedToInvestment ?? 0, 0);
                        otherInvestmentRecogPF = Math.Round(actualInvestmentMade - pfContributionBothPart, 0);
                        taxRebateDueToInvestment = Math.Round(taxRebateDueToInvestment, 0);
                    }
                }
                else
                {
                    if (employee.IsResidential)
                    {
                        employeeActualYearlyInvestmentAmount = (employeeActualYearlyInvestmentAmount ?? 0) + pfContributionBothPart;

                        // Find Tax Rebate on Taxable Income
                        var taxRebateDueToTaxableIncome = totalTaxableIncome / 100 * actualRebatePercentage;
                        taxRebateDueToTaxableIncome = taxRebateDueToTaxableIncome > (taxSetting.TaxInvestmentSetting.RebateAmount ?? 0) ? taxSetting.TaxInvestmentSetting.RebateAmount ?? 0 : taxRebateDueToTaxableIncome;
                        taxRebateDueToTaxableIncome = Math.Round(taxRebateDueToTaxableIncome, MidpointRounding.AwayFromZero);

                        // Find Tax Rebate on actual investion amount
                        var rebateOnInvestment = (employeeActualYearlyInvestmentAmount ?? 0) * (taxSetting.TaxInvestmentSetting.MaxInvestmentPercentage / 100);
                        rebateOnInvestment = Math.Round(rebateOnInvestment, MidpointRounding.AwayFromZero);

                        // Find min rebate between Taxable income rebate & Actual investment rebate
                        var minRebate = Math.Min(taxRebateDueToTaxableIncome, rebateOnInvestment);

                        // Take geniune rebate amount MAX 10 LAC
                        taxRebateDueToInvestment = Math.Min(minRebate, taxSetting.TaxInvestmentSetting.RebateAmount ?? 0);

                        actualInvestmentMade = employeeActualYearlyInvestmentAmount ?? 0;
                        otherInvestmentRecogPF = actualInvestmentMade - pfContributionBothPart;
                        taxRebateDueToInvestment = Math.Round(taxRebateDueToInvestment, 0);
                    }
                }

                #region Default Investment
                decimal defaultTaxRebateDueToInvestment = 0;
                decimal? defaultEmployeeMayNeedToInvestment = 0;
                decimal defaultActualInvestmentMade = 0;
                decimal defaultOtherInvestmentRecogPF = 0;

                if (employee.IsResidential && totalTaxLiability > 0)
                {
                    defaultTaxRebateDueToInvestment = totalTaxableIncome / 100 * actualRebatePercentage;
                    defaultTaxRebateDueToInvestment = defaultTaxRebateDueToInvestment > (taxSetting.TaxInvestmentSetting.RebateAmount ?? 0) ? taxSetting.TaxInvestmentSetting.RebateAmount ?? 0 : defaultTaxRebateDueToInvestment;

                    defaultEmployeeMayNeedToInvestment = defaultTaxRebateDueToInvestment / (taxSetting.TaxInvestmentSetting.MaxInvestmentPercentage / 100);
                    defaultActualInvestmentMade = Math.Round(defaultEmployeeMayNeedToInvestment ?? 0, 0);
                    defaultOtherInvestmentRecogPF = Math.Round(defaultActualInvestmentMade - pfContributionBothPart, 0);
                    defaultTaxRebateDueToInvestment = Math.Round(defaultTaxRebateDueToInvestment, 0);
                }
                #endregion


                var tillNowTaxDeducted = await GetEmployeeTaxDeductedTillMonthAsync(employee.EmployeeId, fiscalYear.FiscalYearId, year, month, user);
                minimumTaxAmount = minimumTaxAmount == 0 ? totalTaxLiability : minimumTaxAmount;

                decimal netTaxPayable = 0;

                if (minimumTaxAmount == totalTaxLiability)
                {
                    netTaxPayable = totalTaxLiability - taxRebateDueToInvestment <= 0 ? 0 : totalTaxLiability - taxRebateDueToInvestment;
                    netTaxPayable = netTaxPayable > 0 ? netTaxPayable : 0;
                    netTaxPayable = Math.Round(netTaxPayable - (carAIT + taxRefundAmount));
                }
                else
                {
                    if (totalTaxLiability == 0)
                    {
                        netTaxPayable = 0;
                    }
                    else if (totalTaxLiability > 0 && totalTaxLiability < taxRebateDueToInvestment)
                    {
                        netTaxPayable = minimumTaxAmount;
                    }
                    else if (totalTaxLiability > 0 && totalTaxLiability - taxRebateDueToInvestment <= minimumTaxAmount)
                    {
                        netTaxPayable = minimumTaxAmount;
                    }
                    else if (totalTaxLiability > 0 && totalTaxLiability - taxRebateDueToInvestment > minimumTaxAmount)
                    {
                        netTaxPayable = totalTaxLiability - taxRebateDueToInvestment;
                    }

                    netTaxPayable = netTaxPayable > 0 ? netTaxPayable : 0;
                    netTaxPayable = netTaxPayable > 0 && netTaxPayable < minimumTaxAmount ? minimumTaxAmount : netTaxPayable;
                    netTaxPayable = Math.Round(netTaxPayable - (carAIT + taxRefundAmount), 0);
                    netTaxPayable = netTaxPayable < 0 ? 0 : netTaxPayable;
                }

                decimal paidTotalTaxTillLastMonth = tillNowTaxDeducted.TillTaxDeducted;
                decimal taxToBeAdjusted = netTaxPayable > 0 ? netTaxPayable - paidTotalTaxTillLastMonth : 0;

                decimal yearlyTaxableIncome = totalTaxableIncome;
                decimal totalTaxPayable = totalTaxLiability;
                decimal invesmentRebate = taxRebateDueToInvestment;
                decimal aitAmount = carAIT; //+ taxRefundAmount;
                decimal excessTaxPaidRefundAmount = taxRefundAmount;
                decimal yearlyTax = netTaxPayable;

                // Projection Calculation
                decimal arrearAmount = 0;
                decimal actualOnceOffAmount = 0;
                decimal totalTaxableIncomeInProjection = 0;
                decimal totalTaxableIncomeInProjectionTrace = 0;

                foreach (var item in taxDetails)
                {
                    decimal diffAmount = 0;
                    if ((item.ProjectRestYear ?? false) == true)
                    {
                        if (item.LessExemptedAmount > 0 && item.TillAmount + item.Amount + item.ProjectedAmount >= item.LessExemptedAmount && arrearAmount > 0)
                        {
                            arrearAmount = arrearAmount + item.Arrear;
                        }
                        else if (item.LessExemptedAmount > 0
                            && item.TillAmount + item.Amount + item.ProjectedAmount < item.LessExemptedAmount
                            && item.TillAmount + item.Amount + item.Arrear + item.ProjectedAmount > item.LessExemptedAmount
                            && item.Arrear > 0
                            )
                        {
                            diffAmount = item.LessExemptedAmount - (item.TillAmount + item.Amount + item.ProjectedAmount);
                            arrearAmount = arrearAmount + (diffAmount < item.Arrear ? item.Arrear - diffAmount : 0);
                        }
                        else if (item.LessExemptedAmount == 0)
                        {
                            arrearAmount = arrearAmount + item.Arrear;
                        }
                    }
                }

                actualOnceOffAmount = taxDetails.Where(i => (i.OnceOffDeduction ?? false) == true).Sum(i => i.CurrentAmount);
                decimal pfArrearAmount = taxDetails.Where(i => i.Flag == "PF").Sum(i => i.Arrear);

                if ((taxSetting.IncomeTaxSetting.CalculateTaxOnArrearAmount ?? false) == false)
                {
                    arrearAmount = 0;
                }

                totalTaxableIncomeInProjection = totalGrossAnnualIncome - (arrearAmount + actualOnceOffAmount);
                var exemptionAmountInProjection = totalTaxableIncomeInProjection * ((taxSetting.IncomeTaxSetting.ExemptionPercentageOfAnnualIncome ?? 0) / 100);
                exemptionAmountInProjection = exemptionAmountInProjection < (taxSetting.IncomeTaxSetting.ExemptionAmountOfAnnualIncome ?? 0) ? exemptionAmountInProjection : taxSetting.IncomeTaxSetting.ExemptionAmountOfAnnualIncome ?? 0;
                totalTaxableIncomeInProjection = totalTaxableIncomeInProjection - (exemptionAmountInProjection + ((taxSetting.IncomeTaxSetting.PFBothPartExemption ?? false) == true ? pfAmountInTaxProcess * 2 : 0));
                totalTaxableIncomeInProjectionTrace = totalTaxableIncomeInProjection;
                List<EmployeeTaxProcessSlab> EmployeeTaxProcessSlabInProjection = new List<EmployeeTaxProcessSlab>();

                if (slabs.Any())
                {
                    int slabCount = 1;
                    foreach (var slab in slabs)
                    {
                        foreach (var item in slabs)
                        {

                            decimal currentRate = item.SlabPercentage;
                            decimal taxableDiffAmt = item.SlabMaximumAmount - item.SlabMininumAmount;
                            long incomeTaxSlabId = item.IncomeTaxSlabId;
                            string impliedCondition = item.ImpliedCondition;
                            decimal individualTaxLiablity = 0;

                            string parameter = "";
                            if (slabCount == 1)
                            {
                                parameter = "On the First BDT-" + taxableDiffAmt.ToString();
                            }
                            else if (slabCount != 1 && slabCount < slabs.Count())
                            {
                                parameter = "On the Next BDT-" + taxableDiffAmt.ToString();
                            }
                            else
                            {
                                parameter = "On remaining balance";
                            }

                            decimal taxableIncome = 0;
                            if (totalTaxableIncomeInProjectionTrace >= taxableDiffAmt)
                            {
                                taxableIncome = taxableDiffAmt;
                            }
                            else if (totalTaxableIncomeInProjectionTrace > 0 && totalTaxableIncomeInProjectionTrace < taxableDiffAmt)
                            {
                                taxableIncome = totalTaxableIncomeInProjectionTrace;
                            }
                            else
                            {
                                taxableIncome = 0;
                            }
                            individualTaxLiablity = taxableIncome > 0 ? taxableIncome / 100 * item.SlabPercentage : 0;

                            EmployeeTaxProcessSlabInProjection.Add(new EmployeeTaxProcessSlab()
                            {
                                EmployeeId = employee.EmployeeId,
                                FiscalYearId = fiscalYear.FiscalYearId,
                                IncomeTaxSlabId = item.IncomeTaxSlabId,
                                ImpliedCondition = item.ImpliedCondition,
                                SlabPercentage = item.SlabPercentage,
                                ParameterName = parameter,
                                TaxableIncome = taxableIncome,
                                TaxLiability = Math.Round(individualTaxLiablity, MidpointRounding.AwayFromZero)

                            });

                            if (totalTaxableIncomeInProjectionTrace > 0)
                            {
                                if (totalTaxableIncomeInProjectionTrace >= taxableDiffAmt)
                                {
                                    totalTaxableIncomeInProjectionTrace = totalTaxableIncomeInProjectionTrace - taxableDiffAmt;
                                }
                                else if (totalTaxableIncomeInProjectionTrace < taxableDiffAmt)
                                {
                                    totalTaxableIncomeInProjectionTrace = totalTaxableIncomeInProjectionTrace - taxableIncome;
                                }
                                else if (totalTaxableIncomeInProjectionTrace > 0 && totalTaxableIncomeInProjectionTrace < taxableDiffAmt)
                                {
                                    totalTaxableIncomeInProjectionTrace = taxableDiffAmt;
                                }
                            }
                            slabCount = slabCount + 1;
                        }
                    }
                }

                decimal totalTaxLiabilityInProjection = EmployeeTaxProcessSlabInProjection.Sum(i => i.TaxLiability ?? 0);
                decimal totalTaxableIncomeInProjectionSlab = Math.Round(EmployeeTaxProcessSlabInProjection.Sum(i => i.TaxableIncome ?? 0), 0);

                decimal pfAmountInTaxProcessInProjection = pfAmountInTaxProcess - pfArrearAmount;
                decimal pfContributionBothPartInProjection = pfAmountInTaxProcessInProjection * 2;
                decimal supplementaryOnceOffTaxAmount = await GetThisMonthSupplementaryOnceOffTaxAsync(employee.EmployeeId, fiscalYear.FiscalYearId, year, month, user);

                decimal taxRebateDueToInvestmentInProjection = 0;
                decimal actualInvestmentMadeInProjection = 0;
                decimal otherInvestmentRecogPFInProjection = 0;
                if (employeeActualYearlyInvestmentAmount == null)
                {
                    // -- REBEAT|ACTUAL INVESTMENT|OTHER INVESTMENT
                    if (employee.IsResidential && totalTaxLiabilityInProjection > 0)
                    {
                        taxRebateDueToInvestmentInProjection = totalTaxableIncomeInProjection / 100 * actualRebatePercentage;
                        taxRebateDueToInvestmentInProjection = taxRebateDueToInvestmentInProjection > (taxSetting.TaxInvestmentSetting.RebateAmount ?? 0) ? taxSetting.TaxInvestmentSetting.RebateAmount ?? 0 : taxRebateDueToInvestmentInProjection;
                        employeeMayNeedToInvestment = taxRebateDueToInvestmentInProjection / (taxSetting.TaxInvestmentSetting.MaxInvestmentPercentage / 100);
                        actualInvestmentMadeInProjection = Math.Round(employeeMayNeedToInvestment ?? 0, 0);
                        otherInvestmentRecogPFInProjection = Math.Round(actualInvestmentMadeInProjection - pfContributionBothPart, 0);
                        taxRebateDueToInvestmentInProjection = Math.Round(taxRebateDueToInvestmentInProjection, 0);
                    }
                }
                else
                {
                    if (employee.IsResidential)
                    {
                        if (totalTaxLiabilityInProjection > 0)
                        {
                            employeeActualYearlyInvestmentAmount = employeeActualYearlyInvestmentAmount + pfContributionBothPartInProjection;
                            // Find Tax Rebate on Taxable Income
                            var taxRebateDueToTaxableIncome = totalTaxableIncomeInProjection / 100 * actualRebatePercentage;
                            taxRebateDueToTaxableIncome = taxRebateDueToTaxableIncome > (taxSetting.TaxInvestmentSetting.RebateAmount ?? 0) ? taxSetting.TaxInvestmentSetting.RebateAmount ?? 0 : taxRebateDueToTaxableIncome;
                            taxRebateDueToTaxableIncome = Math.Round(taxRebateDueToTaxableIncome, MidpointRounding.AwayFromZero);

                            // Find Tax Rebate on actual investion amount
                            var rebateOnInvestment = (employeeActualYearlyInvestmentAmount ?? 0) * (taxSetting.TaxInvestmentSetting.MaxInvestmentPercentage / 100);
                            rebateOnInvestment = Math.Round(rebateOnInvestment, MidpointRounding.AwayFromZero);

                            // Find min rebate between Taxable income rebate & Actual investment rebate
                            var minRebate = Math.Min(taxRebateDueToTaxableIncome, rebateOnInvestment);

                            // Take geniune rebate amount MAX 10 LAC
                            taxRebateDueToInvestmentInProjection = Math.Min(minRebate, taxSetting.TaxInvestmentSetting.RebateAmount ?? 0);

                            actualInvestmentMadeInProjection = employeeActualYearlyInvestmentAmount ?? 0;
                            otherInvestmentRecogPFInProjection = actualInvestmentMadeInProjection - pfContributionBothPartInProjection;
                            taxRebateDueToInvestmentInProjection = Math.Round(taxRebateDueToInvestmentInProjection, 0);
                        }

                    }
                }

                minimumTaxAmount = minimumTaxAmount == 0 ? totalTaxLiabilityInProjection : minimumTaxAmount;

                decimal netTaxPayableInProjection = 0;

                if (minimumTaxAmount == totalTaxLiabilityInProjection)
                {
                    netTaxPayableInProjection = totalTaxLiabilityInProjection - taxRebateDueToInvestmentInProjection <= 0 ? 0 : totalTaxLiabilityInProjection - taxRebateDueToInvestmentInProjection;
                    netTaxPayableInProjection = netTaxPayableInProjection > 0 ? netTaxPayableInProjection : 0;
                    netTaxPayableInProjection = Math.Round(netTaxPayableInProjection - (carAIT + taxRefundAmount), 0);
                }
                else
                {
                    if (totalTaxLiabilityInProjection == 0)
                    {
                        netTaxPayableInProjection = 0;
                    }
                    else if (totalTaxLiabilityInProjection > 0 && totalTaxLiabilityInProjection < taxRebateDueToInvestmentInProjection)
                    {
                        netTaxPayableInProjection = minimumTaxAmount;
                    }
                    else if (totalTaxLiabilityInProjection > 0 && totalTaxLiabilityInProjection - taxRebateDueToInvestmentInProjection <= minimumTaxAmount)
                    {
                        netTaxPayableInProjection = minimumTaxAmount;
                    }
                    else if (totalTaxLiabilityInProjection > 0 && totalTaxLiabilityInProjection - taxRebateDueToInvestmentInProjection > minimumTaxAmount)
                    {
                        netTaxPayableInProjection = totalTaxLiabilityInProjection - taxRebateDueToInvestmentInProjection;
                    }

                    netTaxPayableInProjection = netTaxPayableInProjection > 0 ? netTaxPayableInProjection : 0;
                    netTaxPayableInProjection = netTaxPayableInProjection > 0 && netTaxPayableInProjection < minimumTaxAmount ? minimumTaxAmount : netTaxPayableInProjection;
                    netTaxPayableInProjection = Math.Round(netTaxPayableInProjection - (carAIT + taxRefundAmount), 0);
                    netTaxPayableInProjection = netTaxPayableInProjection < 0 ? 0 : netTaxPayableInProjection;
                }

                decimal taxToBeAdjustedInProjection = netTaxPayableInProjection > 0 ? netTaxPayableInProjection - tillNowTaxDeducted.TillTaxDeducted : 0;

                remainMonth = employee.TerminationDate != null
                    && employee.TerminationDate.Value.IsDateBetweenTwoDates(fiscalYear.FiscalYearFrom.Value, fiscalYear.FiscalYearTo.Value)
                    && employee.TerminationDate.Value.Day == employee.TerminationDate.Value.DaysInAMonth() ? employee.RemainFiscalYearMonth + 1 : remainMonth + 1;

                decimal projectionTax = 0;
                decimal actualProjectionTax = 0;


                if (remainMonth > 0)
                {
                    projectionTax = netTaxPayableInProjection > 0 ? (netTaxPayableInProjection - (tillNowTaxDeducted.TillProjectionTax + tillNowTaxDeducted.TillOnceOffTax)) / remainMonth : 0;
                    actualProjectionTax = netTaxPayableInProjection > 0 ? (netTaxPayableInProjection - tillNowTaxDeducted.TillTaxDeducted) / remainMonth : 0;
                    projectionTax = actualProjectionTax;
                }
                else
                {
                    projectionTax = netTaxPayableInProjection - tillNowTaxDeducted.TillTaxDeducted;
                    actualProjectionTax = netTaxPayableInProjection - tillNowTaxDeducted.TillTaxDeducted;
                }


                projectionTax = projectionTax < 0 ? 0 : Math.Round(projectionTax, MidpointRounding.AwayFromZero);
                decimal onceOffTax = netTaxPayable - netTaxPayableInProjection;
                onceOffTax = onceOffTax == 1 ? 0 : onceOffTax;

                //  Adjust extra tax deduction amount
                if (actualProjectionTax < 0 && onceOffTax > 0 && onceOffTax >= Math.Abs(actualProjectionTax))
                {
                    onceOffTax = onceOffTax - Math.Abs(actualProjectionTax);
                }

                decimal monthlyTax = projectionTax + onceOffTax;
                monthlyTax = monthlyTax > 0 ? monthlyTax : 0;

                decimal paidTotalTaxIncludingThisMonth = Math.Round(tillNowTaxDeducted.TillTaxDeducted + monthlyTax);


                decimal actualTaxDeductionAmount = 0;
                if (monthlyTax == 0)
                {
                    actualTaxDeductionAmount = monthlyTax;
                }
                if (monthlyTax > 0 && (taxSetting.IncomeTaxSetting.MonthlyTaxDeductionPercentage ?? 0) == 0)
                {
                    actualTaxDeductionAmount = 0;
                }
                if (monthlyTax > 0 && (taxSetting.IncomeTaxSetting.MonthlyTaxDeductionPercentage ?? 0) > 0)
                {
                    actualTaxDeductionAmount = Math.Round(monthlyTax / 100 * (taxSetting.IncomeTaxSetting.MonthlyTaxDeductionPercentage ?? 0));

                }

                // Table: Employee Tax Process
                string ytdLabel = "";
                var firstOfSalaryMonth = new DateTime(year, month, 1);
                if (month == 7)
                {
                    ytdLabel = "YTD Amount";
                }
                else
                {
                    string monthName = firstOfSalaryMonth.AddMonths(-1).GetMonthName();
                    ytdLabel = "YTD-" + monthName;
                }

                // Projection Label
                string projectionLabel = "";
                DateTime? fiscalYearEndDate = null;
                if (employee.TerminationDate == null)
                {
                    fiscalYearEndDate = fiscalYear.FiscalYearTo;
                }
                else
                {
                    if (employee.TerminationDate.Value.IsDateBetweenTwoDates(fiscalYear.FiscalYearFrom.Value, fiscalYear.FiscalYearTo.Value))
                    {
                        fiscalYearEndDate = employee.TerminationDate;
                    }
                    else
                    {
                        fiscalYearEndDate = fiscalYear.FiscalYearTo;
                    }
                }

                var nextMonth = firstOfSalaryMonth.AddMonths(1);
                if (nextMonth.Month != fiscalYearEndDate.Value.Month)
                {
                    projectionLabel = nextMonth.ToString("MMM") + "'" + nextMonth.ToString("yy") + "-";
                }
                projectionLabel = projectionLabel + fiscalYearEndDate.Value.ToString("MMM") + "'" + fiscalYearEndDate.Value.ToString("yy");

                #region Employee Tax Process Info

                employeeTaxProcessedInfo.EmployeeTaxProcess.EmployeeId = employee.EmployeeId;
                employeeTaxProcessedInfo.EmployeeTaxProcess.SalaryProcessId = employee.SalaryProcessId;
                employeeTaxProcessedInfo.EmployeeTaxProcess.SalaryProcessDetailId = employee.SalaryProcessDetailId;
                employeeTaxProcessedInfo.EmployeeTaxProcess.FiscalYearId = fiscalYear.FiscalYearId;
                employeeTaxProcessedInfo.EmployeeTaxProcess.SalaryMonth = (short)month;
                employeeTaxProcessedInfo.EmployeeTaxProcess.SalaryYear = (short)year;
                employeeTaxProcessedInfo.EmployeeTaxProcess.YearlyTaxableIncome = yearlyTaxableIncome;
                employeeTaxProcessedInfo.EmployeeTaxProcess.TotalTaxPayable = totalTaxPayable;
                employeeTaxProcessedInfo.EmployeeTaxProcess.InvestmentRebateAmount = invesmentRebate;
                employeeTaxProcessedInfo.EmployeeTaxProcess.AITAmount = aitAmount;
                employeeTaxProcessedInfo.EmployeeTaxProcess.TaxReturnAmount = 0;
                employeeTaxProcessedInfo.EmployeeTaxProcess.ProjectionLabel = projectionLabel;
                employeeTaxProcessedInfo.EmployeeTaxProcess.YTDLabel = ytdLabel;
                employeeTaxProcessedInfo.EmployeeTaxProcess.ExcessTaxPaidRefundAmount = excessTaxPaidRefundAmount;
                employeeTaxProcessedInfo.EmployeeTaxProcess.YearlyTax = yearlyTax;
                employeeTaxProcessedInfo.EmployeeTaxProcess.PaidTotalTax = paidTotalTaxTillLastMonth;
                employeeTaxProcessedInfo.EmployeeTaxProcess.MonthlyTax = monthlyTax;
                employeeTaxProcessedInfo.EmployeeTaxProcess.CreatedBy = user.ActionUserId;
                employeeTaxProcessedInfo.EmployeeTaxProcess.CreatedDate = DateTime.Now;
                employeeTaxProcessedInfo.EmployeeTaxProcess.CompanyId = user.CompanyId;
                employeeTaxProcessedInfo.EmployeeTaxProcess.OrganizationId = user.OrganizationId;
                employeeTaxProcessedInfo.EmployeeTaxProcess.BranchId = employee.BranchId;
                employeeTaxProcessedInfo.EmployeeTaxProcess.RemainMonth = (short)employee.RemainFiscalYearMonth;
                employeeTaxProcessedInfo.EmployeeTaxProcess.PFContributionBothPart = pfContributionBothPart;
                employeeTaxProcessedInfo.EmployeeTaxProcess.OtherInvestment = otherInvestmentRecogPF;
                employeeTaxProcessedInfo.EmployeeTaxProcess.ActualInvestmentMade = actualInvestmentMade;
                employeeTaxProcessedInfo.EmployeeTaxProcess.ProjectionTax = projectionTax;
                employeeTaxProcessedInfo.EmployeeTaxProcess.OnceOffTax = onceOffTax;
                employeeTaxProcessedInfo.EmployeeTaxProcess.ProjectionAmount = totalTaxableIncomeInProjection;
                employeeTaxProcessedInfo.EmployeeTaxProcess.OnceOffAmount = actualOnceOffAmount;
                employeeTaxProcessedInfo.EmployeeTaxProcess.ArrearAmount = arrearAmount;
                employeeTaxProcessedInfo.EmployeeTaxProcess.TaxableIncome = totalTaxableIncome;
                employeeTaxProcessedInfo.EmployeeTaxProcess.ExemptionAmountOnAnnualIncome = minimumAnnualExemptionAmount;
                employeeTaxProcessedInfo.EmployeeTaxProcess.TotalTillMonthAllowanceAmount = totalTillAmount;
                employeeTaxProcessedInfo.EmployeeTaxProcess.TotalCurrentMonthAllowanceAmount = totalCurrentAmount;
                employeeTaxProcessedInfo.EmployeeTaxProcess.TotalProjectedAllowanceAmount = totalProjectedAmount;
                employeeTaxProcessedInfo.EmployeeTaxProcess.TotalGrossAnnualIncome = totalGrossAnnualIncome;
                employeeTaxProcessedInfo.EmployeeTaxProcess.TotalLessExemptedAmount = totalLessExemptedAmount;
                employeeTaxProcessedInfo.EmployeeTaxProcess.GrossTaxableIncome = grossTaxableIncome;
                employeeTaxProcessedInfo.EmployeeTaxProcess.PFExemption = totalPFExemptionAmount;
                employeeTaxProcessedInfo.EmployeeTaxProcess.TotalIncomeAfterPFExemption = totalIncomeAfterPFExemption;
                employeeTaxProcessedInfo.EmployeeTaxProcess.ActualTaxDeductionAmount = actualTaxDeductionAmount;
                employeeTaxProcessedInfo.EmployeeTaxProcess.SupplementaryOnceffTax = supplementaryOnceOffTaxAmount;
                employeeTaxProcessedInfo.EmployeeTaxProcess.TillProjectionTax = tillNowTaxDeducted.TillProjectionTax;
                employeeTaxProcessedInfo.EmployeeTaxProcess.TillOnceOffTax = tillNowTaxDeducted.TillOnceOffTax;
                employeeTaxProcessedInfo.EmployeeTaxProcess.ActualProjectionTax = actualProjectionTax;
                employeeTaxProcessedInfo.EmployeeTaxProcess.DefaultInvestment = defaultActualInvestmentMade;
                employeeTaxProcessedInfo.EmployeeTaxProcess.DefaultRebate = defaultTaxRebateDueToInvestment;
                employeeTaxProcessedInfo.EmployeeTaxProcess.TotalAdjustmentAmount =
                    taxDetails.Sum(i => i.TillAdjustment) + taxDetails.Sum(i => i.CurrentAdjustment);

                #endregion

                #region Employee Tax Detail
                foreach (var item in taxDetails)
                {
                    employeeTaxProcessedInfo.EmployeeTaxProcessDetails.Add(new EmployeeTaxProcessDetail()
                    {
                        AllowanceHeadName = null,
                        AllowanceConfigId = item.AllowanceConfigId,
                        AllowanceHeadId = 0,
                        AllowanceNameId = item.AllowanceNameId,
                        AllowanceName = item.AllowanceName,
                        EmployeeId = employee.EmployeeId,
                        BranchId = employee.BranchId,
                        BonusId = 0,
                        BonusName = null,
                        CompanyId = user.CompanyId,
                        OrganizationId = user.OrganizationId,
                        CreatedBy = user.ActionUserId,
                        CreatedDate = DateTime.Now,
                        TaxItem = item.Flag,
                        TillDateIncome = item.TillAmount,
                        CurrentMonthIncome = item.CurrentAmount,
                        ProjectedIncome = item.ProjectedAmount,
                        GrossAnnualIncome = item.GrossAnnualIncome,
                        LessExempted = item.LessExemptedAmount,
                        TotalTaxableIncome = item.TotalTaxableIncome,
                        SalaryProcessId = employee.SalaryProcessId ?? 0,
                        SalaryProcessDetailId = employee.SalaryProcessDetailId ?? 0,
                        SalaryMonth = (short)month,
                        SalaryYear = (short)year,
                        FiscalYearId = fiscalYear.FiscalYearId,
                        IsPerquisite = false,
                        Remarks = "",
                        ArrearAmount = item.Arrear,
                        IsProjected = item.ProjectRestYear ?? false,
                        TillAdjusmentAmount = item.TillAdjustment,
                        CurrentAdjusmentAmount = item.CurrentAdjustment
                    });
                }
                #endregion

            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "TaxRulesBusiness", "TaxRulesIY2324", user);
            }
            return employeeTaxProcessedInfo;
        }
        public async Task<TaxProcessedInfo> TaxRules(EligibleEmployeeForTaxType employee, FiscalYear fiscalYear, List<Shared.Payroll.Process.Tax.TaxDetailInTaxProcess> taxDetails, PayrollModuleConfig payrollModuleConfig, AllowanceInfo allowanceInfo, int year, int month, string flag, AppUser user)
        {
            TaxProcessedInfo taxProcessedInfo = new TaxProcessedInfo();
            try
            {
                int cc = await _employeeFreeCarBusiness.GetEmployeeFreeCarByEmployeeIdInTaxProcessAsync(employee, fiscalYear, year, month, user);
                var taxSetting = await _taxSettingBusiness.GetTaxSettingByFiscalYearIdAsync(fiscalYear.FiscalYearId, user);
                //decimal freeCarTillAmount = await _employeeFreeCarBusiness.CCTillAmountInTaxProcessAsync(employee, fiscalYear, year, month, user);

                //if (cc > 0 || freeCarTillAmount > 0)
                //{
                //    decimal ccAmount = 0;
                //    if (cc > 0 && cc <= (taxSetting.IncomeTaxSetting.FreeCarCCMinimumLimit ?? 0))
                //    {
                //        ccAmount = taxSetting.IncomeTaxSetting.FreeCarMinTaxableAmount ?? 0;
                //    }
                //    else
                //    {
                //        ccAmount = taxSetting.IncomeTaxSetting.FreeCarMaxTaxableAmount ?? 0;
                //    }

                //    taxDetails.Add(new Shared.Payroll.Process.Tax.TaxDetailInTaxProcess()
                //    {
                //        EmployeeId = employee.EmployeeId,
                //        AllowanceNameId = 0,
                //        AllowanceName = "Free Car",
                //        ProjectRestYear = true,
                //        OnceOffDeduction = false,
                //        Flag = "FREE CAR",
                //        TillAmount = freeCarTillAmount,
                //        CurrentAmount = ccAmount,
                //        Amount = 0,
                //        Arrear = 0,

                //        ReviewAmount = 0,
                //        RemainFiscalYearMonth = (short)employee.RemainMonth,
                //        ProjectedAmount = employee.IsDiscontinued == false ? ccAmount * employee.RemainMonth : 0
                //    });
                //}
                foreach (var item in taxDetails)
                {
                    item.Month = (short)month;
                    item.Year = (short)year;
                    item.EmployeeId = employee.EmployeeId;
                    item.GrossAnnualIncome = item.TillAmount + item.CurrentAmount + item.ProjectedAmount;
                    item.LessExemptedAmount = 0;
                    item.ExemptionAmount = 0;
                    item.ExemptionPercentage = 0;
                    item.RemainFiscalYearMonth = (short)employee.RemainMonth;
                    item.TotalTaxableIncome = item.TillAmount + item.CurrentAmount + item.ProjectedAmount;
                    
                }

                var dt = taxDetails.ToDataTable();
                // Tax Process - Income Details
                taxProcessedInfo.EmployeeTaxProcessDetails = taxDetails;

                decimal totalPFExemptionAmount = 0;
                if ((taxSetting.IncomeTaxSetting.PFBothPartExemption ?? false) == true)
                {
                    totalPFExemptionAmount = taxDetails.Where(i => i.Flag == "PF").Sum(i => i.TotalTaxableIncome) * 2;
                }

                decimal totalTillAmount = taxDetails.Sum(i => i.TillAmount);
                decimal totalCurrentAmount = taxDetails.Sum(i => i.CurrentAmount);
                decimal totalProjectedAmount = taxDetails.Sum(i => i.ProjectedAmount);
                decimal totalGrossAnnualIncome = taxDetails.Sum(i => i.GrossAnnualIncome);
                decimal totalLessExemptedAmount = taxDetails.Sum(i => i.LessExemptedAmount);
                decimal grossTaxableIncome = taxDetails.Sum(i => i.TotalTaxableIncome);

                decimal totalIncomeAfterPFExemption = 0;
                decimal actualAnnualExemptionAmount = grossTaxableIncome / 100 * taxSetting.IncomeTaxSetting.ExemptionPercentageOfAnnualIncome ?? 0;
                decimal minimumAnnualExemptionAmount = 0;

                if (actualAnnualExemptionAmount < taxSetting.IncomeTaxSetting.ExemptionAmountOfAnnualIncome)
                {
                    minimumAnnualExemptionAmount = actualAnnualExemptionAmount;
                }
                else
                {
                    minimumAnnualExemptionAmount = taxSetting.IncomeTaxSetting.ExemptionAmountOfAnnualIncome ?? 0;
                }

                decimal totalTaxableIncome = grossTaxableIncome - minimumAnnualExemptionAmount;
                totalTaxableIncome = totalTaxableIncome > 0 ? totalTaxableIncome : 0;
                totalTaxableIncome = totalPFExemptionAmount > 0 ? totalTaxableIncome - totalPFExemptionAmount : totalTaxableIncome;
                totalIncomeAfterPFExemption = totalTaxableIncome;

                decimal totalTaxableIncomeTrace = totalTaxableIncome;
                // Tax Process - Slab Details
                taxProcessedInfo.EmployeeTaxProcessSlabs = await TaxSlab(employee, fiscalYear, totalTaxableIncomeTrace, year, month, taxSetting, user);
                decimal totalTaxLiability = taxProcessedInfo.EmployeeTaxProcessSlabs.Sum(i => i.TaxLiability ?? 0);
                // Minimum Tax Amount
                decimal minimumTaxAmount = 0;
                if ((employee.MinimumTaxAmount ?? 0) <= 0)
                {
                    minimumTaxAmount = taxSetting.IncomeTaxSetting.MinTaxAmount ?? 0;
                }
                else
                {
                    minimumTaxAmount = employee.MinimumTaxAmount ?? 0;
                }

                // AIT
                var carAIT = await _taxAITBusiness.GetCarAITAmountInTaxProcessAsync(employee.EmployeeId, fiscalYear.FiscalYearId, user);
                // Tax Refund
                var taxRefundAmount = await _taxAITBusiness.GetTaxRefundAmountInTaxProcessAsync(employee.EmployeeId, fiscalYear.FiscalYearId, user);
                // Start of Rebate Calculation
                decimal pfAmountInTaxProcess = 0;
                decimal pfContributionBothPart = 0;
                decimal otherInvestmentRecogPF = 0;
                decimal actualInvestmentMade = 0;
                decimal taxRebateDueToInvestment = 0;
                decimal actualRebatePercentage = 0;
                decimal? employeeYearlyInvestmentAmount = null;
                decimal? employeeMayNeedToInvestment = null;

                if (taxSetting.TaxInvestmentSetting.IsFlatRebate)
                {
                    actualRebatePercentage = taxSetting.TaxInvestmentSetting.MinRebate;
                }
                pfAmountInTaxProcess = taxDetails.Where(i => i.Flag == "PF").Sum(i => i.TotalTaxableIncome);
                pfContributionBothPart = Math.Round(pfAmountInTaxProcess, MidpointRounding.AwayFromZero) * 2;

                decimal? employeeActualYearlyInvestmentAmount = await _employeeInvestmentSubmissionBusiness.GetYearlInvestmentAmountInTaxProcessAsync(employee.EmployeeId, fiscalYear.FiscalYearId, user);

                if (employeeActualYearlyInvestmentAmount == null && employee.IsResidential && totalTaxLiability > 0)
                {
                    // -- REBEAT|ACTUAL INVESTMENT|OTHER INVESTMENT
                    taxRebateDueToInvestment = totalTaxableIncome / 100 * actualRebatePercentage;
                    taxRebateDueToInvestment = taxRebateDueToInvestment > (taxSetting.TaxInvestmentSetting.RebateAmount ?? 0) ? taxSetting.TaxInvestmentSetting.RebateAmount ?? 0 : taxRebateDueToInvestment;
                    employeeMayNeedToInvestment = taxRebateDueToInvestment / (taxSetting.TaxInvestmentSetting.MaxInvestmentPercentage / 100);
                    actualInvestmentMade = Math.Round(employeeMayNeedToInvestment ?? 0, 0);
                    otherInvestmentRecogPF = Math.Round(actualInvestmentMade - pfContributionBothPart, 0);
                    taxRebateDueToInvestment = Math.Round(taxRebateDueToInvestment, 0);
                }
                else
                {

                    if (employee.IsResidential && totalTaxLiability > 0)
                    {
                        employeeActualYearlyInvestmentAmount = (employeeActualYearlyInvestmentAmount ?? 0) + pfContributionBothPart;

                        // Find Tax Rebate on Taxable Income
                        var taxRebateDueToTaxableIncome = totalTaxableIncome / 100 * actualRebatePercentage;
                        taxRebateDueToTaxableIncome = taxRebateDueToTaxableIncome > (taxSetting.TaxInvestmentSetting.RebateAmount ?? 0) ? taxSetting.TaxInvestmentSetting.RebateAmount ?? 0 : taxRebateDueToTaxableIncome;
                        taxRebateDueToTaxableIncome = Math.Round(taxRebateDueToTaxableIncome, MidpointRounding.AwayFromZero);

                        // Find Tax Rebate on actual investion amount
                        var rebateOnInvestment = (employeeActualYearlyInvestmentAmount ?? 0) * (taxSetting.TaxInvestmentSetting.MaxInvestmentPercentage / 100);
                        rebateOnInvestment = Math.Round(rebateOnInvestment, MidpointRounding.AwayFromZero);

                        // Find min rebate between Taxable income rebate & Actual investment rebate
                        var minRebate = Math.Min(taxRebateDueToTaxableIncome, rebateOnInvestment);

                        // Take geniune rebate amount MAX 10 LAC
                        taxRebateDueToInvestment = Math.Min(minRebate, taxSetting.TaxInvestmentSetting.RebateAmount ?? 0);

                        actualInvestmentMade = employeeActualYearlyInvestmentAmount ?? 0;
                        otherInvestmentRecogPF = actualInvestmentMade - pfContributionBothPart;
                        taxRebateDueToInvestment = Math.Round(taxRebateDueToInvestment, 0);

                    }
                }


                var tillNowTaxDeducted = await GetEmployeeTaxDeductedTillMonthAsync(employee.EmployeeId, fiscalYear.FiscalYearId, year, month, user);
                minimumTaxAmount = minimumTaxAmount == 0 ? totalTaxLiability : minimumTaxAmount;

                decimal netTaxPayable = 0;

                if (minimumTaxAmount == totalTaxLiability)
                {
                    netTaxPayable = totalTaxLiability - taxRebateDueToInvestment <= 0 ? 0 : totalTaxLiability - taxRebateDueToInvestment;
                    netTaxPayable = netTaxPayable > 0 ? netTaxPayable : 0;
                    netTaxPayable = Math.Round(netTaxPayable - (carAIT + taxRefundAmount));
                }
                else
                {
                    if (totalTaxLiability == 0)
                    {
                        netTaxPayable = 0;
                    }
                    else if (totalTaxLiability > 0 && totalTaxLiability < taxRebateDueToInvestment)
                    {
                        netTaxPayable = minimumTaxAmount;
                    }
                    else if (totalTaxLiability > 0 && totalTaxLiability - taxRebateDueToInvestment <= minimumTaxAmount)
                    {
                        netTaxPayable = minimumTaxAmount;
                    }
                    else if (totalTaxLiability > 0 && totalTaxLiability - taxRebateDueToInvestment > minimumTaxAmount)
                    {
                        netTaxPayable = totalTaxLiability - taxRebateDueToInvestment;
                    }

                    netTaxPayable = netTaxPayable > 0 ? netTaxPayable : 0;
                    netTaxPayable = Math.Round(netTaxPayable - (carAIT + taxRefundAmount), 0);
                    netTaxPayable = netTaxPayable > 0 && netTaxPayable < minimumTaxAmount ? minimumTaxAmount : netTaxPayable;
                    netTaxPayable = netTaxPayable < 0 ? 0 : netTaxPayable;
                }

                decimal paidTotalTaxTillLastMonth = tillNowTaxDeducted.TillTaxDeducted;
                decimal taxToBeAdjusted = netTaxPayable > 0 ? netTaxPayable - paidTotalTaxTillLastMonth : 0;

                decimal yearlyTaxableIncome = totalTaxableIncome;
                decimal totalTaxPayable = totalTaxLiability;
                decimal invesmentRebate = taxRebateDueToInvestment;
                decimal aitAmount = carAIT; //+ taxRefundAmount;
                decimal excessTaxPaidRefundAmount = taxRefundAmount;
                decimal yearlyTax = netTaxPayable;


                // Projection Calculation
                decimal arrearAmount = 0;
                decimal actualOnceOffAmount = 0;
                decimal totalTaxableIncomeInProjection = 0;
                decimal totalTaxableIncomeInProjectionTrace = 0;

                foreach (var item in taxDetails)
                {
                    decimal diffAmount = 0;
                    if ((item.ProjectRestYear ?? false) == true)
                    {
                        if (item.LessExemptedAmount > 0 && item.TillAmount + item.Amount + item.ProjectedAmount >= item.LessExemptedAmount && arrearAmount > 0)
                        {
                            arrearAmount = arrearAmount + item.Arrear;
                        }
                        else if (item.LessExemptedAmount > 0
                            && item.TillAmount + item.Amount + item.ProjectedAmount < item.LessExemptedAmount
                            && item.TillAmount + item.Amount + item.Arrear + item.ProjectedAmount > item.LessExemptedAmount
                            && item.Arrear > 0
                            )
                        {
                            diffAmount = item.LessExemptedAmount - (item.TillAmount + item.Amount + item.ProjectedAmount);
                            arrearAmount = arrearAmount + (diffAmount < item.Arrear ? item.Arrear - diffAmount : 0);
                        }
                        else if (item.LessExemptedAmount == 0)
                        {
                            arrearAmount = arrearAmount + item.Arrear;
                        }
                    }
                }

                actualOnceOffAmount = taxDetails.Where(i => (i.OnceOffDeduction ?? false) == true).Sum(i => i.CurrentAmount);
                decimal pfArrearAmount = taxDetails.Where(i => i.Flag == "PF").Sum(i => i.Arrear);

                if ((taxSetting.IncomeTaxSetting.CalculateTaxOnArrearAmount ?? false) == false)
                {
                    arrearAmount = 0;
                }

                totalTaxableIncomeInProjection = totalGrossAnnualIncome - (arrearAmount + actualOnceOffAmount);
                var exemptionAmountInProjection = totalTaxableIncomeInProjection * ((taxSetting.IncomeTaxSetting.ExemptionPercentageOfAnnualIncome ?? 0) / 100);
                exemptionAmountInProjection = exemptionAmountInProjection < (taxSetting.IncomeTaxSetting.ExemptionAmountOfAnnualIncome ?? 0) ? exemptionAmountInProjection : taxSetting.IncomeTaxSetting.ExemptionAmountOfAnnualIncome ?? 0;
                totalTaxableIncomeInProjection = totalTaxableIncomeInProjection - (exemptionAmountInProjection + ((taxSetting.IncomeTaxSetting.PFBothPartExemption ?? false) == true ? pfAmountInTaxProcess * 2 : 0));
                totalTaxableIncomeInProjectionTrace = totalTaxableIncomeInProjection;
                List<TaxProcessSlab> EmployeeTaxProcessSlabInProjection = new List<TaxProcessSlab>();

                EmployeeTaxProcessSlabInProjection = await TaxSlab(employee, fiscalYear, totalTaxableIncomeInProjectionTrace, year, month, taxSetting, user);

                decimal totalTaxLiabilityInProjection = EmployeeTaxProcessSlabInProjection.Sum(i => i.TaxLiability ?? 0);
                decimal totalTaxableIncomeInProjectionSlab = Math.Round(EmployeeTaxProcessSlabInProjection.Sum(i => i.TaxableIncome ?? 0), 0);

                decimal pfAmountInTaxProcessInProjection = pfAmountInTaxProcess - pfArrearAmount;
                decimal pfContributionBothPartInProjection = pfAmountInTaxProcessInProjection * 2;
                decimal supplementaryOnceOffTaxAmount = await GetThisMonthSupplementaryOnceOffTaxAsync(employee.EmployeeId, fiscalYear.FiscalYearId, year, month, user);

                decimal taxRebateDueToInvestmentInProjection = 0;
                decimal actualInvestmentMadeInProjection = 0;
                decimal otherInvestmentRecogPFInProjection = 0;

                if (employeeYearlyInvestmentAmount == null && totalTaxLiabilityInProjection > 0 && employee.IsResidential)
                {
                    // -- REBEAT|ACTUAL INVESTMENT|OTHER INVESTMENT
                    taxRebateDueToInvestmentInProjection = totalTaxableIncomeInProjection / 100 * actualRebatePercentage;
                    taxRebateDueToInvestmentInProjection = taxRebateDueToInvestmentInProjection > (taxSetting.TaxInvestmentSetting.RebateAmount ?? 0) ? taxSetting.TaxInvestmentSetting.RebateAmount ?? 0 : taxRebateDueToInvestmentInProjection;
                    employeeMayNeedToInvestment = taxRebateDueToInvestmentInProjection / (taxSetting.TaxInvestmentSetting.MaxInvestmentPercentage / 100);
                    actualInvestmentMadeInProjection = Math.Round(employeeMayNeedToInvestment ?? 0, 0);
                    otherInvestmentRecogPFInProjection = Math.Round(actualInvestmentMadeInProjection - pfContributionBothPart, 0);
                    taxRebateDueToInvestmentInProjection = Math.Round(taxRebateDueToInvestmentInProjection, 0);
                }
                else
                {
                    if (employee.IsResidential && totalTaxLiabilityInProjection > 0)
                    {
                        employeeActualYearlyInvestmentAmount = employeeActualYearlyInvestmentAmount + pfContributionBothPartInProjection;

                        // Find Tax Rebate on Taxable Income
                        var taxRebateDueToTaxableIncome = totalTaxableIncomeInProjection / 100 * actualRebatePercentage;
                        taxRebateDueToTaxableIncome = taxRebateDueToTaxableIncome > (taxSetting.TaxInvestmentSetting.RebateAmount ?? 0) ? taxSetting.TaxInvestmentSetting.RebateAmount ?? 0 : taxRebateDueToTaxableIncome;
                        taxRebateDueToTaxableIncome = Math.Round(taxRebateDueToTaxableIncome, MidpointRounding.AwayFromZero);

                        // Find Tax Rebate on actual investion amount
                        var rebateOnInvestment = (employeeActualYearlyInvestmentAmount ?? 0) * (taxSetting.TaxInvestmentSetting.MaxInvestmentPercentage / 100);
                        rebateOnInvestment = Math.Round(rebateOnInvestment, MidpointRounding.AwayFromZero);

                        // Find min rebate between Taxable income rebate & Actual investment rebate
                        var minRebate = Math.Min(taxRebateDueToTaxableIncome, rebateOnInvestment);

                        // Take geniune rebate amount MAX 10 LAC
                        taxRebateDueToInvestmentInProjection = Math.Min(minRebate, taxSetting.TaxInvestmentSetting.RebateAmount ?? 0);

                        actualInvestmentMadeInProjection = employeeActualYearlyInvestmentAmount ?? 0;
                        otherInvestmentRecogPFInProjection = actualInvestmentMadeInProjection - pfContributionBothPartInProjection;
                        taxRebateDueToInvestmentInProjection = Math.Round(taxRebateDueToInvestmentInProjection, 0);
                    }
                }

                minimumTaxAmount = minimumTaxAmount == 0 ? totalTaxLiabilityInProjection : minimumTaxAmount;

                decimal netTaxPayableInProjection = 0;

                if (minimumTaxAmount == totalTaxLiabilityInProjection)
                {
                    netTaxPayableInProjection = totalTaxLiabilityInProjection - taxRebateDueToInvestmentInProjection <= 0 ? 0 : totalTaxLiabilityInProjection - taxRebateDueToInvestmentInProjection;
                    netTaxPayableInProjection = netTaxPayableInProjection > 0 ? netTaxPayableInProjection : 0;
                    netTaxPayableInProjection = Math.Round(netTaxPayableInProjection - (carAIT + taxRefundAmount), 0);
                }
                else
                {
                    if (totalTaxLiabilityInProjection == 0)
                    {
                        netTaxPayableInProjection = 0;
                    }
                    else if (totalTaxLiabilityInProjection > 0 && totalTaxLiabilityInProjection < taxRebateDueToInvestmentInProjection)
                    {
                        netTaxPayableInProjection = minimumTaxAmount;
                    }
                    else if (totalTaxLiabilityInProjection > 0 && totalTaxLiabilityInProjection - taxRebateDueToInvestmentInProjection <= minimumTaxAmount)
                    {
                        netTaxPayableInProjection = minimumTaxAmount;
                    }
                    else if (totalTaxLiabilityInProjection > 0 && totalTaxLiabilityInProjection - taxRebateDueToInvestmentInProjection > minimumTaxAmount)
                    {
                        netTaxPayableInProjection = totalTaxLiabilityInProjection - taxRebateDueToInvestmentInProjection;
                    }

                    netTaxPayableInProjection = netTaxPayableInProjection > 0 ? netTaxPayableInProjection : 0;
                    netTaxPayableInProjection = Math.Round(netTaxPayableInProjection - (carAIT + taxRefundAmount), 0);
                    netTaxPayableInProjection = netTaxPayableInProjection > 0 && netTaxPayableInProjection < minimumTaxAmount ? minimumTaxAmount : netTaxPayableInProjection;
                    netTaxPayableInProjection = netTaxPayableInProjection < 0 ? 0 : netTaxPayableInProjection;
                }

                decimal taxToBeAdjustedInProjection = netTaxPayableInProjection > 0 ? netTaxPayableInProjection - tillNowTaxDeducted.TillTaxDeducted : 0;

                int remainMonth = employee.TerminationDate != null
                    && employee.TerminationDate.Value.IsDateBetweenTwoDates(fiscalYear.FiscalYearFrom.Value, fiscalYear.FiscalYearTo.Value)
                    && employee.TerminationDate.Value.Day == employee.TerminationDate.Value.DaysInAMonth() ? employee.RemainFiscalYearMonth + 1 : employee.RemainMonth + 1;

                decimal projectionTax = 0;
                decimal actualProjectionTax = 0;
                decimal actualOnceOffTax = 0;


                if (remainMonth > 0)
                {
                    projectionTax = netTaxPayableInProjection > 0 ? (netTaxPayableInProjection - (tillNowTaxDeducted.TillProjectionTax + tillNowTaxDeducted.TillOnceOffTax)) / remainMonth : 0;
                    actualProjectionTax = netTaxPayableInProjection > 0 ? (netTaxPayableInProjection - tillNowTaxDeducted.TillTaxDeducted) / remainMonth : 0;
                    projectionTax = actualProjectionTax;
                }
                else
                {
                    projectionTax = netTaxPayableInProjection - tillNowTaxDeducted.TillTaxDeducted;
                    actualProjectionTax = netTaxPayableInProjection - tillNowTaxDeducted.TillTaxDeducted;
                }


                projectionTax = projectionTax < 0 ? 0 : Math.Round(projectionTax, MidpointRounding.AwayFromZero);
                decimal onceOffTax = netTaxPayable - netTaxPayableInProjection;
                onceOffTax = onceOffTax == 1 || onceOffTax == -1 ? 0 : onceOffTax;
                decimal monthlyTax = projectionTax + onceOffTax;
                monthlyTax = monthlyTax > 0 ? monthlyTax : 0;
                actualOnceOffTax = onceOffTax;

                if (flag == "Supplementary")
                {
                    decimal OnceOffTaxPaid = await OnceOffTaxPaidInThisMonth(employee.EmployeeId, fiscalYear.FiscalYearId, year, month, user);
                    if (OnceOffTaxPaid > 0)
                    {
                        onceOffTax = onceOffTax - OnceOffTaxPaid;
                        onceOffTax = onceOffTax == 1 || onceOffTax == -1 ? 0 : onceOffTax;
                    }
                }

                decimal paidTotalTaxIncludingThisMonth = Math.Round(tillNowTaxDeducted.TillTaxDeducted + monthlyTax);

                decimal actualTaxDeductionAmount = 0;
                if (monthlyTax == 0)
                {
                    actualTaxDeductionAmount = monthlyTax;
                }
                if (monthlyTax > 0 && (taxSetting.IncomeTaxSetting.MonthlyTaxDeductionPercentage ?? 0) == 0)
                {
                    actualTaxDeductionAmount = 0;
                }
                if (monthlyTax > 0 && (taxSetting.IncomeTaxSetting.MonthlyTaxDeductionPercentage ?? 0) > 0)
                {
                    actualTaxDeductionAmount = Math.Round(monthlyTax / 100 * (taxSetting.IncomeTaxSetting.MonthlyTaxDeductionPercentage ?? 0));
                }

                // Table: Employee Tax Process
                string ytdLabel = "";
                var firstOfSalaryMonth = new DateTime(year, month, 1);
                if (month == 7)
                {
                    ytdLabel = "YTD Amount";
                }
                else
                {
                    string monthName = firstOfSalaryMonth.AddMonths(-1).GetMonthName();
                    ytdLabel = "YTD-" + monthName;
                }

                // Projection Label
                string projectionLabel = "";
                DateTime? fiscalYearEndDate = null;
                if (employee.TerminationDate == null)
                {
                    fiscalYearEndDate = fiscalYear.FiscalYearTo;
                }
                else
                {
                    if (employee.TerminationDate.Value.IsDateBetweenTwoDates(fiscalYear.FiscalYearFrom.Value, fiscalYear.FiscalYearTo.Value))
                    {
                        fiscalYearEndDate = employee.TerminationDate;
                    }
                    else
                    {
                        fiscalYearEndDate = fiscalYear.FiscalYearTo;
                    }
                }

                var nextMonth = firstOfSalaryMonth.AddMonths(1);
                if (remainMonth > 0 && nextMonth.Month != fiscalYearEndDate.Value.Month)
                {
                    projectionLabel = nextMonth.ToString("MMM") + "'" + nextMonth.ToString("yy") + "-";
                }
                else
                {
                    projectionLabel = "";
                }
                projectionLabel = projectionLabel + fiscalYearEndDate.Value.ToString("MMM") + "'" + fiscalYearEndDate.Value.ToString("yy");

                taxProcessedInfo.EmployeeTaxProcess.EmployeeId = employee.EmployeeId;
                taxProcessedInfo.EmployeeTaxProcess.SalaryProcessId = employee.SalaryProcessId;
                taxProcessedInfo.EmployeeTaxProcess.SalaryProcessDetailId = employee.SalaryProcessDetailId;
                taxProcessedInfo.EmployeeTaxProcess.FiscalYearId = fiscalYear.FiscalYearId;
                taxProcessedInfo.EmployeeTaxProcess.Month = (short)month;
                taxProcessedInfo.EmployeeTaxProcess.Year = (short)year;
                taxProcessedInfo.EmployeeTaxProcess.YearlyTaxableIncome = yearlyTaxableIncome;
                taxProcessedInfo.EmployeeTaxProcess.TotalTaxPayable = totalTaxPayable;
                taxProcessedInfo.EmployeeTaxProcess.InvestmentRebateAmount = invesmentRebate;
                taxProcessedInfo.EmployeeTaxProcess.AITAmount = aitAmount;
                taxProcessedInfo.EmployeeTaxProcess.TaxReturnAmount = 0;
                taxProcessedInfo.EmployeeTaxProcess.ProjectionLabel = projectionLabel;
                taxProcessedInfo.EmployeeTaxProcess.YTDLabel = ytdLabel;
                taxProcessedInfo.EmployeeTaxProcess.ExcessTaxPaidRefundAmount = excessTaxPaidRefundAmount;
                taxProcessedInfo.EmployeeTaxProcess.YearlyTax = yearlyTax;
                taxProcessedInfo.EmployeeTaxProcess.PaidTotalTax = paidTotalTaxTillLastMonth;
                taxProcessedInfo.EmployeeTaxProcess.MonthlyTax = monthlyTax;
                taxProcessedInfo.EmployeeTaxProcess.RemainMonth = (short)employee.RemainFiscalYearMonth;
                taxProcessedInfo.EmployeeTaxProcess.PFContributionBothPart = pfContributionBothPart;
                taxProcessedInfo.EmployeeTaxProcess.OtherInvestment = otherInvestmentRecogPF;
                taxProcessedInfo.EmployeeTaxProcess.ActualInvestmentMade = actualInvestmentMade;
                taxProcessedInfo.EmployeeTaxProcess.ProjectionTax = projectionTax;
                taxProcessedInfo.EmployeeTaxProcess.OnceOffTax = onceOffTax;
                taxProcessedInfo.EmployeeTaxProcess.ProjectionAmount = totalTaxableIncomeInProjection;
                taxProcessedInfo.EmployeeTaxProcess.OnceOffAmount = actualOnceOffAmount;
                taxProcessedInfo.EmployeeTaxProcess.ArrearAmount = arrearAmount;
                taxProcessedInfo.EmployeeTaxProcess.TaxableIncome = totalTaxableIncome;
                taxProcessedInfo.EmployeeTaxProcess.ExemptionAmountOnAnnualIncome = minimumAnnualExemptionAmount;
                taxProcessedInfo.EmployeeTaxProcess.TotalTillMonthAllowanceAmount = totalTillAmount;
                taxProcessedInfo.EmployeeTaxProcess.TotalCurrentMonthAllowanceAmount = totalCurrentAmount;
                taxProcessedInfo.EmployeeTaxProcess.TotalProjectedAllowanceAmount = totalProjectedAmount;
                taxProcessedInfo.EmployeeTaxProcess.TotalGrossAnnualIncome = totalGrossAnnualIncome;
                taxProcessedInfo.EmployeeTaxProcess.TotalLessExemptedAmount = totalLessExemptedAmount;
                taxProcessedInfo.EmployeeTaxProcess.GrossTaxableIncome = grossTaxableIncome;
                taxProcessedInfo.EmployeeTaxProcess.PFExemption = totalPFExemptionAmount;
                taxProcessedInfo.EmployeeTaxProcess.TotalIncomeAfterPFExemption = totalIncomeAfterPFExemption;
                taxProcessedInfo.EmployeeTaxProcess.ActualTaxDeductionAmount = actualTaxDeductionAmount;
                taxProcessedInfo.EmployeeTaxProcess.SupplementaryOnceffTax = supplementaryOnceOffTaxAmount;
                taxProcessedInfo.EmployeeTaxProcess.TillProjectionTax = tillNowTaxDeducted.TillProjectionTax;
                taxProcessedInfo.EmployeeTaxProcess.TillOnceOffTax = tillNowTaxDeducted.TillOnceOffTax;
                taxProcessedInfo.EmployeeTaxProcess.ActualProjectionTax = actualProjectionTax;
                taxProcessedInfo.EmployeeTaxProcess.ActualOnceOffTax = actualOnceOffTax;
                taxProcessedInfo.EmployeeTaxProcess.TotalAdjustmentAmount =
                    taxProcessedInfo.EmployeeTaxProcessDetails.Sum(i => i.TillAdjustment) +
                    taxProcessedInfo.EmployeeTaxProcessDetails.Sum(i => i.CurrentAdjustment);

            }
            catch (Exception ex)
            {


            }
            return taxProcessedInfo;
        }
        public async Task<List<TaxProcessSlab>> TaxSlab(EligibleEmployeeForTaxType employee, FiscalYear fiscalYear, decimal taxableIncome, int year, int month, TaxSettingInTaxProcess taxSetting, AppUser user)
        {
            decimal totalTaxableIncomeTrace = taxableIncome;
            List<TaxProcessSlab> taxProcessSlabs = new List<TaxProcessSlab>();
            var slabs = await _specialTaxSlabBusiness.GetByEmployeeId(employee.EmployeeId, fiscalYear.FiscalYearId, user);
            if (!slabs.Any())
            {
                if (!employee.IsResidential)
                {
                    if (employee.TotalServiceDays <= taxSetting.IncomeTaxSetting.NonResidentialPeriod)
                    {
                        slabs = await _incomeTaxSlabBusiness.GetIncomeTaxSlabsByImpliedConditionAsync("Non Residential", fiscalYear.FiscalYearId, user);
                    }
                    else
                    {
                        slabs = await _incomeTaxSlabBusiness.GetIncomeTaxSlabsByImpliedConditionAsync(employee.Gender, fiscalYear.FiscalYearId, user);
                    }
                }
                else
                {
                    slabs = await _incomeTaxSlabBusiness.GetIncomeTaxSlabsByImpliedConditionAsync(employee.Gender, fiscalYear.FiscalYearId, user);
                }

            }

            if (slabs.Any())
            {
                slabs = slabs.OrderBy(i => i.SlabPercentage);
                int slabCount = 1;
                foreach (var item in slabs)
                {

                    decimal currentRate = item.SlabPercentage;
                    decimal taxableDiffAmt = item.SlabMaximumAmount - item.SlabMininumAmount;
                    long incomeTaxSlabId = item.IncomeTaxSlabId;
                    string impliedCondition = item.ImpliedCondition;
                    decimal individualTaxLiablity = 0;

                    string parameter = "";
                    if (slabCount == 1)
                    {
                        parameter = "On the First BDT-" + taxableDiffAmt.ToString();
                    }
                    else if (slabCount != 1 && slabCount < slabs.Count())
                    {
                        parameter = "On the Next BDT-" + taxableDiffAmt.ToString();
                    }
                    else
                    {
                        parameter = "On remaining balance";
                    }

                    taxableIncome = 0;
                    if (totalTaxableIncomeTrace >= taxableDiffAmt)
                    {
                        taxableIncome = taxableDiffAmt;
                    }
                    else if (totalTaxableIncomeTrace > 0 && totalTaxableIncomeTrace < taxableDiffAmt)
                    {
                        taxableIncome = totalTaxableIncomeTrace;
                    }
                    else
                    {
                        taxableIncome = 0;
                    }
                    individualTaxLiablity = taxableIncome > 0 ? taxableIncome / 100 * item.SlabPercentage : 0;

                    taxProcessSlabs.Add(new TaxProcessSlab()
                    {
                        EmployeeId = employee.EmployeeId,
                        FiscalYearId = fiscalYear.FiscalYearId,
                        IncomeTaxSlabId = item.IncomeTaxSlabId,
                        ImpliedCondition = item.ImpliedCondition,
                        SlabPercentage = item.SlabPercentage,
                        ParameterName = parameter,
                        TaxableIncome = Math.Round(taxableIncome, MidpointRounding.AwayFromZero),
                        TaxLiability = Math.Round(individualTaxLiablity, MidpointRounding.AwayFromZero),
                        SalaryDate = DateTimeExtension.LastDateOfAMonth(year, month),
                        Month = (short)month,
                        Year = (short)year,
                    });

                    if (totalTaxableIncomeTrace > 0)
                    {
                        if (totalTaxableIncomeTrace >= taxableDiffAmt)
                        {
                            totalTaxableIncomeTrace = totalTaxableIncomeTrace - taxableDiffAmt;
                        }
                        else if (totalTaxableIncomeTrace < taxableDiffAmt)
                        {
                            totalTaxableIncomeTrace = totalTaxableIncomeTrace - taxableIncome;
                        }
                        else if (totalTaxableIncomeTrace > 0 && totalTaxableIncomeTrace < taxableDiffAmt)
                        {
                            totalTaxableIncomeTrace = taxableDiffAmt;
                        }
                    }
                    slabCount = slabCount + 1;
                }
            }
            return taxProcessSlabs;
        }
        public async Task<decimal> OnceOffTaxPaidInThisMonth(long employeeId, long fiscalYearId, int year, int month, AppUser user)
        {
            decimal? onceOffTax = 0;
            try
            {
                var query = $@"SELECT OnceOffTax=SUM(OnceOffTax) From (SELECT OnceOffTax=SUM(ISNULL(TAX.OnceOffTax,0)) FROM Payroll_EmployeeTaxProcess TAX
                INNER JOIN Payroll_SalaryProcessDetail SPD ON TAX.SalaryProcessDetailId=SPD.SalaryProcessDetailId
                AND SPD.EmployeeId=TAX.EmployeeId
                INNER JOIN Payroll_SalaryProcess SP ON SPD.SalaryProcessId= SP.SalaryProcessId
                Where SP.IsDisbursed=1 AND SPD.EmployeeId=@EmployeeId AND SPD.SalaryMonth=@Month AND SPD.SalaryYear=@Year
                AND TAX.FiscalYearId =@FiscalYearId
                UNION ALL
                SELECT OnceOffTax=SUM(ISNULL(SPA.OnceOffAmount,0)) FROM Payroll_SupplementaryPaymentAmount SPA
                INNER JOIN Payroll_SupplementaryPaymentProcessInfo SPI ON 
                SPA.PaymentProcessInfoId = SPI.PaymentProcessInfoId
                Where SPA.EmployeeId=@EmployeeId AND SPI.IsDisbursed=1 AND SPI.PaymentMonth=@Month AND SPI.PayableYear=@Year
                AND SPI.FiscalYearId=@FiscalYearId) tbl";

                onceOffTax = await _dapper.SqlQueryFirstAsync<decimal?>(user.Database, query, new
                {
                    EmployeeId = employeeId,
                    FiscalYearId = fiscalYearId,
                    Month = month,
                    Year = year
                }, CommandType.Text);
                if (onceOffTax == null)
                {
                    onceOffTax = 0;
                }
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "TaxRulesBusiness", "OnceOffTaxPaidInThisMonth", user);
            }
            return onceOffTax ?? 0;
        }
        public async Task<decimal> GetBeforeThisMonthSupplementaryOnceOffTaxAsync(long employeeId, long fiscalYearId, int year, int month, AppUser user)
        {
            decimal amount = 0;
            try
            {
                var query = $@"SELECT SUM((SELECT ISNULL(SPA.OnceOffAmount,0) FROM Payroll_SupplementaryPaymentAmount SPA 
                Where dbo.fnGetFirstDateOfAMonth(SPA.PaymentYear,SPA.PaymentMonth) < dbo.fnGetFirstDateOfAMonth(@Year,@Month) 
                AND SPA.PaymentYear > 0 AND SPA.PaymentMonth > 0 AND SPA.FiscalYearId=@FiscalYearId))";

                amount = (await _dapper.SqlQueryFirstAsync<decimal>(user.Database, query, new
                {
                    Year = year,
                    Month = month,
                    FiscalYearId = fiscalYearId
                }, CommandType.Text));
            }
            catch (Exception ex)
            {
            }
            return amount;
        }
    }
}
