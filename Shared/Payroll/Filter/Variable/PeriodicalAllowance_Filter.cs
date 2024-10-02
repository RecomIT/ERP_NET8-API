using Shared.OtherModels.Pagination;

namespace Shared.Payroll.Filter.Variable
{
    public class PeriodicalAllowance_Filter : Sortparam
    {
        public string AllowanceNameId { get; set; }
        public string SpecifyFor { get; set; }
        public string AmountBaseOn { get; set; }
        public string StateStatus { get; set; }
    }
}
