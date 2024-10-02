using System.Threading.Tasks;
using Shared.OtherModels.User;
using System.Collections.Generic;
using Shared.OtherModels.Response;
using Shared.Attendance.ViewModel.Scheduler;
using Shared.Attendance.DTO.Scheduler;
using Shared.Attendance.Filter.Scheduler;

namespace BLL.Attendance.Interface.Scheduler
{
    public interface ISchedulerDetailBusiness
    {
        Task<IEnumerable<SchedulerDetailViewModel>> GetSchedulerDetailsAsync(SchedulerDetail_Filter filter, AppUser user);
        Task<IEnumerable<SchedulerDetailListViewModel>> GetSchedulerDetailListAsync(SchedulerDetail_Filter filter, AppUser user);
        Task<ExecutionStatus> SaveSchedulerParticipantStatusAsync(SchedulerParticipantStatusDTO model, AppUser user);
    }
}
