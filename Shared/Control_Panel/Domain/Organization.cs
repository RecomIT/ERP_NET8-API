using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Control_Panel.Domain
{
    [Table("tblOrganizations")]
    public class Organization
    {
        [Key]
        public long OrganizationId { get; set; }
        [StringLength(100)]
        public string OrgUniqueId { get; set; }
        [StringLength(20)]
        public string OrgCode { get; set; } // C-0001
        [StringLength(150)]
        public string OrganizationName { get; set; }
        [StringLength(50)]
        public string ShortName { get; set; }
        [StringLength(100)]
        public string SiteThumbnailPath { get; set; }
        [StringLength(150)]
        public string Address { get; set; }
        [StringLength(150)]
        public string Email { get; set; }
        [StringLength(50)]
        public string PhoneNumber { get; set; }
        [StringLength(50)]
        public string MobileNumber { get; set; }
        [StringLength(50)]
        public string Fax { get; set; }
        [StringLength(100)]
        public string Website { get; set; }
        public bool IsActive { get; set; }
        [Column(TypeName = "date")]
        public DateTime? ContractStartDate { get; set; }
        [Column(TypeName = "date")]
        public DateTime? ContractExpireDate { get; set; }
        [StringLength(200)]
        public string Remarks { get; set; }
        public byte[] OrgPic { get; set; }
        [StringLength(50)]
        public string OrgImageFormat { get; set; }
        [StringLength(250)]
        public string OrgLogoPath { get; set; }
        public byte[] ReportPic { get; set; }
        [StringLength(50)]
        public string ReportImageFormat { get; set; }
        [StringLength(250)]
        public string ReportLogoPath { get; set; }
        public long? AppId { get; set; }
        [StringLength(100)]
        public string AppName { get; set; }
        [StringLength(50)]
        public string StorageName { get; set; }
        public ICollection<Company> Companies { get; set; }
        [StringLength(100)]
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        [StringLength(100)]
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
