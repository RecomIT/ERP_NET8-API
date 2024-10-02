using Shared.Payroll.Domain.AIT;
using Shared.Payroll.Domain.Tax;
using Shared.Payroll.Domain.Bonus;
using Shared.Payroll.Domain.Setup;
using Shared.Payroll.Domain.Salary;
using Shared.Payroll.Domain.Payment;
using Microsoft.EntityFrameworkCore;
using Shared.Payroll.Domain.Variable;
using Shared.Payroll.Domain.Allowance;
using Shared.Payroll.Domain.Deduction;
using Shared.Payroll.Domain.CashSalary;
using Shared.Payroll.Domain.WalletPayment;
using Shared.Payroll.Domain.Configuration.FreeCar;
using Shared.Payroll.Domain.Incentive.MonthlyIncentive;
using Shared.Payroll.Domain.Incentive.QuarterlyIncentive;

namespace DAL.Context.Payroll
{
    public class PayrollDbContext : DbContext
    {
        public PayrollDbContext(DbContextOptions<PayrollDbContext> options) : base(options)
        {
        }
        public DbSet<AllowanceHead> Paryroll_AllowanceHeads { get; set; }
        public DbSet<AllowanceName> Paryroll_AllownaceNames { get; set; }
        public DbSet<SalaryAllowanceConfigurationInfo> Payroll_SalaryAllowanceConfigurationInfo { get; set; }
        public DbSet<SalaryAllowanceConfigurationDetail> Payroll_SalaryAllowanceConfigurationDetail { get; set; }
        public DbSet<DeductionHead> Paryroll_DeductionHeads { get; set; }
        public DbSet<DeductionName> Paryroll_DeductionNames { get; set; }
        public DbSet<SalaryReviewInfo> Payroll_SalaryReviewInfo { get; set; }
        public DbSet<SalaryReviewDetail> Payroll_SalaryReviewDetail { get; set; }
        public DbSet<AllowanceConfiguration> Payroll_AllowanceConfiguration { get; set; }
        public DbSet<DeductionConfiguration> Payroll_DeductionConfiguration { get; set; }
        public DbSet<FiscalYear> Payroll_FiscalYear { get; set; }

        // Variable
        public DbSet<MonthlyVariableAllowance> Payroll_MonthlyVariableAllowance { get; set; }
        public DbSet<PeriodicallyVariableAllowanceInfo> Payroll_PeriodicallyVariableAllowanceInfo { get; set; }
        public DbSet<PeriodicallyVariableAllowanceDetail> Payroll_PeriodicallyVariableAllowanceDetail { get; set; }
        public DbSet<MonthlyVariableDeduction> Payroll_MonthlyVariableDeduction { get; set; }
        public DbSet<PeriodicallyVariableDeductionInfo> Payroll_PeriodicallyVariableDeductionInfo { get; set; }
        public DbSet<PeriodicallyVariableDeductionDetail> Payroll_PeriodicallyVariableDeductionDetail { get; set; }
        public DbSet<PrincipleAmountInfo> Payroll_PrincipleAmountInfo { get; set; }

        //Salary Process
        public DbSet<SalaryProcess> Payroll_SalaryProcess { get; set; }
        public DbSet<SalaryProcessDetail> Payroll_SalaryProcessDetail { get; set; }
        public DbSet<SalaryAllowance> Payroll_SalaryAllowance { get; set; }
        public DbSet<SalaryDeduction> Payroll_SalaryDeduction { get; set; }
        public DbSet<SalaryAllowanceArrear> Payroll_SalaryAllowanceArrear { get; set; }
        public DbSet<SalaryProcessSummery> Payroll_SalaryProcessSummery { get; set; }
        public DbSet<SalaryComponentHistory> Payroll_SalaryComponentHistory { get; set; }

        // Adjustment
        public DbSet<SalaryAllowanceAdjustment> Payroll_SalaryAllowanceAdjustments { get; set; }
        public DbSet<SalaryDeductionAdjustment> Payroll_SalaryDeductionAdjustments { get; set; }
        public DbSet<SalaryAllowanceArrearAdjustment> Payroll_SalaryAllowanceArrearAdjustment { get; set; }


        // Upload
        public DbSet<UploadAllowance> Payroll_UploadAllowance { get; set; }
        public DbSet<UploadDeduction> Payroll_UploadDeduction { get; set; }

        // Salary Hold
        public DbSet<SalaryHold> Payroll_SalaryHold { get; set; }

        // Tax
        public DbSet<IncomeTaxSlab> Payroll_IncomeTaxSlab { get; set; }
        public DbSet<SpecialTaxSlab> Payroll_SpecialTaxSlab { get; set; }
        public DbSet<IncomeTaxSetting> Payroll_IncomeTaxSettingId { get; set; }
        public DbSet<TaxExemptionSetting> Payroll_TaxExemptionSetting { get; set; }
        public DbSet<TaxInvestmentSetting> Payroll_TaxInvestmentSetting { get; set; }
        public DbSet<EmployeeTaxProcess> Payroll_EmployeeTaxProcess { get; set; }
        public DbSet<EmployeeTaxProcessDetail> Payroll_EmployeeTaxProcessDetail { get; set; }
        public DbSet<EmployeeTaxProcessSlab> Payroll_EmployeeTaxProcessSlab { get; set; }
        public DbSet<EmployeeYearlyInvestment> Payroll_EmployeeYearlyInvestment { get; set; }
        public DbSet<EmployeeTaxReturnSubmission> Payroll_EmployeeTaxReturnSubmission { get; set; }
        public DbSet<ActualTaxDeduction> Payroll_ActualTaxDeduction { get; set; }
        public DbSet<FinalTaxProcess> Payroll_FinalTaxProcess { get; set; }
        public DbSet<FinalTaxProcessDetail> Payroll_FinalTaxProcessDetail { get; set; }
        public DbSet<FinalTaxProcessSlab> Payroll_FinalTaxProcessSlab { get; set; }
        // Bonus
        public DbSet<Bonus> Payroll_Bonus { get; set; }
        public DbSet<BonusConfig> Payroll_BonusConfig { get; set; }
        public DbSet<BonusProcess> Payroll_BonusProcess { get; set; }
        public DbSet<BonusProcessDetail> Payroll_BonusProcessDetail { get; set; }
        public DbSet<EmployeeExcludedFromBonus> Payroll_EmployeeExcludedFromBonus { get; set; }

