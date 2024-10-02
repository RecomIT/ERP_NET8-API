using Shared.OtherModels.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Dashboard.User.Leave.Interface
{
    public interface IMyLeaveHistoryBusiness
    {

        Task<object> GetLeavePeriodYearAsync(AppUser user);

        Task<object> GetLeavePeriodMonthAsync(dynamic filter, AppUser user);


        Task<object> GetMyLeaveHistoryAsync(dynamic filter, AppUser user);
    }
}
