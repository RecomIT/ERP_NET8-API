using System.ComponentModel.DataAnnotations;

namespace Shared.Employee.DTO.Account
{
    public class BankBranchDTO
    {
        public int BankBranchId { get; set; }
        [Required, StringLength(100)]
        public string BankBranchName { get; set; }
        [StringLength(100)]
        public string BankBranchNameInBengali { get; set; }
        [StringLength(100)]
        public string RoutingNumber { get; set; }
        public bool IsActive { get; set; }
        public int BankId { get; set; }
    }
}
