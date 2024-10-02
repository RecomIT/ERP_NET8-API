using Shared.OtherModels.User;
using Shared.OtherModels.Response;
using Shared.Payroll.ViewModel.Salary;

namespace BLL.Salary.Salary.Interface
{
    public interface IUploadSalaryComponentBusiness
    {
        Task<ExecutionStatus> UploadSalaryAllowanceAsync(List<SalaryAllowanceViewModel> allowances, AppUser user);
        Task<ExecutionStatus> UploadSalaryDeductionAsync(List<SalaryDeductionViewModel> deductions, AppUser user);
        Task<ExecutionStatus> UploadAllowanceAsync(List<SalaryAllowanceViewModel> allowances, AppUser user);
        Task<ExecutionStatus> UploadDeductionAsync(List<SalaryDeductionViewModel> deductions, AppUser user);
        Task<IEnumerable<UploadAllowanceViewModel>> GetUploadAllowancesAsync(long? uploadId, long? allowanceNameId, long? employeeId, short? month, short? year, AppUser user);
        Task<IEnumerable<UploadDeductionViewModel>> GetUploadDeductionsAsync(long? uploadId, long? deductionNameId, long? employeeId, short? month, short? year, AppUser user);
        Task<ExecutionStatus> UpdateAllowanceAsync(long? uploadId, long allowanceNameId, long? employeeId, short? month, short? year, decimal amount, AppUser user);
        Task<ExecutionStatus> UpdateDeductionAsync(long? uploadId, long deductionNameId, long? employeeId, short? month, short? year, decimal amount, AppUser user);
    }
}
