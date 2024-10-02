using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using BLL.Tax.Implementation;
using BLL.Tax.Interface;

namespace BLL.Tax
{
    public class TaxModuleInjector
    {
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IActualTaxDeductionBusiness, ActualTaxDeductionBusiness>();
            services.AddScoped<IEmployeeInvestmentSubmissionBusiness, EmployeeInvestmentSubmissionBusiness>();
            services.AddScoped<IIncomeTaxSlabBusiness, IncomeTaxSlabBusiness>();
            services.AddScoped<ISpecialTaxSlabBusiness, SpecialTaxSlabBusiness>();
            services.AddScoped<ITaxAITBusiness, TaxAITBusiness>();
            services.AddScoped<ITaxProcessBusiness, TaxProcessBusiness>();
            services.AddScoped<ITaxReturnSubmissionBusiness, TaxReturnSubmissionBusiness>();
            services.AddScoped<ITaxSettingBusiness, TaxSettingBusiness>();
            services.AddScoped<ITaxZoneBusiness, TaxZoneBusiness>();
            services.AddScoped<ITaxReportBusiness, TaxReportBusiness>();
            services.AddScoped<IExecuteTaxProcess, ExecuteTaxProcess>();
            services.AddScoped<IEmployeeFreeCarBusiness, EmployeeFreeCarBusiness>();
            services.AddScoped<ITaxRulesBusiness, TaxRulesBusiness>();
            services.AddScoped<ITaxRefundBusiness, TaxRefundBusiness>();
            services.AddScoped<ITaxService, TaxService>();
            services.AddScoped<ITaxChallanBusiness, TaxChallanBusiness>();
            services.AddScoped<IFinalTaxProcessBusiness, FinalTaxProcessBusiness>();

        }
    }
}
