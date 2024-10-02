using BLL.Overtime.Implementation;
using BLL.Overtime.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BLL.Overtime
{
    public static class OvertimeModuleInjector
    {
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IOvertimeBusiness, OvertimeBusiness>();
        }
    }
}
