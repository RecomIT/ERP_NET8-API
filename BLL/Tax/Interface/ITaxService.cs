using Shared.OtherModels.User;
using Shared.Payroll.Process.Tax;
using Shared.Payroll.Domain.Setup;
using Shared.Control_Panel.Domain;
using Shared.Payroll.ViewModel.Salary;
using Shared.Payroll.ViewModel.Payment;
using Shared.Payroll.Process.Allowance;

namespace BLL.Tax.Interface
{
    public interface ITaxService
    {
        bool IsThisEmployeeDiscontinuedWithinThisFiscalYear(EligibleEmployeeForTaxType employee, FiscalYear fiscalYearInfo);

        int RemainProjectionMonth(DateTime firstDateOfThisMonth, DateTime fiscalYearTo); // Employee
        int RemainProjectionMonthForThisEmployee(EligibleEmployeeForTaxType employee, FiscalYear fiscalYearInfo, DateTime firstDateOfThisMonth, int remainFiscalYearMonth); // Employee
        Task<int> CountOfSalaryReceipt(long employeeId, long fiscalYearId, short month, AppUser user); // Employee
        Task<long> LastSalaryReviewId(EligibleEmployeeForTaxType employee, int paymentYear, int paymentMonth, AppUser user);
        Task<IEnumerable<SalaryReviewDetailViewModel>> GetSalaryReviewDetailsAsync(long employeeId, long salaryReviewId, AppUser user);
        Task<decimal> GetAllowanceTillAmountAsync(long employeeId, long allowanceNameId, FiscalYear fiscalYear, int month, int year, AppUser user);
        Task<decimal> GetAllowanceCurrentAmountAsync(long employeeId, long allowanceNameId, FiscalYear fiscalYear, int month, int year, AppUser user);
        Task<decimal> GetAllowanceArrearAmountAsync(long employeeId, long allowanceNameId, FiscalYear fiscalYear, int month, int year, AppUser user);
        Task<IEnumerable<TaxDetailInTaxProcess>> AllowancesEarnedByThisEmployeeByThisFiscalYearAsync(EligibleEmployeeForTaxType employee, FiscalYear fiscalYear, int month, int year, long lastSalaryReviewId, IEnumerable<SalaryReviewDetailViewModel> salaryReviewDetails, int remainProjectionMonthForThisEmployee, AppUser user);
        Task<EmployeeDepositPaymentViewModel> GetEmployeeAllowanceAccuredAmountInTaxProcessAsync(long employeeId, long allowanceNameId, short year, short month, long fiscalYearId, AppUser user);
        Task<IEnumerable<TaxDetailInTaxProcess>> ProjectionCalculation(EligibleEmployeeForTaxType employee, IEnumerable<TaxDetailInTaxProcess> earnedByThisEmployees, FiscalYear fiscalYearInfom, int remainProjectionMonthForThisEmployee, AllowanceInfo allowanceInfo, int year, int month, AppUser user);
        Task<decimal> GetProjectionAmountWhenDiscontinued(EligibleEmployeeForTaxType employee, decimal amount, FiscalYear fiscalYear, DateTime startDate, AppUser user);
        Task<List<TaxDetailInTaxProcess>> IncomeDetails(EligibleEmployeeForTaxType employee, FiscalYear fiscalYearInfo, List<TaxDetailInTaxProcess> earnings, List<SalaryReviewDetailViewModel> salaryReviewDetails, AllowanceInfo allowanceInfo, PayrollModuleConfig payrollModuleConfig, int year, int month, AppUser user);
        Task<TaxDetailInTaxProcess> GetEmployeePFAmountAsync(EligibleEmployeeForTaxType employee, FiscalYear fiscalYear, int year, int month, AppUser user);
        Task<IEnumerable<DeductableIncomeOfThisEmployee>> DeductableIncomeOfThisEmployeeAsync(EligibleEmployeeForTaxType employee, FiscalYear fiscalYear, int month, int year, AppUser user);
        Task<decimal> GetDeductableTillAmountAsync(long employeeId, long deductionNameId, FiscalYear fiscalYear, int month, int year, AppUser user);
        Task<decimal> GetDeductableCurrentAmountAsync(long employeeId, long deductionNameId, FiscalYear fiscalYear, int month, int year, AppUser user);
        Task<IEnumerable<TaxDetailInTaxProcess>> MargeTaxDetailsAsync(EligibleEmployeeForTaxType employee, IEnumerable<TaxDetailInTaxProcess> taxDetails, AppUser user);
        Task<decimal> GetAllowanceTillAdjustmentAsync(long employeeId, long allowanceNameId, FiscalYear fiscalYear, int month, int year, AppUser user);
        Task<decimal> GetAllowanceCurrentAdjustmentAsync(long employeeId, long allowanceNameId, FiscalYear fiscalYear, int month, int year, AppUser user);
    }
}
