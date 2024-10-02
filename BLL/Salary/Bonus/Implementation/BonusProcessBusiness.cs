using System.Data;
using Shared.Services;
using BLL.Base.Interface;
using Shared.OtherModels.User;
using BLL.Salary.Bonus.Interface;
using BLL.Salary.Setup.Interface;
using Shared.OtherModels.Response;
using BLL.Salary.Salary.Interface;
using BLL.Administration.Interface;
using Shared.OtherModels.Pagination;
using BLL.Salary.Allowance.Interface;
using DAL.DapperObject.Interface;
using Shared.Payroll.DTO.Bonus;
using BLL.Tax.Interface;
using Shared.Payroll.ViewModel.Salary;
using Shared.Payroll.Domain.Bonus;
using Shared.Payroll.ViewModel.Bonus;
using Shared.Payroll.ViewModel.Setup;
using Shared.Payroll.Filter.Allowance;
using Shared.Payroll.ViewModel.Tax;
using Shared.Payroll.Filter.Tax;
using Shared.Payroll.Filter.Bonus;
using Shared.Payroll.Filter.Salary;

namespace BLL.Salary.Bonus.Implementation
{
    public class BonusProcessBusiness : IBonusProcessBusiness
    {
        private readonly IDapperData _dapper;
        private readonly ISysLogger _sysLogger;

        List<EmployeeTaxProcessDTO> listEmployeeTaxProcess;
        List<EmployeeBonusTaxDetailDTO> listEmployeeTaxProcessDetail;
        List<EmployeeBonusTaxSlabDTO> listEmployeeBonusTaxSlabs;

        private readonly IModuleConfigBusiness _moduleConfigBusiness;
        private readonly IBonusBusiness _bonusBusiness;
        private readonly IFiscalYearBusiness _fiscalYearBusiness;
        private readonly IAllowanceNameBusiness _allowanceNameBusiness;
        private readonly IAllowanceConfigBusiness _allowanceConfigBusiness;
        private readonly ISalaryProcessBusiness _salaryProcessBusiness;
        private readonly ITaxProcessBusiness _taxProcessBusiness;
        private readonly ITaxSettingBusiness _taxSettingBusiness;
        private readonly IIncomeTaxSlabBusiness _incomeTaxSlabBusiness;
        private readonly ITaxAITBusiness _taxAITBusiness;
        private readonly ISalaryReviewBusiness _salaryReviewBusiness;

