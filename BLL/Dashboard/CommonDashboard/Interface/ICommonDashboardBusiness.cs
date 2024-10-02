using Shared.Helpers;
using Shared.OtherModels.User;
using System.Threading.Tasks;

namespace BLL.Dashboard.CommonDashboard.Interface
{
    public interface ICommonDashboardBusiness
    {
        // ----------------------- >>> GetCompanyHolidayAndEventsAsync
        Task<object> GetCompanyHolidayAndEventsAsync(AppUser user);


        // ----------------------- >>> GetEmployeeContactAsync
        Task<object> SaveCompanyEventsAsync(dynamic filter, AppUser user);



        Task<object> GetEmployeeBloodGroupsAsync(AppUser user);


        // ----------------------- >>> GetEmployeeContactAsync
        Task<object> GetEmployeeContactAsync(dynamic filter, AppUser user);
    }
}
