using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using BLL.PF.Implementation;
using BLL.PF.Interface;

namespace BLL.PF
{
    public static class PFModuleInjector
    {
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IPFServiceBusiness, PFServiceBusiness>();
            services.AddScoped<IWundermanPFServiceBusiness, WundermanPFServiceBusiness>();
        }
    }
}
