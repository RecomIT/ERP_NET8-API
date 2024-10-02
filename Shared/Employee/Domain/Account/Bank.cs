using Microsoft.EntityFrameworkCore;
using Shared.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Employee.Domain.Account
{
    [Table("HR_Banks"), Index("BankName", "BankCode", "IsActive", "CompanyId", "OrganizationId", IsUnique = false, Name = "IX_HR_Banks_NonClusteredIndex")]
    public class Bank : BaseModel
    {
        [Key]
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
