using Shared.OtherModels.User;
using Shared.OtherModels.Response;
using Shared.Payroll.DTO.Tax;
using Shared.Payroll.ViewModel.Tax;
using Shared.Payroll.Filter.Tax;

namespace BLL.Tax.Interface
{
    public interface IActualTaxDeductionBusiness
    {
        Task<ExecutionStatus> SaveActualTaxDeductionAsync(ActualTaxDeductionDTO model, AppUser user);
        Task<IEnumerable<ActualTaxDeductionViewModel>> GetActualTaxDeductionInfosAsync(ActualTaxDeduction_Filter filter, AppUser user);
        Task<ExecutionStatus> ValidatorAsync(ActualTaxDeductionDTO model, AppUser user);
        Task<IEnumerable<ExecutionStatus>> SaveUploadInfosAsync(List<ActualTaxDeductionDTO> model, AppUser user);
        Task<IEnumerable<ExecutionStatus>> SaveApprovalAsync(ActualTaxDeductionApprovalDTO model, AppUser user);
        Task<ExecutionStatus> UpdateActaulTaxDeductedInSalaryAndTaxAsync(UpdateActaulTaxDeductedDTO model, AppUser user);

    }
}