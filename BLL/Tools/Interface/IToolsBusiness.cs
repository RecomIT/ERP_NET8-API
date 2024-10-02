using Shared.Tools.Domain;
using Shared.Tools.DTO;
using Shared.Tools.ViewModel;

namespace BLL.Tools.Interface
{
    public interface IToolsBusiness
    {
        string GetFiscalYear();

        string GetAssismentYear();

        decimal GetTotalAnnualIncome(EasyTaxDTO model);

        decimal GetExemptionOfAnnualIncome(decimal totalAnnualIncome);

        decimal GetTaxableIncome(decimal totalAnnualIncome, decimal exemptionOfAnnualIncome);

        List<ReportTaxCardIncomeHead> GetSalaryComponentDetails(EasyTaxDTO model);

        List<IncomeTaxSlabParameter> GetIncomeTaxSlabParameters(EasyTaxDTO model);

        List<EmployeeTaxSlab> GetEmployeeTaxSlabs(List<IncomeTaxSlabParameter> incomeTaxSlabParameters, decimal taxableIncome);

        InvestmentRebate GetInvestmentRebate(decimal yearlyPF, decimal investmentAmount, decimal taxableIncome);

        decimal GetNetTaxPayable(decimal taxLiability, decimal totalRebate, EasyTaxDTO model);
    }
}
