using Microsoft.EntityFrameworkCore;
using Shared.BaseModels.For_DomainModel;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Shared.Asset_Module.Models.Domain.Creation
{
    [Table("Asset_Assigning")]
    [Index("AssigningId", "CompanyId", "OrganizationId", IsUnique = false,Name = "IX_Asset_Assigning_NonClusteredIndex")]

    public class Assigning : BaseModel2
    {
        [Key]
        public long AssigningId { get; set; }
        [Column(TypeName = "date")]       
        public Nullable<DateTime> TransactionDate { get; set; }
        public long? EmployeeId { get; set; }
        public long AssetId { get; set; }        
        public string ProductId { get; set; }
        public bool Approved { get; set; }        
        [StringLength(200)]
        public string Remarks { get; set; }
    }
}
