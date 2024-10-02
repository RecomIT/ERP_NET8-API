using DAL;
using Shared;
using BLL.Base.Implementation;
using BLL.Base.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using BLL.Administration.Implementation;
using BLL.Administration.Interface;
using BLL.Download.Implementation;
using BLL.Download.Interface;
using BLL.Asset;
using BLL.Attendance;
using BLL.Employee;
using BLL.Leave;
using BLL.Overtime;
using BLL.PF;
using BLL.Salary;
using BLL.Separation;
using BLL.Tax;
using BLL.Dashboard.CommonDashboard;
using BLL.Dashboard.DataService;
using BLL.Expense_Reimbursement;
using BLL.Tools;

namespace BLL
{
    public static class BLLInjector
    {
        
      public static void  BLLConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
        
            // Injecting Shared
            SharedInjector.SharedConfigureServices(services, configuration);

            // Injecting DAL
            DALInjector.DALConfigureServices(services);

            // Md. Mahbur Rahman
            //DALInjector.AddDatabaseContext(services);

            // Others Injection
            services.AddScoped<ISysLogger, SysLogger>();
            services.AddScoped<IReportBase, ReportBase>();
            services.AddScoped<IModuleConfig, ModuleConfig>();
            
            // Control Panel Business
            services.AddScoped<ILoginManager, LoginManager>();
            services.AddScoped<IAppConfigBusiness, AppConfigBusiness>();
            services.AddScoped<IOrgInitBusiness, OrgInitBusiness>();
            services.AddScoped<IOrganizationConfig, OrganizationConfig>();
            services.AddScoped<IUserConfigBusiness, UserConfigBusiness>();
            services.AddScoped<IModuleConfigBusiness, ModuleConfigBusiness>();
            services.AddScoped<IReportConfigBusiness, ReportConfigBusiness>();
            services.AddScoped<IBranchInfoBusiness, BranchInfoBusiness>();
            services.AddScoped<IUserLogReportBusiness, UserLogReportBusiness>();
            services.AddScoped<IEmailFor2FABusiness, EmailFor2FABusiness>();
            services.AddScoped<IGoogleAuthenticatorBusiness, GoogleAuthenticatorBusiness>();

            DataServiceInjector.DataServices(services, configuration);

            CommonDashboardInjector.CommonDashboardServices(services, configuration);

            // Employee Module //
            EmployeeModuleInjector.ConfigureServices(services, configuration);
            //-----------------//

            // Attendance Module //
            AttendanceModuleInjector.ConfigureServices(services, configuration);
            //------------------//

            // Leave Module //
            LeaveModuleInjector.ConfigureServices(services, configuration);
            //-------------//

            // Separation Module //
            SeparationModuleInjector.ConfigureServices(services, configuration);
            //-----------------//

            // Salary Module //
            SalaryModuleInjector.ConfigureServices(services, configuration);
            //--------------//

            // Tax Module //
            TaxModuleInjector.ConfigureServices(services, configuration);
            //-----------//

            // PF Module //
            PFModuleInjector.ConfigureServices(services, configuration);
            //-----------//

            // Overtime Module //
            OvertimeModuleInjector.ConfigureServices(services, configuration);
            // Tools Module //
            ToolsModuleInjector.ConfigureServices(services, configuration);

            // Asset Module //
            AssetModuleInjector.ConfigureServices(services, configuration);     

            // Expense & Reimbursement Module //
            Expense_ReimbursementModuleInjector.ConfigureServices(services, configuration);


            // Download
            services.AddScoped<IDownloadBusiness, DownloadBusiness>();

        }
    }
}
