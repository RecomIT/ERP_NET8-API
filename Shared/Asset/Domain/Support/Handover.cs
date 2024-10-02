using Microsoft.EntityFrameworkCore;
using Shared.BaseModels.For_DomainModel;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Shared.Asset_Module.Models.Domain.Support
{
    [Table("Asset_Handover")]
    [Index("HandoverId", "CompanyId", "OrganizationId", IsUnique = false,Name = "IX_Asset_Handover_NonClusteredIndex")]

    public class Handover : BaseModel2
    {
        [Key]
        public long HandoverId { get; set; }
        [Column(TypeName = "date")]
        public Nullable<DateTime> TransactionDate { get; set; }
        public long? EmployeeId { get; set; }
        public long AssetId { get; set; }     
        public string ProductId { get; set; }      
        public string Condition { get; set; }
        [StringLength(200)]
        public string Remarks { get; set; }

    }
}
