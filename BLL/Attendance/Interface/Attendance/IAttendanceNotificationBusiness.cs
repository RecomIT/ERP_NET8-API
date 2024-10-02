using System.Threading.Tasks;
using System.Collections.Generic;
using Shared.OtherModels.User;
using Shared.Attendance.ViewModel.Attendance;

namespace BLL.Attendance.Interface.Attendance
{
    public interface IAttendanceNotificationBusiness
    {
        Task<IEnumerable<EmployeeLateEntryEmailNotification>> EmployeeLateEmailAlertProcess(AppUser user);
        Task<IEnumerable<EmployeeLateEntryEmailNotification>> EmployeeLateEntryEmailNotificationProcess(AppUser user);
        Task AttendanceEmailNotificationProcess(AppUser user);
    }
}
