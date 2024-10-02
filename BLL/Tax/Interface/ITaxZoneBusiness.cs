using Shared.OtherModels.User;
using Shared.OtherModels.Response;
using Shared.OtherModels.DataService;
using Shared.Payroll.ViewModel.Tax;
using Shared.Payroll.Filter.Tax;

namespace BLL.Tax.Interface
{
    public interface ITaxZoneBusiness
    {
        Task<EmployeeTaxZoneViewModel> GetEmployeeTaxZonesAsync(EmployeeTaxZone_Filter query, AppUser user);
        Task<ExecutionStatus> SaveEmployeeTaxZoneAsync(List<EmployeeTaxZoneViewModel> model, AppUser user);
        Task<IEnumerable<EmployeeTaxZoneViewModel>> GetEmployeeTaxZoneListAsync(EmployeeTaxZone_Filter query, AppUser user);
        Task<ExecutionStatus> UpdateTaxZoneAsync(EmployeeTaxZoneViewModel model, AppUser user);
        Task<ExecutionStatus> TaxZoneValidatorAsync(List<EmployeeTaxZoneViewModel> model, AppUser user);
        Task<EmployeeTaxZoneViewModel> GetEmployeeTaxZoneByIdAsync(long id, AppUser user);
        Task<IEnumerable<EmployeeTaxZoneViewModel>> GetEmployeeTaxZonesAsync(long? employeeId, string taxZone, AppUser user);
        Task<IEnumerable<Select2Dropdown>> GetTaxZoneNameExtensionAsync(string taxZone, AppUser user);
        Task<ExecutionStatus> UploadTaxZoneAsync(List<EmployeeTaxZoneViewModel> taxZones, AppUser user);
    }
}
