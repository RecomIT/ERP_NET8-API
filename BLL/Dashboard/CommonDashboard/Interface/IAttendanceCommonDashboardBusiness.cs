using Shared.OtherModels.User;
using System.Collections.Generic;
using System.Threading.Tasks;
using Shared.Models.Dashboard.CommonDashboard.QueryParam.Attendance;

namespace BLL.Dashboard.CommonDashboard.Interface
{
    public interface IAttendanceCommonDashboardBusiness
    {

        // ----------------------- >>> WeeklyEmployeeAttendanceSummary
        Task<object> GetEmployeeAttendanceYearsAsync(AppUser user);


        // ----------------------- >>> GetAttendanceMonthWithDataByYearAsync
        Task<object> GetAttendanceMonthWithDataByYearAsync(dynamic filter, AppUser user);


        // ----------------------- >>> WeeklyEmployeeAttendanceSummary
        Task<object> GetMyRecentAttendanceSummaryAsync(AppUser user);


        // ----------------------- >>> GetMyAttendanceSummery
        Task<object> GetMyAttendanceSummaryAsync(dynamic filter, AppUser user);

        Task<object> GetGeoLocationAttendanceAsync(GeoLocationAttendance filter, AppUser user);


        // ----------------- Get WorkShift
        Task<object> GetEmployeeWorkShiftAsync(AppUser user);
        Task<object> GetCheckPunchInPunchOutAsync(AppUser user);

        // With Pagination
        Task<IEnumerable<object>> GetMyGeoLocationAttendanceAsync(dynamic filter, AppUser user);

    }
}
