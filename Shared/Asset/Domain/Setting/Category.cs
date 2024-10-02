using Microsoft.EntityFrameworkCore;
using Shared.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Shared.Asset_Module.Models.Domain.Setting
{
    [Table("Asset_Category"), Index("Name", "IsActive", "CompanyId", "OrganizationId", IsUnique = false, Name = "IX_Asset_Category_NonClusteredIndex")]

    public class Category : BaseModel
    {
        [Key]
        public int CategoryId { get; set; }

        [Required, StringLength(200)]
        public string Name { get; set; }

        [StringLength(200)]
        public string NameInBengali { get; set; }

        public bool IsActive { get; set; }

        [StringLength(200)]
        public string Remarks { get; set; }
    }
}
