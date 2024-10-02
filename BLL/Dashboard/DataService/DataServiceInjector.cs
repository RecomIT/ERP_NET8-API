using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using BLL.Dashboard.DataService.Implementation;
using BLL.Dashboard.DataService.Interface;

namespace BLL.Dashboard.DataService
{
    public static class DataServiceInjector
    {
        public static void DataServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IDataGetService, DataGetService>();
        }
    }

}
