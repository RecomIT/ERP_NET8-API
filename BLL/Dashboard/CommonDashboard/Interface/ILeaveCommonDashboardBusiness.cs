using Shared.Helpers;
using Shared.OtherModels.User;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.Dashboard.CommonDashboard.Interface
{
    public interface ILeaveCommonDashboardBusiness
    {

        // ----------------------- >>> GetMyLeaveSummeryAsync
        Task<object> GetMyLeaveSummaryAsync(dynamic filter, AppUser user);

        // ----------------------- >>> GetLeaveTypeSummeryAsync
        Task<object> GetMyLeaveTypeSummaryAsync(AppUser user);


        Task<IEnumerable<object>> GetMyLeaveAppliedRecordsAsync(dynamic filter, AppUser user);


    }
}
