using System.Threading.Tasks;
using Shared.OtherModels.User;
using System.Collections.Generic;
using Shared.OtherModels.Response;
using Shared.Attendance.ViewModel.Scheduler;
using Shared.Attendance.DTO.Scheduler;
using Shared.Attendance.Filter.Scheduler;

namespace BLL.Attendance.Interface.Scheduler
{
    public interface ISchedulerInfoBusiness
    {
        Task<IEnumerable<SchedulerInfoViewModel>> GetSchedulerInfosAsync(SchedulerInfo_Filter filter, AppUser user);
        Task<ExecutionStatus> SaveSchedulerAsync(SchedulerInfoDTO schedulerInfo, AppUser user);
        Task<ExecutionStatus> SaveSchedulerStatusAsync(SchedulerInfoStatusDTO model, AppUser user);
        Task<ExecutionStatus> DeleteSchedulerInfoAsync(DeleteSchedulerInfoDTO model, AppUser user);
        Task<SchedulerInfoViewModel> GetSchedulerInfoWithDetailsAsync(SchedulerInfo_Filter model, AppUser user);
    }
}
