using Microsoft.Extensions.DependencyInjection;
using DAL.Repository.Leave.Interface;
using DAL.Repository.Leave.Implementation;
namespace DAL.Repository.Leave
{
    public static class DALLeaveModuleInjector
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<ILeaveTypeRepository, LeaveTypeRepository>();
            services.AddScoped<ILeaveSettingRepository, LeaveSettingRepository>();
            services.AddScoped<ILeaveBalanceRepository, LeaveBalanceRepository>();
            services.AddScoped<ILeaveHistoryRepository, LeaveHistoryRepository>();
            services.AddScoped<ILeaveRequestRepository, LeaveRequestRepository>();
        }
    }
}
