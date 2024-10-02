using Shared.OtherModels.User;
using Shared.Payroll.Domain.Setup;
using Shared.Payroll.Process.Tax;

namespace BLL.Tax.Interface
{
    public interface IEmployeeFreeCarBusiness
    {
        Task<int> GetEmployeeFreeCarByEmployeeIdInTaxProcessAsync(EligibleEmployeeForTaxType employee, FiscalYear fiscalYear, int year, int month, AppUser user);
        Task<decimal> CCTillAmountInTaxProcessAsync(EligibleEmployeeForTaxType employee, FiscalYear fiscalYear, int year, int month, AppUser user);
    }
}
