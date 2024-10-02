using System.Data;
using Shared.Control_Panel.Filter;
using Shared.OtherModels.User;

namespace BLL.Administration.Interface
{
    public interface IUserLogReportBusiness
    {
        Task<DataTable> GetUserAccessStatusInfoAsync(UserLogReport_Filter filter, AppUser user);
        Task<DataTable> GetUserPrivilegeInfoAsync(UserLogReport_Filter filter, AppUser user);
        Task<DataTable> GetUserRolePrivilegeInfoAsync(UserLogReport_Filter filter, AppUser user);
    }
}
