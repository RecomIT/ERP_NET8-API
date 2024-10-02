using Microsoft.EntityFrameworkCore;
using Shared.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Shared.Asset_Module.Models.Domain.Setting
{
    [Table("Asset_Vendor"), Index("Name", "IsActive", "CompanyId", "OrganizationId", IsUnique = false, Name = "IX_Asset_Vendor_NonClusteredIndex")]

    public class Vendor : BaseModel
    {
        [Key]
        public int VendorId { get; set; }

        [Required, StringLength(200)]
        public string Name { get; set; }

        [StringLength(200)]
        public string NameInBengali { get; set; }

        public bool IsActive { get; set; }

        [StringLength(200)]
        public string Remarks { get; set; }
    }
}
