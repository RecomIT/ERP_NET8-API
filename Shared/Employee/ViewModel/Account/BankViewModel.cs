using Shared.Models;
using System.ComponentModel.DataAnnotations;

namespace Shared.Employee.ViewModel.Account
{
    public class BankViewModel : BaseViewModel2
    {
        public int BankId { get; set; }
        [Required, StringLength(100)]
        public string BankName { get; set; }
        [StringLength(100)]
        public string BankNameInBengali { get; set; }
        [StringLength(100)]
        public string BankCode { get; set; }
        public bool IsActive { get; set; }
    }
}
