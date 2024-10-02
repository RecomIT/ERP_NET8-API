namespace Shared.Payroll.ViewModel.Salary
{
    public class SalaryBatchNoViewModel
    {
        public long SalaryProcessId { get; set; }
        public string BatchNo { get; set; }
        public short SalaryYear { get; set; }
        public short SalaryMonth { get; set; }
        public bool IsDisbursed { get; set; }
    }
}
