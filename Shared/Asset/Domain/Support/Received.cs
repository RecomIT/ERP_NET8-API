using Microsoft.EntityFrameworkCore;
using Shared.BaseModels.For_DomainModel;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Shared.Asset_Module.Models.Domain.Support
{
    [Table("Asset_Received")]
    [Index("ReceivedId", "CompanyId", "OrganizationId", IsUnique = false,Name = "IX_Asset_Received_NonClusteredIndex")]

    public class Received : BaseModel2
    {
        [Key]
        public long ReceivedId { get; set; }
        [Column(TypeName = "date")]
        public Nullable<DateTime> TransactionDate { get; set; }
        public long? EmployeeId { get; set; }
        public long AssetId { get; set; }
        public long AssigningId { get; set; }
        public bool IsServicing { get; set; }
        public string ProductId { get; set; }       
        [StringLength(200)]
        public string Remarks { get; set; }
       
 
        
    }
}
