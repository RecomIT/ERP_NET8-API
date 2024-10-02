using Shared.OtherModels.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Dashboard.Admin.HR.Interface
{
    public interface IHrDashboardBusiness
    {
        Task<object> GetTotalEmployeeAsync(AppUser user);
        Task<object> GetReligionsAsync(AppUser user);
        Task<object> GetAverageEmployeeDetailsAsync(AppUser user);
        Task<object> GetHrDashboardDataAsync(AppUser user);
    }
}
