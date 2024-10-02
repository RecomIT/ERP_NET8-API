using BLL.Salary.Bonus.Interface;
using BLL.Salary.Setup.Interface;
using BLL.Salary.Salary.Interface;
using BLL.Salary.Payment.Interface;
using BLL.Salary.Variable.Interface;
using BLL.Salary.Deduction.Interface;
using BLL.Salary.Allowance.Interface;
using BLL.Salary.Incentive.Interface;
using BLL.Salary.Bonus.Implementation;
using BLL.Salary.Setup.Implementation;
using BLL.Salary.CashSalary.Interface;
using BLL.Salary.Salary.Implementation;
using BLL.Salary.Payment.Implementation;
using Microsoft.Extensions.Configuration;
using BLL.Salary.Variable.Implementation;
using BLL.Salary.WalletPayment.Interface;
using BLL.Salary.Incentive.Implementation;
using BLL.Salary.Allowance.Implementation;
using BLL.Salary.Deduction.Implementation;
using BLL.Salary.CashSalary.Implementation;
using BLL.Salary.WalletPayment.Implementation;
using Microsoft.Extensions.DependencyInjection;

namespace BLL.Salary
{
    public class SalaryModuleInjector
    {
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            // Setup
            services.AddScoped<IFiscalYearBusiness, FiscalYearBusiness>();

            // Allowance
            services.AddScoped<IAllowanceConfigBusiness, AllowanceConfigBusiness>();
            services.AddScoped<IAllowanceHeadBusiness, AllowanceHeadBusiness>();
            services.AddScoped<IAllowanceNameBusiness, AllowanceNameBusiness>();
            services.AddScoped<ISalaryAllowanceConfigBusiness, SalaryAllowanceConfigBusiness>();

            // Deduction
            services.AddScoped<IDeductionConfigBusiness, DeductionConfigBusiness>();
            services.AddScoped<IDeductionHeadBusiness, DeductionHeadBusiness>();
            services.AddScoped<IDeductionNameBusiness, DeductionNameBusiness>();

            // Variable
            services.AddScoped<IMonthlyVariableAllowanceBusiness, MonthlyVariableAllowanceBusiness>();
            services.AddScoped<IMonthlyVariableDeductionBusiness, MonthlyVariableDeductionBusiness>();
            services.AddScoped<IPeriodicallyVariableAllowanceBusiness, PeriodicallyVariableAllowanceBusiness>();
            services.AddScoped<IPeriodicallyVariableDeductionBusiness, PeriodicallyVariableDeductionBusiness>();

            // Payment
            services.AddScoped<ISupplementaryPaymentAmountBusiness, SupplementaryPaymentAmountBusiness>();
            services.AddScoped<ISupplementaryPaymentProcessBusiness, SupplementaryPaymentProcessBusiness>();
            services.AddScoped<ISupplementaryPaymentReportBusiness, SupplementaryPaymentReportBusiness>();

            // Wallet Payment
            services.AddScoped<IWalletPaymentBusiness, WalletPaymentBusiness>();
            // Salary

            services.AddScoped<ISalaryHoldBusiness, SalaryHoldBusiness>();
            services.AddScoped<ISalaryProcessBusiness, SalaryProcessBusiness>();
            services.AddScoped<IExecuteSalaryProcess, ExecuteSalaryProcess>();
            services.AddScoped<ISalaryReportBusiness, SalaryReportBusiness>();
            services.AddScoped<ISalaryReviewBusiness, SalaryReviewBusiness>();
            services.AddScoped<IProjectedPaymentBusiness, ProjectedPaymentBusiness>();
            services.AddScoped<IUploadSalaryComponentBusiness, UploadSalaryComponentBusiness>();
            services.AddScoped<IConditionalDepositAllowanceConfigBusiness, ConditionalDepositAllowanceConfigBusiness>();
            services.AddScoped<IDepositAllowancePaymentHistoryBusiness, DepositAllowancePaymentHistoryBusiness>();
            services.AddScoped<IDepositeAllowanceHistoryBusiness, DepositeAllowanceHistoryBusiness>();
            services.AddScoped<IServiceAnniversaryAllowanceBusiness, ServiceAnniversaryAllowanceBusiness>();
            services.AddScoped<IMonthlyAllowanceConfigBusiness, MonthlyAllowanceConfigBusiness>();
            services.AddScoped<IConditionalProjectedPaymentBusiness, ConditionalProjectedPaymentBusiness>();
            services.AddScoped<ISalaryAllowanceArrearAdjustmentBusiness, SalaryAllowanceArrearAdjustmentBusiness>();
            services.AddScoped<ISalaryComponentHistoriesBusiness, SalaryComponentHistoriesBusiness>();

            services.AddScoped<IMonthlyIncentiveBusiness, MonthlyIncentiveBusiness>();
            services.AddScoped<IQuarterlyIncentiveBusiness, QuarterlyIncentiveBusiness>();

            // Cash Salary
            services.AddScoped<ICashSalaryBusiness, CashSalaryBusiness>();

            // Bonus
            services.AddScoped<IBonusBusiness, BonusBusiness>();
            services.AddScoped<IBonusProcessBusiness, BonusProcessBusiness>();

        }
    }
}
