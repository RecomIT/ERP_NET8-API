using System;

namespace Shared.Payroll.ViewModel.Bonus
{
    public class ExecuteBonusProcess
    {
        public long BonusId { get; set; }
        public long BonusConfigId { get; set; }
        public string SelectedEmployees { get; set; }
        public long? ProcessByDepartmentId { get; set; }
        public long? ProcessByBranchId { get; set; }
        public short BonusMonth { get; set; }
        public short BonusYear { get; set; }
        public DateTime? ProcessDate { get; set; } // Cutoff Date
        public DateTime? PaymentDate { get; set; }
    }
}
