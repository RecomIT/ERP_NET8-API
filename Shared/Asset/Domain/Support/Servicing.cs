using Microsoft.EntityFrameworkCore;
using Shared.BaseModels.For_DomainModel;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Shared.Asset_Module.Models.Domain.Support
{
    [Table("Asset_Servicing")]
    [Index("ServicingId", "CompanyId", "OrganizationId", IsUnique = false,Name = "IX_Asset_Servicing_NonClusteredIndex")]

    public class Servicing : BaseModel2
    {
        [Key]
        public long ServicingId { get; set; }
        [Column(TypeName = "date")]
        public Nullable<DateTime> TransactionDate { get; set; }
        public long VendorId { get; set; }
        public long AssetId { get; set; }    
        public string ProductId { get; set; }
        public string Number { get; set; }
        public string Token { get; set; } 
        [StringLength(300)]
        public string Remarks { get; set; }       
        public bool IsRepaired { get; set; }

    }
}
