using Shared.OtherModels.User;
using Shared.OtherModels.Response;
using Shared.Payroll.DTO.Deduction;
using Shared.OtherModels.DataService;
using Shared.Payroll.Filter.Deduction;
using Shared.Payroll.ViewModel.Deduction;

namespace BLL.Salary.Deduction.Interface
{
    public interface IDeductionHeadBusiness
    {
        Task<ExecutionStatus> SaveDeductionHeadAsync(DeductionHeadDTO model, AppUser user);
        Task<IEnumerable<DeductionHeadViewModel>> GetDeductionHeadsAsync(DeductionHead_Filter filter, AppUser user);
        Task<IEnumerable<Select2Dropdown>> GetDeductionHeadExtensionAsync(AppUser user);
        Task<ExecutionStatus> DeductionHeadValidatorAsync(DeductionHeadDTO model, AppUser user);
        Task<IEnumerable<Dropdown>> GetDeductionHeadDropdownAsync(AppUser user);
    }
}
