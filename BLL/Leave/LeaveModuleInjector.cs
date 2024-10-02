using BLL.Leave.Implementation.Balance;
using BLL.Leave.Implementation.Encashment;
using BLL.Leave.Implementation.LeaveSetting;
using BLL.Leave.Implementation.Report;
using BLL.Leave.Implementation.Request;
using BLL.Leave.Implementation.Type;
using BLL.Leave.Interface.Balance;
using BLL.Leave.Interface.Encashment;
using BLL.Leave.Interface.LeaveSetting;
using BLL.Leave.Interface.Report;
using BLL.Leave.Interface.Request;
using BLL.Leave.Interface.Type;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BLL.Leave
{
    public static class LeaveModuleInjector
    {
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ILeaveTypeBusiness, LeaveTypeBusiness>();
            services.AddScoped<ILeaveSettingBusiness, LeaveSettingBusiness>();
            services.AddScoped<IEmployeeLeaveBalanceBusiness, EmployeeLeaveBalanceBusiness>();
            services.AddScoped<IEmployeeLeaveRequestBusiness, EmployeeLeaveRequestBusiness>();
            services.AddScoped<ILeaveReportBusiness, LeaveReportBusiness>();
            services.AddScoped<ILeaveEncashmentBusiness, LeaveEncashmentBusiness>();

            services.AddScoped<ILeaveTypeRepo, LeaveTypeRepo>();    
        }
    }
}
