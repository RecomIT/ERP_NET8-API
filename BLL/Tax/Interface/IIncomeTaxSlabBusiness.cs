using Shared.OtherModels.User;
using Shared.OtherModels.Response;
using Shared.Payroll.ViewModel.Tax;

namespace BLL.Tax.Interface
{
    public interface IIncomeTaxSlabBusiness
    {
        Task<ExecutionStatus> SaveIncomeTaxSlabAsync(TaxSlabInfo model, AppUser user);
        Task<ExecutionStatus> UpdateIncomeTaxSlabAsync(TaxSlabUpdate model, AppUser user);
        Task<IEnumerable<IncomeTaxSlabViewModel>> GetIncomeTaxSlabsAsync(long? IncomeTaxSlabId, string ImpliedCondition, long? FiscalYearId, AppUser user);
        Task<IEnumerable<TaxSlabData>> GetIncomeTaxSlabsDataAsync(long? IncomeTaxSlabId, string ImpliedCondition, long? FiscalYearId, AppUser user);
        Task<ExecutionStatus> ValidateIncomeTaxSlabAsync(TaxSlabInfo model, AppUser user);
        Task<ExecutionStatus> ValidateIncomeTaxSlabAsync(IncomeTaxSlabViewModel model, AppUser user);
        Task<IEnumerable<IncomeTaxSlabViewModel>> GetIncomeTaxSlabsByImpliedConditionAsync(string ImpliedCondition, long FiscalYearId, AppUser user);
    }
}
