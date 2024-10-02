using System.Threading.Tasks;
using Shared.OtherModels.User;
using System.Collections.Generic;
using Shared.Attendance.ViewModel.Attendance;
using Shared.Attendance.Filter.Attendance;

namespace BLL.Attendance.Interface.Attendance
{
    public interface IEmployeeAttendanceDataBusiness
    {
        Task<IEnumerable<AttendanceSummeryViewModel>> GetEmployeesAttendanceSummeryAsync(AttendanceSummary_Filter filter, AppUser user);
        Task<IEnumerable<EmployeeDailyAttendanceViewModel>> GetEmployeesDailyAttendanceAsync(DailyAttendance_Filter filter, AppUser user);
    }
}
