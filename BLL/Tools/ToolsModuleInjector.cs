using BLL.Tools.Implementation;
using BLL.Tools.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BLL.Tools
{
    public static class ToolsModuleInjector
    {
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IToolsBusiness, ToolsBusiness>();
        }
    }


}




