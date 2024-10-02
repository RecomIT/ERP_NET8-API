using Shared.OtherModels.Pagination;

namespace Shared.Payroll.Filter.Payment
{
    public class ConditionalProjected_Filter : Sortparam
    {
        public string AllowanceNameId { get; set; }
        public string FiscalYearId { get; set; }
        public string StateStatus { get; set; }
    }
}