        // AIT
        public DbSet<TaxDocumentSubmission> Payroll_TaxDocumentSubmission { get; set; }
        // Tax_Zone
        public DbSet<EmployeeTaxZone> Payroll_EmployeeTaxZone { get; set; }

        // Bonus_Tax_Process
        public DbSet<EmployeeBonusTaxProcess> Payroll_EmployeeBonusTaxProcess { get; set; }
        public DbSet<EmployeeBonusTaxProcessDetail> Payroll_EmployeeBonusTaxProcessDetail { get; set; }
        public DbSet<EmployeeBonusTaxProcessSlab> Payroll_EmployeeBonusTaxProcessSlab { get; set; }

        // Employee Free Car
        public DbSet<EmployeeFreeCar> Payroll_EmployeeFreeCar { get; set; }
        public DbSet<TaxChallan> Payroll_TaxChallan { get; set; }

        // Supplementary Payment
        public DbSet<SupplementaryPaymentAmount> Payroll_SupplementaryPaymentAmount { get; set; }
        public DbSet<SupplementaryPaymentProcessInfo> Payroll_SupplementaryPaymentProcessInfo { get; set; }
        public DbSet<SupplementaryPaymentTaxInfo> Payroll_SupplementaryPaymentTaxInfo { get; set; }
        public DbSet<SupplementaryPaymentTaxDetail> Payroll_SupplementaryPaymentTaxDetail { get; set; }
        public DbSet<SupplementaryPaymentTaxSlab> Payroll_SupplementaryPaymentTaxSlab { get; set; }

        //Incentive Payment
        public DbSet<QuarterlyIncentiveProcess> Payroll_QuarterlyIncentiveProcess { get; set; }
        public DbSet<QuarterlyIncentiveProcessDetail> Payroll_QuarterlyIncentiveProcessDetail { get; set; }
        public DbSet<MonthlyIncentiveProcess> Payroll_MonthlyIncentiveProcess { get; set; }
        public DbSet<MonthlyIncentiveProcessDetail> Payroll_MonthlyIncentiveProcessDetail { get; set; }

        //Wallet Payment
        //public DbSet<InternalDesignation> InternalDesignation { get; set; }
        public DbSet<WalletPaymentConfiguration> WalletPaymentConfiguration { get; set; }
        //Cash Payment
        public DbSet<CashSalaryHead> CashSalaryHead { get; set; }
        public DbSet<UploadCashSalary> UploadCashSalary { get; set; }
        public DbSet<CashSalaryProcess> CashSalaryProcess { get; set; }
        public DbSet<CashSalaryProcessDetail> CashSalaryProcessDetail { get; set; }
        public DbSet<AnonymousEmployeeInfo> AnonymousEmployeeInfo { get; set; }

        // Employee Projected Allowances
        public DbSet<EmployeeProjectedAllowanceProcessInfo> Payroll_EmployeeProjectedAllowanceProcessInfo { get; set; }
        public DbSet<EmployeeProjectedPayment> Payroll_EmployeeProjectedAllowance { get; set; }

        // Deposite Allowance
        public DbSet<EmployeeDepositAllowanceConfig> Payroll_EmployeeDepositAllowanceConfig { get; set; }
        public DbSet<ConditionalDepositAllowanceConfig> Payroll_ConditionalDepositAllowanceConfig { get; set; }
        public DbSet<DepositAllowanceHistory> Payroll_DepositAllowanceHistory { get; set; }
        public DbSet<DepositAllowancePaymentHistory> Payroll_DepositAllowancePaymentHistory { get; set; }

        // Job Anniversary Allowance
        public DbSet<ServiceAnniversaryAllowance> Payroll_JobAnniversaryAllowance { get; set; }
        public DbSet<RecipientsofServiceAnniversaryAllowance> Payroll_RecipientsofServiceAnniversaryAllowance { get; set; }

        // Monthly Allowance Config
        public DbSet<MonthlyAllowanceConfig> Payroll_MonthlyAllowanceConfig { get; set; }
        public DbSet<MonthlyAllowanceHistory> Payroll_MonthlyAllowanceHistory { get; set; }

        // Conditional Projected Payment
        public DbSet<ConditionalProjectedPayment> Payroll_ConditionalProjectedPayment { get; set; }
        public DbSet<ConditionalProjectedPaymentParameter> Payroll_ConditionalProjectedPaymentParameter { get; set; }
        public DbSet<ConditionalProjectedPaymentExcludeParameter> Payroll_ConditionalProjectedPaymentExcludeParameter { get; set; }
        public DbSet<ConditionalProjectedPaymentDetail> Payroll_ConditionalProjectedPaymentDetail { get; set; }





        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}
