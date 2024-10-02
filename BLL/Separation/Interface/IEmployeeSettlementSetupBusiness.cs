
using Shared.OtherModels.User;


namespace BLL.Separation.Interface
{
    public interface IEmployeeSettlementSetupBusiness
    {

        Task<object> GetPendingSettlementSetupEmployeesAsync(AppUser user);
        Task<object> GetResignationSetupEmployeeListAsync(AppUser user);


        Task<IEnumerable<object>> GetPendingSettlementSetupListAsync(dynamic filter, AppUser user);

        Task<IEnumerable<object>> GetSettlementSetupListAsync(dynamic filter, AppUser user);



        Task<object> SaveEmployeeSettlementSetupAsync(dynamic filter, AppUser user);


    }
}
