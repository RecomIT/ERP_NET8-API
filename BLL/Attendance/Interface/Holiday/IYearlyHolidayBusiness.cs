using System.Threading.Tasks;
using Shared.OtherModels.User;
using System.Collections.Generic;
using Shared.OtherModels.Response;
using Shared.Attendance.ViewModel.Holiday;
using Shared.Attendance.DTO.Holiday;

namespace BLL.Attendance.Interface.Holiday
{
    public interface IYearlyHolidayBusiness
    {
        Task<ExecutionStatus> SaveYearlyHolidayAsync(YearlyHolidayDTO model, AppUser user);
        Task<IEnumerable<YearlyHolidayViewModel>> GetYearlyHolidaysAsync(long yearlyHolidayId, AppUser user);
        Task<IEnumerable<YearlyHolidayViewModel>> AssignYearlyHolidayAsync(AppUser user);
        Task<ExecutionStatus> SaveYearlyPublicHolidayAsync(List<YearlyHolidayDTO> models, AppUser user);
    }
}
