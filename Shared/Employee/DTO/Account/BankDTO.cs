using System.ComponentModel.DataAnnotations;

namespace Shared.Employee.DTO.Account
{
    public class BankDTO
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
