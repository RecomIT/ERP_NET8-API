using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Control_Panel.Domain
{
    public class ApplicationRole : IdentityRole<Guid>
    {
        public bool IsActive { get; set; }
        [StringLength(100)]
        public string Description { get; set; }
        public bool? IsSysadmin { get; set; }
        public bool? IsGroupAdmin { get; set; }
        public bool? IsCompanyAdmin { get; set; }
        public bool? IsBranchAdmin { get; set; }
        public long CompanyId { get; set; }
        [ForeignKey("Organization")]
        public long OrganizationId { get; set; }
        public Organization Organization { get; set; }
        [StringLength(100)]
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        [StringLength(100)]
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
