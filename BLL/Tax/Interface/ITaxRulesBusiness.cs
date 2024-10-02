using Shared.OtherModels.User;
using Shared.Payroll.Process.Tax;
using Shared.Payroll.Domain.Setup;
using Shared.Control_Panel.Domain;
using Shared.Payroll.Process.Allowance;

namespace BLL.Tax.Interface
{
    public interface ITaxRulesBusiness
    {
        Task<EmployeeTaxProcessedInfo> TaxRulesIY2324(EligibleEmployeeForTaxType employee, FiscalYear fiscalYear, List<Shared.Payroll.Process.Tax.TaxDetailInTaxProcess> taxDetails, PayrollModuleConfig payrollModuleConfig, AllowanceInfo allowanceInfo, int totalMonthReceipt, int remainMonth, int year, int month, AppUser user);
        Task<TaxDeductedTillMonth> GetEmployeeTaxDeductedTillMonthAsync(long employeeId, long fiscalYearId, int year, int month, AppUser user);
        Task<decimal> GetThisMonthSupplementaryOnceOffTaxAsync(long employeeId, long fiscalYearId, int year, int month, AppUser user);
        Task<decimal> GetBeforeThisMonthSupplementaryOnceOffTaxAsync(long employeeId, long fiscalYearId, int year, int month, AppUser user);
        Task<TaxProcessedInfo> TaxRules(EligibleEmployeeForTaxType employee, FiscalYear fiscalYear, List<Shared.Payroll.Process.Tax.TaxDetailInTaxProcess> taxDetails, PayrollModuleConfig payrollModuleConfig, AllowanceInfo allowanceInfo, int year, int month, string flag, AppUser user); // Flag Supplementary Tax / Salary Tax
        Task<List<TaxProcessSlab>> TaxSlab(EligibleEmployeeForTaxType employee, FiscalYear fiscalYear, decimal taxableIncome, int year, int month, TaxSettingInTaxProcess taxSetting, AppUser user);
        Task<decimal> OnceOffTaxPaidInThisMonth(long employeeId, long fiscalYearId, int year, int month, AppUser user);
    }
}
