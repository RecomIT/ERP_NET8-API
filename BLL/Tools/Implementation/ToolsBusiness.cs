using BLL.Tools.Interface;
using Shared.Tools.Domain;
using Shared.Tools.DTO;
using Shared.Tools.ViewModel;

namespace BLL.Tools.Implementation
{
    public class ToolsBusiness : IToolsBusiness
    {
        private readonly IncomeTaxParameter _IncomeTaxParameter;

        public ToolsBusiness()
        {
            _IncomeTaxParameter = new IncomeTaxParameter();
        }

        public string GetFiscalYear()
        {
            var currentDate = DateTime.Now;
            // Determine the fiscal year based on the current date
            int fiscalYear;
            if (currentDate.Month >= 7) // Fiscal year starts from July
            {
                fiscalYear = currentDate.Year;
            }
            else {
                fiscalYear = currentDate.Year - 1;
            }

            // Build the fiscal year string representation
            string result = fiscalYear.ToString() + "-" + (fiscalYear + 1).ToString().Substring(2, 2);
            return result;
        }

        public string GetAssismentYear()
        {
            var currentDate = DateTime.Now;
            // Determine the fiscal year based on the current date
            int fiscalYear;
            if (currentDate.Month >= 7) // Fiscal year starts from July
            {
                fiscalYear = currentDate.Year;
            }
            else {
                fiscalYear = currentDate.Year - 1;
            }

            // Build the fiscal year string representation
            string result = (fiscalYear + 1).ToString() + "-" + (fiscalYear + 2).ToString().Substring(2, 2);
            return result;
        }

        public decimal GetTotalAnnualIncome(EasyTaxDTO model)
        {
            decimal totalAnnualIncome = model.Basic.Amount +
                             model.HouseRent.Amount +
                             model.Medical.Amount +
                             model.Conveyance.Amount +
                             model.Bonus.Amount +
                             model.PF.Amount +
                             model.OtherAllowances.Amount;

            return totalAnnualIncome;
        }

        public decimal GetExemptionOfAnnualIncome(decimal totalAnnualIncome)
        {
            decimal exemptionOfAnnualIncome = Math.Min(_IncomeTaxParameter.MaxExemptionAmountofAnnualIncome, (totalAnnualIncome * _IncomeTaxParameter.ExemptionPercentofAnnualIncome / 100));
            
            return Math.Round(exemptionOfAnnualIncome);
        }

        public decimal GetTaxableIncome(decimal totalAnnualIncome, decimal exemptionOfAnnualIncome)
        {
            decimal taxableIncome;

            taxableIncome = totalAnnualIncome - exemptionOfAnnualIncome;

            return taxableIncome;
        }

        public List<IncomeTaxSlabParameter> GetIncomeTaxSlabParameters(EasyTaxDTO model)
        {
            var slabList = new List<IncomeTaxSlabParameter> {

                new IncomeTaxSlabParameter {
                    Id = 1,
                    Name = "First",
                    DisplayName = "On the First BDT-",
                    Amount = 350000,
                    Percentage = 0

                },
                new IncomeTaxSlabParameter {
                    Id = 2,
                    Name = "Second",
                    DisplayName = "On the Next BDT-",
                    Amount = 100000,
                    Percentage = 5

                },
                new IncomeTaxSlabParameter {
                    Id = 3,
                    Name = "Third",
                    DisplayName = "On the Next BDT-",
                    Amount = 400000,
                    Percentage = 10

                },
                new IncomeTaxSlabParameter {
                    Id = 4,
                    Name = "Fourth",
                    DisplayName = "On the Next BDT-",
                    Amount = 500000,
                    Percentage = 15

                },
                new IncomeTaxSlabParameter {
                    Id = 5,
                    Name = "Fifth",
                    DisplayName = "On the Next BDT-",
                    Amount = 500000,
                    Percentage = 20

                },

                new IncomeTaxSlabParameter {
                    Id = 6,
                    Name = "Sixth",
                    DisplayName = "On the Next BDT-",
                    Amount = 2000000,
                    Percentage = 25

                },

                new IncomeTaxSlabParameter {
                    Id = 7,
                    Name = "Remaining",
                    DisplayName = "On Remaining Balance",
                    Amount = 0,
                    Percentage = 30

                },

            };

            var firstSlab = slabList.FirstOrDefault(x => x.Name == "First");

            if (model.Gender == "Female" || model.OverAged) {
                firstSlab.Amount = 400000;
            }

            if (model.Gender == "Others" || model.PhysicallyChallenged) {
                firstSlab.Amount = 475000;
            }

            if (model.FreedomFighters) {
                firstSlab.Amount = 500000;
            }

            return slabList;
        }

