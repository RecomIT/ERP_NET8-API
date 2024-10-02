using Shared.OtherModels.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Dashboard.Admin.Interface
{
    public interface IEmployeeLeaveBusiness
    {
        Task<object> GetEmployeesAsync(AppUser user);

        //Task<object> GetEmployeesLeaveAsync(dynamic filter, AppUser user);

        Task<IEnumerable<object>> GetEmployeesLeaveAsync(dynamic filter, AppUser user);

        Task<IEnumerable<object>> GetEmployeesLeaveApprovalAsync(dynamic filter, AppUser user);
    }
}
