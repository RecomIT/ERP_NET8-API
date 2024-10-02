using Shared.Models;

namespace Shared.Payroll.ViewModel.Bonus
{
    public class EmployeeExcludedFromBonusViewModel : BaseViewModel3
    {
        public long ExcludeId { get; set; }
        public long EmployeeId { get; set; }
        public long BonusId { get; set; }
        public long BonusConfigId { get; set; }

        // Prop
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string BonusName { get; set; }
        public string BonusConfigCode { get; set; }
    }
}
