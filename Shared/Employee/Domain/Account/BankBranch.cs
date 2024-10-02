using Microsoft.EntityFrameworkCore;
using Shared.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Shared.Employee.Domain.Account
{
    [Table("HR_BankBranches"), Index("BankBranchName", "RoutingNumber", "IsActive", "CompanyId", "OrganizationId", IsUnique = false, Name = "IX_HR_BankBranches_NonClusteredIndex")]
    public class BankBranch : BaseModel
    {
        [Key]
        public int BankBranchId { get; set; }
        [Required, StringLength(100)]
        public string BankBranchName { get; set; }
        [StringLength(100)]
        public string BankBranchNameInBengali { get; set; }
        [StringLength(100)]
        public string RoutingNumber { get; set; }
        public bool IsActive { get; set; }
        [ForeignKey("Bank")]
        public int BankId { get; set; }
        public Bank Bank { get; set; }
    }
}
