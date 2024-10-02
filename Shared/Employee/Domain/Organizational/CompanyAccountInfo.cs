using Shared.BaseModels.For_DomainModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Employee.Domain.Organizational
{
    [Table("HR_CompanyAccountInfo")]
    public class CompanyAccountInfo : BaseModel
    {
        [Key]
        public long Id { get; set; }
        public long BankId { get; set; }
        public long BankBranchId { get; set; }
        [StringLength(200)]
        public string AccountName { get; set; }
        [StringLength(200)]
        public string AccountNumber { get; set; }
        [StringLength(300)]
        public string Remarks { get; set; }
    }
}
