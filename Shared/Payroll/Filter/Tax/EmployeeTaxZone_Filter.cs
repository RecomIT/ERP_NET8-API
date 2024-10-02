using Shared.OtherModels.Pagination;

namespace Shared.Payroll.Filter.Tax
{
    public class EmployeeTaxZone_Filter : Sortparam
    {
        public string EmployeeTaxZoneId { get; set; }
        public string EmployeeId { get; set; }
        public string TaxZone { get; set; }
    }
}
