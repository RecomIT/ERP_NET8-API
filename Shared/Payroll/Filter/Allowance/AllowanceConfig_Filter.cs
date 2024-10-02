using Shared.OtherModels.Pagination;

namespace Shared.Payroll.Filter.Allowance
{
    public class AllowanceConfig_Filter : Sortparam
    {
        public string ConfigId { get; set; }
        public string AllowanceNameId { get; set; }
        public string StateStatus { get; set; }
        public string ActivationDateFrom { get; set; }
        public string ActivationDateTo { get; set; }
        public string DeactivationDateFrom { get; set; }
        public string DeactivationDateTo { get; set; }
    }
}
