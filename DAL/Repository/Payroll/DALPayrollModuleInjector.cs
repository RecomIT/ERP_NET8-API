using DAL.Payroll.Repository.Interface;
using DAL.Payroll.Repository.Implementation;
using Microsoft.Extensions.DependencyInjection;


namespace DAL.Repository.Payroll
{
    public static class DALPayrollModuleInjector
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IMonthlyVariableAllowanceRepository, MonthlyVariableAllowanceRepository>();
            services.AddScoped<IMonthlyVariableDeductionRepository, MonthlyVariableDeductionRepository>();
            services.AddScoped<IConditionalProjectedPaymentParameterRepository, ConditionalProjectedPaymentParameterRepository>();
            services.AddScoped<IConditionalProjectedPaymentExcludeParameterRepository, ConditionalProjectedPaymentExcludeParameterRepository>();
            services.AddScoped<IConditionalDepositAllowanceConfigRepository, ConditionalDepositAllowanceConfigRepository>();
            services.AddScoped<IFiscalYearRepository, FiscalYearRepository>();
            services.AddScoped<IDepositAllowancePaymentHistoryRepository, DepositAllowancePaymentHistoryRepository>();
            services.AddScoped<IAllowanceNameRepository, AllowanceNameRepository>();
            services.AddScoped<ITaxDocumentSubmissionRepository, TaxDocumentSubmissionRepository>();
            services.AddScoped<ISupplementaryPaymentProcessInfoRepository, SupplementaryPaymentProcessInfoRepository>();
            services.AddScoped<IProjectedPaymentRepository, ProjectedPaymentRepository>();
        }
    }
}
