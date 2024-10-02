using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using BLL.Dashboard.CommonDashboard.Implementation;
using BLL.Dashboard.CommonDashboard.Interface;
using BLL.Dashboard.Admin.Implentation;
using BLL.Dashboard.Admin.Interface;
using BLL.Dashboard.SubordiantesLeave.Implementation;
using BLL.Dashboard.SubordiantesLeave.Interface;
using BLL.Dashboard.Admin.HR.Implementation;
using BLL.Dashboard.Admin.HR.Interface;
using BLL.Dashboard.User.Leave.Implementation;
using BLL.Dashboard.User.Leave.Interface;

namespace BLL.Dashboard.CommonDashboard
{
    public static class CommonDashboardInjector
    {
        public static void CommonDashboardServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IAttendanceCommonDashboardBusiness, AttendanceCommonDashboardBusiness>();
            services.AddScoped<ILeaveCommonDashboardBusiness, LeaveCommonDashboardBusiness>();
            services.AddScoped<ICommonDashboardBusiness, CommonDashboardBusiness>();

            services.AddScoped<ISubordinatesLeaveBusiness, SubordiantesLeaveBusiness>();
            services.AddScoped<IEmployeeLeaveBusiness, EmployeeLeaveBusines>();


            services.AddScoped<IMyLeaveHistoryBusiness, MyLeaveHistoryBusiness>();
            services.AddScoped<IHrDashboardBusiness, HrDashboardBusiness>();



        }
    }
}
