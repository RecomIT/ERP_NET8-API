using Shared.OtherModels.User;
using Shared.Payroll.Helpers.SalaryProcess;

namespace BLL.Salary.Salary.Interface
{
    public interface ISalaryProcessHelperBusiness
    {
        Task<FiscalYearInfoByDate> GetFiscalYearInfoByDateAsync(string date, AppUser user);
        Task<IEnumerable<EligibleEmployeeForSalary>> GetEligibleEmployeeForSalaryAsync(string salaryDate, AppUser user);
    }
}
