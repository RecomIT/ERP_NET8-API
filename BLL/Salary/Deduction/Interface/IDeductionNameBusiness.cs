using Shared.OtherModels.User;
using Shared.OtherModels.Response;
using Shared.Payroll.DTO.Deduction;
using Shared.OtherModels.DataService;
using Shared.Payroll.Filter.Deduction;
using Shared.Payroll.ViewModel.Deduction;

namespace BLL.Salary.Deduction.Interface
{
    public interface IDeductionNameBusiness
    {
        Task<ExecutionStatus> SaveDeductionNameAsync(DeductionNameDTO model, AppUser user);
        Task<IEnumerable<DeductionNameViewModel>> GetDeductionNamesAsync(DeductionName_Filter filter, AppUser user);
        Task<IEnumerable<Select2Dropdown>> GetDeductionNameExtensionAsync(string DeductionType, long DeductionHeadId, AppUser user);
        Task<ExecutionStatus> DeductionNameValidatorAsync(DeductionNameDTO model, AppUser user);
        Task<IEnumerable<Dropdown>> GetDeductionNameDropdownAsync(AppUser user);
    }
}
