using Microsoft.EntityFrameworkCore;
using Shared.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Shared.Asset_Module.Models.Domain.Setting
{
    [Table("Asset_SubCategory"), Index("Name", "IsActive", "CompanyId", "OrganizationId", IsUnique = false, Name = "IX_Asset_SubCategory_NonClusteredIndex")]

    public class SubCategory : BaseModel
    {
        [Key]
        public int SubCategoryId { get; set; }

        [Required]
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
