namespace Shared.Payroll.Filter.Payment
{
    public class EligibilityInMonthlyAllowanceConfig_Filter
    {
        public long JobType { get; set; }
        public string Religion { get; set; }
        public string MaritalStatus { get; set; }
        public string PhysicalCondition { get; set; }
        public string Gender { get; set; }
        public string PaymentDate { get; set; }
    }
}
