using Shared.OtherModels.Response;
using Shared.OtherModels.User;
using Shared.Payroll.DTO.SalaryHold;
using Shared.Payroll.Filter.Salary;
using Shared.Payroll.ViewModel.Salary;

namespace BLL.Salary.Salary.Interface
{
    public interface ISalaryHoldBusiness
    {
        Task<ExecutionStatus> SaveSalaryHoldAsync(SalaryHoldDTO model, AppUser user);
        Task<IEnumerable<ExecutionStatus>> SaveUploadHoldSalaryAsync(List<SalaryHoldDTO> models, AppUser user);
        Task<ExecutionStatus> ValidatorSalaryHoldAsync(SalaryHoldDTO model, AppUser user);
        Task<IEnumerable<SalaryHoldViewModel>> GetSalaryHoldListAsync(SalaryHold_Filter filter, AppUser user);
        Task<IEnumerable<SalaryHoldViewModel>> GetEmployeeUnholdSalaryInfoAsync(long employeeId, int unholdMonth, int unholdYear, AppUser user);
    }
}