        public BonusProcessBusiness(IDapperData dapper, ISysLogger sysLogger,
            IModuleConfigBusiness moduleConfigBusiness,
            IBonusBusiness bonusBusiness,
            IFiscalYearBusiness fiscalYearBusiness,
            IAllowanceNameBusiness allowanceNameBusiness,
            ISalaryProcessBusiness salaryProcessBusiness,
            ITaxProcessBusiness taxProcessBusiness,
            ITaxSettingBusiness taxSettingBusiness,
            IIncomeTaxSlabBusiness incomeTaxSlabBusiness,
            ITaxAITBusiness taxAITBusiness,
            ISalaryReviewBusiness salaryReviewBusiness,
            IAllowanceConfigBusiness allowanceConfigBusiness
            )
        {
            _dapper = dapper;
            _sysLogger = sysLogger;
            _moduleConfigBusiness = moduleConfigBusiness;
            _bonusBusiness = bonusBusiness;
            _fiscalYearBusiness = fiscalYearBusiness;
            _allowanceNameBusiness = allowanceNameBusiness;
            _salaryProcessBusiness = salaryProcessBusiness;
            _taxProcessBusiness = taxProcessBusiness;
            _taxSettingBusiness = taxSettingBusiness;
            _incomeTaxSlabBusiness = incomeTaxSlabBusiness;
            _taxAITBusiness = taxAITBusiness;
            _salaryReviewBusiness = salaryReviewBusiness;
            _allowanceConfigBusiness = allowanceConfigBusiness;
        }
        public async Task<BonusTaxAmount> BonusTaxProcessAsync(BonusTaxProcess taxProcess, EligibleEmployeesForBonus employee, FiscalYearViewModel fiscalYearInfo, SalaryReviewInfoViewModel currentSalaryReview, IEnumerable<SalaryReviewDetailViewModel> currentSalaryReviewDetails, AppUser user)
        {
            BonusTaxAmount taxAmount = new BonusTaxAmount();
            try
            {
                var daysInBonusMonth = DateTime.DaysInMonth(taxProcess.ProcessDate.Value.Year, taxProcess.ProcessDate.Value.Month);

                var remainFiscalYearMonth = Utility.GetMonthDiffExcludingThisMonth(taxProcess.ProcessDate.Value, fiscalYearInfo.FiscalYearTo.Value);

                var employeeTaxableFixedSalaryHead = await _allowanceNameBusiness.GetEmployeeTaxableAllowances(taxProcess.EmployeeId, taxProcess.SalaryReviewId, user);

                var firstDateOfThisMonth = new DateTime(taxProcess.ProcessDate.Value.Year, taxProcess.ProcessDate.Value.Month, 1);

                var lastDateOfThisMonth =
                    new DateTime(taxProcess.ProcessDate.Value.Year, taxProcess.ProcessDate.Value.Month, DateTime.DaysInMonth(taxProcess.ProcessDate.Value.Year, taxProcess.ProcessDate.Value.Month));

                var remainProjectionMonthForThisEmployee = employee.TerminationDate == null ? remainFiscalYearMonth : 0;
                remainProjectionMonthForThisEmployee = employee.TerminationDate != null && employee.TerminationStatus == "Approved" ?
                    Utility.GetMonthDiffExcludingThisMonth(employee.TerminationDate.Value, fiscalYearInfo.FiscalYearTo.Value) : remainProjectionMonthForThisEmployee;

                if (employee.TerminationDate == null)
                {
                    remainProjectionMonthForThisEmployee = remainFiscalYearMonth;
                }
                else if (employee.TerminationDate != null && employee.TerminationStatus == "Approved")
                {
                    remainProjectionMonthForThisEmployee = Utility.GetMonthDiffExcludingThisMonth(firstDateOfThisMonth, employee.TerminationDate.Value);
                }

                var employee_salary_details = await _salaryProcessBusiness.GetSalaryProcessDetailsAsync(0, 0, employee.EmployeeId, fiscalYearInfo.FiscalYearId, 0, 0, 0, null, user);

                var countOfSalaryReceiptInThisFiscalYear = employee_salary_details.Where(i => i.FiscalYearId == fiscalYearInfo.FiscalYearId).Count();

                var sumOfRemainProjectionAndCountOfSalaryReceipt = remainProjectionMonthForThisEmployee + countOfSalaryReceiptInThisFiscalYear + 1; // Here 1 is current 

                var thisMonthSalaryProcessInfo = employee_salary_details.FirstOrDefault(item => item.SalaryMonth == (short)taxProcess.ProcessDate.Value.Month && item.FiscalYearId == fiscalYearInfo.FiscalYearId);

                var salaryProcessId = thisMonthSalaryProcessInfo == null ? 0 : thisMonthSalaryProcessInfo.SalaryProcessId;
                var salaryProcessDetailId = thisMonthSalaryProcessInfo == null ? 0 : thisMonthSalaryProcessInfo.SalaryProcessDetailId;

                var bonus_month_salary_details = employee_salary_details.FirstOrDefault(item =>
                item.FiscalYearId == fiscalYearInfo.FiscalYearId && item.SalaryMonth == (short)taxProcess.ProcessDate.Value.Month && item.SalaryYear == (short)taxProcess.ProcessDate.Value.Year);

                var thisMonthBasic = bonus_month_salary_details == null ? 0 : bonus_month_salary_details.ThisMonthBasic;
                var currentGross = taxProcess.Gross;
                var currentBasic = taxProcess.Basic;

                var hr = currentSalaryReviewDetails.FirstOrDefault(item => item.AllowanceFlag == "HR");
                var currentHR = hr != null ? hr.CurrentAmount : 0;

                var medical = currentSalaryReviewDetails.FirstOrDefault(item => item.AllowanceFlag == "MEDICAL");
                var currentMedical = medical != null ? medical.CurrentAmount : 0;

                var conveyance = currentSalaryReviewDetails.FirstOrDefault(item => item.AllowanceFlag == "CONVEYANCE");
                var currentConveyance = conveyance != null ? conveyance.CurrentAmount : 0;

                var lfa = currentSalaryReviewDetails.FirstOrDefault(item => item.AllowanceFlag == "LFA");
                var currentLAF = lfa != null ? lfa.CurrentAmount : 0;

                // All allowance which are earned by this employee within this fiscal year
                var salary_allowances = await _salaryProcessBusiness.GetEmplyeeSalaryAllowancesAsync(employee.EmployeeId, 0, 0, 0, fiscalYearInfo.FiscalYearId, 0, 0, null, user);

                // distinct allowance which are earned by this employee
                var distinct_salary_allowances = salary_allowances.Select(allowance => new { allowance.AllowanceNameId, allowance.AllowanceName }).Distinct().ToList();

                var allowances_in_config = await _allowanceConfigBusiness.GetAllownaceConfigurationsAsync(new AllowanceConfig_Filter()
                {
                    ConfigId = "0",
                    StateStatus = "Approved"
                }, user);

                // All taxable allowances in Payroll System 
                var taxable_allowances = allowances_in_config.Where(item => item.IsTaxable == true).Select(item => new { item.AllowanceNameId, item.AllowanceName, IsOnceOffTax = item.IsOnceOffTax ?? false, IsProjection = item.ProjectRestYear ?? false, item.ConfigId, item.AllowanceFlag }).ToList();

                // 
                var tbl_tax_details = (await _taxProcessBusiness.GetTaxableAllowanceIncomeDetail(employee.EmployeeId, fiscalYearInfo.FiscalYearId, firstDateOfThisMonth.ToString("yyyy-MM-dd"), (short)taxProcess.ProcessDate.Value.Month, (short)taxProcess.ProcessDate.Value.Year, fiscalYearInfo.FiscalYearFrom.Value.ToString("yyyy-MM-dd"), fiscalYearInfo.FiscalYearTo.Value.ToString("yyyy-MM-dd"), (short)remainProjectionMonthForThisEmployee, user)).ToList();


                if (tbl_tax_details.Count > 0)
                {

                    foreach (var item in tbl_tax_details)
                    {
                        var allowance = currentSalaryReviewDetails.FirstOrDefault(i => i.AllowanceFlag == item.Flag);
                        bool isProjectedAmountZero = item.ProjectedAmount == 0 ? true : false;
                        item.CurrentAmount = item.CurrentAmount > 0 ? item.CurrentAmount : allowance != null ? allowance.CurrentAmount : 0;
                        item.ProjectedAmount = item.ProjectedAmount == 0 ? allowance != null ? allowance.CurrentAmount * remainProjectionMonthForThisEmployee : 0 : 0;
                        if (isProjectedAmountZero)
                        {
                            item.GrossAnnualIncome = item.TillAmount + item.CurrentAmount + item.ProjectedAmount;
                        }
                    }

                    // Find: Festival Bonus Id
                    var festival_bonus = (await _allowanceNameBusiness.GetAllowanceNamesAsync(new AllowanceName_Filter()
                    {
                        AllowanceFlag = "FB"
                    }, user)).FirstOrDefault();

                    var festival_bonus_id = festival_bonus != null ? festival_bonus.AllowanceNameId : 0;

                    // Till Festival Bonus Amount
                    //....

                    // This Month Festival Bonus Amount
                    //....

                    // Find Uploaded Festival Bonus Amount------
                    //....
                    var currentMonthUploadedFestivalBonusAmount = salary_allowances.Where(item => item.AllowanceNameId == festival_bonus_id
                    && item.SalaryMonth == (short)taxProcess.ProcessDate.Value.Month).Select(item =>
                    item.Amount + item.ArrearAmount + item.AdjustmentAmount).FirstOrDefault();

                    var tillMonthUploadedFestivalBonusAmount = salary_allowances.Where(item => item.AllowanceNameId == festival_bonus_id
                    && item.SalaryMonth != (short)taxProcess.ProcessDate.Value.Month
                    && item.SalaryYear != (short)taxProcess.ProcessDate.Value.Year
                    && item.SalaryDate >= fiscalYearInfo.FiscalYearFrom
                    && item.SalaryDate <= fiscalYearInfo.FiscalYearTo).Select(item => new { totalAmount = item.Amount + item.ArrearAmount + item.AdjustmentAmount }).Sum(item => item.totalAmount);

                    //Find: Festival Projected Amount
                    //....
                    decimal festivalBonusProjectedAmount = 0;
                    var employeeRemainFestivalBonusForTaxProcess = await _taxProcessBusiness.GetEmployeeRamainFestivalBonusAsync(fiscalYearInfo.FiscalYearId, employee.ReligionName, employee.DateOfConfirmation.HasValue ? employee.DateOfConfirmation.Value.Date.ToString("yyyy-MM-dd") : null, taxProcess.ProcessDate.Value.Date.ToString("yyyy-MM-dd"), user);

                    foreach (var item in employeeRemainFestivalBonusForTaxProcess)
                    {
                        decimal festival_amount =
                            item.BasedOn == "Basic" ? taxProcess.Basic * (item.BonusPercentage ?? 0 / 100) * item.BonusCount ?? 0 :
                            item.BasedOn == "Gross" ? taxProcess.Gross * (item.BonusPercentage ?? 0 / 100) * item.BonusCount ?? 0 :
                            item.BasedOn == "Flat" ? taxProcess.BonusAmount ?? 0 : 0
                            ;

                        festival_amount = item.MaximumAmount != null && item.MaximumAmount > 0 && festival_amount > item.MaximumAmount ? item.MaximumAmount ?? 0 :
                          festival_amount;
                        festivalBonusProjectedAmount = festivalBonusProjectedAmount + festival_amount;
                    }

                    if (currentMonthUploadedFestivalBonusAmount > 0 || tillMonthUploadedFestivalBonusAmount > 0 || festivalBonusProjectedAmount > 0)
                    {
                        TaxDetailInTaxProcess taxDetailInTaxProcess = new TaxDetailInTaxProcess();
                        taxDetailInTaxProcess.AllowanceNameId = festival_bonus_id;
                        taxDetailInTaxProcess.AllowanceName = "Festival Bonus";
                        taxDetailInTaxProcess.Flag = "FB";

                        taxDetailInTaxProcess.TillAmount = tillMonthUploadedFestivalBonusAmount;
                        taxDetailInTaxProcess.CurrentAmount = currentMonthUploadedFestivalBonusAmount + (taxProcess.BonusAmount ?? 0);
                        taxDetailInTaxProcess.Amount = currentMonthUploadedFestivalBonusAmount + (taxProcess.BonusAmount ?? 0);
                        taxDetailInTaxProcess.AdjustmentAmount = 0;
                        taxDetailInTaxProcess.ArrearAmount = 0;
                        taxDetailInTaxProcess.ProjectedAmount = festivalBonusProjectedAmount;
                        taxDetailInTaxProcess.GrossAnnualIncome = tillMonthUploadedFestivalBonusAmount + currentMonthUploadedFestivalBonusAmount + festivalBonusProjectedAmount;
                        taxDetailInTaxProcess.LessExemptedAmount = 0;
                        taxDetailInTaxProcess.TotalTaxableIncome = 0;
                        taxDetailInTaxProcess.ExemptionAmount = 0;
                        taxDetailInTaxProcess.ExemptionPercentage = 0;

                        tbl_tax_details.Add(taxDetailInTaxProcess);
                    }

                    if (taxProcess.IsFestivalBonus.HasValue && taxProcess.IsFestivalBonus.Value == true && (taxProcess.BonusAmount ?? 0) > 0 && (taxProcess.IsOnceOff ?? false) == true && (taxProcess.IsOnceOff ?? false) == true)
                    {
                        TaxDetailInTaxProcess taxDetailInTaxProcess = new TaxDetailInTaxProcess();
                        taxDetailInTaxProcess.AllowanceNameId = 0;
                        taxDetailInTaxProcess.AllowanceName = taxProcess.BonusName;
                        taxDetailInTaxProcess.Flag = taxProcess.BonusName;

                        taxDetailInTaxProcess.TillAmount = 0;
                        taxDetailInTaxProcess.CurrentAmount = taxProcess.BonusAmount ?? 0;
                        taxDetailInTaxProcess.Amount = taxProcess.BonusAmount ?? 0;
                        taxDetailInTaxProcess.AdjustmentAmount = 0;
                        taxDetailInTaxProcess.ArrearAmount = 0;
                        taxDetailInTaxProcess.ProjectedAmount = 0;
                        taxDetailInTaxProcess.GrossAnnualIncome = taxProcess.BonusAmount ?? 0;
                        taxDetailInTaxProcess.LessExemptedAmount = 0;
                        taxDetailInTaxProcess.TotalTaxableIncome = 0;
                        taxDetailInTaxProcess.ExemptionAmount = 0;
                        taxDetailInTaxProcess.ExemptionPercentage = 0;
                        tbl_tax_details.Add(taxDetailInTaxProcess);
                    }

                    //: Find PF Amount
                    var pfAllowance = (await _allowanceNameBusiness.GetAllowanceNamesAsync(new AllowanceName_Filter()
                    {
                        AllowanceFlag = "PF"
                    }, user)).FirstOrDefault();
                    long pfAllowanceId = pfAllowance != null ? pfAllowance.AllowanceNameId : 0;

                    var pfAmount = employee_salary_details.Where(item => item.SalaryMonth == (short)taxProcess.ProcessDate.Value.Month).Sum(item => item.PFAmount);
                    var pfArrear = employee_salary_details.Where(item => item.SalaryYear == (short)taxProcess.ProcessDate.Value.Year).Sum(item => item.PFArrear);

                    TaxDetailInTaxProcess taxDetailInTaxProcess_pf = new TaxDetailInTaxProcess();
                    taxDetailInTaxProcess_pf.AllowanceNameId = pfAllowanceId;
                    taxDetailInTaxProcess_pf.AllowanceName = pfAllowance.Name;
                    taxDetailInTaxProcess_pf.Flag = "PF";
                    taxDetailInTaxProcess_pf.TillAmount = employee_salary_details.Where(item =>
                    item.SalaryMonth != (short)taxProcess.ProcessDate.Value.Month).Sum(item => item.PFAmount + item.PFArrear);
                    taxDetailInTaxProcess_pf.CurrentAmount = pfAmount + pfArrear;
                    taxDetailInTaxProcess_pf.Amount = pfAmount;
                    taxDetailInTaxProcess_pf.ArrearAmount = pfArrear;
                    taxDetailInTaxProcess_pf.AdjustmentAmount = 0;
                    taxDetailInTaxProcess_pf.IsProjection = true;
                    taxDetailInTaxProcess_pf.GrossAnnualIncome =
                        taxDetailInTaxProcess_pf.TillAmount + taxDetailInTaxProcess_pf.CurrentAmount + taxDetailInTaxProcess_pf.ProjectedAmount;
                    taxDetailInTaxProcess_pf.ProjectedAmount = pfAmount * remainProjectionMonthForThisEmployee;
                    if (taxDetailInTaxProcess_pf.GrossAnnualIncome > 0)
                    {
                        tbl_tax_details.Add(taxDetailInTaxProcess_pf);
                    }


                    var annual_basic_info = tbl_tax_details.FirstOrDefault(item => item.Flag.ToLower() == "BASIC".ToLower());
                    var annual_basic_amount = annual_basic_info.GrossAnnualIncome;

                    var tax_setting_info = (await _taxSettingBusiness.GetTaxSettingsAsync(0, fiscalYearInfo.FiscalYearId, null, user)).FirstOrDefault();
                    var tax_setting_details = await _taxSettingBusiness.GetTaxSettingAsync(tax_setting_info.IncomeTaxSettingId, user);

                    decimal takeAmount = 0;
                    decimal exempAmount = 0;
                    var taxExemptionSettings = tax_setting_details.TaxExemptionSettings;
                    // Exemption Calculation
                    foreach (var item in tbl_tax_details)
                    {
                        // Find: Allowance item exemption.
                        var i = item;
                        var this_item_tax_setting = taxExemptionSettings.Where(i => i.Allowance == (item.Flag == null ? "" : item.Flag)).FirstOrDefault();

                        if (item.Flag != null && item.Flag.ToLower() != "BASIC".ToLower() && this_item_tax_setting != null)
                        {
                            exempAmount = (this_item_tax_setting.MaxExemptionAmount ?? 0) / 12 * sumOfRemainProjectionAndCountOfSalaryReceipt;
                            if (this_item_tax_setting.BasedOfAllowance != "" && this_item_tax_setting.BasedOfAllowance == "Annual Basic" && this_item_tax_setting.Allowance != "HR")
                            {
                                if (this_item_tax_setting.TakeLowerAmount ?? false == true)
                                {
                                    var exempAmountByExemPercentage = annual_basic_amount / 100 * this_item_tax_setting.MaxExemptionPercentage ?? 0;
                                    var exempAmountByExemAmount = this_item_tax_setting.MaxExemptionAmount / 12 * sumOfRemainProjectionAndCountOfSalaryReceipt ?? 0;

                                    takeAmount = exempAmountByExemPercentage > exempAmountByExemAmount ? exempAmountByExemAmount : exempAmountByExemPercentage;

                                    takeAmount = takeAmount > item.GrossAnnualIncome ? item.GrossAnnualIncome : takeAmount;

                                    takeAmount = this_item_tax_setting.TakeMinAmount ?? false == true ?
                                        item.GrossAnnualIncome >= exempAmountByExemAmount ? exempAmountByExemAmount : item.GrossAnnualIncome : takeAmount;
                                }
                                else if (this_item_tax_setting.UptoMaxAmount ?? false == true)
                                {
                                    var exempAmountByExemAmount = annual_basic_amount - (this_item_tax_setting.MaxExemptionAmount ?? 0);
                                    takeAmount = exempAmountByExemAmount;
                                }
                                else if (this_item_tax_setting.UptoMaxPercentage ?? false == true)
                                {
                                    var exempAmountByExemPercentage = annual_basic_amount - annual_basic_amount * this_item_tax_setting.MaxExemptionPercentage;
                                    takeAmount = exempAmountByExemPercentage ?? 0;
                                }

                                item.LessExemptedAmount = Math.Round(takeAmount, 0);
                                item.TotalTaxableIncome = Math.Round(item.GrossAnnualIncome - takeAmount);
                                item.ExemptionPercentage = this_item_tax_setting.MaxExemptionPercentage ?? 0;
                                item.ExemptionAmount = exempAmount;
                            }

                            else if (this_item_tax_setting.BasedOfAllowance != "" && this_item_tax_setting.BasedOfAllowance == "Annual Basic"
                                && this_item_tax_setting.Allowance == "HR")
                            {
                                if (this_item_tax_setting.TakeLowerAmount ?? false == true)
                                {
                                    var exempAmountByExemPercentage = annual_basic_amount / 100 * this_item_tax_setting.MaxExemptionPercentage ?? 0;
                                    var exempAmountByExemAmount = this_item_tax_setting.MaxExemptionAmount / 12 ?? 0;
                                    var eachMonthlyHRexmp = exempAmountByExemPercentage / sumOfRemainProjectionAndCountOfSalaryReceipt;

                                    takeAmount = eachMonthlyHRexmp < exempAmountByExemAmount ? eachMonthlyHRexmp : exempAmountByExemAmount;
                                    takeAmount = Math.Round(takeAmount, 4) > exempAmountByExemAmount ? exempAmountByExemAmount * sumOfRemainProjectionAndCountOfSalaryReceipt
                                        : Math.Round(takeAmount * sumOfRemainProjectionAndCountOfSalaryReceipt, 4);
                                    takeAmount = item.GrossAnnualIncome < takeAmount ? item.GrossAnnualIncome : takeAmount;
                                    exempAmount = exempAmountByExemAmount * sumOfRemainProjectionAndCountOfSalaryReceipt;
                                }
                                else if (this_item_tax_setting.UptoMaxAmount ?? false == true)
                                {
                                    var exempAmountByExemAmount = annual_basic_amount - this_item_tax_setting.MaxExemptionAmount / 12 * sumOfRemainProjectionAndCountOfSalaryReceipt;
                                    takeAmount = exempAmountByExemAmount ?? 0;
                                }
                                else if (this_item_tax_setting.UptoMaxPercentage ?? false == true)
                                {
                                    var exempAmountByExemPercentage = annual_basic_amount - annual_basic_amount / 100 * this_item_tax_setting.MaxExemptionPercentage;
                                    takeAmount = exempAmountByExemPercentage ?? 0;
                                }
                                item.LessExemptedAmount = Math.Round(takeAmount, 2);
                                item.TotalTaxableIncome = Math.Round(item.GrossAnnualIncome - takeAmount, 2);
                                item.ExemptionPercentage = this_item_tax_setting.MaxExemptionPercentage ?? 0;
                                item.ExemptionAmount = exempAmount;
                            }

                            else if (this_item_tax_setting.BasedOfAllowance != null && this_item_tax_setting.BasedOfAllowance == "Self")
                            {
                                if (this_item_tax_setting.TakeLowerAmount ?? false == true)
                                {
                                    var exempAmountByExemPercentage = item.GrossAnnualIncome / 100 * this_item_tax_setting.MaxExemptionPercentage ?? 0;
                                    var exempAmountByExemAmount = exempAmount;

                                    takeAmount = exempAmountByExemPercentage < exempAmountByExemAmount ? exempAmountByExemPercentage : exempAmountByExemAmount;
                                    takeAmount = takeAmount > item.GrossAnnualIncome ? item.GrossAnnualIncome : takeAmount;
                                }
                                else if (this_item_tax_setting.UptoMaxAmount ?? false == true)
                                {
                                    takeAmount = this_item_tax_setting.MaxExemptionAmount ?? 0; // / 12 * sumOfRemainProjectionAndCountOfSalaryReceipt
                                    takeAmount = takeAmount > item.GrossAnnualIncome ? item.GrossAnnualIncome : takeAmount;
                                }
                                else if (this_item_tax_setting.UptoMaxPercentage ?? false == true)
                                {
                                    takeAmount = item.GrossAnnualIncome / 100 * (this_item_tax_setting.MaxExemptionPercentage ?? 0);
                                    takeAmount = takeAmount > item.GrossAnnualIncome ? item.GrossAnnualIncome : takeAmount;
                                }

                                item.LessExemptedAmount = Math.Round(takeAmount, 2) > item.GrossAnnualIncome ? Math.Round(takeAmount, 2) : Math.Round(item.GrossAnnualIncome, 2);
                                item.TotalTaxableIncome = item.GrossAnnualIncome < takeAmount ? 0 : Math.Round(item.GrossAnnualIncome - takeAmount, 2);
                                item.ExemptionPercentage = this_item_tax_setting.MaxExemptionPercentage ?? 0;
                                item.ExemptionAmount = exempAmount;
                            }

                            else if (this_item_tax_setting.BasedOfAllowance == null || this_item_tax_setting.BasedOfAllowance == "")
                            {
                                if (this_item_tax_setting.TakeLowerAmount ?? false == true)
                                {
                                    var exempAmountByExemPercentage = item.GrossAnnualIncome / 100 * this_item_tax_setting.MaxExemptionPercentage ?? 0;
                                    var exempAmountByExemAmount = (this_item_tax_setting.MaxExemptionAmount ?? 0) / 12 * sumOfRemainProjectionAndCountOfSalaryReceipt;

                                    takeAmount = exempAmountByExemPercentage < exempAmountByExemAmount ? exempAmountByExemPercentage : exempAmountByExemAmount;
                                    takeAmount = takeAmount > item.GrossAnnualIncome ? item.GrossAnnualIncome : takeAmount;
                                }
                                else if (this_item_tax_setting.UptoMaxAmount ?? false == true)
                                {
                                    exempAmount = this_item_tax_setting.MaxExemptionAmount ?? 0; // / 12 * sumOfRemainProjectionAndCountOfSalaryReceipt);
                                    takeAmount = exempAmount > item.GrossAnnualIncome ? item.GrossAnnualIncome : exempAmount;
                                }
                                else if (this_item_tax_setting.UptoMaxPercentage ?? false == true)
                                {

                                }
                                item.LessExemptedAmount = Math.Round(takeAmount, 2) > item.GrossAnnualIncome ? Math.Round(takeAmount, 2) : Math.Round(item.GrossAnnualIncome, 2);
                                item.TotalTaxableIncome = item.GrossAnnualIncome < takeAmount ? 0 : Math.Round(item.GrossAnnualIncome - takeAmount, 2);
                                item.ExemptionPercentage = this_item_tax_setting.MaxExemptionPercentage ?? 0;
                                item.ExemptionAmount = exempAmount;
                            }
                            exempAmount = 0;
                        }
                        else
                        {
                            item.LessExemptedAmount = 0;
                            item.TotalTaxableIncome = item.GrossAnnualIncome;
                            item.ExemptionPercentage = this_item_tax_setting == null ? 0 : this_item_tax_setting.MaxExemptionPercentage ?? 0;
                            item.ExemptionAmount = exempAmount;
                        }

                        EmployeeBonusTaxDetailDTO employeeBonusTaxDetail = new EmployeeBonusTaxDetailDTO();
                        employeeBonusTaxDetail.EmployeeId = employee.EmployeeId;
                        employeeBonusTaxDetail.AllowanceNameId = item.AllowanceNameId;
                        employeeBonusTaxDetail.TaxItem = item.Flag;
                        employeeBonusTaxDetail.AllowanceName = item.AllowanceName;
                        employeeBonusTaxDetail.AllowanceConfigId = item.AllowanceConfigId;
                        employeeBonusTaxDetail.TillDateIncome = item.TillAmount;
                        employeeBonusTaxDetail.CurrentMonthIncome = item.CurrentAmount;
                        employeeBonusTaxDetail.GrossAnnualIncome = item.GrossAnnualIncome;
                        employeeBonusTaxDetail.LessExempted = item.ExemptionAmount;
                        employeeBonusTaxDetail.TotalTaxableIncome = item.TotalTaxableIncome;
                        employeeBonusTaxDetail.FiscalYearId = fiscalYearInfo.FiscalYearId;
                        listEmployeeTaxProcessDetail.Add(employeeBonusTaxDetail);
                    }

                    //----------- INCOME TAX SLAB -------------

                    var totalTaxableIncome = tbl_tax_details.Sum(item => item.TotalTaxableIncome);
                    var totalTillDateAmount = tbl_tax_details.Sum(item => item.TillAmount);
                    var totalCurrentAmount = tbl_tax_details.Sum(item => item.CurrentAmount);
                    var totalProjectedAmount = tbl_tax_details.Sum(item => item.ProjectedAmount);
                    var totalGrossAnnualIncome = tbl_tax_details.Sum(item => item.GrossAnnualIncome);
                    var totalLessExemptedAmount = tbl_tax_details.Sum(item => item.LessExemptedAmount);

                    //var tax_slab=

                    var income_tax_slab = await _incomeTaxSlabBusiness.GetIncomeTaxSlabsAsync(0, null, fiscalYearInfo.FiscalYearId, user);

                    var employee_income_slab = income_tax_slab.Where(item => item.ImpliedCondition == employee.Gender);

                    employee_income_slab = employee_income_slab.Count() == 0 ? employee_income_slab.Where(item => item.ImpliedCondition == "Regradless") : employee_income_slab;

                    decimal minTaxAmount = 5000;

                    int index = 1;
                    decimal totalTaxableIncomeTrace = totalTaxableIncome;

                    foreach (var item in employee_income_slab)
                    {
                        var parameter = index == 1 ? "On the First BDT-" + (item.SlabMaximumAmount - item.SlabMininumAmount).ToString() :
                               index == employee_income_slab.Count() ? "On remaining balance" :
                                 index != employee_income_slab.Count() ? "On the Next BDT-" + (item.SlabMaximumAmount - item.SlabMininumAmount).ToString() : ""

                            ;
                        var taxableIncome =
                            totalTaxableIncomeTrace >= item.SlabMaximumAmount - item.SlabMininumAmount ? item.SlabMaximumAmount - item.SlabMininumAmount :
                            totalTaxableIncomeTrace == 0 ? 0 :
                            totalTaxableIncomeTrace > 0 && totalTaxableIncomeTrace < item.SlabMaximumAmount - item.SlabMininumAmount ? totalTaxableIncomeTrace : 0;

                        var individualTaxLiablity = taxableIncome > 0 ? taxableIncome / 100 * item.SlabPercentage : 0;

                        EmployeeBonusTaxSlabDTO employeeTaxProcessSlab = new EmployeeBonusTaxSlabDTO();
                        employeeTaxProcessSlab.EmployeeId = employee.EmployeeId;
                        employeeTaxProcessSlab.FiscalYearId = fiscalYearInfo.FiscalYearId;
                        employeeTaxProcessSlab.IncomeTaxSlabId = item.IncomeTaxSlabId;
                        employeeTaxProcessSlab.ImpliedCondition = item.ImpliedCondition;
                        employeeTaxProcessSlab.SlabPercentage = item.SlabPercentage;
                        employeeTaxProcessSlab.ParameterName = parameter;
                        employeeTaxProcessSlab.TaxableIncome = taxableIncome;
                        employeeTaxProcessSlab.TaxLiablity = individualTaxLiablity;

                        if (totalTaxableIncomeTrace > 0)
                        {
                            totalTaxableIncomeTrace =
                                totalTaxableIncomeTrace >= item.SlabMaximumAmount - item.SlabMininumAmount
                                    ? totalTaxableIncomeTrace - (item.SlabMaximumAmount - item.SlabMininumAmount) :
                                     totalTaxableIncomeTrace < item.SlabMaximumAmount - item.SlabMininumAmount ? totalTaxableIncomeTrace - taxableIncome :
                                     totalTaxableIncomeTrace > 0 && totalTaxableIncomeTrace < item.SlabMaximumAmount - item.SlabMininumAmount ? totalTaxableIncomeTrace : 0
                                    ;
                        }
                        listEmployeeBonusTaxSlabs.Add(employeeTaxProcessSlab);
                        index++;
                    }

                    // AIT
                    var list_ait = (await _taxAITBusiness.GetEmployeeAITDocumentsAsync(new TaxDocumentQuery()
                    {
                        EmployeeId = employee.EmployeeId.ToString(),
                        FiscalYearId = fiscalYearInfo.FiscalYearId.ToString()
                    }, user)).ListOfObject;

                    var ait_amount = list_ait == null ? 0 : list_ait.Where(item => item.CertificateType == "AIT").Sum(item => item.Amount ?? 0);

                    var tax_Refund_Amount = list_ait == null ? 0 : list_ait.Where(item => item.CertificateType == "CTE").Sum(item => item.Amount ?? 0);

                    // Start of Rebate Calculation

                    // Tax Investment Setting Variable
                    var tax_setting = tax_setting_details.TaxInvestmentSettings.FirstOrDefault();

                    var pf_info = tbl_tax_details.FirstOrDefault(item => item.Flag == "PF");
                    decimal pfAmountInTaxProcess = pf_info != null ? pf_info.TotalTaxableIncome : 0;
                    decimal pfContributionBothPart = pfAmountInTaxProcess * 2;
                    decimal actualInvestmentMade = Math.Round(totalTaxableIncome / 100 * tax_setting.MaxInvestmentPercentage, 0);
                    decimal otherInvestmentRecogPF = pf_info != null ? actualInvestmentMade - pfContributionBothPart : 0;

                    var totalTaxLiability = listEmployeeBonusTaxSlabs.Sum(item => item.TaxLiablity);
                    decimal taxRebateDueToInvestment = 0;

                    decimal actualRebatePercentage = 0;
                    if (tax_setting.IsFlatRebate)
                    {
                        actualRebatePercentage = tax_setting.MinRebate;
                    }

                    taxRebateDueToInvestment = Math.Round(actualInvestmentMade > 0 ? actualInvestmentMade / 100 * actualRebatePercentage : 0, 0);

                    decimal netTaxPayable = totalTaxLiability == 0 ? 0 :
                        totalTaxLiability > 0 && totalTaxLiability < taxRebateDueToInvestment ? minTaxAmount :
                        totalTaxLiability > 0 && totalTaxLiability - taxRebateDueToInvestment <= minTaxAmount ? minTaxAmount :
                        totalTaxLiability > 0 && totalTaxLiability - taxRebateDueToInvestment > minTaxAmount ? totalTaxLiability - taxRebateDueToInvestment : 0;

                    netTaxPayable = netTaxPayable > 0 ? netTaxPayable : 0;
                    netTaxPayable = Math.Round(netTaxPayable - (ait_amount + tax_Refund_Amount), 0);

                    netTaxPayable = netTaxPayable > 0 && netTaxPayable < minTaxAmount ? minTaxAmount : netTaxPayable;

                    decimal taxToBeAdjusted = netTaxPayable - 0;

                    decimal paidTotalTaxIncludingThisMonth = 0;

                    decimal yearlyTaxableIncome = totalTaxableIncome;
                    decimal totalTaxPayable = totalTaxLiability;
                    decimal investmentRebate = taxRebateDueToInvestment;
                    decimal aitAmount = ait_amount;
                    decimal taxReturnAmount = 0;
                    decimal excessTaxPaidRefundAmount = taxReturnAmount;
                    decimal yearlyTax = netTaxPayable;

                    decimal paidTotalTaxTillLastMonth = 0;

                    paidTotalTaxIncludingThisMonth = paidTotalTaxTillLastMonth - 0;

                    decimal arrearAmount = 0;
                    decimal actualOnceOffAmount = taxProcess.IsOnceOff ?? false == true ? taxProcess.BonusAmount ?? 0 : 0;
                    decimal actualProjectedAmount = yearlyTaxableIncome - actualOnceOffAmount;
                    decimal actualProjectedAmountTrace = actualProjectedAmount;

                    List<EmployeeBonusTaxSlabViewModel> employeeBonusTaxSlabInProjection = new List<EmployeeBonusTaxSlabViewModel>();

                    index = 1;
                    foreach (var item in employee_income_slab)
                    {
                        decimal taxableDiffAmt = item.SlabMaximumAmount - item.SlabMininumAmount;
                        string impliedCondition = item.ImpliedCondition;

                        decimal individualTaxLiablity = 0;
                        string parameter = index == 1 ? "On the First BDT-" + taxableDiffAmt.ToString() :
                                index == employee_income_slab.Count() ? "On remaining balance" :
                                    index != employee_income_slab.Count() ? "On the Next BDT-" + taxableDiffAmt.ToString() : ""
                            ;

                        decimal taxableIncome = 0;

                        taxableIncome = actualProjectedAmountTrace >= taxableDiffAmt ? taxableDiffAmt :
                                actualProjectedAmountTrace == 0 ? 0 :
                                actualProjectedAmountTrace > 0 && actualProjectedAmountTrace < taxableDiffAmt ? actualProjectedAmountTrace : 0
                            ;

                        individualTaxLiablity = taxableIncome > 0 ? taxableIncome / 100 * item.SlabPercentage : 0;

                        //parameter = ()
                        EmployeeBonusTaxSlabViewModel EmployeeBonusTaxSlab = new EmployeeBonusTaxSlabViewModel();
                        EmployeeBonusTaxSlab.EmployeeId = employee.EmployeeId;
                        EmployeeBonusTaxSlab.FiscalYearId = fiscalYearInfo.FiscalYearId;
                        EmployeeBonusTaxSlab.IncomeTaxSlabId = item.IncomeTaxSlabId;
                        EmployeeBonusTaxSlab.ImpliedCondition = item.ImpliedCondition;
                        EmployeeBonusTaxSlab.SlabPercentage = item.SlabPercentage;
                        EmployeeBonusTaxSlab.ParameterName = parameter;
                        EmployeeBonusTaxSlab.TaxableIncome = taxableIncome;
                        EmployeeBonusTaxSlab.TaxLiablity = individualTaxLiablity;

                        if (actualProjectedAmountTrace > 0)
                        {
                            actualProjectedAmountTrace = actualProjectedAmountTrace >= taxableDiffAmt ? actualProjectedAmountTrace - taxableDiffAmt :
                                actualProjectedAmountTrace < taxableDiffAmt ? actualProjectedAmountTrace - taxableIncome :
                                actualProjectedAmountTrace > 0 && actualProjectedAmountTrace < taxableDiffAmt ? taxableDiffAmt : 0;
                        }
                        employeeBonusTaxSlabInProjection.Add(EmployeeBonusTaxSlab);
                        index++;
                    }

                    decimal totalTaxLiabilityInProjection = employeeBonusTaxSlabInProjection.Sum(item => item.TaxLiablity);
                    decimal totalTaxableIncomeInProjection = employeeBonusTaxSlabInProjection.Sum(item => item.TaxableIncome);

                    decimal projectionTax = 0;
                    decimal onceOffTax = 0;

                    decimal pfAmountInTaxProcessInProjection = 0;
                    decimal pfContributionBothPartInProjection = pfAmountInTaxProcessInProjection * 2;
                    decimal actualInvestmentMadeInProjection = Math.Round(totalTaxableIncomeInProjection / 100 * tax_setting.MaxInvestmentPercentage, 0);
                    decimal otherInvestmentRecogPFInProjection = Math.Round(actualInvestmentMadeInProjection - pfContributionBothPartInProjection, 0);
                    decimal taxRebateDueToInvestmentInProjection = actualInvestmentMadeInProjection > 0 ? actualInvestmentMadeInProjection / 100 * actualRebatePercentage : 0;

                    decimal netTaxPayableInProjection =
                            totalTaxLiabilityInProjection == 0 ? 0 :
                                totalTaxLiabilityInProjection > 0 && totalTaxLiabilityInProjection < taxRebateDueToInvestmentInProjection ? minTaxAmount :
                                totalTaxLiabilityInProjection > 0 && totalTaxLiabilityInProjection - taxRebateDueToInvestmentInProjection <= minTaxAmount ? minTaxAmount :
                                    totalTaxLiabilityInProjection > 0 && totalTaxLiabilityInProjection - taxRebateDueToInvestmentInProjection > minTaxAmount ?
                                    totalTaxLiabilityInProjection - taxRebateDueToInvestmentInProjection : 0


                        ;

                    netTaxPayableInProjection = netTaxPayableInProjection > 0 ? netTaxPayableInProjection : 0;
                    netTaxPayableInProjection = Math.Round(netTaxPayableInProjection - (ait_amount + tax_Refund_Amount), 0);
                    netTaxPayableInProjection = Math.Round(netTaxPayableInProjection > 0 && netTaxPayableInProjection < minTaxAmount ? minTaxAmount : netTaxPayableInProjection, 0);

                    decimal taxToBeAdjustedInProjection = netTaxPayableInProjection - 0;
                    projectionTax = Math.Round((netTaxPayableInProjection - paidTotalTaxTillLastMonth) / remainFiscalYearMonth + 1, 0);
                    onceOffTax = netTaxPayable - netTaxPayableInProjection;

                    decimal monthlytax = Math.Round(projectionTax + onceOffTax, 0);

                    paidTotalTaxIncludingThisMonth = Math.Round(paidTotalTaxTillLastMonth + monthlytax, 0);

                    taxAmount.TaxAmount = projectionTax + onceOffTax;
                    taxAmount.ProjectionTax = projectionTax;
                    taxAmount.OnceOffTax = onceOffTax;
                    taxAmount.RemainMonth = (short)remainFiscalYearMonth;

                    EmployeeTaxProcessDTO employeeTaxProcess = new EmployeeTaxProcessDTO();
                    employeeTaxProcess.FiscalYearId = fiscalYearInfo.FiscalYearId;
                    employeeTaxProcess.BonusId = taxProcess.BonusId;
                    employeeTaxProcess.BonusConfigId = taxProcess.BonusConfigId;
                    employeeTaxProcess.BonusMonth = (short)taxProcess.ProcessDate.Value.Month;
                    employeeTaxProcess.BonusYear = (short)taxProcess.ProcessDate.Value.Year;
                    employeeTaxProcess.AITAmount = aitAmount;
                    employeeTaxProcess.ArrearAmount = arrearAmount;
                    employeeTaxProcess.InvestmentRebateAmount = investmentRebate;
                    employeeTaxProcess.RemainMonth = (short)remainFiscalYearMonth;
                    employeeTaxProcess.EmployeeId = employee.EmployeeId;
                    employeeTaxProcess.ProjectionTax = projectionTax;
                    employeeTaxProcess.ProjectionAmount = actualProjectedAmount;
                    employeeTaxProcess.OnceOffAmount = actualOnceOffAmount;
                    employeeTaxProcess.OnceOffTax = onceOffTax;
                    employeeTaxProcess.MonthlyTax = monthlytax;
                    employeeTaxProcess.PaidTotalTax = paidTotalTaxIncludingThisMonth;
                    employeeTaxProcess.YearlyTax = yearlyTax;
                    employeeTaxProcess.YearlyTaxableIncome = yearlyTaxableIncome;
                    employeeTaxProcess.PFContributionBothPart = pfContributionBothPart;
                    employeeTaxProcess.OtherInvestment = otherInvestmentRecogPF;
                    employeeTaxProcess.TaxReturnAmount = taxReturnAmount;
                    employeeTaxProcess.ActualInvestmentMade = actualInvestmentMade;
                    listEmployeeTaxProcess.Add(employeeTaxProcess);
                }
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "BonusProcessBusiness", "BonusTaxProcessAsync", user);
            }
            return taxAmount;
        }
        public async Task<ExecutionStatus> ExecuteBonusProcessAsync(ExecuteBonusProcess bonusProcess, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            BonusProcessDTO process = new BonusProcessDTO();
            List<BonusProcessDetailViewModel> BonusProcessDetails = new List<BonusProcessDetailViewModel>();
            try
            {
                // Globla Configuratin For Bonus;
                var payrollModuleConfig = await _moduleConfigBusiness.GetPayrollModuleConfigsAsync(user.CompanyId, user.OrganizationId);

                // Get Eligible Employee For Bonus
                var eligibleEmployees = await GetEligibleEmployeesAsync(bonusProcess, user);

                // Bonus Info ...
                var bonus = (await _bonusBusiness.GetBonusesAsync(string.Empty, bonusProcess.BonusId, user)).FirstOrDefault();

                // Bonus Detail...
                var bonusConfig = (await _bonusBusiness.GetBonusConfigsAsync(new BonusQuery()
                {
                    BonusId = bonusProcess.BonusId.ToString(),
                    BonusConfigId = bonusProcess.BonusConfigId.ToString()
                }, user)).FirstOrDefault();

                // Fiscal Year
                if (bonusConfig != null)
                {
                    var fiscalYearInfo = await _fiscalYearBusiness.GetFiscalYearAsync(bonusConfig.FiscalYearId, user);

                    // all employees current salary reviews;
                    var salary_reviews = await _salaryReviewBusiness.GetEmployeeLastSalaryReviewAccordingToCutOffDate(0,
                        bonusProcess.ProcessDate.Value.ToString("yyyy-MM-dd"), user);

                    foreach (var employee in eligibleEmployees)
                    {
                        BonusProcessDetailViewModel bonusProcessDetail = new BonusProcessDetailViewModel();
                        if (!employee.IsDiscontinued)
                        {

                            decimal bonus_amount = 0;
                            var salary_review_info = salary_reviews.FirstOrDefault(item => item.EmployeeId == employee.EmployeeId);

                            if (salary_review_info != null)
                            {
                                var salary_review_details = await _salaryReviewBusiness.GetSalaryReviewDetailsAsync(new SalaryReview_Filter() { SalaryReviewInfoId = salary_review_info.SalaryReviewInfoId.ToString(), EmployeeId = salary_review_info.EmployeeId.ToString() }, user);

                                decimal basicAmount = 0;

                                if (salary_review_details.FirstOrDefault(item => item.AllowanceFlag == "BASIC") != null)
                                {
                                    basicAmount = salary_review_details.FirstOrDefault(item => item.AllowanceFlag == "BASIC").CurrentAmount;
                                }
                                var gross_salary = salary_review_info.BaseType == "Gross Base" ? salary_review_info.CurrentSalaryAmount : salary_review_details.Sum(item => item.AllowanceAmount);
                                var basic_salary = salary_review_info.BaseType == "Basic Base" ? salary_review_info.CurrentSalaryAmount : basicAmount;

                                if (bonusConfig.BasedOn == "Basic")
                                {
                                    bonus_amount = Math.Round(basic_salary * (bonusConfig.Percentage.Value / 100));
                                }
                                else if (bonusConfig.BasedOn == "Gross")
                                {
                                    bonus_amount = Math.Round(gross_salary.Value * (bonusConfig.Percentage.Value / 100));
                                }
                                else if (bonusConfig.BasedOn == "Flat")
                                {
                                    bonus_amount = Math.Round(bonusConfig.Amount.Value);
                                }

                                bonusProcessDetail.OnceOffTax = 0;

                                BonusTaxAmount taxAmount = new BonusTaxAmount();
                                if (bonusConfig.IsTaxable.HasValue && bonusConfig.IsTaxable.HasValue == true && bonusConfig.IsTaxAdjustedWithSalary == false)
                                {
                                    listEmployeeTaxProcess = new List<EmployeeTaxProcessDTO>();
                                    listEmployeeBonusTaxSlabs = new List<EmployeeBonusTaxSlabDTO>();
                                    listEmployeeTaxProcessDetail = new List<EmployeeBonusTaxDetailDTO>();

                                    BonusTaxProcess taxProcess = new BonusTaxProcess();
                                    taxProcess.BonusId = bonusConfig.BonusId;
                                    taxProcess.BonusConfigId = bonusConfig.BonusConfigId;
                                    taxProcess.IsTaxable = bonusConfig.IsTaxable.Value;
                                    taxProcess.IsTaxDistributed = bonusConfig.IsTaxDistributed.Value;
                                    taxProcess.IsPaymentProjected = bonusConfig.IsPaymentProjected.Value;
                                    taxProcess.IsOnceOff = bonusConfig.IsOnceOff.Value;
                                    taxProcess.IsFestivalBonus = bonusConfig.IsFestival.Value;
                                    taxProcess.EmployeeId = employee.EmployeeId;
                                    taxProcess.SalaryReviewId = salary_review_info.SalaryReviewInfoId;
                                    taxProcess.ProcessDate = bonusProcess.ProcessDate;
                                    taxProcess.Gross = gross_salary;
                                    taxProcess.Basic = basic_salary;
                                    taxProcess.BonusAmount = bonus_amount;
                                    taxProcess.BonusName = bonus.BonusName;
                                    taxAmount = await BonusTaxProcessAsync(taxProcess, employee, fiscalYearInfo, salary_review_info, salary_review_details, user);
                                }

                                bonusProcessDetail.EmployeeId = employee.EmployeeId;
                                bonusProcessDetail.BonusMonth = bonusProcess.BonusMonth;
                                bonusProcessDetail.BonusYear = bonusProcess.BonusYear;
                                bonusProcessDetail.Amount = bonus_amount;
                                bonusProcessDetail.OnceOffTax = taxAmount.OnceOffTax;
                                bonusProcessDetail.ProcessDate = bonusProcess.ProcessDate;
                                bonusProcessDetail.CompanyId = user.CompanyId;
                                bonusProcessDetail.OrganizationId = user.OrganizationId;
                                BonusProcessDetails.Add(bonusProcessDetail);
                            }
                        }
                    }

                    if (eligibleEmployees.Count() > 0)
                    {
                        process.BonusId = bonusProcess.BonusId;
                        process.BonusConfigId = bonusProcess.BonusConfigId;
                        process.BonusMonth = bonusProcess.BonusMonth;
                        process.BonusYear = bonusProcess.BonusYear;
                        process.FiscalYearId = fiscalYearInfo.FiscalYearId;
                        process.ProcessDate = bonusProcess.ProcessDate ?? DateTime.Now;
                        process.PaymentDate = bonusProcess.ProcessDate ?? DateTime.Now;
                        process.DepartmentId = bonusProcess.ProcessByDepartmentId ?? 0;
                        process.BranchId = bonusProcess.ProcessByBranchId ?? 0;
                        process.TotalEmployees = eligibleEmployees.Count();
                        process.TotalAmount = BonusProcessDetails.Sum(item => item.Amount);
                        var totalTax = BonusProcessDetails.Sum(item => item.OnceOffTax);
                        process.TotalTax = totalTax ?? 0;



                        var BonusProcessDetailJson = Utility.JsonData(BonusProcessDetails);

                        var sp_name = "sp_Payroll_BonusProcess";
                        var parameters = Utility.DappperParams(process, user, addUserId: true);
                        parameters.Add("BonusProcessDetailJson", BonusProcessDetailJson);

                        if (listEmployeeTaxProcess != null && listEmployeeTaxProcess.Count > 0)
                        {
                            var taxProcessJSON = Utility.JsonData(listEmployeeTaxProcess);
                            parameters.Add("TaxProcessJSON", taxProcessJSON);
                        }
                        if (listEmployeeTaxProcessDetail != null && listEmployeeTaxProcessDetail.Count > 0)
                        {
                            var taxProcessDetailJSON = Utility.JsonData(listEmployeeTaxProcessDetail);
                            parameters.Add("TaxProcessDetailJSON", taxProcessDetailJSON);
                        }
                        if (listEmployeeBonusTaxSlabs != null && listEmployeeBonusTaxSlabs.Count > 0)
                        {
                            var taxSlabsJSON = Utility.JsonData(listEmployeeBonusTaxSlabs);
                            parameters.Add("TaxSlabsJSON", taxSlabsJSON);
                        }
                        parameters.Add("ExecutionFlag", Data.Insert);

                        executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
                    }
                    else
                    {
                        executionStatus = new ExecutionStatus()
                        {
                            Status = false,
                            Msg = "No Eligible Employee Found"
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "BonusProcessBusiness", "ExecuteBonusProcessAsync", user);
            }
            return executionStatus;
        }
        public Task<IEnumerable<EligibleEmployeesForBonus>> GetEligibleEmployeesAsync(ExecuteBonusProcess bonusProcess, AppUser user)
        {
            throw new NotImplementedException();
        }

        public async Task<DBResponse<BonusProcessViewModel>> GetBonusProcessesInfoAsync(BonusProcessInfo_Filter filter, AppUser user)
        {
            DBResponse<BonusProcessViewModel> data = new DBResponse<BonusProcessViewModel>();
            DBResponse response = new DBResponse();
            try
            {
                var sp_name = "sp_Payroll_BonusProcess_List";
                var parameters = Utility.DappperParams(filter, user, addBaseProperty: true, addUserId: true, excludeProps: new string[] { "TotalRows", "TotalPages" });
                parameters.Add("ExecutionFlag", "BonusInfo");
                response = await _dapper.SqlQueryFirstAsync<DBResponse>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
                data.ListOfObject = Utility.JsonToObject<IEnumerable<BonusProcessViewModel>>(response.JSONData) ?? new List<BonusProcessViewModel>();
                data.Pageparam = new Pageparam() { PageNumber = response.PageNumber, PageSize = response.PageSize, TotalPages = response.TotalPages, TotalRows = response.TotalRows };
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "BonusBusiness", "GetBonusProcessesInfoAsync", user);
            }
            return data;
        }

        public async Task<DBResponse<BonusProcessDetailViewModel>> GetBonusProcessDetailAsync(BonusProcessDetail_Filter filter, AppUser user)
        {
            DBResponse<BonusProcessDetailViewModel> data = new DBResponse<BonusProcessDetailViewModel>();
            DBResponse response = new DBResponse();
            try
            {
                var sp_name = "sp_Payroll_BonusProcess_List";
                var parameters = Utility.DappperParams(filter, user, addBaseProperty: true, addUserId: true, excludeProps: new string[] { "TotalRows", "TotalPages" });
                parameters.Add("ExecutionFlag", "BonusDetail");
                response = await _dapper.SqlQueryFirstAsync<DBResponse>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
                data.ListOfObject = Utility.JsonToObject<IEnumerable<BonusProcessDetailViewModel>>(response.JSONData) ?? new List<BonusProcessDetailViewModel>();
                data.Pageparam = new Pageparam() { PageNumber = response.PageNumber, PageSize = response.PageSize, TotalPages = response.TotalPages, TotalRows = response.TotalRows };
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "BonusBusiness", "GetBonusProcessDetailAsync", user);
            }
            return data;
        }

