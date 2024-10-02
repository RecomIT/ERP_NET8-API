using Microsoft.EntityFrameworkCore;
using Shared.BaseModels.For_DomainModel;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Shared.Asset_Module.Models.Domain.Resignation
{
    [Table("Asset_Damaged")]
    [Index("DamagedId", "CompanyId", "OrganizationId", IsUnique = false,Name = "IX_Asset_Damaged_NonClusteredIndex")]

    public class Damaged : BaseModel2
    {
        [Key]
        public long DamagedId { get; set; }
        public long AssetId { get; set; }
        public long AssigningId { get; set; }
        [Column(TypeName = "date")]       
        public Nullable<DateTime> TransactionDate { get; set; }
        public long? EmployeeId { get; set; }              
        public string ProductId { get; set; }  
        [StringLength(200)]
        public string Remarks { get; set; }
    }
}
