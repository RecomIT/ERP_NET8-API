using Shared.OtherModels.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Separation.Interface.Supervisor
{
    public interface ISupervisorApprovalBusiness
    {
        Task<IEnumerable<object>> GetEmployeeResignationListForSupervisorAsync(dynamic filter, AppUser user);

    }
}
