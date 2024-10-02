using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Employee.Domain.External_Tables
{
    [Table("tblBranches"), Keyless]
    public class tblBranches
    {
        public long BranchId { get; set; }
        [StringLength(100)]
        public string BranchUniqueId { get; set; }
        [Required, StringLength(100)]
        public string BranchName { get; set; }
        [Required, StringLength(5)]
        public string ShortName { get; set; }
        [StringLength(50)]
        public string BranchCode { get; set; }
        [StringLength(20), DataType(DataType.PhoneNumber)]
        public string MobileNo { get; set; }
        [StringLength(200), DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [StringLength(20), DataType(DataType.PhoneNumber)]
        public string PhoneNo { get; set; }
        [StringLength(20)]
        public string Fax { get; set; }
        [StringLength(200)]
        public string Address { get; set; }
        public bool IsActive { get; set; }
        [StringLength(200)]
        public string Remarks { get; set; }
        public long DivisionId { get; set; }
    }
}
