using System.ComponentModel.DataAnnotations;


namespace Shared.Employee.ViewModel.Info
{
    public class EmployeePaymentInfo
    {
        [StringLength(30)]
        public string PaymentMode { get; set; }
        [StringLength(50)]
        public string AgentName { get; set; }
        [StringLength(50)]
        public string AccountNo { get; set; }
        public long BankId { get; set; }
        public long BankBranchId { get; set; }
    }
}
