using Microsoft.AspNetCore.Http;
using Shared.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace Shared.Control_Panel.ViewModels
{
    public class CompanyViewModel : BaseModel
    {
        [Required, StringLength(100)]
        public string CompanyName { get; set; }
        [StringLength(50)]
        public string CompanyCode { get; set; }
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
        public DateTime? ContractExpireDate { get; set; }
        [StringLength(200)]
        public string Remarks { get; set; }
        public byte[] CompanyPic { get; set; }
        [StringLength(250)]
        public string CompanyLogoPath { get; set; }
        [StringLength(50)]
        public string CompanyImageFormat { get; set; }
        public string CompanyBase64Pic { get; set; }
        public IFormFile CompanyPicFile { get; set; }
        public byte[] ReportPic { get; set; }
        [StringLength(250)]
        public string ReportLogoPath { get; set; }
        [StringLength(50)]
        public string ReportImageFormat { get; set; }
        public string ReportBase64Pic { get; set; }
        public IFormFile ReportPicFile { get; set; }
        public string OrganizationName { get; set; }
    }
}
