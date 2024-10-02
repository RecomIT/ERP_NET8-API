using Microsoft.EntityFrameworkCore;
using Shared.BaseModels.For_DomainModel;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Shared.Asset_Module.Models.Domain.Creation
{
    [Table("Asset_Product")]
    [Index("Id", "CompanyId", "OrganizationId", IsUnique = false, Name = "IX_Asset_Product_NonClusteredIndex")]
    public class Product : BaseModel
    {
        [Key]
        public long Id { get; set; }
        public long AssetId { get; set; }
        public long  AssigningId { get; set; }
        public string Type { get; set; }
        public string ProductID { get; set; }
        public string Number { get; set; }
        public string PIN { get; set; }
        public string PUK { get; set; }
        public string IMEI1 { get; set; }
        public string IMEI2 { get; set; }
        public string LANMAC { get; set; }
        public string LANIP { get; set; }
        public string WiFiMAC { get; set; }
        public string WiFiIP { get; set; }
        public bool Assigned { get; set; }
        public bool Condition { get; set; }
    }
}
