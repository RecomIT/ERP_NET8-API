using Shared.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Shared.Control_Panel.ViewModels
{
    public class BranchViewModel : BaseModel
    {
        public long BranchId { get; set; }
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
        [Required, StringLength(200)]
        public string Address { get; set; }
        public bool IsActive { get; set; }
        [StringLength(200)]
        public string Remarks { get; set; }
        public long ZoneId { get; set; }
        [StringLength(100)]
        public string ZoneName { get; set; }
        public long DistrictId { get; set; }
        [StringLength(100)]
        public string DistrictName { get; set; }
        public long DivisionId { get; set; }
        [StringLength(100)]
        public string DivisioName { get; set; }
        [StringLength(100)]
        public string CompanyName { get; set; }
        [StringLength(100)]
        public string OrganizationName { get; set; }
    }
}
