using Shared.OtherModels.User;
using Shared.OtherModels.Response;
using Shared.Payroll.DTO.Allowance;
using Shared.OtherModels.DataService;
using Shared.Payroll.Filter.Allowance;
using Shared.Payroll.ViewModel.Allowance;
using Shared.Payroll.Process.Allowance;

namespace BLL.Salary.Allowance.Interface
{
    public interface IAllowanceNameBusiness
    {
        Task<ExecutionStatus> SaveAllowanceNameAsync(AllowanceNameDTO model, AppUser user);
        Task<IEnumerable<AllowanceNameViewModel>> GetAllowanceNamesAsync(AllowanceName_Filter filter, AppUser user);
        Task<AllowanceNameViewModel> GetAllowanceNameByIdAsync(long allowanceNameId, AppUser user);
        Task<IEnumerable<Select2Dropdown>> GetAllowanceNameExtensionAsync(string allowanceType, long allowanceHeadId, AppUser user);
        Task<ExecutionStatus> AllowanceNameValidatorAsync(AllowanceNameDTO model, AppUser user);
        Task<IEnumerable<EmployeeTaxableAllowance>> GetEmployeeTaxableAllowances(long employeeId, long SalaryReviewInfoId, AppUser user);
        Task<ExecutionStatus> SaveAllowanceNameWithConfigAsync(AllowanceNameDTO model, AppUser user);
        Task<IEnumerable<Dropdown>> GetAllowanceNameDropdownAsync(AppUser user);
        Task<AllowanceInfo> GetAllowanceInfos(AppUser user);
    }
}
