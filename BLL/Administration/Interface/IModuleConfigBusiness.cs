using Shared.Control_Panel.ViewModels;
using Shared.OtherModels.Response;
namespace BLL.Administration.Interface
{
    public interface IModuleConfigBusiness
    {
        Task<IEnumerable<HRModuleConfigViewModel>> GetHRModuleConfigsAsync(long companyId, long organizationId);
        Task<ExecutionStatus> SaveHRModuleConfigsAsync(HRModuleConfigViewModel model);
        Task<IEnumerable<PayrollModuleConfigViewModel>> GetPayrollModuleConfigsAsync(long companyId, long organizationId);
        Task<ExecutionStatus> SavePayrollModuleConfigsAsync(PayrollModuleConfigViewModel model);
        Task<IEnumerable<PFModuleConfigViewModel>> GetPFModuleConfigsAsync(long companyId, long organizationId);
        Task<ExecutionStatus> SavePFModuleConfigsAsync(PFModuleConfigViewModel model);
    }
}
