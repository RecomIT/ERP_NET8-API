
using Shared.OtherModels.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Dashboard.SubordiantesLeave.Interface
{
    public interface ISubordinatesLeaveBusiness
    {

        Task<object> IsSupervisorOrFinalApprovalAsync(AppUser user);

        Task<object> GetSubordinatesEmployeesAsync(AppUser user);

        // Task<object> GetSubordinatesLeaveAsync(dynamic filter, AppUser user);

        // Task<object> GetSubordinatesLeaveApprovalAsync(dynamic filter, AppUser user);



        /// ------------------------------
        /// With Pagination
        /// ------------------------------

        Task<IEnumerable<object>> GetSubordinatesLeaveAsync(dynamic filter, AppUser user);

        Task<IEnumerable<object>> GetSubordinatesLeaveApprovalAsync(dynamic filter, AppUser user);

    }
}
