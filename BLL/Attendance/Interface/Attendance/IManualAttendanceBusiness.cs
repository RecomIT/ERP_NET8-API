using System.Threading.Tasks;
using Shared.OtherModels.User;
using System.Collections.Generic;
using Shared.OtherModels.Response;
using Shared.Attendance.ViewModel.Attendance;
using Shared.Attendance.DTO.Attendance;
using Shared.Attendance.Filter.Attendance;

namespace BLL.Attendance.Interface.Attendance
{
    public interface IManualAttendanceBusiness
    {
        Task<IEnumerable<EmployeeManualAttendanceViewModel>> GetEmployeeManualAttendancesAsync(EmployeeManualAttendance_Filter filter, AppUser user);
        Task<ExecutionStatus> SaveManualAttendanceAsync(EmployeeManualAttendanceDTO model, AppUser user);
        Task<ExecutionStatus> SaveManualAttendanceStatusAsync(ManualAttendanceStatusDTO model, AppUser user);
        Task<ExecutionStatus> DeleteManualAttendanceAsync(DeleteManualAttendanceDTO model, AppUser user);
        Task<IEnumerable<EmployeeManualAttendanceViewModel>> GetSubordinatesManualAttendancesRequestsAsync(SubordinatesManualAttendances_Filter filter, AppUser user);
    }
}
