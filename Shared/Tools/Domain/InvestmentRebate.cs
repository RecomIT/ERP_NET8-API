using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Tools.Domain
{
    public class InvestmentRebate
    {
        public decimal PFContributionsBothParts { get; set; }

        public decimal OtherInvestmentRecogExceptPF { get; set; }

        public decimal ActualInvestmentMade { get; set; }

        public decimal TaxRebateDueToInvestment { get; set; }

    }
}
