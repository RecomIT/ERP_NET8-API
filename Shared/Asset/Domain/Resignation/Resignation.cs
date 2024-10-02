using Microsoft.EntityFrameworkCore;
using Shared.BaseModels.For_DomainModel;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Shared.Asset_Module.Models.Domain.Resignation
{
    [Table("Asset_Resignation")]
    [Index("ResignationId", "CompanyId", "OrganizationId", IsUnique = false,Name = "IX_Asset_Resignation_NonClusteredIndex")]

    public class Resignation : BaseModel2
    {
        [Key]
        public long ResignationId { get; set; }
        public long AssetId { get; set; }
        public long AssigningId { get; set; }
        [Column(TypeName = "date")]       
        public Nullable<DateTime> TransactionDate { get; set; }
        public long? EmployeeId { get; set; }              
        public string ProductId { get; set; }
        public bool IsReturned { get; set; }
        [StringLength(200)]
        public string Condition { get; set; }
        [StringLength(200)]
        public string Remarks { get; set; }
    }
}
