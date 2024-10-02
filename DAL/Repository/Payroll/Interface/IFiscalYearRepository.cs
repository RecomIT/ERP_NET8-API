using DAL.Repository.Base.Interface;
using Shared.OtherModels.User;
using Shared.Payroll.Domain.Setup;

namespace DAL.Payroll.Repository.Interface
{
    public interface IFiscalYearRepository : IDapperBaseRepository<FiscalYear>
    {
        Task<FiscalYear> GetCurrectFiscalYearAsync(AppUser user);
        Task<FiscalYear> GetFiscalYearByDateAsync(string date,AppUser user);
        Task<FiscalYear> GetFiscalYearByYearAsync(int year,AppUser user);
    }
}
