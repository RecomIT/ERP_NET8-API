using System.Threading.Tasks;
using Shared.OtherModels.User;
using System.Collections.Generic;
using Shared.Attendance.Report;
using Shared.Attendance.Filter.Report;

namespace BLL.Attendance.Interface.Report
{
    public interface IAttendanceReportBusiness
    {
        Task<EmployeeAttendanceReport> EmployeeAttendanceReportAsync(EmployeeAttendanceReport_Filter filter, AppUser user);
        Task<IEnumerable<DailyAttendance>> DailyAttendancesReportAsync(DailyAttendanceReport_Filter filter, AppUser user);
    }
}
