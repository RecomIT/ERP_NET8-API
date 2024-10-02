using Shared.OtherModels.Pagination;

namespace Shared.Payroll.Filter.Tax
{
    public class EmployeeYearInvestment_Filter : Pageparam
    {
        public string Id { get; set; }
        public string FiscalYearId { get; set; }
        public string EmployeeId { get; set; }
    }
}
