using Microsoft.EntityFrameworkCore;
using Shared.Models.HR.DomainModels.LateConsideration.Early_Departure;
using Shared.Models.Dashboard.CommonDashboard.Domain;
using Shared.Attendance.Domain.Attendance;
using Shared.Attendance.Domain.Holiday;
using Shared.Attendance.Domain.Scheduler;
using Shared.Attendance.Domain.Workshift;
using Shared.Attendance.Domain.Attendance.LateConsideration;

namespace DAL.Context.Attendance
{
    public class AttendanceModuleDbContext : DbContext
    {
        public AttendanceModuleDbContext(DbContextOptions<AttendanceModuleDbContext> options) : base(options)
        {
            //this.Database.Migrate();
        }

        // Common Dashboard
        public DbSet<AttendanceStatus> HR_AttendanceStatus { get; set; }

        // Holiday
        public DbSet<YearlyHoliday> HR_YearlyHolidays { get; set; }
        public DbSet<PublicHoliday> HR_PublicHolidays { get; set; }

        // Shift
        public DbSet<WorkShift> HR_Workshifts { get; set; }
        public DbSet<WorkShiftWeekend> HR_WorkShiftWeekends { get; set; }
        public DbSet<EmployeeWorkShift> HR_EmployeeWorkShifts { get; set; }

        // Attendance
        public DbSet<EmployeeDailyAttendance> HR_EmployeeDailyAttendances { get; set; }
        public DbSet<EmployeeComplianceDailyAttendance> HR_EmployeeComplianceDailyAttendance { get; set; }
        public DbSet<EmployeeMonthlyAttendenceSummery> HR_EmployeeMonthlyAttendenceSummery { get; set; }
        public DbSet<EmployeeComplianceMonthlyAttendenceSummery> HR_EmployeeComplianceMonthlyAttendenceSummery { get; set; }
        public DbSet<EmployeeManualAttendance> HR_EmployeeManualAttendance { get; set; }
        public DbSet<AttendanceProcess> HR_AttendanceProcess { get; set; }
        public DbSet<AttendanceRowData> HR_AttendanceRowData { get; set; }
        public DbSet<GeoLocationAttendance> HR_GeoLocationAttendance { get; set; }

        // Scheduler
        public DbSet<SchedulerInfo> HR_SchedulerInfo { get; set; }
        public DbSet<SchedulerDetail> HR_SchedulerDetail { get; set; }

        public DbSet<LateRequests> LateRequests { get; set; }
        public DbSet<LateRequestsDetail> LateRequestsDetails { get; set; }
        public DbSet<LateReason> LateReasons { get; set; }
        public DbSet<EarlyDeparture> HREarlyDepartures { get; set; }

    }
}
