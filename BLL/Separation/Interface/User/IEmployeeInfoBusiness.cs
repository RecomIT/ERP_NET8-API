using Shared.OtherModels.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Separation.Interface.User
{
    public interface IEmployeeInfoBusiness
    {
        Task<object> GetEmployeesInfoAsync(AppUser user);

        Task<object> GetEmployeeDetailsAsync(dynamic filter, AppUser user);

        Task<object> GetEmployeesAsync(AppUser user);
    }
}
