
using DAL.DapperObject;
using DAL.Logger.Interface;
using DAL.Logger.Implementation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using DAL.Context.Control_Panel;
using DAL.UnitOfWork.Control_Panel.Interface;
using DAL.UnitOfWork.Control_Panel.Implementation;
using DAL.DapperObject.Interface;
using DAL.DapperObject.Implementation;
using DAL.Context.Employee;
using DAL.Repository.Employee;
using DAL.Context.Attendance;
using DAL.Context.Leave;
using DAL.Repository.Leave;
using DAL.Context.Training;
using DAL.Context.Asset;
using DAL.Context.Overtime;
using DAL.Context.Payroll;
using DAL.Context.Separation;
using Shared.Control_Panel.Domain;
using DAL.Database_Context.Database_Connection.Interface;
using DAL.Database_Context.Database_Connection.Implementation;

using DAL.Context.Expense_Reimbursement;

using DAL.Repository.Payroll;
using DAL.UnitOfWork.Payroll.Interface;
using DAL.UnitOfWork.Payroll.Implementation;



namespace DAL
{
    public static class DALInjector
    {


        private static void RegisterControlPanelDbContext<TContext>(IServiceCollection services)
               where TContext : DbContext
        {
            services.AddDbContext<TContext>((serviceProvider, options) => {
                var dbConnection = serviceProvider.GetRequiredService<IDBConnection>();
                options.UseSqlServer(dbConnection.GetControlPanelConnectionString());
            });
        }


        private static void RegisterHRMSDbContext<TContext>(IServiceCollection services)
               where TContext : DbContext
        {
            services.AddDbContext<TContext>((serviceProvider, options) => {
                var dbConnection = serviceProvider.GetRequiredService<IDBConnection>();
                options.UseSqlServer(dbConnection.GetHRMSConnectionString());
            });
        }


        public static void DALConfigureServices(IServiceCollection services)
        {

            services.AddScoped<IDBConnection, DBConnection>();

            RegisterControlPanelDbContext<ControlPanelDbContext>(services);
            RegisterHRMSDbContext<EmployeeModuleDbContext>(services);
            RegisterHRMSDbContext<AttendanceModuleDbContext>(services);
            RegisterHRMSDbContext<Expense_ReimbursementModuleDbContext>(services);
            RegisterHRMSDbContext<LeaveModuleDbContext>(services);
            RegisterHRMSDbContext<SeparationModuleDbContext>(services);
            RegisterHRMSDbContext<AssetModuleDbContext>(services);
            RegisterHRMSDbContext<TrainingModuleDbContext>(services);
            RegisterHRMSDbContext<PayrollDbContext>(services);
            RegisterHRMSDbContext<OvertimeModuleDbContext>(services);



            services.AddIdentity<ApplicationUser, ApplicationRole>(config => {
                config.Password.RequireUppercase = true;
                config.Password.RequireLowercase = true;
                config.Password.RequireDigit = true;
                config.Password.RequireNonAlphanumeric = true; // special character
                config.User.RequireUniqueEmail = true;
                //config.Lockout = new LockoutOptions() { 
                //    MaxFailedAccessAttempts = 5,
                //    DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5)
                //};
                //config.Tokens.PasswordResetTokenProvider = TokenOptions.DefaultProvider;
            })
            .AddDefaultTokenProviders()
            .AddEntityFrameworkStores<ControlPanelDbContext>();



            // Dapper
            services.AddSingleton<DapperContext>();
            services.AddTransient<IDapperData, DapperData>();
            services.AddSingleton<IClientDatabase, ClientDatabase>();
            services.AddScoped<IControlPanelUnitOfWork, ControlPanelUnitOfWork>();
            services.AddScoped<IPayrollUnitOfWork, PayrollUnitOfWork>();


            // Database
            services.AddScoped<IDALSysLogger, DALSysLogger>();


            // Repository
            DALEmployeeModuleInjector.ConfigureServices(services);
            DALLeaveModuleInjector.ConfigureServices(services);
            DALPayrollModuleInjector.ConfigureServices(services);



        }

    }
}
