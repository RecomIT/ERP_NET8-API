using Shared.OtherModels.User;
using Shared.Payroll.Domain.Tax;
using Shared.Payroll.ViewModel.Tax;

namespace BLL.Tax.Interface
{
    public interface ISpecialTaxSlabBusiness
    {
        Task<IEnumerable<IncomeTaxSlabViewModel>> GetByEmployeeId(long employeeId, long fiscalYearId, AppUser user);
    }
}
