using System.Threading.Tasks;
using Shared.OtherModels.User;
using Shared.OtherModels.Response;
using System.Collections.Generic;
using Shared.Attendance.DTO.AttendanceDataPulling.AgaKhan;

namespace BLL.Attendance.Interface.Attendance
{
    public interface IAttendanceDataPullingService
    {
        #region AGA KHAN SERVICES
        Task<AgakhanDataPullingParameterDTO> GetAgakhanDataPullingParameter(AppUser user);
        Task<ExecutionStatus> AgakhanDataPulling(AppUser user);
        Task<IEnumerable<AgakhanUserDTO>> GetAgakhanAttendanceDeviceUsersDataAsync(string deviceConnectionString, int lastId, AppUser user);
        Task<IEnumerable<AgakhanEventLogDTO>> GetAgakhanAttendanceDataAsync(string deviceConnectionString, int lastId, AppUser user);
        #endregion
    }
}
