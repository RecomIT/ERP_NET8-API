using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace Shared.Control_Panel.ViewModels
{
    public class OrganizationViewModel
    {
        public long OrganizationId { get; set; }
        [Required, StringLength(150)]
        public string OrganizationName { get; set; }
        [Required, StringLength(20)]
        public string OrgCode { get; set; }
        [Required, StringLength(50)]
        public string ShortName { get; set; }
        [Required, StringLength(150)]
        public string Address { get; set; }
        [Required, StringLength(150)]
        public string Email { get; set; }
        [StringLength(50)]
        public string PhoneNumber { get; set; }
        [Required, StringLength(50)]
        public string MobileNumber { get; set; }
        [StringLength(50)]
        public string Fax { get; set; }
        [StringLength(100)]
        public string Website { get; set; }
        public bool IsActive { get; set; }
        public DateTime? ContractStartDate { get; set; }
        public DateTime? ContractExpireDate { get; set; }
        [StringLength(200)]
        public string Remarks { get; set; }
        public IFormFile OrgPicFile { get; set; }
        public byte[] OrgPic { get; set; }
        [StringLength(50)]
        public string OrgImageFormat { get; set; }
        public string OrgBase64Pic { get; set; }
        [StringLength(250)]
        public string OrgLogoPath { get; set; }
        public IFormFile ReportPicFile { get; set; }
        [StringLength(50)]
        public string ReportImageFormat { get; set; }
        public string ReportBase64Pic { get; set; }
        public byte[] ReportPic { get; set; }
        [StringLength(250)]
        public string ReportLogoPath { get; set; }
        public long? AppId { get; set; }
        [StringLength(100)]
        public string AppName { get; set; }
        [StringLength(100)]
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        [StringLength(100)]
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
