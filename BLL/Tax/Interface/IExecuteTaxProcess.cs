using Shared.OtherModels.User;
using Shared.OtherModels.Response;
using Shared.Payroll.Process.Tax;
using Shared.Payroll.Filter.Tax;
using Shared.Control_Panel.Domain;
using Shared.Payroll.Domain.Setup;
using Shared.Payroll.Process.Allowance;

namespace BLL.Tax.Interface
{
    public interface IExecuteTaxProcess
    {
        Task<ExecutionStatus> SalaryTaxProcessAsync(TaxProcessExecution execution, AppUser user);
        Task<IEnumerable<EligibleEmployeeForTaxType>> GetEligibleEmployeesAysnc(string incomingFlag, EligibleEmployeeForTax_Filter filter, AppUser user);
        Task<EmployeeTaxProcessedInfo> SalaryTaxDetailsAsync(FiscalYear fiscalYearInfo, PayrollModuleConfig payrollModuleConfig, EligibleEmployeeForTaxType employee, AllowanceInfo allowanceInfo, int month, int year, AppUser user);
        Task<IEnumerable<AllowancesEarnedByThisEmployee>> AllowancesEarnedByThisEmployeeByThisFiscalYearAsync(EligibleEmployeeForTaxType employee, FiscalYear fiscalYear, int month, int year, long lastSalaryReviewId, AppUser user);
        Task<decimal> GetAllowanceTillAmountAsync(long employeeId, long allowanceNameId, FiscalYear fiscalYear, int month, int year, AppUser user);
        Task<decimal> GetAllowanceTillAdjustmentAsync(long employeeId, long allowanceNameId, FiscalYear fiscalYear, int month, int year, AppUser user);
        Task<decimal> GetDeductableTillAmountAsync(long employeeId, long deductionNameId, FiscalYear fiscalYear, int month, int year, AppUser user);
        Task<decimal> GetAllowanceCurrentAmountAsync(long employeeId, long allowanceNameId, FiscalYear fiscalYear, int month, int year, AppUser user);
        Task<decimal> GetAllowanceCurrentAdjustmentAsync(long employeeId, long allowanceNameId, FiscalYear fiscalYear, int month, int year, AppUser user);
        Task<decimal> GetDeductableCurrentAmountAsync(long employeeId, long deductionNameId, FiscalYear fiscalYear, int month, int year, AppUser user);
        Task<decimal> GetAllowanceArrearAmountAsync(long employeeId, long allowanceNameId, FiscalYear fiscalYear, int month, int year, AppUser user);
        Task<decimal> GetProjectionAmountWhenDiscontinued(EligibleEmployeeForTaxType employee, decimal amount, FiscalYear fiscalYear, DateTime startDate, AppUser user);
        Task<TaxDetailInTaxProcess> GetEmployeePFAmountAsync(EligibleEmployeeForTaxType employee, FiscalYear fiscalYear, int year, int month, AppUser user);
        Task<IEnumerable<TaxDetailInTaxProcess>> MargeTaxDetailsAsync(EligibleEmployeeForTaxType employee, IEnumerable<TaxDetailInTaxProcess> taxDetails, AppUser user);
        Task<ExecutionStatus> SaveTaxAsync(List<EmployeeTaxProcessedInfo> taxProcessInfo, FiscalYear fiscalYear, int month, int year, bool effectOnSalary, AppUser user);
        Task<IEnumerable<DeductableIncomeOfThisEmployee>> DeductableIncomeOfThisEmployeeAsync(EligibleEmployeeForTaxType employee, FiscalYear fiscalYear, int month, int year, AppUser user);
    }
}
