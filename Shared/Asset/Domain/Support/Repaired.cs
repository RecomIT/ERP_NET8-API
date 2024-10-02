using Microsoft.EntityFrameworkCore;
using Shared.BaseModels.For_DomainModel;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Shared.Asset_Module.Models.Domain.Support
{
    [Table("Asset_Repaired")]
    [Index("RepairedId", "CompanyId", "OrganizationId", IsUnique = false,Name = "IX_Asset_Repaired_NonClusteredIndex")]

    public class Repaired : BaseModel2
    {
        [Key]
        public long RepairedId { get; set; }
        public long ServicingId { get; set; }
        [Column(TypeName = "date")]
        public Nullable<DateTime> TransactionDate { get; set; }
        public long VendorId { get; set; }
        public long AssetId { get; set; }    
        public string ProductId { get; set; }
        public string Number { get; set; }
        [Column(TypeName ="decimal(18,2)")]
        public  decimal? CostingAmount { get;}
        [StringLength(350)]
        public string Token { get; set; }
        [StringLength(300)]
        public string Remarks { get; set; }
       
 
        
    }
}
