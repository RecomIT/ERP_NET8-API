using Shared.OtherModels.User;
using Shared.Payroll.DTO.Setup;
using Shared.Payroll.Domain.Setup;
using Shared.OtherModels.Response;
using Shared.OtherModels.DataService;
using Shared.Payroll.ViewModel.Setup;

namespace BLL.Salary.Setup.Interface
{
    public interface IFiscalYearBusiness
    {
        Task<IEnumerable<FiscalYearViewModel>> GetFiscalYearsAsync(long? fiscalYearId, string fiscalYearRange, AppUser user);
        Task<FiscalYearViewModel> GetFiscalYearAsync(long? fiscalYearId, AppUser user);
        Task<ExecutionStatus> SaveFiscalYearAsync(FiscalYearDTO model, AppUser user);
        Task<ExecutionStatus> DeleteFiscalYearAsync(long fiscalYearId, AppUser user);
        Task<IEnumerable<Select2Dropdown>> GetFiscalYearsExtensionAsync(AppUser user);
        Task<FiscalYearViewModel> GetCurrentFiscalYearAsync(AppUser user);
        Task<IEnumerable<Dropdown>> GetFiscalYearDropdownAysnc(AppUser user);
        Task<FiscalYear> GetFiscalYearInfoWithinADate(string date, AppUser user);
        Task<FiscalYear> GetFiscalYearByIdAsync(long fiscalYearId, AppUser user);

    }
}
