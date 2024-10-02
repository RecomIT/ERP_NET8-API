using BLL.Attendance.Implementation.Attendance;
using BLL.Attendance.Implementation.Holiday;
using BLL.Attendance.Implementation.Report;
using BLL.Attendance.Implementation.Scheduler;
using BLL.Attendance.Implementation.WorkShift;
using BLL.Attendance.Interface.Attendance;
using BLL.Attendance.Interface.Holiday;
using BLL.Attendance.Interface.Report;
using BLL.Attendance.Interface.Scheduler;
using BLL.Attendance.Interface.WorkShift;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BLL.Attendance
{
    public static class AttendanceModuleInjector
    {
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IPublicHolidayBusiness, PublicHolidayBusiness>();
            services.AddScoped<IYearlyHolidayBusiness, YearlyHolidayBusiness>();

            services.AddScoped<IWorkShiftBusiness, WorkShiftBusiness>();
            services.AddScoped<IEmployeeWorkShiftBusiness, EmployeeWorkShiftBusiness>();

            services.AddScoped<IManualAttendanceBusiness, ManualAttendanceBusiness>();
            services.AddScoped<IEmployeeAttendanceDataBusiness, EmployeeAttendanceDataBusiness>();
            services.AddScoped<IAttendanceProcessBusiness, AttendanceProcessBusiness>();
            services.AddScoped<IAttendanceNotificationBusiness, AttendanceNotificationBusiness>();
            services.AddScoped<IAttendanceDataPullingService, AttendanceDataPullingService>();

            services.AddScoped<ISchedulerInfoBusiness, SchedulerInfoBusiness>();
            services.AddScoped<ISchedulerDetailBusiness, SchedulerDetailBusiness>();

            services.AddScoped<IAttendanceReportBusiness, AttendanceReportBusiness>();
            services.AddScoped<ILateConsiderationBusiness, LateConsiderationBusiness>();

        }
    }
}
