using Shared.OtherModels.Pagination;

namespace Shared.Models.Dashboard.CommonDashboard.QueryParam.Attendance
{
    public class GeoLocationAttendance : Sortparam
    {
        public string AttendanceDate { get; set; }
        public string AttendanceTime { get; set; }
        public string AttendanceLocation { get; set; }
        public string AttendanceRemarks { get; set; }
        public string ActionName { get; set; }
        public string AttendanceType { get; set; }
        public string SelectedEmployeeId { get; set; }
    }
}