        public async Task<ExecutionStatus> DisbursedBonusAsync(DisbursedUndoBonusDTO model, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                var sp_name = "sp_Payroll_BonusProcess";
                var parameters = Utility.DappperParams(model, user);
                parameters.Add("ExecutionFlag", Data.Disbursed);
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                executionStatus = ResponseMessage.Invalid();
                await _sysLogger.SaveHRMSException(ex, user.Database, "BonusBusiness", "DisbursedBonusAsync", user);
            }
            return executionStatus;
        }

        public async Task<ExecutionStatus> UndoBonusAsync(DisbursedUndoBonusDTO model, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                var sp_name = "sp_Payroll_BonusProcess";
                var parameters = Utility.DappperParams(model, user);
                parameters.Add("ExecutionFlag", Data.Delete);
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                executionStatus = ResponseMessage.Invalid();
                await _sysLogger.SaveHRMSException(ex, user.Database, "BonusBusiness", "UndoBonusAsync", user);
            }
            return executionStatus;
        }

        public async Task<ExecutionStatus> UndoEmployeeBonusAsync(UndoEmployeeBonus model, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                var sp_name = "sp_Payroll_BonusProcess";
                var parameters = Utility.DappperParams(model, user);
                parameters.Add("ExecutionFlag", "Employee_Undo_Bonus");
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                executionStatus = ResponseMessage.Invalid();
                await _sysLogger.SaveHRMSException(ex, user.Database, "BonusBusiness", "UndoBonusAsync", user);
            }
            return executionStatus;
        }

        public async Task<ExecutionStatus> SaveExcludeEmployeeFromBonusAsync(EmployeeExcludedFromBonusDTO model, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                var sp_name = "sp_Payroll_ExcludedEmployeeFromBonus_Insert_Update_Delete";
                var param = Utility.DappperParams(model, user);
                param.Add("ExecutionFlag", model.ExcludeId > 0 ? Data.Update : Data.Insert);
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, param, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                executionStatus = ResponseMessage.Invalid();
                await _sysLogger.SaveHRMSException(ex, user.Database, "BonusBusiness", "SaveExcludeEmployeeFromBonusAsync", user);
            }
            return executionStatus;
        }

        public async Task<IEnumerable<EmployeeExcludedFromBonusViewModel>> GetExcludedEmployeesFromBonusAsync(ExcludeEmployeedFromBonus_Filter filter, AppUser user)
        {
            IEnumerable<EmployeeExcludedFromBonusViewModel> data = new List<EmployeeExcludedFromBonusViewModel>();
            try
            {
                var sp_name = "sp_Payroll_ExcludedEmployeeFromBonus_List";
                var param = Utility.DappperParams(filter, user, addUserId: false);
                data = await _dapper.SqlQueryListAsync<EmployeeExcludedFromBonusViewModel>(user.Database, sp_name, param, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "BonusBusiness", "GetExcludedEmployeesFromBonusAsync", user);
            }
            return data;
        }

        public async Task<ExecutionStatus> DeleteEmployeeFromExcludeListAsync(EmployeeExcludedFromBonusDTO model, AppUser user)
        {
            ExecutionStatus executionStatus = null;
            try
            {
                var sp_name = "sp_Payroll_ExcludedEmployeeFromBonus_Insert_Update_Delete";
                var param = Utility.DappperParams(model, user);
                param.Add("ExecutionFlag", Data.Delete);
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, param, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                executionStatus = ResponseMessage.Invalid();
                await _sysLogger.SaveHRMSException(ex, user.Database, "BonusBusiness", "DeleteEmployeeFromExcludeListAsync", user);
            }
            return executionStatus;
        }
    }

}
