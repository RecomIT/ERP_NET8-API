using System.ComponentModel.DataAnnotations;

namespace Shared.Payroll.ViewModel.Salary
{
    public class LastSalaryProcessInfoViewModel
    {
        public long SalaryProcessId { get; set; }
        [StringLength(30)]
        public string BatchNo { get; set; }
        [StringLength(50)]
        public string ProcessType { get; set; }
        [StringLength(50)]
        public string SalaryProcessUniqId { get; set; }
    }
}
