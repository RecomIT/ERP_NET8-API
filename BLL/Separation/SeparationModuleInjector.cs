using BLL.Separation.Implementation;
using BLL.Separation.Implementation.Admin;
using BLL.Separation.Implementation.Supervisor;
using BLL.Separation.Implementation.User;
using BLL.Separation.Interface;
using BLL.Separation.Interface.Admin;
using BLL.Separation.Interface.Supervisor;
using BLL.Separation.Interface.User;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BLL.Separation
{
    public static class SeparationModuleInjector
    {
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IEmployeeInfoBusiness, EmployeeInfoBusiness>();
            services.AddScoped<IUserResignationBusiness, UserReignationBusiness>();


            // Supervisor // Starting ...

            services.AddScoped<ISupervisorApprovalBusiness, SupervisorApprovalBusiness>();

            // Supervisor // Ending ...


            // Admin // Starting ...

            services.AddScoped<IEmployeeResignationsApprovalBusiness, EmployeeResignationsApprovalBusiness>();

            // Admin // Ending ...



            services.AddScoped<IEmployeeResignationRequestBusiness, EmployeeResignationRequestBusiness>();

            services.AddScoped<IEmployeeSettlementSetupBusiness, EmployeeSettlementSetupBusiness>();



            services.AddScoped<IEmployeeResignationBusiness, EmployeeResignationBusiness>();

            services.AddScoped<IResignationCategoryBusiness, ResignationCategoryBusiness>();
        }
    }
}
