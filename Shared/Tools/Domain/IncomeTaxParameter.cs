namespace Shared.Tools.Domain
{
    public class IncomeTaxParameter
    {
        public decimal MaxInvestmentAmount { get; set; } = 15000000;

        public decimal MaxInvestmentPercentage { get; set; } = 20;

        public decimal MaxInvestmentExemptedPercentage { get; set; } = 15;

        public decimal MaxAmountforMaxExemptionPercentage { get; set; } = 1500_000;

        public decimal MinInvestmentExemptedPercentage { get; set; } = 15;

        public decimal MaxExemptionAmountofAnnualIncome { get; set; } = 450000;

        public decimal ExemptionPercentofAnnualIncome { get; set; } = 33.333333333M;

        public decimal PercentofIncomeforRebate { get; set; } = 3;

        public decimal MaximumAmountofRebate { get; set; } = 1000000;

        public decimal PercentageofActualInvestmentforRebate { get; set; } = 15;

        public decimal MinimumTaxAmount { get; set; } = 5000;

        public decimal NonResidentialTaxRate { get; set; } = 30;

        public decimal ExemptionAmountofGFAmount { get; set; } = 25000000;

    }
}