        public List<EmployeeTaxSlab> GetEmployeeTaxSlabs(List<IncomeTaxSlabParameter> incomeTaxSlabParameters, decimal totalTaxableIncome)
        {
            var employeeTaxSlabs = new List<EmployeeTaxSlab>();

            var lastSlab = incomeTaxSlabParameters.LastOrDefault();

            foreach (var slabParam in incomeTaxSlabParameters) {
                
                decimal taxableIncome = 0;
                decimal liability = 0;

                taxableIncome = Math.Min(totalTaxableIncome, slabParam.Amount);


                liability = taxableIncome * (slabParam.Percentage/100);

                var slab = new EmployeeTaxSlab {
                    
                    Id = slabParam.Id,
                    CurrentRate = slabParam.Percentage,
                    Parameter = $"{slabParam.DisplayName}{slabParam.Amount}",
                    TaxableIncome = Math.Round(taxableIncome),
                    TaxLiability = Math.Round(liability),
                };

                if (lastSlab == slabParam) {
                    slab.Parameter = slabParam.DisplayName;
                    slab.TaxableIncome = Math.Round(totalTaxableIncome);
                    slab.TaxLiability = Math.Round(totalTaxableIncome * (slabParam.Percentage / 100));

                    totalTaxableIncome -= totalTaxableIncome;
                }

                employeeTaxSlabs.Add(slab);

                
                //Debug.WriteLine($"{slab.CurrentRate}% | {slab.Parameter} | {slab.TaxableIncome} | {slab.TaxLiability}");

                totalTaxableIncome -= taxableIncome;
            }

            return employeeTaxSlabs;
        }

        public InvestmentRebate GetInvestmentRebate(decimal yearlyPF, decimal investmentAmount, decimal taxableIncome)
        {
            
            decimal eligibleRebateBasedOnTaxableIncome = taxableIncome * _IncomeTaxParameter.PercentofIncomeforRebate / 100;
            decimal pfBothPartContribution = yearlyPF * 2;

            decimal actualInvestmentTotal;

            if (investmentAmount > 0) {
                actualInvestmentTotal = investmentAmount + pfBothPartContribution;
            }
            else {
                actualInvestmentTotal = pfBothPartContribution;
            }

            decimal eligibleRebateBasedOnInvestment = actualInvestmentTotal * (_IncomeTaxParameter.PercentageofActualInvestmentforRebate / 100);

            decimal totalRebate = Math.Min(Math.Min(eligibleRebateBasedOnTaxableIncome, eligibleRebateBasedOnInvestment), _IncomeTaxParameter.MaximumAmountofRebate);

            var invRebate = new InvestmentRebate() {
                ActualInvestmentMade = actualInvestmentTotal,
                OtherInvestmentRecogExceptPF = investmentAmount,
                PFContributionsBothParts = pfBothPartContribution,
                TaxRebateDueToInvestment = Math.Round(totalRebate)
            };

            return invRebate;
        }

        public decimal GetNetTaxPayable(decimal taxLiability, decimal totalRebate, EasyTaxDTO model)
        {
            decimal netTaxPayable;

            decimal minimumTax = getMinimumTax(model.Region);

            if (taxLiability == 0) 
            {
                netTaxPayable = 0;
            }
            else if (totalRebate > taxLiability) 
            {
                netTaxPayable = minimumTax;
            }
            else 
            {
                if ((taxLiability - totalRebate) <= minimumTax) { netTaxPayable = minimumTax; }
                else { netTaxPayable = taxLiability - totalRebate; }
            }


            var amount = netTaxPayable - (model.AITAmount + model.RefundAmount);

            return amount <= 0 ? 0 : amount;
        }

        private decimal getMinimumTax(string region)
        {
            var result = region switch {
                "Dhaka" => 5000,
                "Chittagong" => 5000,
                "Other City Corp." => 4000,
                "Outside City Corp." => 3000,
                _ => 5000
            };

            return result;
        }

        public List<ReportTaxCardIncomeHead> GetSalaryComponentDetails(EasyTaxDTO model)
        {
            var salaryComponentList = new List<SalaryComponent>
                {
                    model.Basic,
                    model.HouseRent,
                    model.Medical,
                    model.Conveyance,
                    model.Bonus,
                    model.PF,
                    model.OtherAllowances

                };

            var empTaxDlist = new List<ReportTaxCardIncomeHead>();

            foreach (var eTd in salaryComponentList.Where(x => x.Amount > 0).ToList())
            {

                var TaxIH = new ReportTaxCardIncomeHead();
                TaxIH.EmployeeId = model.EmployeeId;
                TaxIH.EmployeeName = model.EmployeeName;
                
                TaxIH.IncomeAnnualGrossName = eTd.Name;
                TaxIH.IncomeTillDateAmount = 0;
                TaxIH.IncomeCurrentMonthAmount = 0;
                TaxIH.IncomeProjectedDateAmount = 0;
                TaxIH.IncomeAnnualGrossAmount = eTd.Amount;
                TaxIH.ExemptedLessAmount = 0;
                TaxIH.IncomeTotalTaxableAmount = eTd.Amount;

                empTaxDlist.Add(TaxIH);
            }

            return empTaxDlist;
        }
    }
}
