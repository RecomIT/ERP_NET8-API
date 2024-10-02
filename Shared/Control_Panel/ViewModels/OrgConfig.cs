using System;
using System.ComponentModel.DataAnnotations;

namespace Shared.Control_Panel.ViewModels
{
    public class ApplicationRoleViewModel
    {
        public string Id { get; set; }
        [StringLength(256)]
        public string Name { get; set; }
        [StringLength(256)]
        public string NormalizedName { get; set; }
        public string ConcurrencyStamp { get; set; }
        public bool IsActive { get; set; }
        [StringLength(100)]
        public string Description { get; set; }
        public long CompanyId { get; set; }
        public string CompanyName { get; set; }
        public long OrganizationId { get; set; }
        public string OrganizationName { get; set; }
        [StringLength(100)]
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        [StringLength(100)]
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
