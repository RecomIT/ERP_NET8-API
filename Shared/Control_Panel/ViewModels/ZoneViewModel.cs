using System;
using System.ComponentModel.DataAnnotations;

namespace Shared.Control_Panel.ViewModels
{
    public class ZoneViewModel
    {
        public long ZoneId { get; set; }
        [Required, StringLength(100)]
        public string ZoneName { get; set; }
        [Required, StringLength(5)]
        public string ShortName { get; set; }
        [StringLength(30)]
        public string ZoneCode { get; set; }
        public bool IsActive { get; set; }
        [Range(0, long.MaxValue)]
        public long DistrictId { get; set; }
        [StringLength(100)]
        public string DistrictName { get; set; }
        public long DivisionId { get; set; }
        [StringLength(100)]
        public string DivisionName { get; set; }
        public long CompanyId { get; set; }
        [StringLength(100)]
        public string CompanyName { get; set; }
        public long OrganizationId { get; set; }
        [StringLength(100)]
        public string OrganizationName { get; set; }
    }
}
