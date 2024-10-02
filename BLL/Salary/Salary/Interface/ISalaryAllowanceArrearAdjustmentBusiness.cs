using Shared.OtherModels.User;
using Shared.Payroll.DTO.Salary;
using Shared.OtherModels.Response;
using Shared.Payroll.Filter.Salary;
using Shared.Payroll.ViewModel.Salary;

namespace BLL.Salary.Salary.Interface
{
    public interface ISalaryAllowanceArrearAdjustmentBusiness
    {
        Task<ExecutionStatus> UploadSalaryAllowanceArrearAdjAsync(List<SalaryAllowanceArrearAdjustmentDTO> model, AppUser user);
        Task<DBResponse<SalaryAllowanceArrearAdjustmentViewModel>> GetSalaryAllowanceArrearAdjustmentListAsync(SalaryAllowanceArrearAdjustment_Filter arrearAdjustment_Filter, AppUser user);
        Task<IEnumerable<SalaryAllowanceArrearAdjustmentViewModel>> GetPendingSalaryAllowanceArrearAdjustmentAsync(SalaryAllowanceArrearAdjustment_Filter filter, AppUser user);
        Task<SalaryAllowanceArrearAdjustmentViewModel> GetSalaryAllowanceArrearAdjustmentByIdAsync(long id, AppUser user);
        Task<ExecutionStatus> UpdateSalaryAllowanceArrearAdjustmentAsync(SalaryAllowanceArrearAdjustmentViewModel model, AppUser user);
        Task<ExecutionStatus> DeleteSalaryAllowanceArrearAdjustmentAsync(SalaryAllowanceArrearAdjustmentViewModel model, AppUser user);
        Task<IEnumerable<SalaryAllowanceArrearAdjustmentViewModel>> GetSalaryAllowanceArrearAdjustmentByEmployeeIdInSalaryProcessAsync(long employeeId, int year, int month, AppUser user);
        Task<ExecutionStatus> SaveAsync(SalaryAllowanceArrearAdjustmentMasterDTO info, AppUser user);
        Task<ExecutionStatus> ArrearAdjustmentApprovalAsync(ArrearAdjustmentApprovalDTO model, AppUser user);
    }
}
