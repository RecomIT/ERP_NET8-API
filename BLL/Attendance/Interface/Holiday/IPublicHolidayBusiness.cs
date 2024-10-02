using System.Threading.Tasks;
using Shared.OtherModels.User;
using System.Collections.Generic;
using Shared.OtherModels.Response;
using Shared.Attendance.ViewModel.Holiday;
using Shared.Attendance.DTO.Holiday;

namespace BLL.Attendance.Interface.Holiday
{
    public interface IPublicHolidayBusiness
    {
        Task<ExecutionStatus> SavePublicHolidayAsync(PublicHolidayDTO model, AppUser user);
        Task<IEnumerable<PublicHolidayViewModel>> GetPublicHolidaysAsync(long publicHolidayId, AppUser user);
        Task<ExecutionStatus> PublicHolidayValidatorAsync(PublicHolidayDTO model, AppUser user);
    }
}
