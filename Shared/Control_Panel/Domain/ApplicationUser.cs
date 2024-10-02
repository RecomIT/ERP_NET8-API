using System;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Control_Panel.Domain
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        [StringLength(150)]
        public string FullName { get; set; }
        [StringLength(50)]
        public string EmployeeId { get; set; }
        public bool IsActive { get; set; }
        public bool IsRoleActive { get; set; }
        [StringLength(50)]
        public string RoleId { get; set; }
        [StringLength(150)]
        public string Address { get; set; }
        public long OrganizationId { get; set; }
        public long CompanyId { get; set; }
        public long DivisionId { get; set; }
        public bool? IsDefaultPassword { get; set; }
        public int PasswordChangedCount { get; set; }
        [StringLength(100)]
        public string DefaultCode { get; set; }
        public string DefaultPasswordHash { get; set; }
        public string DefaultSecurityStamp { get; set; }
        [Column(TypeName = "date")]
        public DateTime? PasswordExpiredDate { get; set; }
        [ForeignKey("Branch")]
        public long BranchId { get; set; }
        public Branch Branch { get; set; }
        [StringLength(100)]
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        [StringLength(100)]
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
