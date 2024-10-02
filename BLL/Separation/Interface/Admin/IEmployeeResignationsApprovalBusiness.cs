using Shared.OtherModels.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Separation.Interface.Admin
{
    public interface IEmployeeResignationsApprovalBusiness
    {
        Task<IEnumerable<object>> GetApprovedResignationRequestsBySupervisorAsync(dynamic filter, AppUser user);
    }
}
